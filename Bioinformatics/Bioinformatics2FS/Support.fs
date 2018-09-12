namespace Bioinformatics

module Files =
    open System.IO
    let toLines filepath =
        seq { 
            use stream = File.OpenRead (filepath + ".txt")
            use reader = new StreamReader(stream)
            while not reader.EndOfStream do yield reader.ReadLine()
        }
    let firstLine = toLines >> Seq.head

    module Fasta =
        type Entry<'a> = {Id: string; Value: 'a }

        //TODO: Handle improperly formated or blank lines
        let rec private parseLinesIntoFastaEntries (lines: string seq) = 
            let isId (l:string) = l.StartsWith(">") = false
            seq { 
                    if Seq.isEmpty lines then () else

                    //Parse Entry - First line is ID, Value = remaining lines until next id or EOS are value lines to be concantenated
                    let id = (lines |> Seq.head).[1..]
                    let valuei = 
                        lines 
                        |> Seq.skip 1
                        |> Seq.takeWhile (fun l -> isId l)
                        |> Seq.fold (fun (i,s) l -> (i+1,s+l)) (0,"")

                    yield {Id=id; Value= (snd valuei)}

                    //Recurse 
                    let rest = lines |> Seq.skip ((fst valuei) + 1)
                    yield! parseLinesIntoFastaEntries rest 
            }
        let readAsEntries = toLines >> parseLinesIntoFastaEntries
        let read = readAsEntries >> (Seq.map (fun fe -> fe.Value))
        module Entries =
            let print fe = printfn "%s %f" fe.Id fe.Value
            let mapValues f es = 
                es |> Seq.map (fun {Id=id; Value=value} -> {Id=id; Value = value |> f})

module Execute =
    let private testPrint i (actual, expected) = 
        let result = actual = expected
        printfn "%i: %b" i result
        if result = false then
            printfn "  ACTUAL:"
            printfn "  %A" actual
            printfn "  EXPECTED:"                
            printfn "  %A" expected
    
    let onDataSet f print filename : unit=
        printfn "**** Executing Dataset: %s ****" filename
        filename
        |> Files.toLines
        |> Seq.toArray
        |> f
        |> print

    let testOnDataSet f filename = onDataSet f (testPrint 0) filename
    let testOnDataSets f filenames = filenames |> Seq.iter (testOnDataSet f)
//    let testOnDataSets f filename linesPerTest =
//        onDataSet (Array.segmented linesPerTest >> Array.map f) (Array.iteri (testPrint) filename

module Char =
    let toString : char seq -> string =  Seq.map string >> String.concat ""
    let incr (x : char) = char (1 + int x)

module String =
    let halve s =
        let isEven i =  i % 2 = 0
        match String.length s with
        | l when isEven l ->
            let i = (l - 1) / 2
            (s.[..i],s.[(i + 1)..], "")
        | l ->
            let i = ((l - 1) / 2) - 1
            (s.[..i],s.[(i + 2)..], (string s.[i+1]) )
    let splitChar (c:char) (s:string) =  s.Split(c)
    let split s = splitChar ' ' s
    let toInts s = split s |> Array.map int
    let toFloats s = split s |> Array.map float
    let toChars (s:string) = [| for c in s -> c |]
    let windowed n s = s |> Seq.windowed n |> Seq.map Char.toString
    let unwindowed (kmers: string seq) = 
        let head = Seq.head kmers
        let i = (String.length head) - 1
        let tail =
            kmers
            |> Seq.tail 
            |> Seq.fold (fun acc kmer -> kmer.[i]::acc) []
            |> Seq.rev 
            |> Char.toString
        head + tail

