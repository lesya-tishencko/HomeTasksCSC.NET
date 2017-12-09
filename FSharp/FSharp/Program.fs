module FSharpHometask =

    let fibonacci n = 
        let fibs = Seq.unfold (fun (a, b) -> Some(a + b, (b, a + b))) (bigint(0), bigint(1))
        fibs |> Seq.take n |> Seq.toList

    let fstTenFibs = fibonacci 10 |> Seq.map (fun x -> int x) |> Seq.toList // [1; 1; 2; 3; 5; 8; 13; 21; 34; 55; 89]

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
        | (x::xs), (y::ys) when x <= y -> x::(merge xs sndPart)
        | (x::xs), (y::ys) -> y::(merge fstPart ys)

    let rec mergesort list = 
        let fsthalf list = list |> Seq.take (List.length list / 2) |> Seq.toList
        let sndhalf list = list |> Seq.skip (List.length list / 2) |> Seq.toList
        match list with
        | [] -> []
        | [x] -> [x]
        | _ -> merge (mergesort (fsthalf list)) (mergesort (sndhalf list))

    type Expression = 
        | Num of int
        | Add of Expression * Expression
        | Sub of Expression * Expression
        | Mul of Expression * Expression
        | Div of Expression * Expression
        | Mod of Expression * Expression
        | Var of string

    let rec eval (env:Map<string,int>) (expr: Expression) =
        match expr with
        | Num n -> n
        | Add(expr1, expr2) -> eval env expr1 + eval env expr2
        | Sub(expr1, expr2) -> eval env expr1 - eval env expr2
        | Mul(expr1, expr2) -> eval env expr1 * eval env expr2
        | Div(expr1, expr2) -> eval env expr1 / eval env expr2
        | Mod(expr1, expr2) -> eval env expr1 % eval env expr2
        | Var a -> env.[a]

    let evalExample = 
        let environment = Map.ofList ["a", 2; "b", -2]
        let expressionTree = Add(Mul(Num 1, Sub(Var "a", Var "b")), Div(Num 24, Num 8))
        eval environment expressionTree // 7

    let factor n = seq{for m in 2..(n |> float|> sqrt |> int) do if n % 2 = 1 && n % m = 0 then yield m}
    let isPrime n = (factor n |> Seq.toList) = [1]
    let primes =  Seq.filter isPrime <| Seq.initInfinite (fun x -> x + 2)

    let fstTenPrimes = primes |> Seq.truncate 10 |> Seq.toList // [2; 3; 5; 7; 11; 13; 17; 19; 23; 29]