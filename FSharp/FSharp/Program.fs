module FSharpHometask =

    let fibonacci n = 
        let fibs = Seq.unfold (fun (a,b) -> Some( a+b, (b, a+b) ) ) (0,1)
        fibs |> Seq.take n |> Seq.toList

    let reverse list = 
        let rec rev list tail = 
            match list with
            | [] -> tail
            | x::xs -> rev xs (x::tail)
        rev list []

    let rec merge fstPart sndPart = 
        match (fstPart, sndPart) with
        | _, [] -> fstPart
        | [], _ -> sndPart
        | (x::xs), (y::ys) when x <= y -> x::(merge xs (y::ys))
        | (x::xs), (y::ys) -> y::(merge (x::xs) ys)

    let rec mergesort list = 
        let fsthalf list = list |> Seq.take (List.length list / 2) |> Seq.toList
        let sndhalf list = list |> Seq.skip (List.length list / 2) |> Seq.toList
        match list with
        | [] -> []
        | [x] -> [x]
        | _ -> merge (mergesort (fsthalf list)) (mergesort (sndhalf list))

    type Expression = 
        | Term of int
        | Add of Expression * Expression
        | Sub of Expression * Expression
        | Mul of Expression * Expression
        | Div of Expression * Expression
        | Mod of Expression * Expression
        | Var of string

    let rec eval (env:Map<string,int>) (expr: Expression) =
        match expr with
        | Term n -> n
        | Add(expr1, expr2) -> eval env expr1 + eval env expr2
        | Sub(expr1, expr2) -> eval env expr1 - eval env expr2
        | Mul(expr1, expr2) -> eval env expr1 * eval env expr2
        | Div(expr1, expr2) -> eval env expr1 / eval env expr2
        | Mod(expr1, expr2) -> eval env expr1 % eval env expr2
        | Var a -> env.[a]

    let factor n = seq{for m in 1..n / 2 do if n % m = 0 then yield m}
    let isPrime n = (factor n |> Seq.toList) = [1]
    let primes =  Seq.filter isPrime <| Seq.initInfinite (fun x ->x + 2)