namespace Bioinformatics

module LinearSpaceAlignments =
    type Scores = int[]
    type Names = string[]
    let noName = "-"
    let s2a = Files.Lines.toCharacterStringArray
    let left (s:Names) mid = if mid = 0 then [||] else s.[.. mid - 1]
    let right (s:Names) mid = if mid = 0 then s else s.[mid ..]

    // Needleman - Wunsch
    // Time  = O(sxl * syl)
    // Space = O(min{ sxl, syl})
    let score alignmentScore insertCost deleteCost inames jnames =
        let il, jl = Array.length inames, Array.length jnames
        let len = jl + 1
        let scoreColumn (prevIScores:Scores, currentIScores:Scores) j =
            seq { 0 .. jl }
            |> Seq.iter (function
                        | 0 -> currentIScores.[0] <- prevIScores.[0] + deleteCost
                        | i -> currentIScores.[i] <-
                                [
                                    currentIScores.[i-1] + insertCost; //insert
                                    prevIScores.[i] + deleteCost; //delete
                                    prevIScores.[i-1] + alignmentScore inames.[j] jnames.[i-1]; //aligment
                                ] |> List.max)
            currentIScores, prevIScores     //make current the previous and re-use previous for current (wil get overwritten)

        // fold score columns until last is reached
        let initialIScorces = (Array.init len (fun i -> i * insertCost)) //initial column has cumulative gap costs for insertions
        let currentIScores = Array.zeroCreate<int> len
        seq { 0 .. il - 1 } 
        |> Seq.fold scoreColumn (initialIScorces,currentIScores)
        |> fst  //return only the current scorces
               
    let hirschbergMidPoint alignmentScore insertCost deleteCost (sx:Names) sxl sy = 
        let score = score alignmentScore insertCost deleteCost 
        let sxmid = sxl / 2
        let left  = score (left sx sxmid) sy
//        printfn "LEFT: %A" left
        let right = score (right sx sxmid|> Array.rev) (Array.rev sy) |> Array.rev 
//        printfn "RIGHT: %A" right
        let combine = Array.zip left right |> Array.mapi (fun i (x,y) -> i, x+y)
//        printfn "COMBINE: %A" combine
        let symid = if sxmid = 0 
                    then combine |> Array.tail |> Array.maxBy snd |> fst
                    else combine |> Array.maxBy snd |> fst
//        printfn "MID: %i x %i" sxmid symid
        (sxmid, symid)

    let Hirschberg alignmentScore insertCost deleteCost sx sy =
        let midpoint = hirschbergMidPoint alignmentScore insertCost deleteCost
        let convert = List.rev >> String.concat ""
        let rec inner (ax, ay) (sx:Names) (sy:Names) =
//            printf "(%s,%s) -> " (String.concat "" sx) (String.concat "" sy)
            match Array.length sx, Array.length sy with
            | 0, syl   -> 
//                        printfn "(-,%s) -> End with Zero length X\n  %A\n  %A"  (String.concat "" sy) ((String.replicate syl noName)::ax) ((String.concat "" sy)::ay)
                        (String.replicate syl noName)::ax, (String.concat "" sy)::ay
            | sxl, 0   -> 
//                        printfn "(%s,-) -> End with Zero length Y\n  %A\n  %A"  (String.concat "" sx) ((String.concat "" sx)::ax) ((String.replicate sxl noName)::ay)
                        (String.concat "" sx)::ax, (String.replicate sxl noName)::ay
            | 1, 1     -> 
//                        printfn "(%s,%s) -> End with Pair\n  %A\n  %A"  (String.concat "" sx) (String.concat "" sy) ((String.concat "" sx)::ax)((String.concat "" sy)::ay)
                        (String.concat "" sx)::ax, (String.concat "" sy)::ay
            | sxl, syl -> 
//                        printfn "(%s,%s) -> Divide"  (String.concat "" sx) (String.concat "" sy)
                        let sxmid,symid = midpoint sx sxl sy
                        let acc = inner (ax, ay) (left sx sxmid) (left sy symid)
                        inner acc (right sx sxmid) (right sy symid)

        inner ([], []) (sx |> s2a) (sy |> s2a)
        |> fun (a1,a2) -> convert a1, convert a2
