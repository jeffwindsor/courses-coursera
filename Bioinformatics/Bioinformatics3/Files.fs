namespace Bioinformatics

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
        let splitChar (c:char) (line:string) =  line.Split(c)
        let split line = splitChar ' ' line
        let splitMap f line = split line |> Array.map f
        let toInts line = splitMap int line
        let toInts2D columns lines =
            let sources = lines |> Array.map toInts 
            Array2D.init (Array.length sources) columns (fun i j -> sources.[i].[j])
        let toFloats line = splitMap float line
        let toCharacterStringArray (s:char seq) = s |> Seq.map string |> Seq.toArray

module Problems =
    let private filename id set = sprintf "%s-%s" id set
    let private print prefix value = printfn "  %s: %A" prefix value
    let private printExpected expected =  print "EXPECTED" expected
    let private printAreEqualResults actual expected =
        match actual = expected with
        | true  -> print "SUCCESS " actual
        | false -> print "FAILURE " actual
    let private printAreEqual actual expected = 
        printAreEqualResults actual expected 
        printExpected expected
        printfn ""

    let private printAreAnyEqual actuals expected = 
        actuals |> Seq.iter (fun a -> printAreEqualResults a expected)
        printExpected expected 
        printfn ""

    let private apply f filename =
        printfn "**** %s ****" filename
        filename |> Files.toLinesArray |> f
    
    let areEqual actual expected testName = 
        printfn "**** %s ****" testName
        printAreEqual actual expected

    let solve id setName f = filename id setName |> apply f
    let validate id setName f = solve id setName f ||> printAreEqual
    let validateAny id setName f = solve id setName f ||> printAreAnyEqual
    
    //Special Cases : just for this class
    let sample id f   = validate id "Sample" f
    let sampleAny id f= validateAny id "Sample" f
    let extra id f    = validate id "Extra" f
    let extraAny id f = validateAny id "Extra" f
    let dataset id f  = solve id "Dataset" f
    let rosalind id f = solve id "Rosalind" f