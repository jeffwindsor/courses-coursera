System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
#load "..\BioinformaticsFS\Debruijn.fs"
open Bioinformatics
open DeBruijn

let inputsToPairsEdges k (input:string) = 
    let splits = input.Split([|"|"|],System.StringSplitOptions.RemoveEmptyEntries)
    Kmers.toPairEdge k splits.[0] splits.[1]

let pairsToPathList k d (kmers: (string * string) list) = 
    let pre =  kmers |> List.take (k + d) |> List.map (fun (a,_) -> a)
    let post = kmers |> List.map (fun (_,b) -> b)
    List.append pre post
    
let stringReconstruction k d lines= 
    lines
    |> Seq.map (inputsToPairsEdges k)
    |> toNodes
    |> toEulerianPath 
    |> pairsToPathList k d
    |> String.unwindowed

//==========================================================
//Tests
"GenePairs-Test"
|> Execute.testOnDataSet
    (fun lines ->
        let expected = lines.[0]
        let ints = lines.[1] |> String.toInts
        let k, d = ints.[0], ints.[1]
        let actual = lines.[2..] |> stringReconstruction k d 
        (actual, expected)
    )
//==========================================================
    
//Assembling Genomes from Read-Pairs | Step 6
Execute.onDataSet 
    (fun lines -> 
        let ints = lines.[0] |> String.toInts
        let k, d = ints.[0], ints.[1]
        lines.[1..] |> stringReconstruction k d 
    )
    (printfn "%s")
    "GenePairs"

//Epilogue: Genome Assembly Faces Real Sequencing Data | Step 9
Execute.onDataSet 
    (fun lines -> lines |> stringReconstruction 120 1000)
    (printfn "%s")
    "GenePairs-FinalChallenge"



//"TAATGCCATGGGATGTT"
//|> String.windowed 8
//|> Seq.sort
//|> Seq.iter (fun s -> printf "(%s|%s) " s.[..2] s.[5..] )
