// 1.4.0.5 - Clump Finding Problem
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Support.fs"

#load "Bioinformatics.fs"
open Bioinformatics

module FrequencyDictionary =
    type Key = string
    type Dict = System.Collections.Generic.Dictionary<Key, int>
    
    let private fill (freqs:Dict) k (window:string) =
        window
        |> String.windowed k
        |> Seq.countBy (fun s -> s) 
        |> Seq.iter (fun (key,count) -> freqs.[key] <- count)

    let private decrement (freqs:Dict) key = freqs.[key] <- freqs.[key] - 1
    let private increment (freqs:Dict) key =
        if freqs.ContainsKey(key) 
        then freqs.[key] <- freqs.[key] + 1
        else freqs.[key] <- 1

    let private slide (freqs:Dict) previousFirstKmer currentLastKmer =
        decrement freqs previousFirstKmer
        increment freqs currentLastKmer

    let private findKeys (freqs:Dict) minCount =
        freqs 
        |> Dict.toSeq
        |> Seq.choose (fun (key, count) -> if count < minCount then None else Some key)     

    let findClumps k L (t:int) (genome:string) =
        let foldFunc (isFill, firstKmerFromLastWindow, (freqs:Dict), (clumps:Set<Key>)) (window:string) =
            let resultOf clumps = (false, window.Substring(0, k), freqs, clumps)
            if isFill then 
                fill freqs k window
                resultOf (new Set<Key>(findKeys freqs t))
            else 
                let currentLastKmer = window.[((String.length window)-k)..]
                slide freqs firstKmerFromLastWindow currentLastKmer
                if clumps.Contains(currentLastKmer) || freqs.[currentLastKmer] < t then 
                    resultOf clumps
                else
                    resultOf (clumps.Add currentLastKmer)
        genome 
        |> String.windowed L
        |> Seq.fold foldFunc (true, "", new Dict(), Set.empty<Key>)
        |> function _,_,_,result -> result

//Problem
let lines = "ClumpFindingProblem"  |> Files.toLines |> Seq.toArray
let genome = lines.[0]
let inputs = lines.[1] |> String.toInts
FrequencyDictionary.findClumps inputs.[0] inputs.[1] inputs.[2] genome
|> Set.map Char.toString
|> Set.iter (printf "%s ") 


//Ecoli
let ecoli = "Z:\Downloads\E-coli" |> Files.toLines |> Seq.head
printfn "Ecoli Frequency Start: Windows %i" ((String.length ecoli)/500)
FrequencyDictionary.findClumps 9 500 3 ecoli
|> Set.map Char.toString
|> Set.iter (printf "%s ") 
printfn "Ecoli Frequency Stop"


let line = "ClumpFindingProblem" |> Files.toLines |> Seq.head
line.Split(' ')
|> Array.length
|> printfn "%i"

