// See https://aka.ms/new-console-template for more information

using System;
using CountingGridCellNeighbors;

float[][] test1 = [
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1]
];

float[][] test2 = [
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1],
   [-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1]
];



// Matrix matrix = new Matrix(11, 11);

// Console.WriteLine(matrix.ToString());

Console.OutputEncoding = System.Text.Encoding.UTF8;

Cell[,] grid = GridGenerator.Generate(height: 11, width: 11, numberOfPositives: 2);
Console.WriteLine($"Total Neighbors: {NeighborhoodFinder.FindNeighbors(grid, n: 3, distanceType: DistanceType.Manhattan, wrap: true)}");
GridPrinter.Print(grid);
