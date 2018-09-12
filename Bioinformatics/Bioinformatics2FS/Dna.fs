namespace Bioinformatics
        
module Dna =
    module Codons =
        let stop = "*"
        let unknown = "_"
        let toAminoAcid = function
        | "GCT" | "GCC" 
        | "GCG" | "GCA" -> "A"
        | "TGT" | "TGC" -> "C"
        | "GAT" | "GAC" -> "D"
        | "GAA" | "GAG" -> "E"
        | "TTT" | "TTC" -> "F"
        | "GGT" | "GGC" 
        | "GGA" | "GGG" -> "G"
        | "CAT" | "CAC" -> "H"
        | "ATT" | "ATC" 
        | "ATA"         -> "I"
        | "AAA" | "AAG" -> "K"
        | "CTT" | "CTC" 
        | "TTG" | "CTG" 
        | "TTA" | "CTA" -> "L"
        | "ATG"         -> "M"     //Also start codon
        | "AAT" | "AAC" -> "N"
        | "CCT" | "CCC" 
        | "CCA" | "CCG" -> "P"
        | "CAA" | "CAG" -> "Q"
        | "CGT" | "CGC" 
        | "AGA" | "CGG" 
        | "AGG" | "CGA" -> "R"
        | "TCT" | "TCC" 
        | "TCA" | "TCG" 
        | "AGT" | "AGC" -> "S"
        | "ACT" | "ACC" 
        | "ACA" | "ACG" -> "T"
        | "GTT" | "GTC" 
        | "GTA" | "GTG" -> "V"
        | "TGG"         -> "W"
        | "TAT" | "TAC" -> "Y"
        | "TAA" | "TAG" 
        | "TGA"         -> stop
        | _             -> unknown

        let toAminoAcids codons = 
            codons |> Seq.map toAminoAcid
    
    let toCodons dna = 
        dna |> Seq.toArray |> Array.chunkBySize 3 |> Array.map Char.toString

    let private toComplementNucleotide = function
        | 'A' -> "T"
        | 'C' -> "G" 
        | 'G' -> "C"
        | 'T' -> "A"
        |  _  -> failwith "Not a DNA Nucleotide"

    let toReverseComplement dnas = 
        dnas |> Seq.map toComplementNucleotide |> Seq.rev |> String.concat ""
        
    let toReadingScenarios xs = seq { for k in 0..2 do yield xs |> Seq.toArray |> Array.skip k }
    let toReadingFrames genome = 
        Seq.append (genome |> toReadingScenarios) (genome |> toReverseComplement |> toReadingScenarios)