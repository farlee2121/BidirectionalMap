module Tests

open System
open Xunit
open FsCheck.Xunit
open System.Collections.Generic
open FsCheck

let (|=) left right = (Array.ofSeq left) = (Array.ofSeq right) 

module Tuple = 
    let fromKeyValue (KeyValue(key, value)) = (key, value) 
    let reverse (a, b) = (b,a)

module KeyValuePair = 
    let reverse (KeyValue(key, value)) = KeyValuePair.Create(value, key) 

[<Property()>]
let ``All initialization methods create the same map`` (mapVals) =
    //NOTE Using Guid instead of NonNull<string> to avoid reverse key conflicts
    let constructor = new BiMap<int, Guid>(mapVals)
    let iterative = new BiMap<int,Guid>()
    mapVals |> Seq.iter (fun kvp -> iterative.Add(kvp.Key, kvp.Value))  
    constructor |= iterative

[<Property>]
let ``forward set = reverse set`` (mapVals) =
    let map = new BiMap<int, Guid>(mapVals)
    map.Forward |= (map.Reverse |> Seq.map KeyValuePair.reverse)

[<Property>]
let ``One to one`` (mapVals) =
    let map = new BiMap<int, Guid>(mapVals)
    map.Forward.Keys
        |>  Seq.forall (fun key -> key = map.Reverse.[map.Forward.[key]])
    && map.Reverse.Keys
        |>  Seq.forall (fun key -> key = map.Forward.[map.Reverse.[key]])

[<Property>]
let ``Top-level as set equals forward set`` (mapVals) =
    let map = new BiMap<int, Guid>(mapVals)
    map |= map.Forward

// ?? after remove, neither set contains the key??