module Array = 
    let halve (s: 'a array)  =
        let isEven i =  i % 2 = 0
        match Array.length s with
        | l when isEven l ->
            let i = (l - 1) / 2
            (s.[..i],s.[(i + 1)..], [||])
        | l ->
            let i = ((l - 1) / 2) - 1
            (s.[..i],s.[(i + 2)..], [| s.[i+1] |] )
    let binarySearch (indexes: 'a array) (value: 'a) =
        let rec inner low high = 
            if (high < low) then
                None
            else
                let mid = (low + high) / 2
                match (compare indexes.[mid] value) with
                | 0 -> Some(mid)
                | 1 -> inner low (mid - 1)  //search low
                | _ -> inner (mid + 1) high //search high
        inner 0 (Array.length indexes)

    let swap i j (arr : 'a []) =
        let tmp = arr.[i]
        arr.[i] <- arr.[j]
        arr.[j] <- tmp

    let insertionSort (arr : 'a []) =
        for i = 1 to arr.Length - 1 do
            let mutable j = i
            while j >= 1 && arr.[j] < arr.[j-1] do
                swap j (j-1) arr
                j <- j - 1
        arr
    let segmented (size:int) (source:'t[]) =
        let maxPos = source.Length - 1
        [| for pos in 0 .. size .. maxPos ->
               source.[pos .. min (pos + size - 1) maxPos] |]

module List = 
    let rec private insertions x = function
        | []             -> [[x]]
        | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

    let rec permutations = function
        | []      -> seq [ [] ]
        | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))

module Seq =
    let print_str   xs = xs |> Seq.iter (printf "%s")
    let prints_str  xs = xs |> Seq.iter (printf "%s ")
    let printn_str  xs = xs |> print_str; printfn "";
    let printsn_str xs = xs |> prints_str; printfn "";
    let sprints_str  xs = xs |> Seq.map (sprintf "%s") |> String.concat " "

    let sprint  xs = xs |> Seq.map (sprintf "%A") |> String.concat ""
    let sprints xs = xs |> Seq.map (sprintf "%A ") |> String.concat ""
    let print   xs = xs |> Seq.iter (printf "%A")
    let prints  xs = xs |> Seq.iter (printf "%A ")
    let printn  xs = xs |> print; printfn "";
    let printsn xs = xs |> prints; printfn "";
    let printi  xs = xs |> Seq.iteri (printf "%i: %A")
    let printni xs = xs |> printi; printfn "";
    let printPS  prefix suffix xs = xs |> Seq.iter (fun x   -> printf "%s%A%s" prefix x suffix)
    let printPSn prefix suffix xs = xs |> printPS prefix suffix; printfn "";
        
    let echo f (s:'a seq) = s |> f; s;

    let toString xs = Seq.fold (fun acc x ->  acc + (sprintf "%A" x)) "" xs
    
//    let toDictionary<'a ,'b when 'a:equality> pairs = 
//        let result = new System.Collections.Generic.Dictionary<'a,'b>()
//        pairs |> Seq.iter (fun (key,count) -> result.[key] <- count)
//        result

    let unfoldCached f state = state |> Seq.unfold f |> Seq.cache

    let filteri2 f xs ys = Seq.mapi2 f xs ys |> Seq.choose id

    let inline groupBySum xs =
        xs |> Seq.groupBy fst
        |> Seq.map (fun (key, items) -> (key, items |> Seq.map snd |> Seq.sum))
    
    let inline groupByList xs =
        xs |> Seq.groupBy fst
        |> Seq.map (fun (key, items) -> (key, items |> Seq.map snd |> List.ofSeq ))

    let copyValueTo value ids =
        ids |> Seq.map (fun id -> (id,value))

    let copyValuesTo idGenerator pairs =
        pairs |> Seq.collect (fun (id,c) -> copyValueTo c (idGenerator id))

    let cartesian xs ys = xs |> Seq.collect (fun x -> ys |> Seq.map (fun y -> x, y))
    let cartesianForwardOnly xs ys = 
        let ys' = ys |> Seq.cache
        xs |> Seq.mapi (fun i x -> ys' |> Seq.take (i+1) |> Seq.map (fun y -> x, y))
        |> Seq.collect (fun xs -> xs)
    let permutations xs = xs |> Seq.toList |> List.permutations 
    let fibonacci = unfoldCached (fun (x,y) -> Some(x, (x + y, x))) (0,1)
    let fibonacciCustom f state = unfoldCached f state

module Dict =
    open System.Collections.Generic
    let toSeq d = d |> Seq.map (fun (KeyValue(k,v)) -> (k,v))
    let toArray (d:IDictionary<_,_>) = d |> toSeq |> Seq.toArray
    let toList (d:IDictionary<_,_>) = d |> toSeq |> Seq.toList
    let ofMap (m:Map<_,_>) = new Dictionary<_,_>(m) :> IDictionary<_,_>
    let ofList l = new Dictionary<_,_>(l |> Map.ofList) :> IDictionary<_,_>
    let ofSeq s = new Dictionary<_,_>(s |> Map.ofSeq) :> IDictionary<_,_>
    let ofArray (a:(_ * _) []) = new Dictionary<_,_>(a |> Map.ofArray) :> IDictionary<_,_>
    let addPair (dict:Dictionary<_, _ list>) (key,value) =
        if dict.ContainsKey(key) 
        then dict.[key] <- value::dict.[key]
        else dict.[key] <- [value]
