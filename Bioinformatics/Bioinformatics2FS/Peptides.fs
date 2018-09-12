namespace Bioinformatics

module Peptides =
    let aminoAcids = "GASPVTCILNDKQEMHFRYW"

    let aminoAcidMass = function
        | 'G'  -> 57
        | 'A'  -> 71
        | 'S'  -> 87
        | 'P'  -> 97
        | 'V'  -> 99
        | 'T'  -> 101
        | 'C'  -> 103
        | 'I'  -> 113
        | 'L'  -> 113
        | 'N'  -> 114
        | 'D'  -> 115
        | 'K'  -> 128
        | 'Q'  -> 128
        | 'E'  -> 129
        | 'M'  -> 131
        | 'H'  -> 137
        | 'F'  -> 147
        | 'R'  -> 156
        | 'Y'  -> 163
        | 'W'  -> 186
        |  a   -> failwith (sprintf "Not known %c" a)

    let massToAminoAcid = function
        | 57 -> Some  "G"
        | 71 -> Some  "A" 
        | 87 -> Some  "S"
        | 97 -> Some  "P"
        | 99 -> Some  "V"
        | 101 -> Some  "T"
        | 103 -> Some  "C"
        | 113 -> Some  "IL"
        | 114 -> Some  "N"
        | 115 -> Some  "D"
        | 128 -> Some  "KQ"
        | 129 -> Some  "E"
        | 131 -> Some  "M"
        | 137 -> Some  "H"
        | 147 -> Some  "F"
        | 156 -> Some  "R"
        | 163 -> Some  "Y"
        | 186 -> Some  "W"
        |  _  -> None
    
    module Spectrum =
        let linear length (prefixMasses:int[]) =
            let values =
                [0..(length-1)]
                |> List.collect (fun i -> 
                    let imass = prefixMasses.[i]
                    [(i+1)..length]
                    |> List.map (fun j -> prefixMasses.[j] - imass)
                    )
                |> List.sort
            0::values
        //"LEQN" |> linearSpectrum |> Seq.iter (printfn "%i")

        let cyclic length (prefixMasses:int[]) =
            let peptideMass = prefixMasses.[length]
            let values =
                [0..(length-1)]
                |> List.collect (fun i -> 
                    let imass = prefixMasses.[i]
                    [(i+1)..length]
                    |> List.collect (fun j -> 
                        let jmass = prefixMasses.[j]
                        if i > 0 && j < length 
                        then [jmass - imass; peptideMass - (jmass - imass)]
                        else [jmass - imass]
                        )
                    )
                |> List.sort
            0::values
        let fromString s = s |> String.toInts |> Array.sort 
        let toMassCounts spectrum = 
            spectrum 
            |> Seq.groupBy (fun x -> x) 
            |> Seq.map (fun (key,xs) -> (key, Seq.length xs))
        let toMassCountMap spectrum = spectrum |> toMassCounts |> Map.ofSeq
        let score (referenceSpectrum:Map<int,int>) (peptideSpectrum:int seq) =
            peptideSpectrum
            |> toMassCounts  
            |> Seq.map (fun (key,count) -> 
                    match referenceSpectrum.ContainsKey key with
                    | true  -> min count referenceSpectrum.[key] 
                    | false -> 0
                )
            |> Seq.sum

//        let scoreCyclic spectrum = 
//            let map = toMassCountMap spectrum
//            peptide |> pepideToSpectrum 
//            score cyclicSpectrum map 
//        //11 = cyclicScore "NQEL" [0;99;113;114;128;227;257;299;355;356;370;371;484]
//
//        let scoreLinear spectrum = 
//            let map = toMassCountMap spectrum 
//            score linearSpectrum map
//        //8 = linearScore "NQEL" [0;99;113;114;128;227;257;299;355;356;370;371;484]

    let asAminoAcidMassString peptide = 
        peptide 
        |> Seq.map (fun aa -> aa |> aminoAcidMass |> string) 
        |> String.concat "-"
