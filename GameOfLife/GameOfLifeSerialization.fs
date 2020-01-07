module GameOfLifeSerialization

open GameOfLife
open System

let serialize (world: World) : string =
    let sb = new System.Text.StringBuilder()
    let aliveCells = Set.ofList world.AliveCells

    for y in 0 .. world.SizeY - 1 do
        for x in 0 .. world.SizeX - 1 do
            let cell = { CellCoordinate.X = x; Y = y }
            let isAlive = aliveCells.Contains cell
            let charRepresentation = if isAlive then '*' else '_'
            sb.Append(charRepresentation) |> ignore
        sb.AppendLine() |> ignore

    sb.ToString()

let deserialize (text: string) : World =
    let lines = text.Split(Environment.NewLine)
    let aliveCells = ResizeArray<CellCoordinate>()

    for y in 0 .. lines.Length - 1 do
        let line = lines.[y]
        for x in 0 .. line.Length - 1 do
            let cell = { CellCoordinate.X = x; Y = y }
            let isAlive = line.[x] = '*'
            if isAlive then aliveCells.Add(cell)

    {
        AliveCells = Seq.toList aliveCells;
        SizeX = (if lines.Length > 0 then lines.[0].Length else 0);
        SizeY = lines.Length
    }