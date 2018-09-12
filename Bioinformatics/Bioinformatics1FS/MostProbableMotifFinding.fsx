System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "Support.fs"
#load "Bioinformatics.fs"
#load "Motifs.fs" 
open Bioinformatics
open Motifs

let toProfiles size lines =
    let transpose size innersize (source: float[][]) =
        Array.init size (fun col -> 
            Array.init innersize (fun row -> source.[row].[col]))
    lines  
    |> Seq.map (fun s -> s |> String.toFloats)
    |> Seq.toArray
    |> transpose size 4

let mostProbableMotifSearch k profiles genome =
    genome 
    |> String.windowed k 
    |> Profiles.firstHighestProbability profiles

//Get a list of all k-mers from given text. (n - k + 1)
//Calculate probabilities of all k-mers in above list. 
//Find a k-mer with maximum probability value.
//Greedy Motif Search | Step 3
["MostProbableMotifFinding-Sample"; "MostProbableMotifFinding-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let genome   = lines.[1]
        let k        = int lines.[2]
        let profiles = lines.[3..] |> toProfiles k

        let actual = mostProbableMotifSearch k profiles genome
        let expected = lines.[0]

        (actual, expected)
    )

"MostProbableMotifFinding"
|> Execute.onDataSet
    (fun lines ->
        let genome   = lines.[0]
        let k        = int lines.[1]
        let profiles = lines.[2..] |> toProfiles k 

        mostProbableMotifSearch k profiles genome
    )
    (printfn "%s")
