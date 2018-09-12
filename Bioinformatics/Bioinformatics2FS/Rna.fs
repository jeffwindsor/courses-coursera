namespace Bioinformatics

module Rna =
    module Codons =
        let stop = "*"
        let unknown = "_"
        let toAminoAcid = function
        | "GCU" | "GCC" 
        | "GCG" | "GCA" -> "A"
        | "UGU" | "UGC" -> "C"
        | "GAU" | "GAC" -> "D"
        | "GAA" | "GAG" -> "E"
        | "UUU" | "UUC" -> "F"
        | "GGU" | "GGC" 
        | "GGA" | "GGG" -> "G"
        | "CAU" | "CAC" -> "H"
        | "AUU" | "AUC" 
        | "AUA"         -> "I"
        | "AAA" | "AAG" -> "K"
        | "CUU" | "CUC" 
        | "UUG" | "CUG" 
        | "UUA" | "CUA" -> "L"
        | "AUG"         -> "M"     //Also start codon
        | "AAU" | "AAC" -> "N"
        | "CCU" | "CCC" 
        | "CCA" | "CCG" -> "P"
        | "CAA" | "CAG" -> "Q"
        | "CGU" | "CGC" 
        | "AGA" | "CGG" 
        | "AGG" | "CGA" -> "R"
        | "UCU" | "UCC" 
        | "UCA" | "UCG" 
        | "AGU" | "AGC" -> "S"
        | "ACU" | "ACC" 
        | "ACA" | "ACG" -> "T"
        | "GUU" | "GUC" 
        | "GUA" | "GUG" -> "V"
        | "UGG"         -> "W"
        | "UAU" | "UAC" -> "Y"
        | "UAA" | "UAG" 
        | "UGA"         -> stop
        | _             -> unknown

        let toAminoAcids codons = 
            codons |> Seq.map toAminoAcid

    let toCodons rna = 
        rna |> Seq.toArray |> Array.chunkBySize 3 |> Array.map Char.toString
//
//    let toAminoAcids rna = 
//        rna |> toCodons |> Seq.map Codons.toAminoAcid
//
//    let toAminoAcidsForAllReadingFrames rna =
//        rna |> toReadingFrames |> Seq.map toAminoAcids
