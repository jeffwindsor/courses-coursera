System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
open Bioinformatics
open Bioinformatics.Matrices

// **************************************
//IV- Toward An Algorithm for Distance-Based Phylogeny Construction – Step 11
//Limb Length Problem
//  Find the limb length for a leaf in a tree.
//  Given: An integer n, followed by an integer j between 0 and n - 1, followed by a space-separated additive distance matrix D (whose elements are integers).
//  Return: The limb length of the leaf in Tree(D) corresponding to row j of this distance matrix (use 0-based indexing).
// **************************************
//Assumes distances are a nxn metric space
let limbLength n (distances: int Matrix) j =
    let limbLength (i,k) = (distances.[i,j] + distances.[j,k] - distances.[i,k]) / 2
    n |> Segments |> L1LessThanL2 |> WithoutIndex j
    |> List.map limbLength
    |> List.min

// **************************************
// INPUT - OUTPUT
// **************************************
let id = "BA7B"
let actual (lines: string[]) = 
    let n = int lines.[0]
    let leaf = (int lines.[1])
    let grid = Files.Lines.toInts2D n lines.[2..(n+1)]
    limbLength n grid leaf

let actualExpected (lines: string[]) = 
    let n = int lines.[0]
    (actual lines, int lines.[n+2])
let print d = printfn "%i" d

// **************************************
// Runs
// **************************************
Solve id "Sample2" actual |> print
Solve id Sample actual |> print
//Problems.extra id actualExpected
//Problems.dataset id actual  |> print
//Problems.rosalind id actual |> print
