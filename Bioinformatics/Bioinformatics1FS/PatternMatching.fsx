System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Support.fs"
#load "Bioinformatics.fs"

open Bioinformatics

let patternMatchFunction f pattern genome = 
    genome 
    |> String.windowed (String.length pattern)
    |> Seq.mapi (fun i x -> if f x pattern then Some i else None)
    |> Seq.choose (fun x -> x)

let patternMatchExact pattern genome =
    patternMatchFunction (fun a b -> a = b) pattern genome

let patternCountExact pattern genome = 
    patternMatchExact pattern genome |> Seq.length     
    
let hammingWithin maxDistance a b = 
    (hammingDistance a b) <= maxDistance
    
let patternMatchHamming maxDistance pattern genome =
    patternMatchFunction (hammingWithin maxDistance) pattern genome

let patternCountHamming maxDistance pattern genome =
    patternMatchHamming maxDistance pattern genome 
    |> Seq.length

////CSGeneratingtheNeighborhoodofaString
//let lines = "CSGeneratingtheNeighborhoodofaString" |> Files.toLines |> Seq.toArray
//let pattern = lines.[0]
//let d = int lines.[1]
//
//neighbors d pattern
//|> Seq.iter (printfn "%s")

////Hamming Distance
//let lines = "HammingDistance" |> Files.toLines |> Seq.toArray
//hammingDistance lines.[0] lines.[1]
//|> printfn "%i"
//
////ApproximatePatternCount
//let lines = "ApproximatePatternCount" |> Files.toLines |> Seq.toArray
//let pattern = lines.[0]
//let genome  = lines.[1]
//let maxDistance = int lines.[2]
//
//patternCountHamming maxDistance pattern genome
//|> printfn "%i"

//// 1.2.0.5 - PAttern Matching
//let answer pattern genome = 
//    patternMatchExact pattern genome
//    |> Seq.sprints
//
//let testExtras = 
//    let lines = "PatternMatching-Extra" |> Files.toLines |> Seq.toArray
//    let pattern = lines.[0]
//    let genome = lines.[1]
//    let expected = lines.[2]
//    let actual = answer pattern genome
//    
//    printfn "%s" actual
//    printfn "%s" expected
//    actual.Trim() = expected
//
//
//let lines = "PatternMatching" |> Files.toLines |> Seq.toArray
//let pattern = lines.[0]
//let genome = lines.[1]
//answer pattern genome |> printfn "%s" 

//PATTERN COUNT
//let text, pattern =
//    let lines = "PatternCount" |> Files.toLines |> Seq.toArray
//    (lines.[0], lines.[1])
//patternCountExact pattern text
//|> printfn "Pattern Count > %i"

//APPROXIMATE PATTERN MATCHING
//let lines = "ApproximatePatternMatching" |> Files.toLines |> Seq.toArray
//let pattern = lines.[0]
//let genome  = lines.[1]
//let maxDistance = int lines.[2]
//
//patternMatchHamming maxDistance pattern genome
//|> Seq.printsn

