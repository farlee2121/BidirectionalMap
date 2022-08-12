Bidirectional Map
=================
![build](https://github.com/farlee2121/BidirectionalMap/workflows/Build/badge.svg)
[![codecov](https://codecov.io/gh/farlee2121/BidirectionalMap/branch/master/graph/badge.svg)](https://codecov.io/gh/farlee2121/BidirectionalMap)
[![Nuget](https://img.shields.io/nuget/v/BidirectionalMap)](https://www.nuget.org/packages/BidirectionalMap/)

Exactly what it sounds like. This library offers a single class `BiMap` that let's you define a two-way one-to-one map between values
the same way you would define a one-way map with a dictionary.

Example:
--------

```cs
using BidirectionalMap;

var capitalCountryMap = new BiMap<string, string>()
{
    { "Italy", "Rome" },
    { "Mumbai", "India" },
    { "USA", "Washington, D.C." },
};

var captial = map.Forward["USA"]); // "Washington, D.C."
var country = map.Reverse["Washington, D.C."]; // "USA"
```

Why?
---

Well, table-driven value mapping is a very powerfull technique that makes conversions more readable, easier to update, and easier to load from non-code sources.  
Some common scenarios for this kind of technique include
 - Mapping to some kind of storage (say, converting between enum and string)
 - Mapping display values to and from requests
 - Wrapping other code (adapter-style) to consume the api on your own terms
 - Choosing an action or configuration based on some kind of type value (this is usually just one-way though)

Install
-------

Available via nuget at https://www.nuget.org/packages/BidirectionalMap/	


Feedback/Bugs/Contribution
--------------------------
 
 Feel free to open an issue to start the conversation. 
