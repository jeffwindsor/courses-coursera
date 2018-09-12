System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "Support.fs"
#load "Bioinformatics.fs"
#load "Motifs.fs" 
open Bioinformatics
open Motifs

let rec firstHighestProbabilityMotifs toprofiles (motifs: string list) genomes = 
    match genomes with
    | [] -> motifs
    | genomeKmers::remainingGenomes ->
        let ps = toprofiles motifs
        let motif = Profiles.firstHighestProbability ps genomeKmers
        firstHighestProbabilityMotifs toprofiles (motif::motifs) remainingGenomes
    
let greedyMotifSearch' toprofiles k genomes =
    let head = genomes |> Seq.head |> toKmers k
    let kmers = 
        genomes 
        |> Seq.tail 
        |> Seq.map (fun g -> g |> toKmers k |> Seq.toList)
        |> Seq.toList

    head
    |> Seq.fold (fun (min, minmotifs) kmer -> 
            let motifs = firstHighestProbabilityMotifs toprofiles [kmer] kmers
            match Motifs.score motifs with
            | s when s < min -> 
                //printfn "%s => New Min: %i %s %A" kmer s (Motifs.consensus motifs) (motifs |> List.rev)
                (s, motifs)
            | s -> 
                //printfn "%s => Keep Min: %i %s %A" kmer s (Motifs.consensus motifs) (motifs |> List.rev)
                (min, minmotifs)
        ) (1000000, [])
    |> snd
    |> Seq.rev

let greedyMotifSearchNaive k genomes = greedyMotifSearch' toProfiles k genomes
let greedyMotifSearch k genomes = greedyMotifSearch' toProfilesLaplace k genomes


//Greedy Motif Search | Step 3
["GreedyMotifFinding-Sample"] //; "GreedyMotifFinding-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let k, t     = 
            let ints = lines.[1] |> String.toInts
            ints.[0], ints.[1]
        let genomes  = lines.[2..]

        let actual = 
            greedyMotifSearch k genomes
            |> Seq.sprints_str
        let expected = lines.[0]

        (actual, expected)
    )

"GreedyMotifFinding"
|> Execute.onDataSet
    (fun lines ->
        let k, t     = 
            let ints = lines.[0] |> String.toInts
            ints.[0], ints.[1]
        let genomes  = lines.[1..]

        greedyMotifSearch k genomes
    )
    (Seq.iter (printfn "%s"))
