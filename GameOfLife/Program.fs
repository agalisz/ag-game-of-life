// Implements Conway's Game Of Life
// https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life on a torus

open System

[<EntryPoint>]
let main argv =
    let fileToRead = if argv.Length > 0 then argv.[0] else "sample_input.txt"
    let all_text = System.IO.File.ReadAllText fileToRead
    let initialWorld = GameOfLifeSerialization.deserialize all_text
    let evolvedWorld = GameOfLife.evolve initialWorld
    let result = GameOfLifeSerialization.serialize evolvedWorld
    Console.WriteLine result
    0
