using System;

/**
 * Map
 * 0 = Empty
 * 1 >= Not empty
 * */

public class Map
{
    private int[,] grid;

    public Map(int[,] grid)
    {
        this.grid = grid;
    }


    public bool IsOpen(int x, int y)
    {
        return grid[x, y] > 0;
    }

    public bool IsOpen(Coordinate coordinate)
    {
        return IsOpen(coordinate.X, coordinate.Y);
    }
    
    public bool IsBlocked(int x, int y)
    {
        return !(IsOpen(x, y));
    }

    public bool IsBlocked(Coordinate coordinate)
    {
        return !(IsOpen(coordinate));
    }

    public bool WithinBounds(Coordinate coordinate)
    {
        if (coordinate.X < 0 || coordinate.Y < 0 || coordinate.X >= grid.GetLength(0) || coordinate.Y >= grid.GetLength(1))
        {
            return false;
        }
        return true;
    }
}
