System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "CircularChromosomes.fs"
open Bioinformatics
open CircularChromosomes
open CircularChromosomes.Cycles
    
//2-Break Distance Problem
//Find the 2-break distance between two genomes.
//Given: Two genomes with circular chromosomes on the same set of synteny blocks.
//Return: The 2-break distance between these two genomes.
let id = "BA6C"
let ``2-Break Distance Problem`` p q =
    let pg, qg = genomeFromInput p, genomeFromInput q
    let blockCount = genomeBlockCount pg  //either/or since they will be the same
    let cycleCount = mergeGenomesToColoredCycles pg qg 
                     |> coloredCycleToConnectedComponents 
                     |> Seq.length

    //Answer
    blockCount - cycleCount

// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``2-Break Distance Problem`` lines.[0] lines.[1]
let actualExpected (lines: string[]) = (actual lines, int lines.[2])
let print d = printfn "%i" d

// **************************************
// Runs
// **************************************
Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print