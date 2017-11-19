module TextPerser

open System

type Parser<'a> = Parser of (string -> Result<'a, string>)

let pchar charToMatch = Parser (fun str ->
    if String.IsNullOrEmpty(str) then
        Error "No more input"
    else
        let first = str.[0]
        if first = charToMatch then
            let remaining = str.[1..]
            Ok (first, remaining)
        else
            Error "Expecting..."
)

let run parser inp =
    let (Parser f) = parser
    f inp

let andThen parser1 parser2 = Parser (fun input ->
    let r1 = run parser1 input
    match r1 with
    | Error msg -> Error msg
    | Ok (val1, remaining1) ->  
        let r2 = run parser2 remaining1
        match r2 with
        | Error msg -> Error msg
        | Ok (val2, remaining2) ->
            let newVal = val1, val2
            Ok (newVal, remaining2)
 )

let ( .>>. ) = andThen

let orElse parser1 parser2 = Parser (fun input ->
    let r1 = run parser1 input
    match r1 with
    | Ok _ -> r1
    | Error _ -> run parser2 input
)

let ( <|> ) = orElse

let choise listOfParsers =
    List.reduce ( <|> ) listOfParsers

let anyOf listOfChars =
    listOfChars |> List.map pchar |> choise

let mapP f parser = Parser ( fun input -> 
    let result = run parser input
    
    match result with 
    | Ok(value, remaining) -> Ok(f value, remaining)
    | Error err -> Error err
)

let ( <!> ) = mapP

let ( |>> ) parser f = mapP f parser 

let sequence listOfParsers =
    let concat p1 p2 =
        p1 .>>. p2
        |>> (fun(l1, l2) -> l1 @ l2)

    listOfParsers
    |> Seq.map (fun p -> p |>> List.singleton)
    |> Seq.reduce concat

let pstring str =
    str
    |> Seq.map pchar
    |> sequence
    |>> Seq.toArray
    |>> String

let returnP x = Parser (fun input ->
    Ok(x, input)    
)

let applyP fP xP =
    (fP .>>. xP)
    |>> (fun (f,x) -> f x)

let ( <*> ) = applyP

let parseDigit = anyOf ['0'..'9']

let parseThreeDigits =
    parseDigit .>>. parseDigit .>>. parseDigit 
    |>> fun ((a, b), c) -> String [|a;b;c|]
    |>> int

run parseThreeDigits "1234A"

//let parseLowercase = 
//    anyOf ['a'..'z']

//let parseDigit = 
//    anyOf ['0'..'9']

//run parseLowercase "aBC"  // Success ('a', "BC")
//run parseLowercase "ABC"  // Failure "Expecting 'z'. Got 'A'"

//run parseDigit "1ABC"  // Success ("1", "ABC")
//run parseDigit "9ABC"  // Success ("9", "ABC")
//run parseDigit "|ABC"  // Failure "Expecting '9'. Got '|'"

//let parseA = pchar 'A'   
//let parseB = pchar 'B'
//let parseC = pchar 'C'
//let bOrElseC = parseB <|> parseC
//let aAndThenBorC = parseA .>>. bOrElseC 

//run aAndThenBorC "ABZ"  // Success (('A', 'B'), "Z")
//run aAndThenBorC "ACZ"  // Success (('A', 'C'), "Z")
//run aAndThenBorC "QBZ"  // Failure "Expecting 'A'. Got 'Q'"
//run aAndThenBorC "AQZ"  // Failure "Expecting 'C'. Got 'Q'"

