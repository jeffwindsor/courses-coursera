System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
#load "WeightedAdjacenyGraph.fs"
#load "UPGMA.fs"
#load "NeighborJoining.fs"
open Bioinformatics
open Bioinformatics.Matrices

type DistanceMatrix = Matrix<float>
type DistanceSegments = WeightedSegment<float> list

let inline LeastSquaresErrorsDiscrepancy (wss:WeightedSegment<'a> list) (m:Matrix<'a>) =
    let leastSquares t d = (t - d) ** 2.0
    wss 
    |> List.map(fun ((i,j),w) -> leastSquares w m.[i,j])
    |> List.sum

//Question 2
let q2m : DistanceMatrix = 
    [|
        "0 13 16 10"
        "13 0 21 15"
        "16 21 0 18"
        "10 15 18 0"
    |]
    |> Files.Lines.toFloats2D 4

let q2T : DistanceSegments = 
    [
        ((0,1),14.0)
        ((0,2),17.0)
        ((0,3),11.0)
        ((1,2),21.0)
        ((1,3),15.0)
        ((2,3),18.0)
    ]
let q2 = LeastSquaresErrorsDiscrepancy q2T q2m


let total graphSize (ds:DistanceMatrix) allVertices = TotalsBy graphSize (fun i j -> 
    if ContainsBoth allVertices i j then ds.[i,j] else NeighborJoining.EmptyDistance)

//Question 3
//let q3m : DistanceMatrix = 
//    [|
//        "0 20 9 11"
//        "20 0 17 11"
//        "9 17 0 8"
//        "11 11 8 0"
//    |]
//    |> Files.Lines.toFloats2D 4
//let q3n = 4
//
//let g' = UPGMA.ConnectWithMeanAverageWeight g a b innerVertice wab
//let ss' = UPGMA.ValidSegmentsAfterConnect n g' m ss a b innerVertice
//
//let q3 = UPGMA.CreateGraph q3n q3m

//Question 4
let q4m : DistanceMatrix = 
    [|
        "0 20 9 11"
        "20 0 17 11"
        "9 17 0 8"
        "11 11 8 0"
    |]
    |> Files.Lines.toFloats2D 4
let q4n = 4
let q4i = 2
let q4j = 3
let q4validIndexes = [0..(q4n - 1)]
let q4tds = total ((q4n * 2) - 1) q4m (Set.ofList q4validIndexes)
let q4 = NeighborJoining.NormalizedDistance q4n q4tds q4i q4j q4m.[q4i,q4j]

//Question 5
let q5m : DistanceMatrix = 
    [|
        "0 14 17 17"
        "14 0 7 13"
        "17 7 0 16"
        "17 13 16 0"
    |]
    |> Files.Lines.toFloats2D 4

let q5n = 4
let q5validIndexes = [0..(q5n - 1)]
let q5tds = total ((q5n * 2) - 1) q5m (Set.ofList q5validIndexes)
let (q5vi,q5vj) = NeighborJoining.ClosestSegment q5n q5m q5tds q5validIndexes
let q5Dik, q5Djk = NeighborJoining.LimbLength q5tds q5n q5vi q5vj q5m.[q5vi,q5vj]
let q5 = q5Dik


printfn "******************************************************"
printfn "Question 2 : %.3f" q2
printfn "Question 4 : %.3f" q4
printfn "Question 5 : %.3f" q5