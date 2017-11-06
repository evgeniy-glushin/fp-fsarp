module CountChange

let  countChange (money: int) (coins: int list)  =   
    let allEqTo a l =
        l |> List.exists (fun x -> a <> x) |> not

    let rec iter total items =            
        match items with
        | [] -> total
        | head :: tail ->            
            let count, _, _, _ = 
                tail
                |> List.map (fun x -> (0, x, 0, false))
                |> List.fold (fun (count, acc, idx, terminated) (_, cur, _, _) -> 
                                let nextAcc, nextIdx = acc + cur, idx + 1
                                if terminated then count, acc, nextIdx, true
                                elif nextAcc < money then count, nextAcc, nextIdx, false
                                elif nextAcc = money then count + 1, 0, nextIdx, (allEqTo acc tail.[..idx])
                                else count, acc, nextIdx, false) (0, head, 0, false)
        
            iter (count + total) tail
        
    match List.filter (fun x -> x <= money) coins with
    | [] -> 0
    | [a] when a = money -> 1
    | posibleCoins ->
        posibleCoins
        |> List.collect (fun x -> [ for _ in 1..money/x do yield x])
        |> List.sortDescending
        |> iter 0


let  countChangeList (money: int) (coins: ResizeArray<int>) =
    List.ofSeq coins |> countChange money
