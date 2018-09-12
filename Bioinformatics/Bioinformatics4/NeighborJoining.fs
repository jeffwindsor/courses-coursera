namespace Bioinformatics

open Matrices
open WeightedAdjacenyGraph

module NeighborJoining =
    type Vertice = int
    type Distance = float
    type Payload = int option //could be anything, not needed
    type DistanceMatrix = Matrix<Distance>
    type DistanceTotals = Distance array
    type DistanceSegment = WeightedSegment<Distance>
    type DistanceGraph = Graph<Vertice, Distance, Payload>

    let EmptyDistance = 0.0
        
    let NormalizedDistance dn (totalDistance:DistanceTotals) i j d =
        // D*i,j = (n - 2) · Di,j - TotalDistanceD(i) - TotalDistanceD(j).
        (float (dn - 2)) * d - totalDistance.[i] - totalDistance.[j]
        
    let NormalizedSegment  dn (totalDistance:DistanceTotals) ((i,j) as s, d) : DistanceSegment =
        (s, NormalizedDistance dn totalDistance i j d)

    let MergeDistance (ds:DistanceMatrix) i j k = 
        // Dk,m = (1/2)(Dk,i + Dk,j - Di,j).
        0.5 * (ds.[k,i] + ds.[k,j] - ds.[i,j])
    
    let LimbLength (totalDistance:DistanceTotals) matrixLength i j dij=
        //Δi,j = (TotalDistanceD(i) - TotalDistanceD(j)) / (n - 2)
        let Δab = (totalDistance.[i] - totalDistance.[j]) / (float (matrixLength - 2))
        //LimbLength(i) = (1/2) (Di,j + Δi,j) 
        let lli = 0.5 * (dij + Δab) 
        //LimbLength(j) = (1/2) (Di,j - Δi,j)
        let llj = 0.5 * (dij - Δab)        
        lli, llj

    let ClosestSegment dn (ds:DistanceMatrix) (totalDistance:DistanceTotals) validIndexes =
        SegmentsById validIndexes
        |> L1LessThanL2
        |> MatrixWeighted ds
        |> List.map (NormalizedSegment dn totalDistance) 
        |> List.minBy snd
        |> fst

    let UpdateMatrixValuesForVMRowAndColumns graphSize (ds:DistanceMatrix) vi vj vm =
        [0 .. (graphSize - 1)]
        |> List.iter (fun vk -> 
            let dkm = if vk=vm then 0.0 else MergeDistance ds vi vj vk
            UpdateValue ds vk vm dkm
            )

    let CreateGraph n (ds:DistanceMatrix) : DistanceGraph =
        let graphSize = ((2 * n) - 1)
        let g = InitializeGraph (fun _ -> None) graphSize
        let total (ds:DistanceMatrix) allVertices = TotalsBy graphSize (fun i j -> if ContainsBoth allVertices i j then ds.[i,j] else EmptyDistance)
        let fPayload _ _ _ = None

        let rec inner dn (vm:Vertice) (ds:DistanceMatrix) (validIndexes:Set<Vertice>) =
            match Set.count validIndexes with
            | 2 -> // Floor
                let vi,vj = vm - 1, vm - 2 // hit bottom use the last two valid inner indexes
                g |> ConnectNodes fPayload (fun _ -> ds.[vi,vj]) (fun _ -> ds.[vj,vi]) vi vj
            | _ -> // Calculate Closest Segment and Values
                let tds = total ds validIndexes
                let (vi,vj) = ClosestSegment dn ds tds (Set.toList validIndexes)
                let Dik, Djk = LimbLength tds dn vi vj ds.[vi,vj]
                UpdateMatrixValuesForVMRowAndColumns graphSize ds vi vj vm
                // Vi and Vj merge to Vm - add remove appropriately
                validIndexes.Remove(vi).Remove(vj).Add(vm)
                |> inner (dn - 1) (vm + 1) ds
                // Build Graph on the way up the stack
                |> ConnectNodesViaNewInnerNode fPayload (fun _ -> Dik) (fun _ -> Djk) vi vj vm

        // Start Recursion
        let expandedMatrix = Array2D.init graphSize graphSize (fun a b -> if (a < n && b < n) then ds.[a,b] else EmptyDistance)
        inner n n expandedMatrix (Set.ofList [0 .. (n - 1)])
