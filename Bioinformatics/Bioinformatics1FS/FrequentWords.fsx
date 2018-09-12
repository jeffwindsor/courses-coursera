System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Support.fs"
open Bioinformatics

let kmerCountMax kmerCounts =
    let kcs = kmerCounts |> Seq.sortByDescending snd
    let max = kcs |> Seq.head |> snd
    kcs
    |> Seq.filter (fun (_,c) -> c = max)
    |> Seq.map (fun (p,_) -> p)

let kCount text k = 
    text
    |> Seq.windowed k
    |> Seq.countBy (fun s -> s)
    |> Seq.sortByDescending snd

let frequentWords text k =
    let counts = kCount text k
    let max = Seq.head counts |> snd
    counts 
    |> Seq.filter (fun x -> (snd x) = max)
    |> Seq.map fst

let kmerCountsWithMismatches k d genome= 
    kmerCounts k genome 
    |> Seq.copyValuesTo (hammingNeighbors d)
    |> Seq.groupBySum

let frequentKmersWithMismatches k d genome =
    kmersWithMismatches k d genome |> kmerCountMax

let frequentKmersWithMismatchesAndReverseCompliment k d genome =
    let kcs = kmersWithMismatches k d genome
    let map = kcs |> Map.ofSeq
     
    kcs 
    |> Seq.map (fun (p,c) -> 
                    let rcp = Dna.toReverseComplement p
                    if map.ContainsKey rcp
                    then (p, c + (map.Item rcp))
                    else (p, c)
                )
    |> kmerCountMax


//1.2.5.8
//A straightforward algorithm for finding the most frequent k-mers in a string Text checks all k-mers appearing in this string (there are |Text| − k + 1 such k-mers) and then computes how many times each k-mer appears in Text. To implement this algorithm, called FrequentWords, we will need to generate an array Count, where Count(i) stores Count(Text, Pattern) for Pattern = Text(i, k) (see figure below).
let test1 =
    let lines = "FrequentWords-Extra" |> Files.toLines |> Seq.toArray
    let expected = 
        (lines.[2].Split(' '))
        |> Array.sortDescending

    let actual = 
        frequentWords lines.[0] (int lines.[1]) 
        |> Seq.map Seq.toString 
        |> Seq.sortDescending
        |> Seq.toArray
    assert (Array.forall2 (fun e a -> a = e) expected actual)

let text, k =
    let lines = "FrequentWords" |> Files.toLines |> Seq.toArray
    (lines.[0], int lines.[1])
frequentWords text k
|> Seq.iter (fun cs -> Array.iter (printf "%c") cs; printf " ";)



//WITH MISMATCHES
Execute.onDataSet
    (fun lines ->
        let genome = lines.[0]
        let ints = lines.[1] |> String.toInts
        let k, d = (ints.[0], ints.[1])
        frequentKmersWithMismatches k d genome
    )
    Seq.printsn_str
    "FrequentWordsWithMismatches"

Execute.testOnDataSets 
    (fun lines ->
        let title = lines.[0]
        let genome = lines.[1]
        let ints = lines.[2] |> String.toInts
        let k, d = (ints.[0], ints.[1])
        let expected = lines.[4].Split(' ') |> Array.sort
        let actual = 
            frequentKmersWithMismatches k d genome
            |> Seq.sort |> Seq.toArray
        (title, actual = expected, actual, expected)
    )
    "FrequentWordsWithMismatches-Tests" 5 


//WITH MISMATCHES AND REVERSE COMPLEMENTS
Execute.onDataSet
    (fun lines ->
        let genome = lines.[0]
        let ints = lines.[1] |> String.toInts
        let k, d = (ints.[0], ints.[1])
        frequentKmersWithMismatchesAndReverseCompliment k d genome
    )
    Seq.printsn_str
    "FrequentWordsWithMismatchesReverseCompliment"

Execute.testOnDataSets 
    (fun lines ->
        let title = lines.[0]
        let genome = lines.[2]
        let ints = lines.[3] |> String.toInts
        let k, d = (ints.[0], ints.[1])
        let expected = lines.[5].Split(' ') |> Array.sort
        let actual = 
            frequentKmersWithMismatchesAndReverseCompliment k d genome
            |> Seq.sort |> Seq.toArray
        (title, actual = expected, actual, expected)
    )
    "FrequentWordsWithMismatchesReverseCompliment-Tests" 6