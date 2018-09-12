System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "CircularChromosomes.fs"
open Bioinformatics
open CircularChromosomes

//Implement GreedySorting
//Given: A signed permutation P.
//Return: The sequence of permutations corresponding to applying GreedySorting to P, ending with the identity permutation.
let flip (Block v) = Block -v
let abs (Block v) = Block (abs v)
let flipBlocks i j a = 
    let sLen =  (j - i) + 1
    if sLen > 1 && i >= 0 && j < (Array.length a)
    then Array.blit (a.[i..j] |> Array.rev |> Array.map flip) 0 a i sLen
    a

let greedySorting (Chromosome blocks) =
    let absi a i = Array.set a i (Array.get a i |> abs); a;
    seq {1 .. (Array.length blocks)}
    |> Seq.fold 
        (fun (bs,acc) n ->
            let i = n - 1
            match Array.get bs i with 
            | Block v when v = n  -> (bs,acc)      //already in order
            | Block v when v = -n -> 
                let a' = (absi (Array.copy bs) i)
                //printfn "flip sign: %A" a'
                (a', a'::acc)
            | _ -> 
                let a' = flipBlocks i (Array.findIndex (fun b -> (abs b) |> value = n) bs) (Array.copy bs)
                match Array.get a' i with 
                | Block v when v = n  -> 
                    //printfn "reverse section: %A" a'
                    (a', a'::acc)
                | _ -> 
                    let a'' = (absi (Array.copy a') i)
                    //printfn "reverse section, then flip : %A, %A" a' a''
                    (a'', a''::a'::acc)
            ) 
        (blocks,[])
    |> snd
    |> Seq.rev
    |> Seq.map Chromosome

let id = "BA6A"
let ``Greedy Sorting`` genomeInput =
    genomeInput 
    |> genomeFromInput
    |> chromosomes
    |> Array.head
    |> greedySorting
    |> Seq.map chromosomeToString
    |> Seq.toArray

// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``Greedy Sorting`` lines.[0] 
let actualExpected (lines: string[]) = (actual lines, lines.[1..])
let print ls = Seq.iter (printfn "%s") ls

// **************************************
// Runs
// **************************************
//Problems.areEqual (reverseSection 1 4 [|1;2;3;4;5;6|]) [|1;5;4;3;2;6|] "reverseSection mid"
//Problems.areEqual (reverseSection 0 5 [|1;2;3;4;5;6|]) [|6;5;4;3;2;1|] "reverseSection mid"
//Problems.areEqual (reverseSection 1 1 [|1;2;3;4;5;6|]) [|1;2;3;4;5;6|]  "reverseSection none"
//Problems.areEqual (reverseSection 4 3 [|1;2;3;4;5;6|]) [|1;2;3;4;5;6|]  "reverseSection negative"
//Problems.areEqual (reverseSection -2 3 [|1;2;3;4;5;6|]) [|1;2;3;4;5;6|] "reverseSection before"
//Problems.areEqual (reverseSection 2 6 [|1;2;3;4;5;6|]) [|1;2;3;4;5;6|]  "reverseSection after"
//Problems.areEqual (greedySorting [|1;2;3;4;5;6|]) [|1;2;3;4;5;6|] "greedySorting no reverses"
//Problems.areEqual (greedySorting [|1;5;4;3;2;6|]) [|1;2;3;4;5;6|] "greedySorting 1"
//Problems.areEqual (greedySorting [|5;2;3;4;6;1|]) [|1;2;3;4;5;6|] "greedySorting 2"
//``Greedy Sorting`` "(+20 +7 +10 +9 +11 +13 +18 -8 -6 -14 +2 -4 -16 +15 +1 +17 +12 -5 +3 -19)"  |> Array.length

Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print