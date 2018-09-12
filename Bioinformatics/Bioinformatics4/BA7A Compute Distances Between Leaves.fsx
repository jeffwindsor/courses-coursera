System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "LeafDistances.fs"
open Bioinformatics
open Bioinformatics.LeafDistances

// **************************************
//Class 4 : Transforming Distance Matrices into Evolutionary Trees – Step 11
//BA7A Distance Between Leaves Problem
//    Compute the distances between leaves in a weighted tree.
//    Given: An integer n followed by the adjacency list of a weighted tree with n leaves.
//    Return: A space-separated n x n (di, j), where di, j is the length of the path between leaves i and j.
// **************************************
let problemNumber = "BA7A"
let analyse (lines: string[]) =
    let lineToEdge (line:string) : string Edge = 
        let values = line.Replace("->",":").Split(':')  // example 0->1:7
        (values.[0], values.[1], int values.[2])

    let leafs    = int lines.[0]
    let lastEdge = ((Array.length lines) - 1)
    let edges    = lines.[1..lastEdge] |> Seq.map lineToEdge
    lastEdge, LeafDistance edges

let test (lines: string[]) =
    let lastEdge, actual = analyse lines
    let expected = lines.[lastEdge + 1 ..]
    (actual, expected)

// **************************************
// RUNS
// **************************************
Solve Sample problemNumber test
Solve Extra problemNumber test
Solve Dataset problemNumber analyse
Solve Rosalind problemNumber analyse