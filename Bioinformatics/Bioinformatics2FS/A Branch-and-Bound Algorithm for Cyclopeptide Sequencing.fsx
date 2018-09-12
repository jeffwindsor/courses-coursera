System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Peptides.fs"
open Bioinformatics
open Peptides

//A Branch-and-Bound Algorithm for Cyclopeptide Sequencing | Step 2
//let cqa = [0;71;101;113;131;184;202;214;232;285;303;315;345;416]
//["TAIM";"MAIT";"MIAT";"MTAI";"TLAM";"MTAL"]
//|> Seq.iter (fun p -> printfn "%s -> %b" p (cqa = cyclicSpectrum p))
//
//let lqa = [0;71;99;101;103;128;129;199;200;204;227;230;231;298;303;328;330;332;333] |> Set.ofList
//["TVQ";"AVQ";"ETC";"CTQ";"CTV";"CET"]
//|> Seq.iter (fun p ->
//     let ls = linearSpectrum p
//     let answer = ls |> Seq.forall lqa.Contains
//     printfn "%s -> %b : %A" p answer ls)

//A Branch-and-Bound Algorithm for Cyclopeptide Sequencing | Step 5
let cyclopeptideSequencing spectrumList =   
    let expand validAminoAcids xs = 
        match Seq.isEmpty xs with
        | true  -> validAminoAcids 
                |> Seq.map (fun aa -> 
                    let mass = aminoAcidMass aa
                    (mass, string aa))
        | false -> validAminoAcids 
                |> Seq.cartesian xs
                |> Seq.map (fun ((mass, peptide), aa) -> 
                    (mass + aminoAcidMass aa, sprintf "%s%c" peptide aa))
    let parentMass = List.last spectrumList
    let spectrumSet = Set.ofList spectrumList
    let validAminoAcids = spectrumList |> List.choose massToAminoAcid |> List.distinct |> String.concat ""
    let filterToWithinSpectrum pairs = 
        pairs |> Seq.filter (fun (mass, _) -> mass <= parentMass && Set.contains mass spectrumSet)
    let isSpectrumMatch peptide =
        let pc = cyclicSpectrum peptide
        //printfn "%s -> %A" peptide pc
        let result = (pc = spectrumList)
        result 

    let rec inner pairs = 
        let expanded = pairs |> expand validAminoAcids |> filterToWithinSpectrum
        let valid    = expanded |> Seq.filter (fun (m,p) -> m = parentMass && isSpectrumMatch p) 
        match Seq.isEmpty valid  with
        | true  -> inner expanded
        | false -> expanded
    inner Seq.empty

let reSort xs = xs |> Seq.sort |> String.concat " "
let asAnswer xs = 
    xs
    |> Seq.map (fun (_,p) -> 
        p 
        |> Seq.map (fun aa -> aa |> aminoAcidMass |> string) 
        |> String.concat "-")
    |> Seq.distinct


[
 "Branch-and-Bound Algorithm for Cyclopeptide Sequencing-Sample";
 "Branch-and-Bound Algorithm for Cyclopeptide Sequencing-Extra"
]
|> Execute.testOnDataSets
    (fun lines ->        
        let expected  = lines.[1] |> String.split |> reSort
        let spectrum  = lines.[0] |> String.toInts |> List.ofArray
        let actual    = cyclopeptideSequencing spectrum |> asAnswer |> reSort
        (actual, expected)
    )  


"Branch-and-Bound Algorithm for Cyclopeptide Sequencing"
|> Execute.onDataSet
    (fun lines ->
        let spectrum  = lines.[0] |> String.toInts |> List.ofArray
        cyclopeptideSequencing spectrum |> asAnswer |> reSort
    ) 
    (printfn "%s")