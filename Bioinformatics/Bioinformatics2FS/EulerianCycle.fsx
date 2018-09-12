System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
#load "..\BioinformaticsFS\Debruijn.fs"
open Bioinformatics
open DeBruijn

let toAnswerFormat xs = xs |> String.concat "->" 

let inputToEdges (input:string) = 
    let splits = input.Split([|" -> "|],System.StringSplitOptions.RemoveEmptyEntries)
    splits.[1].Split(',')
    |> Array.map (fun target -> {NodeIn=splits.[0]; Label=input; NodeOut=target; })
let letinputsToEdges inputs = 
    inputs |> Seq.collect inputToEdges 
    

let findPath inputs = 
    letinputsToEdges inputs
    |> toNodes
    |> toEulerianPath

//==========================================================
//Tests
["EulerianCycle-Test";"EulerianPath-Test"]
|> Execute.testOnDataSets (fun lines -> (lines.[1..] |> findPath |> toAnswerFormat, lines.[0])) 
//==========================================================

//From Euler's Theorem to an Algorithm for Finding Eulerian Cycles | Step 2
Execute.onDataSet 
    (fun lines -> lines |> findPath) 
    (toAnswerFormat >> printfn "%s")
    "EulerianCycle"

//From Euler's Theorem to an Algorithm for Finding Eulerian Cycles | Step 5
Execute.onDataSet 
    (fun lines -> lines |> findPath) 
    (toAnswerFormat >> printfn "%s")
    "EulerianPath"