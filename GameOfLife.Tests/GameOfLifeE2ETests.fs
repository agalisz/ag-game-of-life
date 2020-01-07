namespace GameOfLife.Tests

open System
open NUnit.Framework

[<TestFixture>]
type GameOfLifeE2ETests () =

    member this.NL = Environment.NewLine

    member this.Verify (expected : string) (initial : string) = 
        let initialWorld = GameOfLifeSerialization.deserialize initial
        let evolvedWorld = GameOfLife.evolve initialWorld
        let result = GameOfLifeSerialization.serialize evolvedWorld
        Assert.AreEqual(expected.Trim(), result.Trim())

    [<Test>]
    member this.OneCellDoesNotSurvive() =
        this.Verify "_" "*"

    [<Test>]
    member this.BlockIsStill() =
        let initial = System.String.Join(this.NL, [
            "____"
            "_**_"
            "_**_"
            "____"])
        let expected = initial
        this.Verify expected initial

    [<Test>]
    member this.TubIsStill() =
        let initial = System.String.Join(this.NL, [
            "_*_"
            "*_*"
            "_*_"])
        let expected = initial
        this.Verify expected initial

    [<Test>]
    member this.BlinkerOscillates() =
        let initial = System.String.Join(this.NL, [
            "_____"
            "__*__"
            "__*__"
            "__*__"
            "_____"])
        let expected = System.String.Join(this.NL, [
            "_____"
            "_____"
            "_***_"
            "_____"
            "_____"])
        this.Verify expected initial
        this.Verify initial expected

    [<Test>]
    member this.DiamondShapeEvolvesToSquare() =
        let initial = System.String.Join(this.NL, [
            "_____"
            "__*__"
            "_***_"
            "__*__"
            "_____"])
        let expected = System.String.Join(this.NL, [
            "_____"
            "_***_"
            "_*_*_"
            "_***_"
            "_____"])
        this.Verify expected initial

    [<Test>]
    member this.SquareEvolvesToDiamondShape() =
        let initial = System.String.Join(this.NL, [
            "_____"
            "_***_"
            "_*_*_"
            "_***_"
            "_____"])
        let expected = System.String.Join(this.NL, [
            "__*__"
            "_*_*_"
            "*___*"
            "_*_*_"
            "__*__"])
        this.Verify expected initial

    [<Test>]
    member this.TorusOnTopLeft() =
        let initial = System.String.Join(this.NL, [
            "_*__"
            "*___"
            "____"
            "___*"])
        let expected = System.String.Join(this.NL, [
            "*___"
            "____"
            "____"
            "____"])
        this.Verify expected initial

    [<Test>]
    member this.TorusOnTopRight() =
        let initial = System.String.Join(this.NL, [
            "__*_"
            "___*"
            "____"
            "*___"])
        let expected = System.String.Join(this.NL, [
            "___*"
            "____"
            "____"
            "____"])
        this.Verify expected initial

    [<Test>]
    member this.TorusOnBottomLeft() =
        let initial = System.String.Join(this.NL, [
            "___*"
            "____"
            "*___"
            "_*__"])
        let expected = System.String.Join(this.NL, [
            "____"
            "____"
            "____"
            "*___"])
        this.Verify expected initial

    [<Test>]
    member this.TorusOnBottomRight() =
        let initial = System.String.Join(this.NL, [
            "*___"
            "____"
            "___*"
            "__*_"])
        let expected = System.String.Join(this.NL, [
            "____"
            "____"
            "____"
            "___*"])
        this.Verify expected initial
