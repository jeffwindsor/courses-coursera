System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
open Bioinformatics

//Answer
Execute.onDataSet
    (fun lines ->
        let genome = lines.[1]
        let k = int lines.[0]
        composition k genome
    )
    (Seq.iter (printfn "%s"))
    "Composition"

//Tests
let lines = "Composition-Tests" |> Files.toLines |> Seq.toArray
let k = int lines.[0]        
let genome = lines.[1]
let expected = lines.[2..] |> Array.sort
let actual = composition k genome |> Seq.sort |> Seq.toArray |> Array.sort

actual = expected