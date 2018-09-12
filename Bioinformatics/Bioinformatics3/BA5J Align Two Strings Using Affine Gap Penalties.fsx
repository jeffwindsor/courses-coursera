System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "MatrixFiles.fs"
open Bioinformatics

module AffineAlignments =
    type TotalScore = int
    type Action = 
        | End               // [.]  back track end point
        | DeletionOpen      // [A↑] back track to Matches up (from Deletions)
        | Deletion          // [↑]  back track up (on Deletions)
        | DeletionClose     // [D]  back track to same point on Deletions (from Matches)
        | InsertionOpen     // [D←] back track to Matches left (from Insertions)
        | Insertion         // [←]  back track left (on Insertions)
        | InsertionClose    // [A]  back track to same point on Insertions (from Matches)
        | Alignment         // [↖]  back track left and up, no need to differntiate substitutions
    type Edge = Action * TotalScore
    type LeveledEdge = Edge * Edge * Edge //upper,middle,lower in book
    type Node = { X:int; Y:int;}
    type Names = string[]
    type Graph = 
        {
            XNames:Names; 
            YNames:Names; 
            // Upper  : Origins, Insertions and Insertion Opens
            // Middle : Origins, Matches, Substitutions, Insertion Closes and Deletion Closes
            // Lower  : Origins, Deletions and Deletion Opens
            Edges:LeveledEdge[,];
            Terminus:Node;
        }

    //Names
    let noName = "-"
    let private nameAt (names:Names) limit i = if 0 < i && i <= limit then names.[i] else noName
    
    //Nodes - in the book (j) iterates on horizontal (x) axis and (i) iterates on veritcal (y) axis
    let nodeEmpty = (End,0)
    let nodeOutOfBounds = (End, -1000000)
    let up n   :Node = {X=n.X; Y=n.Y-1}   // i - 1
    let left n :Node = {X=n.X-1; Y=n.Y}   // j - 1
    let leftup  = (left>>up)
    let rowOf xTerminus y = seq { for x in 0 .. xTerminus do yield {X=x;Y=y}}
    let colOf yTerminus x = seq { for y in 0 .. yTerminus do yield {X=x;Y=y}}
    let byRows terminusPoint = seq { for y in 0 .. terminusPoint.Y do yield! rowOf terminusPoint.X y} //by row, important for fills

    //Edges
    let totalScore = snd
    let action = fst
    let add x e = e |> totalScore |> (+) x
    let edgeEmpty = (nodeEmpty,nodeEmpty,nodeEmpty)
    let upper  (i,_,_) = i
    let middle (_,m,_) = m
    let lower  (_,_,d) = d
    let edgeMaxLevel (u,m,l) = 
        match [u;m;l] |> List.maxBy totalScore with
        | x when x = u -> upper
        | x when x = m -> middle
        | _ -> lower

    //Graph
    let nameX g n = nameAt g.XNames g.Terminus.X n.X
    let nameY g n = nameAt g.YNames g.Terminus.Y n.Y
    let name g n = nameX g n, nameY g n
    let get g n = 
        match n.X, n.Y with
        | x,y when x<0 || y<0 -> edgeEmpty
        | x,y                 -> g.Edges.[x,y]

    let build scoreF s1 s2 = 
        // create graph
        let n1l,n2l = (String.length s1), (String.length s2)
        let g =
            {
                XNames = (noName + s1) |> Files.Lines.toCharacterStringArray; 
                YNames = (noName + s2) |> Files.Lines.toCharacterStringArray;
                Edges = Array2D.create (n1l+1) (n2l+1) edgeEmpty; 
                Terminus = {X=n1l;Y=n2l}; 
            }
        //printfn "Build : %s x %s " s1 s2
        //printfn "Terminus : %i x %i " g.Terminus.X g.Terminus.Y
        //fill grpah with alignments
        let set g n a = g.Edges.[n.X,n.Y] <- a;
        byRows g.Terminus |> Seq.iter (fun n -> scoreF g n |> set g n )
        g

    //Scoring - NeedlemanWunsch
    let score mu sigma epsilon g n =
        //printfn "Score %ix%i %A" n.X n.Y (name g n)
        match n with
        | {X=x;Y=y} when x=0 && y=0 -> (nodeOutOfBounds,(End, 0),nodeOutOfBounds) //will lead to stop
        | {X=x;Y=y} when y=0        -> ((Insertion, epsilon * x),nodeOutOfBounds,nodeOutOfBounds) //follow top row to origin
        | {X=x;Y=y} when x=0        -> (nodeOutOfBounds,nodeOutOfBounds,(Deletion,epsilon*y)) // follow first col to origin
        | _ ->
            let get n = get g n
            let upper = 
                let n' = n |> left |> get
                [ (Insertion, upper n' |> add epsilon); (InsertionOpen, middle n' |> add sigma)] |> Seq.maxBy snd
            let lower = 
                let n' = n |> up |> get
                [ (Deletion, lower n' |> add epsilon); (DeletionOpen, middle n' |> add sigma)] |> Seq.maxBy snd
            let middle = 
                let n' = n |> leftup |> get
                [ (Alignment, middle n' |> add (name g n ||> mu)); (InsertionClose, snd upper); (DeletionClose, snd lower)] |> Seq.maxBy snd
            (upper,middle,lower)
    
    let rec path g =
        let rec inner ((n1s,n2s) as acc) f n =  //acc for tail recursion
            let accumlate n1' n2' = (n1'::n1s, n2'::n2s)
            //printfn "Path %ix%i %A -> %A" n.X n.Y (name g n) (get g n |> f |> action)
            match get g n |> f |> action with
            | End            -> acc // [.]  back track end point
            | DeletionOpen      -> inner (accumlate noName (nameY g n)) middle (n |> up)     // [A↑] back track to Matches up (from Deletions)
            | Deletion          -> inner (accumlate noName (nameY g n)) lower  (n |> up)     // [↑]  back track up (on Deletions)
            | DeletionClose     -> inner acc                            lower   n            // [D]  back track to same point on Deletions (from Matches)
            | InsertionOpen     -> inner (accumlate (nameX g n) noName) middle (n |> left)   // [D←] back track to Matches left (from Insertions)
            | Insertion         -> inner (accumlate (nameX g n) noName) upper  (n |> left)   // [←]  back track left (on Insertions)
            | InsertionClose    -> inner acc                            upper   n            // [A]  back track to same point on Insertions (from Matches)
            | Alignment             -> inner (name g n ||> accumlate)       middle (n |> leftup) // [↖]  back track left and up
        let startNode = g.Terminus
        let startEdge = get g startNode
        let startLevel = startEdge |> edgeMaxLevel
        inner ([],[]) startLevel startNode
        |> fun (n1s,n2s) -> (startEdge |> startLevel |> totalScore |> string, String.concat "" n1s, String.concat "" n2s)


//  BA5J Align Two Strings Using Affine Gap Penalties
//    A gap is a contiguous sequence of spaces in a row of an alignment. One way to score gaps more appropriately is to define an affine penalty for a gap of length k as σ + ε · (k − 1), where σ is the gap opening penalty, assessed to the first symbol in the gap, and ε is the gap extension penalty, assessed to each additional symbol in the gap. We typically select ε to be smaller than σ so that the affine penalty for a gap of length k is smaller than the penalty for k independent single-nucleotide indels (σ · k).
//    Alignment with Affine Gap Penalties Problem: Construct a highest-scoring global alignment of two strings (with affine gap penalties).
//    Given: Two amino acid strings v and w (each of length at most 100).
//    Return: The maximum alignment score between v and w, followed by an alignment of v and w achieving this maximum score. Use the BLOSUM62 scoring matrix, a gap opening penalty of 11, and a gap extension penalty of 1.
open AffineAlignments
let id = "BA5J"
let ``Align Two Strings Using Affine Gap Penalties`` matrix sigma epslion s1 s2 =
    let mu = MatrixFiles.fileToScoringFunction matrix
    let sm = score mu sigma epslion
    build sm s1 s2 
    //|> (fun g -> printfn "%A" (get g {X=1;Y=1;}); g;)
    |> path

 
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``Align Two Strings Using Affine Gap Penalties`` "BLOSUM62" -11 -1 lines.[0] lines.[1]
let actualExpected (lines: string[]) = (actual lines, (lines.[2], lines.[3], lines.[4]))
let print = fun (s,one,two) -> printfn "%s\n%s\n%s" s one two
// **************************************
// Runs
// **************************************
Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print
