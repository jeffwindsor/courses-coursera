System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
open Bioinformatics

module MultiAlignments =
    let s2a = Files.Lines.toCharacterStringArray

    module Edge =
        type TotalScore = int
        type BacktrackAlong = I | J | K | IJ | IK | JK | IJK | Stop
        type T = BacktrackAlong * TotalScore
        let total = snd
        let backtrackAlong = fst
        let origin:T = (Stop,0)

    module Node =
        open Edge
        type T = int * int * int
        let create i j k = (i,j,k)
        let backtrack (i,j,k) along = 
            match along with
            | I   -> create (i-1) j k 
            | J   -> create i (j-1) k
            | K   -> create i j (k-1)
            | IJ  -> create (i-1) (j-1) k
            | IK  -> create (i-1) j (k-1)
            | JK  -> create i (j-1) (k-1)
            | IJK -> create (i-1) (j-1) (k-1)
            | Stop -> failwith "Stop not valid"

    module Names =
        type T = string[]
        let noName = "-"
        let get (names:T) i = if i = -1 then noName else names.[i]
        
    module Graph =
        type T = 
            {
                INames:Names.T
                JNames:Names.T
                KNames:Names.T
                Edges: Edge.T[,,]
                Terminus:Node.T
            }
        let out = -1
        let private getNames g (i,j,k) = Names.get g.INames i, Names.get g.JNames j, Names.get g.KNames k
        let private setEdge g (i,j,k) e = g.Edges.[i,j,k] <- e
        let private getEdge g (i,j,k) = 
            match i, j, k with 
            // OUT OF BOUNDS NODES
            | -1,-1,-1 ->  Edge.origin   // Origin
            |  _,-1,-1 -> (Edge.I, 0)    // On the I Boundry
            | -1, _,-1 -> (Edge.J, 0)    // On the J Boundry
            | -1,-1, _ -> (Edge.K, 0)    // On the K Boundry
            |  _, _,-1 -> (Edge.IJ, 0)   // On the IJ Plane
            |  _,-1, _ -> (Edge.IK, 0)   // On the IK Plane
            | -1, _, _ -> (Edge.JK, 0)   // On the JK Plane
            | _        -> g.Edges.[i,j,k]

        let build s1 s2 s3 =
            let maxIncomingEdge g n = 
                let score' along delta = along, (Node.backtrack n along |> getEdge g |> Edge.total) + delta
                let v,w,u = getNames g n
                [
                    score' Edge.I   0;
                    score' Edge.J   0;
                    score' Edge.K   0;
                    score' Edge.IJ  0;
                    score' Edge.IK  0;
                    score' Edge.JK  0;
                    score' Edge.IJK (if v=w && w=u then 1 else 0);
                ] |> List.maxBy snd

            let s1l,s2l,s3l = (String.length s1), (String.length s2), (String.length s3)
            let g =
                {
                    INames = s1 |> s2a
                    JNames = s2 |> s2a
                    KNames = s3 |> s2a
                    Edges = Array3D.create s1l s2l s3l Edge.origin; 
                    Terminus = Node.create (s1l - 1) (s2l - 1) (s3l - 1)
                }
            //fill nodes
            let (ix,jx,kx) = g.Terminus
            seq { for k in 0 .. kx do
                    for i in 0 .. ix do
                        for j in 0 .. jx do 
                        yield Node.create i j k
                } |> Seq.iter (fun n -> maxIncomingEdge g n |> setEdge g n )
            //return graph
            g

        let path g =
            printfn "I [%i]: %A" (Array.length g.INames) g.INames
            printfn "J [%i]: %A" (Array.length g.JNames) g.JNames
            printfn "K [%i]: %A" (Array.length g.KNames) g.KNames
            printfn "Terminus: %A" g.Terminus
                
            let rec inner (nIs,nJs,nKs) n =  //acc for tail recursion
                let recurse edge nI nJ nK = 
                    printfn "%A -- %A --> (%s, %s, %s)" n edge nI nJ nK
                    inner (nI::nIs, nJ::nJs, nK::nKs) (Node.backtrack n (edge |> Edge.backtrackAlong))

                let nI,nJ,nK = getNames g n
                let nn = Names.noName
                let edge = getEdge g n 
                match edge |> Edge.backtrackAlong with
                | Edge.I    -> recurse edge nI nn nn // i _ _
                | Edge.J    -> recurse edge nn nJ nn // _ j _
                | Edge.K    -> recurse edge nn nn nK // _ _ k
                | Edge.IJ   -> recurse edge nI nJ nn // i j _
                | Edge.IK   -> recurse edge nI nn nK // i _ k
                | Edge.JK   -> recurse edge nn nJ nK // _ j k
                | Edge.IJK  -> recurse edge nI nJ nK // i j k
                | Edge.Stop -> 
                    printfn "%A -- %A --> (%s, %s, %s)" n edge nI nJ nK
                    (nIs,nJs,nKs)

            let lcs = getEdge g g.Terminus |> Edge.total
            inner ([],[],[]) g.Terminus
            |> fun (nIs,nJs,nKs) -> (lcs, String.concat "" nIs, String.concat "" nJs, String.concat "" nKs)

//  BA5M Find a Highest-Scoring Multiple Sequence Alignment
//    Multiple Longest Common Subsequence Problem: Find a longest common subsequence of multiple strings.
//    Given: Three DNA strings.
//    Return: The maximum score of a multiple alignment of these three strings, followed by a multiple alignment of the three strings achieving this maximum. Use a scoring function in which the score of an alignment column is 1 if all three symbols are identical and 0 otherwise. (If more than one multiple alignment achieve the maximum, you may return any one.)
let id = "BA5M"
let ``Find a Highest-Scoring Multiple Sequence Alignment`` s1 s2 s3 =
    MultiAlignments.Graph.build s1 s2 s3
    |> MultiAlignments.Graph.path

 
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``Find a Highest-Scoring Multiple Sequence Alignment`` lines.[0] lines.[1] lines.[2]
let actualExpected (lines: string[]) = (actual lines, (int lines.[3], lines.[4], lines.[5], lines.[6]))
let print = fun (s,one,two,three) -> printfn "%i\n%s\n%s\n%s" s one two three

// **************************************
// Runs
// **************************************
``Find a Highest-Scoring Multiple Sequence Alignment`` "ACGATACGT" "CCCATTAAGT" "GACTATAGAA" |> print
Problems.validate id "Test-FullMatch" actualExpected
Problems.validate id "Test-IGap" actualExpected
Problems.validate id "Test-JGap" actualExpected
Problems.validate id "Test-KGap" actualExpected

Problems.validate id "EpilogueStep2Example" actualExpected

Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print
