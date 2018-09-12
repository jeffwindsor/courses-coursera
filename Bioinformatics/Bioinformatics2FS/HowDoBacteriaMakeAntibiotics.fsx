System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Dna.fs"
#load "..\BioinformaticsFS\Rna.fs"
open Bioinformatics

let aminoAcidsTranslate rnas = 
    rnas
    |> Rna.toCodons
    |> Seq.map Rna.Codons.toAminoAcid

let findIndexes predicate xs =
    xs 
    |> Seq.mapi (fun i x -> (i,x)) 
    |> Seq.fold (fun acc (i,x) -> 
            match predicate x with
            | true  -> i::acc
            | false -> acc 
        ) []

//How Do Bacteria Make Antibiotics? | Step 5
let proteinsTranslateFirst rnas = 
    rnas |> aminoAcidsTranslate
    |> Seq.takeWhile (fun aa -> aa <> Rna.Codons.stop)
    |> String.concat ""

["CCCCGUACGGAGAUGAAA";"CCACGUACUGAAAUUAAC";"CCCAGUACCGAGAUGAAU";"CCGAGGACCGAAAUCAAC"]
|> Seq.map (aminoAcidsTranslate >> String.concat "")
|> Seq.iter (printfn "%s")


["HowDoBacteriaMakeAntibiotics-5-Sample";"HowDoBacteriaMakeAntibiotics-5-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let rnas     = lines.[0]
        let expected = lines.[1]
        let actual   = proteinsTranslateFirst rnas
        (actual, expected)
    )
"HowDoBacteriaMakeAntibiotics-5"
|> Execute.onDataSet
    (fun lines ->
        let rnas     = lines.[0]
        proteinsTranslateFirst rnas
    )
    (printfn "%s")

//How Do Bacteria Make Antibiotics? | Step 8
let peptideEncoding proteins dna =
    let encodings proteins (codons:string[]) =
        //printfn "Codons: %A" codons
        let aa = codons |> Dna.Codons.toAminoAcids |> (String.concat "")
        //printfn "AminoAcids: %s" aa
        proteins
        |> Seq.map (fun p -> 
            let lp = (String.length p)
            (p, lp, String.windowed lp aa))
        |> Seq.map (fun (p, lp, xs) -> 
            let indexes = findIndexes (fun x -> x = p) xs
            //printfn "%s %i %A %s" p lp indexes aa
            (lp, indexes))
        |> Seq.collect (fun (l,idxs) -> 
            let matches = idxs |> Seq.map (fun i -> codons.[i..(i+l-1)])
            //printfn "  %A" matches
            matches |> Seq.map (String.concat "")
            )
    let scenarioEncodings proteins scenarios = 
        scenarios
        |> Seq.map Dna.toCodons
        |> Seq.collect (encodings proteins)

    let forwards = dna |> Dna.toReadingScenarios |> scenarioEncodings proteins
    let reverses = 
        dna 
        |> Dna.toReverseComplement
        |> Dna.toReadingScenarios 
        |> scenarioEncodings proteins 
        |> Seq.map Dna.toReverseComplement

    Seq.append forwards reverses

["HowDoBacteriaMakeAntibiotics-8-Sample"; "HowDoBacteriaMakeAntibiotics-8-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let expected = lines.[0] |> String.split |> Array.sort 

        let rnas     = lines.[1]
        let proteins = lines.[2..]
        let actual   = 
            peptideEncoding proteins rnas
            |> Seq.toArray
            |> Array.sort
        (actual, expected)
    )
"HowDoBacteriaMakeAntibiotics-8"
|> Execute.onDataSet
    (fun lines ->
        let rnas     = lines.[0]
        let proteins = lines.[1..]
        peptideEncoding proteins rnas
    )
    (Seq.iter (printfn "%s"))
    
//How Do Bacteria Make Antibiotics? | Step 9
"B_brevis"
|> Execute.onDataSet
    (fun lines ->
        let dnas     = lines.[0..]
        let proteins = ["VKLFPWFNQY"]
        dnas |> Seq.collect (peptideEncoding proteins)
    )
    (Seq.iter (printfn "%s"))