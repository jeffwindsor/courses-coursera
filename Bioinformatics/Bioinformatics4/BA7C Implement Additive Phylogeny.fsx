System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
#load "AdditivePhylogeny.fs"
open Bioinformatics
open Bioinformatics.AdditivePhylogeny

// **************************************
//IV - Additive Phylogeny – Step 6
//Additive Phylogeny Problem
//  Construct the simple tree fitting an additive matrix.
//  Given: n and a tab-delimited n x n additive matrix.
//  Return: A weighted adjacency list for the simple tree fitting this matrix.
//  Note on formatting: The adjacency list must have consecutive integer node labels starting from 0. 
//      The n leaves must be labeled 0, 1, ..., n-1 in order of their appearance in the distance matrix. 
//      Labels for internal nodes may be labeled in any order but must start from n and increase consecutively.
//
//Examples
//  See 5.3.1 Exact Reconstruction of Additive Trees in [http://lectures.molgen.mpg.de/Algorithmische_Bioinformatik_WS0405/phylogeny_script.pdf]
// **************************************
let id = "BA7C"
let actual (lines: string[]) = 
    let n = int lines.[0]
    createGraph n (Files.Lines.toInts2D n lines.[1..n])
    |> toAdjacencies
    |> List.map (fun (a,b,w) -> sprintf "%i->%i:%i" a b w)

let withExpected (lines: string[]) = 
    let n = int lines.[0]
    (actual lines, lines.[n+1..] |> Array.toList)

let toAnswer lines = Seq.iter (fun line -> printfn "%s" line) lines

// **************************************
// Runs
// **************************************
Solve id Sample withExpected ||> AreEqualListComparison
//Solve id Extra withExpected  ||> AreEqualListComparison
//Solve id Dataset actual |> toAnswer
//Solve id Rosalind actual |> toAnswer

