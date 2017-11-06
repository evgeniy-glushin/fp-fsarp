module rec Balancing 

let balance (chars: string) =  
    let rec parenthesesCounter balancerAcc items =
        match items with
        | [] -> balancerAcc = 0
        | head :: tail when balancerAcc >= 0 -> 
            let balancerCur = 
                if head = '(' then balancerAcc + 1
                elif head = ')' then balancerAcc - 1
                else balancerAcc                
            
            parenthesesCounter balancerCur tail
        | _ when balancerAcc < 0 -> false 
        
    let charsList = chars.ToCharArray() |> Array.toList
    parenthesesCounter 0 charsList
