System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Support.fs"
#load "Bioinformatics.fs" 
#load "Motifs.fs" 
open Bioinformatics
open Motifs

let randomizedMotifSearch k t genomes =
    let genomesAsKmers = 
        genomes 
        |> Seq.map (fun g -> g |> toKmers k |> Seq.toArray)
        |> Seq.toList
    let numberOfKmersPer = genomesAsKmers |> List.head |> Array.length
    let randonMotifs = random t numberOfKmersPer genomesAsKmers

    let rec inner motifs = 
        // PROFILE LAPLACE current motifs
        let profile = motifs |> toProfilesLaplace
//        printfn "Motifs: %A" motifs
//        printfn "Profile: %A" profile 
        // CREATE new motifs from PROFILE + GENOMES
        let motifs' = genomesAsKmers |> Profiles.toMotifs profile
        // IF SCORE(NEW) < SCORE(current) THEN rec on new ELSE RETURN current
        match score motifs, score motifs' with
        | s, s' when s' < s -> 
            //printfn " Smaller Score: %i %A -> %i %A" s (motifs |> List.rev) s' (motifs' |> List.rev)
            inner motifs'
        | s, s' -> 
            //printfn " Not Smaller Score: %i %A -> %i %A" s (motifs |> List.rev) s' (motifs' |> List.rev)
            (s, motifs)

    inner randonMotifs

let randomizedMotifSearchIterations n k t genomes =
    seq {1..n}
    |> Seq.fold (fun (s,ms) _ ->
        match randomizedMotifSearch k t genomes with
        | (s',ms') when s' < s -> (s',ms')
        | _ -> (s,ms)
        ) (1000000, [])
    |> snd

//Randomized Motif Search | Step 5
["RandomMotifSearch-Sample";"RandomMotifSearch-D1"; "RandomMotifSearch-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let k, t  = 
            let ints = lines.[1] |> String.toInts
            ints.[0], ints.[1]
        let genomes  = lines.[2..]

        let actual = 
            randomizedMotifSearchIterations 1000 k t genomes 
            |> Seq.sprints_str
        let expected = lines.[0]

        (actual, expected)
    )

"RandomMotifSearch"
|> Execute.onDataSet
    (fun lines ->
        let k, t     = 
            let ints = lines.[0] |> String.toInts
            ints.[0], ints.[1]
        let genomes  = lines.[1..]

        randomizedMotifSearchIterations 1000 k t genomes
    )
    (Seq.iter (printfn "%s"))
