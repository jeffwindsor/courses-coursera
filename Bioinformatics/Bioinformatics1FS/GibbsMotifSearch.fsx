System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "Support.fs"
#load "Bioinformatics.fs" 
#load "Motifs.fs" 
open Bioinformatics
open Motifs

let rec randomizedMotifSearch motifs genomesAsKmers = 
    let profile = motifs |> toProfilesLaplace
    let motifs' = genomesAsKmers |> Profiles.toMotifs profile
    match score motifs, score motifs' with
    | s, s' when s' < s -> 
        //printfn " Smaller Score: %i %A -> %i %A" s (motifs |> List.rev) s' (motifs' |> List.rev)
        randomizedMotifSearch motifs' genomesAsKmers
    | s, s' -> 
        //printfn " Not Smaller Score: %i %A -> %i %A" s (motifs |> List.rev) s' (motifs' |> List.rev)
        (s, motifs)

let randomizedMotifSearchIterations n t genomesAsKmers =
    let initialMotifs genomeCount = 
        random genomeCount (genomesAsKmers |> List.head |> Array.length) genomesAsKmers

    seq {1..n}
    |> Seq.fold (fun (s,ms) _ ->
        match randomizedMotifSearch (initialMotifs t) genomesAsKmers with
        | (s',ms') when s' < s -> (s',ms')
        | _ -> (s,ms)
        ) (1000000, [])
    |> snd

let gibbsMotifSearch (rnd:System.Random) intialMotifs t n (genomesAsKmers: string[] list) =
    let remove i (xs: 'a list) =
        let x = xs.[i]
        xs |> List.filter (fun x' -> x<>x')
    let replace i x (xs: 'a list) =
        xs |> List.mapi (fun i' x' -> if i'=i then x else x')
    let randomKmer profiles kmers =
        let probabilities = 
            kmers |> Seq.map (fun kmer -> (Profiles.probability profiles kmer, kmer))
        let total = 
            probabilities |> Seq.map fst |> Seq.reduce (+)
        let r = rnd.NextDouble() * total
        let min, max, result =
            probabilities 
            |> Seq.scan (fun (_,max,_) (p,k) -> (max, max + p, k)) (0.0, 0.0,"")
            |> Seq.find (fun (min,max,_) -> min <= r && r < max)
        //printfn "%A : %f <= %f < %f : %f" result min r max total
        result

    List.init n (fun _ -> rnd.Next(0,t))
    |> Seq.fold (fun (s,(motifs:string list)) ti ->
        let profiles = motifs |> remove ti |> toProfilesLaplace
        let kmer = randomKmer profiles genomesAsKmers.[ti]
        let motifs'  = motifs |> replace ti kmer

        match score motifs' with
        | s' when s' < s -> 
            //printfn "LT:  %i %i %A -> %i %A" ti s motifs s' motifs'
            (s',motifs')
        | s' -> 
            //printfn "GTE: %i %i %A -> %i %A" ti s motifs s' motifs'
            (s,motifs)
        ) (score intialMotifs, intialMotifs)

let gibbsMotifSearchIterations k t n genomes =
    let genomesAsKmers = 
        genomes 
        |> Seq.map (fun g -> g |> toKmers k |> Seq.toArray)
        |> Seq.toList
    let rnd = System.Random()
    let randomMotifs genomeCount = 
        //random genomeCount (genomesAsKmers |> List.head |> Array.length) genomesAsKmers
        randomizedMotifSearchIterations 30 genomeCount genomesAsKmers

    seq {1..20}
    |> Seq.fold (fun (s,ms) i ->
        //printfn "%i" i
        match gibbsMotifSearch rnd (randomMotifs t) t n genomesAsKmers with
        | (s',ms') when s' < s -> 
            //printfn "%i) New: %i %A" i s' ms'
            (s',ms')
        | (s',ms') -> 
            //printfn "%i) No Change %i %A" i s' ms'
            (s,ms)
        ) (1000000, [])
    |> snd

//Randomized Motif Search | Step 5
//["GibbsMotifSearch-Sample"] 
["GibbsMotifSearch-Sample";"GibbsMotifSearch-Sample2"; "GibbsMotifSearch-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let k, t, n  = 
            let ints = lines.[1] |> String.toInts
            ints.[0], ints.[1], ints.[2]
        let genomes  = lines.[2..]

        let actual = 
            gibbsMotifSearchIterations k t n genomes 
            |> Seq.sprints_str
        let expected = lines.[0]

        (actual, expected)
    )

"GibbsMotifSearch"
|> Execute.onDataSet
    (fun lines ->
        let k, t , n    = 
            let ints = lines.[0] |> String.toInts
            ints.[0], ints.[1], ints.[1]
        let genomes  = lines.[1..]

        gibbsMotifSearchIterations k t n genomes
    )
    (Seq.iter (printfn "%s"))
