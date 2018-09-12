System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Peptides.fs"
open Bioinformatics
open Peptides

let convolution' spectrum = 
    spectrum
    |> Seq.cartesianForwardOnly spectrum
    |> Seq.map    (fun (x,y) ->  x - y)
    |> Seq.filter (fun x -> x > 0)

let counts spectrum = 
    spectrum
    |> convolution'
    |> Seq.filter  (fun x -> 57 <= x && x <= 200)  //only values in aminoacid mass range
    |> Seq.countBy (fun x -> x)
    |> Seq.sortByDescending snd    

let convolution'' top spectrum=
    let pairs = counts spectrum
    //Take up to top element, including any ties
    let nthCount = pairs |> Seq.item top |> snd
    pairs 
    |> Seq.takeWhile (fun (_,count) -> count >= nthCount)
    |> Seq.map fst
    |> Seq.sort

counts [0;86;160;234;308;320;382]
|> Seq.map (printfn "%A")
//convolution'' 1 [57;57;71;99;129;137;170;186;194;208;228;265;285;299;307;323;356;364;394;422;493]
//|> Seq.iter (printfn "%A")
//let asAminoAcidMassString ((peptide, _), _) = 
//    Peptides.asAminoAcidMassString peptide

let convolutionCyclopeptideSequencing n m spectrum =
    let validmasses = convolution'' m spectrum |> Seq.cache
    let referenceSpectrumCountMap = spectrum |> Spectrum.toMassCountMap
    let score masses =
        let spectrum = 
            let ms = masses |> Array.ofList
            ms
            |> Array.scan (fun totalMass mass -> totalMass + mass) 0
            |> Spectrum.linear (Array.length ms) 
        Spectrum.score referenceSpectrumCountMap spectrum
    let expand validMasses maxMass xs = 
        match Seq.isEmpty xs with
            | true  -> validMasses 
                    |> Seq.map (fun mass -> ([mass], mass))
            | false -> validMasses 
                    |> Seq.cartesian xs
                    |> Seq.map (fun ((ms, totalMass), mass) -> 
                        (mass::ms, totalMass + mass))
                    |> Seq.filter (fun (_, totalMass) -> totalMass <= maxMass)
    let scores expanded = 
        expanded 
        |> Seq.map (fun (masses,total) -> ((masses,total), score masses))  //score failing with just masses, use mass list/something instread of string
        |> Seq.sortByDescending snd
        |> Seq.cache

    let trim n scored =
        match Seq.length scored with 
        | ls when ls > n ->
            let nthScore = scored |> Seq.item n |> snd
            scored 
            |> Seq.takeWhile (fun (_,score) -> score >= nthScore)
        | _ ->
            scored

    let parentMass = List.last spectrum
    printfn "Parent Mass %i >> %A" parentMass validmasses
    let rec inner atMass xs =
        printfn "Inner %A" (Seq.length xs)
        let expanded = xs |> expand validmasses parentMass |> Seq.cache
        match Seq.isEmpty expanded with
        | true  -> 
            printfn "expanded empty -> complete"
            atMass 
            |> Seq.map (fun (ms,total) -> (List.rev ms, total))
            |> scores 
            |> trim 1
        | false -> 
            printfn "  Max Mass %A" (expanded |> Seq.maxBy snd)
            let atMass' = atMass @ (expanded |> Seq.filter (fun (_,m) -> m = parentMass) |> Seq.toList)
            printfn "recurse %A" (Seq.length expanded)
            expanded |> scores |> trim n |> Seq.map fst |> inner atMass'
            
    inner List.empty Seq.empty

// EEFNEIDNKPYM
// IDNKPYMEEFGGE
// LDNKPYMEEFGGE
// IDNQPYMEEFGGE
// LDNQP YMEEF GGE
//[113;115;114;128;97;163;131;129;129;147;57;57;129]
//|> Seq.choose massToAminoAcid
//|> Seq.iter (printfn "%A")

// The Spectral Convolution Saves the Day | Step 7
[
 "The Spectral Convolution Saves the Day-7-Sample";
 "The Spectral Convolution Saves the Day-7-Extra"
]
|> Execute.testOnDataSets
    (fun lines ->        
        let expected  = lines.[3] 
        let m = int lines.[0]
        let n = int lines.[1]
        let spectrum = lines.[2] |> Spectrum.fromString |> Seq.toList
        let actual = 
            spectrum
            |> convolutionCyclopeptideSequencing n m 
            |> Seq.head
            |> fun ((masses,_),_) -> masses |> Seq.map string |> String.concat "-"
        (actual, expected)
    )  
"The Spectral Convolution Saves the Day-7"
|> Execute.onDataSet
    (fun lines ->
        let m = int lines.[0]
        let n = int lines.[1]
        let spectrum = lines.[2] |> Spectrum.fromString |> Seq.toList
        spectrum
        |> convolutionCyclopeptideSequencing n m 
        |> Seq.head
        |> fun ((masses,_),_) -> masses |> Seq.map string |> String.concat "-"
    ) 
    (printfn "%s")

// The Spectral Convolution Saves the Day | Step 4
[
 "The Spectral Convolution Saves the Day-4-Sample";
 "The Spectral Convolution Saves the Day-4-Extra"
]
|> Execute.testOnDataSets
    (fun lines ->        
        let expected  = lines.[1] |> String.toInts |> Seq.sort |> Seq.map string |> String.concat " "
        let actual    = 
            lines.[0] 
            |> Spectrum.fromString
            |> convolution' 
            |> Seq.sort |> Seq.map string |> String.concat " "
        (actual, expected)
    )  


"The Spectral Convolution Saves the Day-4"
|> Execute.onDataSet
    (fun lines ->
            lines.[0] |> String.toInts |> Seq.sort
            |> convolution' 
            |> Seq.sort |> Seq.map string |> String.concat " "
    ) 
    (printfn "%s")