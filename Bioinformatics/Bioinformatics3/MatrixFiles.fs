namespace Bioinformatics

module MatrixFiles =
    let fileToScoringFunction filename = 
        let lines = filename |> Files.toLinesArray
        let header = lines.[0]
        let n = String.length header
        let scores = Files.Lines.toInts2D n lines.[1..n]
        let headerindex = header |> Seq.mapi (fun i c -> (string c,i)) |> Map.ofSeq
        fun a b -> scores.[Map.find a headerindex, Map.find b headerindex]