System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Peptides.fs"
open Bioinformatics
open Peptides

//Cyclopeptide Sequencing Problem (for spectra with errors): 
//  Find a cyclic peptide having maximum score against an experimental spectrum.
//Input: A collection of integers Spectrum.
//Output: A cyclic peptide Peptide maximizing Score(Peptide, Spectrum) over all peptides Peptide with
//  mass equal to ParentMass(Spectrum).

let expand maxMass peptides = 
    match Seq.isEmpty peptides with
        | true  -> aminoAcids 
                |> Seq.map (fun aa -> (string aa, aminoAcidMass aa))
        | false -> aminoAcids 
                |> Seq.cartesian peptides
                |> Seq.map (fun ((p, m), aa) -> 
                    (sprintf "%s%c" p aa, m + aminoAcidMass aa))
                |> Seq.filter (fun (_, m) -> m <= maxMass)
let trim score n peptides =
    let scored = 
        peptides 
        |> Seq.map (fun (p,m) -> ((p,m), score p)) 
        |> Seq.sortByDescending snd
        |> Seq.cache
    let trimmed = 
        if Seq.length scored > n 
        then 
            let nthScore = scored |> Seq.item (n-1) |> snd
            scored |> Seq.takeWhile (fun (_,score) -> score >= nthScore)
        else 
            scored
    trimmed |> Seq.map fst    

let leaderboardCyclopeptideSequencing spectrum n =
    let parentMass = List.last spectrum
    printfn "Parent Mass %i" parentMass
    let score = Spectrum.scoreLinear spectrum 
    let rec inner peptides =
        printfn "Inner %A" (Seq.length peptides)
        let expanded = peptides |> expand parentMass |>Seq.cache
        printfn "  Max Mass %A" (expanded |> Seq.maxBy snd)
        let scored   = expanded |> Seq.filter (fun (_,m) -> m = parentMass)
        match Seq.isEmpty scored  with
        | true  -> 
            printfn "Mass not reached %A" (Seq.length expanded)
            expanded |> (trim score n) |> inner
        | false -> 
            //printfn "Mass matched: %A" scored
            scored 
            |> Seq.map (fun (p,m) -> ((p,m),score p))
    inner Seq.empty

let asAminoAcidMassString ((peptide, _), _) = 
    peptide 
    |> Seq.map (fun aa -> aa |> aminoAcidMass |> string) 
    |> String.concat "-"

let maxPeptide ps =
    ps
    |> Seq.fold (fun ((p',m'),s') ((p,m),s) ->
        if s > s' then ((p,m),s)
        else ((p',m'),s')
        ) (("",0),0)


//================================================
//Mass Spectrometry Meets Golf | Step 7
//================================================
[
    //"Mass Spectrometry Meets Golf-7-Sample";
    "Mass Spectrometry Meets Golf-7-Extra"
]
|>Execute.testOnDataSets
    (fun lines ->
        let n = int lines.[0]
        let spectrum = lines.[1] |> String.toInts |> Array.toList
        let expected = lines.[2]
        let actual = 
            leaderboardCyclopeptideSequencing spectrum n
            |> maxPeptide
            |> asAminoAcidMassString
        (actual, expected)
    )
"Mass Spectrometry Meets Golf-7"
|>Execute.onDataSet
    (fun lines ->
        let n = int lines.[0]
        let spectrum = lines.[1] |> String.toInts |> Array.toList
        leaderboardCyclopeptideSequencing spectrum n
        |> maxPeptide
        |> asAminoAcidMassString
    )
    (printfn "%s")

//================================================
//Mass Spectrometry Meets Golf | Step 3
//================================================
//"Mass Spectrometry Meets Golf-3"
//|>Execute.onDataSet
//    (fun lines ->
//        let n = int lines.[0]
//        let spectrum = lines.[1] |> String.toInts |> Array.toList
//        leaderboardCyclopeptideSequencing spectrum n
//        |> asAnswer
//    )
//    (printfn "%s")