System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "CircularChromosomes.fs"
open Bioinformatics
open CircularChromosomes

//Number of Breakpoints Problem
//Find the number of breakpoints in a permutation.
//Given: A signed permutation P.
//Return: The number of breakpoints in P.

let breakpoints (Chromosome bs) =
    //cap the ends
    Array.append [| Block 0 |] (Array.append bs [| Array.last bs |> value |> (+) 1 |> Block |])
    // count break points
    |> Seq.pairwise
    |> Seq.filter (fun (Block a, Block b) -> b - a <> 1)
    |> Seq.length


let id = "BA6B"
let ``Number of Breakpoints Problem`` genomeInput =
    genomeInput
    |> genomeFromInput
    |> chromosomes
    |> Seq.head 
    |> breakpoints 

// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``Number of Breakpoints Problem`` lines.[0] 
let actualExpected (lines: string[]) = (actual lines, int lines.[1])
let print count = (printfn "%i" count)

// **************************************
// Runs
// **************************************
//``Number of Breakpoints Problem`` "(+10 +6 -8 -7 +17 -20 +18 +19 -5 -16 -11 -4 -3 -2 +13 +14 -1 +9 -12 +15)"

Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print