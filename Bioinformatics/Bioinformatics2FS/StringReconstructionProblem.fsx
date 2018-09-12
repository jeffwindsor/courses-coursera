System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
#load "..\BioinformaticsFS\Debruijn.fs"
open Bioinformatics
open DeBruijn

let findPath k inputs = 
    inputs
    |> Seq.map (Kmers.toEdge k)
    |> toNodes
    |> toEulerianPath

//==========================================================
//Tests
"StringReconstructionProblem-Test"
|> Execute.testOnDataSet 
    (fun lines -> 
        let k = int lines.[1]
        let actual = lines.[2..] |> findPath k |> String.unwindowed
        (actual, lines.[0])
        ) 
    
//==========================================================

//From Euler's Theorem to an Algorithm for Finding Eulerian Cycles | Step 6
"StringReconstructionProblem"
|> Execute.onDataSet 
    (fun lines -> lines.[1..] |> findPath (int lines.[0])) 
    (String.unwindowed >> printfn "%s")
    