namespace Bioinformatics

open Matrices
open WeightedAdjacenyGraph

module UPGMA =
    type Vertice = int
    type Distance = float
    type Payload = 
        {
            Distance:Distance
            SubVertices: Vertice list
        }
    type DistanceMatrix = Matrix<Distance>
    type DistanceTotals = Distance array
    type DistanceSegment = WeightedSegment<Distance>
    type DistanceGraph = Graph<Vertice, Distance, Payload>

    let ConnectWithMeanAverageWeight (g:DistanceGraph) a b iid distance : DistanceGraph= 
        let meanWeight : Distance = distance / 2.0
        let na = FindNode g a
        let nb = FindNode g b
        let fPayload _ weight node =
            match node.Id with
            | id when id = iid -> 
                {
                    Distance=weight
                    SubVertices= na.Payload.SubVertices @ nb.Payload.SubVertices
                }  
            | _ -> node.Payload
        let fWeightAI _ = meanWeight - na.Payload.Distance
        let fWeightBI _ = meanWeight - nb.Payload.Distance
        g |> ConnectNodesViaNewInnerNode fPayload fWeightAI fWeightBI na.Id nb.Id iid 

    let ValidSegmentsAfterConnect n (g:DistanceGraph) (m:DistanceMatrix) (dss:DistanceSegment list) a b innerVertice : DistanceSegment list =
        let isNotConnected c = c <> a && c <> b
        let withoutConnected = dss |> List.filter (fun ((c,d),_) -> c |> isNotConnected && d |> isNotConnected)
        let ni = FindNode g innerVertice
        let newLegsWithInnerVertice = 
            DistinctVertices dss 
            |> List.filter (fun c -> c |> isNotConnected)
            |> List.map (fun c -> 
                let p = (c,innerVertice)
                match c with 
                | c when c < n ->  //c is leaf - shortcut
                    let distance = ni.Payload.SubVertices |> List.map (fun sv -> m.[c,sv]) |> List.average
                    p, distance
                | _ ->             //c is a cluster
                    let nc = FindNode g c
                    let distance = 
                        nc.Payload.SubVertices 
                        |> List.collect (fun csv -> ni.Payload.SubVertices |> List.map (fun nsv -> m.[csv,nsv]))
                        |> List.average
                    p, distance
                )
        withoutConnected@newLegsWithInnerVertice

    let CreateGraph n (m:DistanceMatrix) : DistanceGraph= 
        let rec inner g ss innerVertice  =
            match ss with
            | [] -> g  // End
            | _  ->    // Find closest point and connect there
                let ((a,b),wab) = ss |> List.minBy snd
                let g' = ConnectWithMeanAverageWeight g a b innerVertice wab
                let ss' = ValidSegmentsAfterConnect n g' m ss a b innerVertice
                printfn "[%i -> %i <- %i] : %A" a innerVertice b ss'
                inner g' ss' (innerVertice + 1)

        // initialize
        let initialGraph = InitializeGraph (fun v -> {Distance=0.0; SubVertices=[v]}) ((2 * n) - 1)
        let initialLegs = n |> Segments |> L1LessThanL2 |> MatrixWeighted m
        inner initialGraph initialLegs n

