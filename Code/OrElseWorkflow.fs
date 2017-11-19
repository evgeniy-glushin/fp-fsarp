[<AutoOpen>]
module OrElseWorkflowBuilder

type OrElseWorkflow() =

    member __.Bind(x, f) = 
        match x with
        | Ok res -> f res
        | Error err -> Error err

    member __.Return(x) =
        Ok x

    member __.ReturnFrom(x) =
        x

    member __.Combine(a, b) =
        match a with
        | Ok res -> Ok res
        | Error _ -> 
            match b with 
            | Ok res -> Ok res
            | Error err -> Error err

    member __.Zero() =
        Error "no implementation"

    member __.Delay(f) =
        f()

let orElse = new OrElseWorkflow()
