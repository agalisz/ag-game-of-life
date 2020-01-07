module GameOfLife

type private CellState = Alive | Dead

type private NumberOfNeighbors = NumberOfNeighbors of int

type private LifeRule = CellState * NumberOfNeighbors -> CellState

type CellCoordinate = { X : int; Y : int }

type private CellInfo = {
    CellCoordinate : CellCoordinate;
    CellState : CellState;
    NumberOfNeighbors : NumberOfNeighbors
}

type private NeighborhoodRule = CellCoordinate -> CellCoordinate list

type World = {
    AliveCells : CellCoordinate list;
    SizeX : int;
    SizeY : int
}

let private conwayLifeRule : LifeRule = function
    | (Alive, NumberOfNeighbors 2) -> Alive
    | (_, NumberOfNeighbors 3) -> Alive
    | _ -> Dead

let private neighboursOnTorus (sizeX : int) (sizeY : int) (cell : CellCoordinate) : CellCoordinate list =
    let x = cell.X
    let y = cell.Y

    let xLeft = if x - 1 < 0 then sizeX - 1 else x - 1
    let xRight = if x + 1 = sizeX then 0 else x + 1
    let yUp = if y - 1 < 0 then sizeY - 1 else y - 1
    let yDown = if y + 1 = sizeY then 0 else y + 1

    [
        { X = xLeft; Y = yUp }; { X = x; Y = yUp }; { X = xRight; Y = yUp };
        { X = xLeft; Y = y }; { X = xRight; Y = y };
        { X = xLeft; Y = yDown }; { X = x; Y = yDown }; { X = xRight; Y = yDown };
    ] |> Set.ofList |> Set.remove cell |> Set.toList

let private neighbours (cells : CellCoordinate list) (neighborhoodRule : NeighborhoodRule) : CellCoordinate list =
    List.collect neighborhoodRule cells

let private countNeighbours (cells : CellCoordinate list) (neighborhoodRule : NeighborhoodRule) : (CellCoordinate * NumberOfNeighbors) list =
    neighbours cells neighborhoodRule
        |> Seq.countBy id
        |> Seq.toList
        |> List.map (fun (cell, count) -> (cell, NumberOfNeighbors count))

let private isAlive (aliveCells : Set<CellCoordinate>) (cell : CellCoordinate) =
    if aliveCells.Contains cell then Alive else Dead

let private evolveOnce (world : World) (lifeRule : LifeRule) (neighborhoodRule : NeighborhoodRule) : World =
    let startAliveCells = world.AliveCells 
    let startAliveCheck = Set.ofList startAliveCells |> isAlive
    let cellsInfos = countNeighbours startAliveCells neighborhoodRule
                        |> List.map (fun (cell, numberOfNeighbors) -> {
                            CellCoordinate = cell;
                            CellState = startAliveCheck cell;
                            NumberOfNeighbors = numberOfNeighbors
                        })

    let evolvedCells = cellsInfos
                        |> List.map (fun cellInfo -> (cellInfo.CellCoordinate, lifeRule (cellInfo.CellState, cellInfo.NumberOfNeighbors)))
                        |> List.where (fun (_, isAlive) -> isAlive = Alive)
                        |> List.map fst

    { world with AliveCells = evolvedCells }

// evolve once using Conway's rules on a torus
let evolve (world : World) : World = evolveOnce world conwayLifeRule (neighboursOnTorus world.SizeX world.SizeY)
