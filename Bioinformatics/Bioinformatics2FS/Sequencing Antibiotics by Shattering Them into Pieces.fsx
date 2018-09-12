System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Peptides.fs"
open Bioinformatics
open Peptides

let prefixMass f peptide =
    peptide |> Array.scan (fun totalMass aa -> totalMass + f aa) 0
//"NQEL" |> prefixMass |> Seq.iter (printfn "%i")

let linearSpectrum peptide = 
    let pa = peptide |> Seq.toArray
    Spectrum.linear (Array.length pa) (prefixMass aminoAcidMass pa)
let cyclicSpectrum peptide = 
    let pa = peptide |> Seq.toArray
    Spectrum.cyclic (Array.length pa) (prefixMass aminoAcidMass pa)

["TMIA";"MIAT";"TAIM";"TALM";"TLAM";"MAIT"]
|> Seq.map linearSpectrum
|> Seq.iter (printfn "%A")

["CET";"CTQ";"TCQ";"QCV";"ETC";"CTV"]
|> Seq.map linearSpectrum
|> Seq.iter (printfn "%A")

let q4_3 = 
    Spectrum.score 
        (Spectrum.toMassCountMap [0;57;71;71;71;104;131;202;202;202;256;333;333;403;404])
        (cyclicSpectrum "MAMA")

let q4_4 = 
    Spectrum.score 
        (Spectrum.toMassCountMap [0;97;97;129;194;196;226;226;244;258;323;323;452])
        (linearSpectrum "PEEP")

//Sequencing Antibiotics by Shattering Them into Pieces | Step 3
let cyclopeptideSequencing n = n * (n - 1)

//Sequencing Antibiotics by Shattering Them into Pieces | Step 4
["Sequencing Antibiotics by Shattering Them into Pieces-Sample";"Sequencing Antibiotics by Shattering Them into Pieces-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let expected = lines.[1]
        let peptide  = lines.[0]
        let actual   = 
            cyclicSpectrum peptide
            |> Seq.map (sprintf "%i")
            |> String.concat " "
        (actual, expected)
    )          

"Sequencing Antibiotics by Shattering Them into Pieces"
|> Execute.onDataSet
    (fun lines ->
        let peptide  = lines.[0]
        cyclicSpectrum peptide
        |> Seq.map (sprintf "%i")
        |> String.concat " "
    )
    (printfn "%s")