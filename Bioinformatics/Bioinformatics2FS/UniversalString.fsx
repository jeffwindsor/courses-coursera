//From Euler's Theorem to an Algorithm for Finding Eulerian Cycles | Step 6
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
#load "..\BioinformaticsFS\Debruijn.fs"
open Bioinformatics
open DeBruijn


let universalKmers alpha k =
    let rec inner = function
        | 1 -> alpha
        | n -> 
            Seq.cartesian alpha (inner (n-1))
            |> Seq.map (fun (a,b) -> a + b)
    inner k
let univesalDnaKmers k = universalKmers dnaNucleotides k

let universalKmers alpha k =
    let rec inner = function
        | 1 -> alpha
        | n -> 
            Seq.cartesian alpha (inner (n-1))
            |> Seq.map (fun (a,b) -> a + b)
    inner k
let universalBinaryKmers k = universalKmers (["0";"1"] |> List.toSeq) k

let toAnswerFormat (kmers: string list) = 
    let head = List.head kmers
    let i = (String.length head) - 1
    let tail =
        kmers
        |> List.tail 
        |> List.fold (fun acc kmer -> kmer.[i]::acc) []
        |> List.rev 
        |> Char.toString
    head + tail

let universalString k = 
    universalBinaryKmers k
    |> kmersToIsolatedEdges k
    |> toEulerianPath
    |> toAnswerFormat

//Extra Dataset
let test0 =
    let k = 4
    let actual = universalString k
    let expected = "0000110010111101"
    let result   = actual = expected
    let title    = "Sample"
    if result 
    then printfn "%s -> %b" title result
    else 
        printfn "%s -> %b" title result
        printfn "  %A" actual 
        printfn "  %A" expected

//Answer
universalString 3
|> printfn "%s"

//universalBinaryKmers 3
//|> kmersToIsolatedEdges 3
//|> toEulerianPath
//|> Seq.iter (printfn "%s")

