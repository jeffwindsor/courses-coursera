namespace Bioinformatics

module SequenceAlignments =
    type TotalScore = int
    type Action = 
          End           // [.] back track end point
        | Deletion      // [↑] deletion = back track up y axis 
        | Insertion     // [←] insertion = back track left on x axis
        | Alignment     // [↖] match = back track left and up - matching nucs
        | Substitution  // [↖] match = back track left and up - non matching nucs
    type Edge = Action * TotalScore
    type Node = {X:int;Y:int}
    type Names = string[]
    type Graph = 
        {
            XNames:Names; 
            YNames:Names; 
            Edges:Edge[,]; 
            Terminus:Node;
        }
    
    //Names
    let noName = "-"
    let private nameAt (names:Names) limit i = if i < 1 || limit < i then noName else names.[i]
    let xName g n = nameAt g.XNames g.Terminus.X n.X
    let yName g n = nameAt g.YNames g.Terminus.Y n.Y  // placing names after -> so 0,0 has no name but terminus does
    let name g n = xName g n, yName g n  // placing names after -> so 0,0 has no name but terminus does
    
    //Nodes - in the book (j) iterates on horizontal (x) axis and (i) iterates on veritcal (y) axis
    let up n : Node = {X=n.X; Y=n.Y-1}
    let left n : Node   = {X=n.X-1; Y=n.Y}
    let leftup = (left>>up)
    let rowOf xTerminus y = seq { for x in 0 .. xTerminus do yield {X=x;Y=y}}
    let colOf yTerminus x = seq { for y in 0 .. yTerminus do yield {X=x;Y=y}}
    let byRows terminusPoint = seq { for y in 0 .. terminusPoint.Y do yield! rowOf terminusPoint.X y} //by row, important for fills

    // Edges
    let emptyEdge = (End,0)

    // Alignments and Grpah
    let row g i = rowOf g.Terminus.X i
    let col g i = colOf g.Terminus.Y i
    let direction (a:Edge) = a |> fst
    let total (a:Edge)     = a |> snd
    let get g n = 
        match n.X, n.Y with
        | x,y when x < 0 || y < 0 -> emptyEdge
        | x,y -> g.Edges.[x,y]
    let maxTotal g points =
        points |> Seq.fold (fun ((s,maxps) as acc) n -> 
            match get g n with
            | (_, s') when s'= 0 -> acc          //short circuit zeros, no need to accumulate zero points
            | (_, s') when s'= s -> (s,n::maxps) //additional max point
            | (_, s') when s'> s -> (s',[n])     //new max score
            | _                  -> acc) (0,[])      //no changeto max score points
    let build scoreF s1 s2 = 
        // create graph
        let n1l,n2l = (String.length s1), (String.length s2)
        let g =
            {
                XNames = (noName + s1) |> Files.Lines.toCharacterStringArray; 
                YNames = (noName + s2) |> Files.Lines.toCharacterStringArray;
                Edges = Array2D.create (n1l+1) (n2l+1) emptyEdge; 
                Terminus = {X=n1l;Y=n2l}; 
            }
        //fill grpah with alignments
        let set g n a = g.Edges.[n.X,n.Y] <- a;
        byRows g.Terminus |> Seq.iter (fun n -> scoreF g n |> set g n )
        g

    let print g = 
        let alignmentToArrow (a,_) = 
            let n s = printf "%4s" s
            match a with
            | End    -> n " "
            | Deletion  -> n "↑"
            | Insertion -> n "←"
            | Alignment     -> n "↖"
            | Substitution  -> n "↖"
        let alignmentToTotal (_,s) = 
            match s with
            | s when s=0 -> printf "    "
            | s -> printf "%4i" s
        let inner f start terminus =
            printf "   "; g.XNames |> Seq.iter (printf "%4s"); printfn ""
            [start.Y .. terminus.Y] |> Seq.iter (fun y ->
                printf "%3s" g.YNames.[y] 
                [start.X .. terminus.X] |> Seq.iter (fun x ->
                    f g.Edges.[x,y])
                printfn ""
            )
        inner alignmentToArrow {X=0;Y=0} g.Terminus; printfn "";
        inner alignmentToTotal {X=0;Y=0} g.Terminus; printfn "";

    module Scoring =
        let private totalfor g n direction delta =  n |> direction |> get g |> total |> (+) delta
        // used for local aligments
        let SmithWaterman matchScoreF substituteScoreF gapCost g n : Edge =
            match n with
            | {X=x;Y=y} when x=0 || y=0 
                    -> (End, 0)
            | _  -> let names = name g n
                    [
                        (End,0);  //reprents edge to 0,0, so called free ride edge
                        (Insertion, totalfor g n left gapCost);
                        (Deletion,  totalfor g n up gapCost);
                        (if names ||> (=) 
                            then Alignment, totalfor g n leftup (names ||> matchScoreF)
                            else Substitution, totalfor g n leftup (names ||> substituteScoreF));
                    ]
                    |> List.maxBy snd
        
        let private generalNeedlemanWunsch selector matchScoreF substituteScoreF gapCost g n : Edge =
            match n with
            | {X=x;Y=y} when x=0 || y=0 
                    -> End, (x * gapCost) + (y * gapCost) 
            | _  -> let names = name g n
                    [
                        (Insertion,totalfor g n left gapCost);
                        (Deletion, totalfor g n up gapCost);
                        (if names ||> (=) 
                            then Alignment, totalfor g n leftup (names ||> matchScoreF)
                            else Substitution, totalfor g n leftup (names ||> substituteScoreF));
                    ]
                    |> selector

        // used for global alignments
        let NeedlemanWunsch matchScoreF substituteScoreF gapCost g n =
            generalNeedlemanWunsch (List.maxBy snd) matchScoreF substituteScoreF gapCost g n

        let EditDistance g n =
            generalNeedlemanWunsch (List.minBy snd) (fun _ _ -> 0) (fun _ _ -> 1) 1 g n

    module Paths =
            // |<--->|
            // |<--->|
            // search terminus (m,n) back to 1,1
            let rec ``global`` g =
                let rec inner ((s,n1s,n2s) as acc) n =  //acc for tail recursion
                    let accumlate s' n1' n2' = (max s s', n1'::n1s, n2'::n2s)
                    match get g n with
                    | (End, s')       -> if n.X=0 && n.Y=0 then acc else accumlate s' (xName g n) (yName g n)
                    | (Alignment,s')
                    | (Substitution,s') -> inner (accumlate s' (xName g n) (yName g n)) (n |> leftup)
                    | (Deletion,s')     -> inner (accumlate s' noName (yName g n)) (n |> up)
                    | (Insertion,s')    -> inner (accumlate s' (xName g n) noName) (n |> left)
                inner (0,[],[]) g.Terminus
                |> fun (s,n1s,n2s) -> (s, String.concat "" n1s, String.concat "" n2s)

            // <---|----|->
            //   <-|----|----->
            // search max score point back to 0 score
            let rec private local g n = 
                let rec inner ((s,n1s,n2s) as acc) n =  //acc for tail recursion
                    let accumlate s' n1' n2' = (max s s', n1'::n1s, n2'::n2s)
                    match get g n with
                    | (End, _)      -> acc
                    | (Alignment,s')
                    | (Substitution,s') -> inner (accumlate s' (xName g n) (yName g n)) (n |> leftup)
                    | (Deletion,s')     -> inner (accumlate s' noName (yName g n)) (n |> up)
                    | (Insertion,s')    -> inner (accumlate s' (xName g n) noName) (n |> left)
                inner (0,[],[]) n
                |> fun (s,n1s,n2s) -> (s, String.concat "" n1s, String.concat "" n2s)

            let locals g = 
                byRows g.Terminus  //all points
                |> maxTotal g
                |> snd
                |> List.map (fun n -> local g n)

            // <---|--->|
            //     |<---|----->
            //search max left column row back to a first column cell
            let overlap g = 
                (
                    if g.Terminus.X < g.Terminus.Y 
                    then col g g.Terminus.X //last column
                    else row g g.Terminus.Y  //last row
                )
                |> Seq.map   (fun n -> local g n)
                |> Seq.groupBy (fun (s,_,_) -> s) 
                |> Seq.maxBy fst
                |> snd
            
            // <---|----|--->
            //     |<-->|
            //search max bottom row back to a first column cell
            let rec private fitting g n = 
                let rec inner ((oris,n1s,n2s,maxs) as acc) n =  //acc for tail recursion
                    let accumlate s n1 n2 = (oris, n1::n1s, n2::n2s, max maxs s)
                    match get g n with
                    | (End, _)       -> acc
                    | (Alignment,s')
                    | (Substitution,s') -> inner (accumlate s' (xName g n) (yName g n)) (n |> leftup)
                    | (Deletion,s')     -> inner (accumlate s' noName (yName g n)) (n |> up)
                    | (Insertion,s')    -> inner (accumlate s' (xName g n) noName) (n |> left)
                inner ((get g n |> total),[],[],0) n

            let fittings g = 
                Seq.append
                    (col g g.Terminus.X) //last column
                    (row g g.Terminus.Y)  //last row
                |> Seq.distinct
                |> maxTotal g
                |> snd
                |> Seq.map (fun n -> fitting g n)
                |> Seq.map (fun (s,s1,s2,_) -> (s,s1,s2))

//            let private convert (s,n1s,n2s) = (s, String.concat "" n1s, String.concat "" n2s)
//            let rec private generalized g n originF=
//                let rec inner ((s,n1s,n2s) as acc) n =  //acc for tail recursion
//                    let accumlate s' n1' n2' = 
//                        (max s s', n1'::n1s, n2'::n2s)
//                    match get g n with
//                    | (Origin, s')       -> originF acc accumlate s' n
//                    | (Match,s')
//                    | (Substitution,s') -> inner (accumlate s' (name1 g n) (name2 g n)) (n |> leftup)
//                    | (Deletion,s')     -> inner (accumlate s' noName (name2 g n)) (n |> up)
//                    | (Insertion,s')    -> inner (accumlate s' (name1 g n) noName) (n |> left)
//                inner (0,[],[]) n
//                |> convert