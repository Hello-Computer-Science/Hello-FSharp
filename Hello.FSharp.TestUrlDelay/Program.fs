open System
open System.Collections.Generic
open System.Threading.Tasks

let random = new Random()

let TestDelay url =
    let delay = (random.Next(1, 15) * 300)
    printfn "%O Testing %s ... (It will take %d ms)" DateTime.UtcNow url delay
    Async.Sleep(delay / 3 |> int) |> Async.RunSynchronously
    (url, delay)

let urls = [ "www.baidu.com"; "www.google.com"; "www.bilibili.com" ]
let queries = 
    urls
    |> List.map (fun url -> 
        Task.Factory.StartNew((fun () -> TestDelay url), TaskCreationOptions.None)
        |> Async.AwaitTask)

let fastest = queries |> Async.Parallel |> Async.RunSynchronously |> Array.minBy snd

printfn "%O %s ms -> Url %s" DateTime.UtcNow (snd fastest |> string) (fst fastest)

