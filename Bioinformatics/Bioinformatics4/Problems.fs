namespace Bioinformatics
open System

[<AutoOpen>]
module Problems =
    module Files =
        open System.IO
        let toLines filepath =
            seq { 
                use stream = File.OpenRead (filepath + ".txt")
                use reader = new StreamReader(stream)
                while not reader.EndOfStream do yield reader.ReadLine()
            }
        let toLinesArray = toLines >> Seq.toArray
        let firstLine = toLines >> Seq.head
        
        module Lines =
            let private convertToArray2D<'a> columns (sources:'a[][]) = Array2D.init (Array.length sources) columns (fun i j -> sources.[i].[j])
            let splitChar (c:char) (line:string) =  line.Split(c)
            let split line = splitChar ' ' line
            let splitMap f line = split line |> Array.map f
            
            let toInts (line:string) = line.Trim() |> splitMap int
            let toInts2D columns lines = lines |> Array.map toInts |> convertToArray2D columns
            let toFloats (line:string) = line.Trim() |> splitMap float
            let toFloats2D columns lines = lines |> Array.map toFloats |> convertToArray2D columns
            let toCharacterStringArray (s:char seq) = s |> Seq.map string |> Seq.toArray
    module Strings =
        let toChars (s:string) = [| for c in s -> c |]
    module Chars =
        let toString : char seq -> string =  Seq.map string >> String.concat ""
    
    let Sample = "Sample"
    let Sample2 = "Sample2"
    let Extra = "Extra"
    let Dataset = "Dataset"
    let Rosalind = "Rosalind"
    let private filename id set = sprintf "%s-%s" id set
    let private print prefix value = printfn "  %s: %A" prefix value
    let private printExpected expected =  print "EXPECTED" expected
    let private printAreEqualResults actual expected =
        match actual = expected with
        | true  -> print "SUCCESS " actual
        | false -> print "FAILURE " actual
    let AreEqual actual expected = 
        printAreEqualResults actual expected 
        printExpected expected
        printfn ""
    let AreEqualListComparison actual expected = 
        let actual' = Set.ofList actual
        let expected' = Set.ofList expected
        let diffa = Set.difference actual' expected'
        let diffe = Set.difference expected' actual'
        match (Set.count diffa) + (Set.count diffe) with
        | 0 -> print "SUCCESS " actual
        | _ -> printfn "FAILURE"
//               printfn "DIFFE"
//               Set.iter (fun d -> printfn "%A" d) diffe
//               printfn "DIFFA"
//               Set.iter (fun d -> printfn "%A" d) diffa 
               printfn "ACTUAL "
               List.iter (fun d -> if (Set.contains d diffa) then printfn "%A *" d else printfn "%A" d) actual 
               printfn "EXPECTED "
               List.iter (fun d -> if (Set.contains d diffe) then printfn "%A *" d else printfn "%A" d) expected 
    
    let private apply f filename =
        printfn "**** %s ****" filename
        filename |> Files.toLinesArray |> f
    let Solve id setName f = filename id setName |> apply f

//    let private printAreAnyEqual actuals expected = 
//        actuals |> Seq.iter (fun a -> printAreEqualResults a expected)
//        printExpected expected 
//        printfn ""
    
//    let areEqual actual expected testName = 
//        printfn "**** %s ****" testName
//        printAreEqual actual expected
