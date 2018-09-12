namespace Bioinformatics

module Motifs =
        let numberOfNucleotiees = 4
        let private toIndex = function
            | 'A' -> 0
            | 'C' -> 1
            | 'G' -> 2
            | 'T' -> 3
            | _ -> failwith "Not DNA Nucleotide"

        let private toNucleotide = function
            | 0 -> "A"
            | 1 -> "C"
            | 2 -> "G"
            | 3 -> "T"
            | _ -> failwith "Not DNA Nucleotide"

        module internal Counts =       
            type T = int array
            type Ts = T array  

            let newTs size = Array.init size (fun _ -> Array.create numberOfNucleotiees 0)

            let private addi (count:T) i = 
                count.[i] <- count.[i] + 1

            let private add (count:T) nucleotide = 
                let i = toIndex nucleotide
                addi count i

            let addAll counts genome = 
                Seq.iter2 add counts genome
                counts

            let applyLaplaceAdjustment counts =
                counts |> Seq.iter (fun c -> c |> Seq.iteri (fun i _ -> addi c i))
                counts

            let maxNucleotide (count:T) =
                count
                |> Seq.mapi (fun i c -> (toNucleotide i, c))
                |> Seq.maxBy snd
                |> fst

            //let scoreColumn (count:T) = (Seq.sum count) - (Seq.max count)
        
        module internal Profiles =
            type T = float array
            type Ts = T array

            let newTs length = Array.init length (fun _ -> Array.create numberOfNucleotiees 0.0)

            let private setCount denom (profile:T) (count:Counts.T) = 
                [0..3] |> Seq.iter (fun i -> profile.[i] <- (float count.[i]) / float denom)

            let setCounts denom (profiles:Ts) (counts:Counts.Ts) = 
                Seq.iter2 (setCount denom) profiles counts
                profiles

            let fromCounts motifs counts =
                let length = Array.length counts
                counts |> setCounts motifs (newTs length)                

            let probability profiles kmer =
                Seq.map2 (fun (p:T) n -> p.[toIndex n]) profiles kmer
                |> Seq.reduce (*)

            let firstHighestProbability profiles kmers =
                kmers
                |> Seq.fold (fun (maxkmer, maxp) kmer -> 
                    match probability profiles kmer with
                    | p when p > maxp -> (kmer, p )
                    | _ -> (maxkmer, maxp)
                    ) ("", -1000000.0)
                |> fst

            let entropyOfValue value = 
                if value < 0.0000001 then 0.0
                else value * log value

            let entropy (profile:T) =
                profile |> Seq.fold (fun acc p -> acc + entropyOfValue p ) 0.0
            
            let toMotifs profile genomes =
                genomes |> List.map (firstHighestProbability profile)

        let randoms n min max = 
            let rnd = System.Random()
            List.init n (fun _ -> rnd.Next(min,max))

        let random numberOfGenmoes numberOfKmersPer genomesAsKmers =
            let rs = randoms numberOfGenmoes 0 (numberOfKmersPer-1)
            List.map2 (fun i (kmers:string[]) -> kmers.[i]) rs genomesAsKmers

        let toCounts (motifs: string seq) =
            let k = motifs |> Seq.head |> Seq.length
            motifs |> Seq.fold Counts.addAll (Counts.newTs k)
        
        let toCountsLaplace (motifs: string seq) =
            motifs |> toCounts |> Counts.applyLaplaceAdjustment
            
        let toProfiles motifs = 
            motifs |> toCounts |> Profiles.fromCounts (motifs |> Seq.length)

        let toProfilesLaplace motifs = 
            motifs |> toCountsLaplace |> Profiles.fromCounts ((motifs |> Seq.length) + numberOfNucleotiees)

        let consensus motifs = 
            toCounts motifs 
            |> Seq.map Counts.maxNucleotide
            |> String.concat ""

        let entropy motifs =
            toProfiles motifs 
            |> Seq.map Profiles.entropy
            |> Seq.sum

        let score motifs = 
            let d pattern (motifs: string seq) = 
                motifs 
                |> Seq.map (hammingDistance pattern)
                |> Seq.sum
            d (consensus motifs) motifs

        let find k genomes =
            let d' pattern kmers = 
                kmers
                |> Seq.map (hammingDistance pattern)
                |> Seq.min
    
            let d pattern genomesAsKmers =
                genomesAsKmers 
                |> Seq.map (d' pattern)
                |> Seq.sum

            //Not could be run twice???
            let genomesAsKmers = 
                genomes 
                |> Seq.map (fun g -> String.windowed k g |> Seq.toList)
        
            let patterns = genomesAsKmers |> Seq.collect (fun a -> a) |> Seq.distinct 

            patterns
            |> Seq.fold (fun ((min:int),(ps:string list)) pattern ->  
                    let x = d pattern genomesAsKmers
                    //printfn "%s -> %i" pattern x
                    match x with
                    | min' when min' = min -> (min', pattern::ps)
                    | min' when min' < min -> (min', [pattern])
                    | _    -> (min, ps))
                    (2147483647, [])
            |> snd
