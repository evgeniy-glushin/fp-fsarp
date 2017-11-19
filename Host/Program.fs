
open System
open OrElseWorkflowBuilder


let val1 () =
    printfn "val1 called"
    Error "Ooooops. Something went wrong!"

let val2 () =
    printfn "val2 called"
    Ok 10

//let orElse = new OrElseWorkflow()

let add f1 f2 =
    orElse {
        return! f2()
        return! f1()

        //printf "after return"

        //let! x1 = f1()
        //let! x2 = f2()

        //return x1 + x2
    }

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
   
    let r = add val1 val2

    match r with
    | Ok res -> printfn "result: %d" res
    | Error err -> printfn "result: %s" err
   
    Console.ReadKey()
    0 // return an integer exit code
