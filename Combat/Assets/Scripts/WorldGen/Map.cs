using System;

/**
 * Map
 * 0 = Empty
 * 1 >= Not empty
 * */

public interface Map
{
    //void SetTile(Coordinate position, int tile);
    //int GetTile(Coordinate position);

    bool IsOpen(int x, int y);

    bool IsOpen(Coordinate coordinate);

    bool IsBlocked(int x, int y);

    bool IsBlocked(Coordinate coordinate);

    bool WithinBounds(Coordinate coordinate);
}
