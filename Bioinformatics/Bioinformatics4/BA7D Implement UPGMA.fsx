System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
#load "WeightedAdjacenyGraph.fs"
#load "UPGMA.fs"
open Bioinformatics
open Bioinformatics.UPGMA

// **************************************
//BA7D - UPGMA Problem
//Construct the ultrametric tree resulting from UPGMA.
//Given: An integer n followed by a space-delimited n x n distance matrix.
//Return: An adjacency list for the ultrametric tree output by UPGMA. Weights should be accurate to three decimal places.
//Note on formatting: The adjacency list must have consecutive integer node labels starting from 0. The n leaves must be labeled 0, 1, ..., n-1 in order of their appearance in the distance matrix. Labels for internal nodes may be labeled in any order but must start from n and increase consecutively.
// **************************************
let id = "BA7D"
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
//Solve id Sample2 actual |> toAnswerFormat
//Solve id Extra withExpected ||> AreEqualListComparison 
//Solve id "BookStep5" withExpected ||> AreEqualListComparison
//Solve id Dataset actual |> print
Solve id Rosalind actual |> print
