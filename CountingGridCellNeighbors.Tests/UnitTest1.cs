using System;
using System.IO;
using Xunit;
using CountingGridCellNeighbors;

namespace CountingGridCellNeighbors.Tests;

// ===== Manhattan Tests =====
public class ManhattanTests
{
    [Fact]
    public void Manhattan_Center_CorrectCount_NoWrap()
    {
        var grid = GridGenerator.Generate(11, 11, 0);
        grid[5, 5] = new Cell(5, 5, 1);

        int count = NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Manhattan, wrap: false);
        Assert.Equal(5, count);
    }

    [Fact]
    public void Manhattan_WrapAround_CornerToCorner()
    {
        var grid = GridGenerator.Generate(11, 11, 0);
        grid[0, 0] = new Cell(0, 0, 1);
        grid[10, 0] = new Cell(10, 0);

        NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Manhattan, wrap: true);
        Assert.True(grid[10, 0].IsNeighbor);
    }
}

// ===== Chebyshev Tests =====
public class ChebyshevTests
{
    [Fact]
    public void Chebyshev_Center_CorrectCount_NoWrap()
    {
        var grid = GridGenerator.Generate(11, 11, 0);
        grid[5, 5] = new Cell(5, 5, 1);

        int count = NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Chebyshev, wrap: false);
        Assert.Equal(9, count);
    }

    [Fact]
    public void Chebyshev_WrapAround_EdgeNeighbors()
    {
        var grid = GridGenerator.Generate(3, 3, 0);
        grid[0, 0] = new Cell(0, 0, 1);

        NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Chebyshev, wrap: true);
        Assert.True(grid[2, 2].IsNeighbor); // Diagonal wrap
    }
}

// ===== Euclidean Tests =====
public class EuclideanTests
{
    [Fact]
    public void Euclidean_Center_CorrectCount_Radius2()
    {
        var grid = GridGenerator.Generate(11, 11, 0);
        grid[5, 5] = new Cell(5, 5, 1);

        int count = NeighborhoodFinder.FindNeighbors(grid, 2, DistanceType.Euclidean, wrap: false);
        Assert.True(count > 9); // Should include more than Chebyshev
    }

    [Fact]
    public void Euclidean_Edge_WrapTest()
    {
        var grid = GridGenerator.Generate(5, 5, 0);
        grid[0, 0] = new Cell(0, 0, 1);

        NeighborhoodFinder.FindNeighbors(grid, 2, DistanceType.Euclidean, wrap: true);
        Assert.True(grid[4, 4].IsNeighbor);
    }
}

// ===== Edge Case Tests =====
public class EdgeCaseTests
{
    [Fact]
    public void SingleCell_Grid_ShouldCountOne()
    {
        var grid = GridGenerator.Generate(1, 1, 0);
        grid[0, 0] = new Cell(0, 0, 1);

        int count = NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Manhattan, wrap: false);
        Assert.Equal(1, count);
    }

    [Fact]
    public void OneRowGrid_BorderPositive_NoWrap()
    {
        var grid = GridGenerator.Generate(1, 5, 0);
        grid[0, 0] = new Cell(0, 0, 1);

        int count = NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Manhattan, wrap: false);
        Assert.Equal(2, count); // Should include self and one neighbor
    }
}

// ===== Random Grid Tests =====
public class RandomGridTests
{
    [Fact]
    public void RandomGrid_ShouldAlwaysFindPositives()
    {
        var grid = GridGenerator.Generate(11, 11, 5, seed: 123);
        int count = NeighborhoodFinder.FindNeighbors(grid, 2, DistanceType.Chebyshev, wrap: false);

        Assert.True(count >= 5); // Each positive includes itself
    }
}

// ===== Visual Output Tests =====
public class VisualOutputTests
{
    [Fact]
    public void GridPrinter_ShouldRenderExpectedSymbols()
    {
        var grid = GridGenerator.Generate(3, 3, 0);
        grid[1, 1] = new Cell(1, 1, 1);

        NeighborhoodFinder.FindNeighbors(grid, 1, DistanceType.Chebyshev, wrap: false);

        using var sw = new StringWriter();
        Console.SetOut(sw);

        GridPrinter.Print(grid);
        var output = sw.ToString();

        Assert.Contains("▩", output); // positive
        Assert.Contains("☒", output); // neighbor
        Assert.Contains("☐", output); // empty
    }
}
