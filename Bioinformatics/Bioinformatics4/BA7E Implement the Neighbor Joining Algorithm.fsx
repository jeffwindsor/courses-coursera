System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
#load "WeightedAdjacenyGraph.fs"
#load "NeighborJoining.fs"
open Bioinformatics
open Bioinformatics.NeighborJoining

// **************************************
//Neighbor Joining Problem
//Construct the tree resulting from applying the neighbor-joining algorithm to a distance matrix.
//Given: An integer n, followed by a space-separated n x n distance matrix.
//Return: An adjacency list for the tree resulting from applying the neighbor-joining algorithm. Edge-weights should be accurate to two decimal places (they are provided to three decimal places in the sample output below).
//Note on formatting: The adjacency list must have consecutive integer node labels starting from 0. The n leaves must be labeled 0, 1, ..., n-1 in order of their appearance in the distance matrix. Labels for internal nodes may be labeled in any order but must start from n and increase consecutively.
// **************************************
let id = "BA7E"
let toAnswerFormat (g:DistanceGraph) =
    g |> List.collect (fun node -> 
        node.Edges 
        |> List.map (fun {Id=ve;Weight=w} -> sprintf "%i->%i:%.3f" node.Id ve (System.Math.Round(w,3)))
        )

let actual (lines: string[]) = 
    let n = int lines.[0]
    let m = Files.Lines.toFloats2D n lines.[1..n]
    CreateGraph n m

let withExpected (lines: string[]) = 
    let n = int lines.[0]
    let expected = lines.[n+1..] |> Array.toList
    (actual lines |> toAnswerFormat, expected)

let print (g:DistanceGraph) = 
    g |> toAnswerFormat
    |> Seq.iter (fun item -> printfn "%s" item)
// **************************************
// RUNS
// **************************************
// Solve id Sample withExpected ||> AreEqualListComparison
// Solve id "ExampleStep3" withExpected ||> AreEqualListComparison
// Solve id Extra withExpected ||> AreEqualListComparison 
// Solve id Dataset actual |> print
// Solve id Rosalind actual |> print