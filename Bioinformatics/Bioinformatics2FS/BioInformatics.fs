namespace Bioinformatics

[<AutoOpen>]
module Bioinformatics =
    let dnaNucleotides = ["A"; "C"; "G"; "T";]

    let toKmers k genome = genome |> String.windowed k

    let kmerCounts k genome = toKmers k genome |> Seq.countBy (fun s -> s)    

    let hammingDistance (one:string) (two:string) =
        Seq.mapi2 (fun i a b -> if a = b then None else Some(i)) one two
        |> Seq.choose (fun x -> x)
        |> Seq.length

    let hammingNeighbors d pattern =
        let rec inner pattern =
            match (String.length pattern) with
            | 1 -> dnaNucleotides
            | _ ->
                let prefix = string pattern.[0]
                let suffix = pattern.[1..]
                inner suffix
                |> List.collect (fun a ->
                    if (hammingDistance suffix a) < d
                    then dnaNucleotides |> List.map (fun n -> n + a)
                    else [prefix + a])

        match d with
        | 0 -> seq [pattern] 
        | _ -> seq (inner pattern |> List.distinct)

    