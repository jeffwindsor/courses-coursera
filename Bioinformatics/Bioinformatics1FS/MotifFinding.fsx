System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "Support.fs"
#load "Bioinformatics.fs"
#load "Motifs.fs" 
open Bioinformatics

let patternExists ps kmers =
    kmers |> Seq.exists (fun k -> ps |> Seq.exists (fun p -> p = k)) 
let patternExistsInAllGenomes d p genomes =
    let ps = hammingNeighbors d p
    let answer = genomes |> Seq.forall (fun g -> patternExists ps g )
    //printfn "%s -> %b" p answer
    answer
let patternsExistsInAllGenomes d p genomes =
    hammingNeighbors d p |> Seq.filter (fun p -> patternExistsInAllGenomes d p genomes )


let bruteForceMotifFInding k d (genomes: string seq) =
    let genomes' =
        genomes 
        |> Seq.toList 
        |> List.map (fun a -> String.windowed k a |> Seq.toList)

    genomes 
    |> Seq.collect (toKmers k)
    |> Seq.distinct
    |> Seq.collect (fun p -> patternsExistsInAllGenomes d p genomes')
    |> Seq.distinct

//Motif Finding Is More Difficult Than You Think | Step 7
["MotifFinding-SampleBF"; "MotifFinding-ExtraBF"]
|> Execute.testOnDataSets
    (fun lines ->
        let expected = lines.[0]
        let ints = lines.[1] |> String.toInts
        let k,d = ints.[0], ints.[1]
        let dnas = lines.[2..]
        let actual = bruteForceMotifFInding k d dnas |> Seq.sort |> String.concat " "
        (actual, expected)
    )

"MotifFindingBF"
|> Execute.onDataSet
    (fun lines ->
        let ints = lines.[0] |> String.toInts
        let k,d = ints.[0], ints.[1]
        let dnas = lines.[1..]
        let actual = bruteForceMotifFInding k d dnas |> Seq.sort
        actual
    )
    (Seq.iter (printf "%s "))

//Motif Finding Is More Difficult Than You Think | Step 7
["MotifFinding-Sample"; "MotifFinding-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let expected = lines.[0]
        let k = int lines.[1]
        let dnas = lines.[2..]
        let actual = Motifs.find k dnas |> Seq.head
        (actual, expected)
    )

"MotifFinding"
|> Execute.onDataSet
    (fun lines ->
        let k = int lines.[0]
        let dnas = lines.[1..]
        Motifs.find k dnas |> Seq.head
    )
    (printfn "%s")


//Scoring Motifs | Step 8
["TCGGGGGTTTTT";
"CCGGTGACTTAC";
"ACGGGGATTTTC";
"TTGGGGACTTTT";
"AAGGGGACTTCC";
"TTGGGGACTTCC";
"TCGGGGATTCAT";
"TCGGGGATTCCT";
"TAGGGGAACTAC";
"TCGGGTATAACC";]
|> Motifs.entropy