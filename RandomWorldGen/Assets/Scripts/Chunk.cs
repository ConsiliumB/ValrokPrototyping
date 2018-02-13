using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    public int xPos;
    public int yPos;
    public int chunkWidth;
    public int chunkHeight;
    public Location entry;
    public Location exit;

    public enum Direction
    {
        NW, NE, SE, SW
    }

    public Chunk(int xPos, int yPos, int width, int height)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.chunkWidth = width;
        this.chunkHeight = height;
    }

    public Chunk AppendChunk(int width, int height, Direction direction)
    {
        Chunk newChunk = null;
        switch (direction)
        {
            case Direction.NW:
                newChunk = new Chunk(
                    Random.Range(xPos - (this.chunkWidth - 2), xPos + (this.chunkWidth - 2)),
                    yPos + this.chunkHeight - 1,
                    width, height);
                break;
            case Direction.NE:
                newChunk = new Chunk(
                    xPos + this.chunkWidth - 1,
                    Random.Range(yPos - (this.chunkHeight - 2), yPos + (this.chunkHeight - 2)),
                    width, height);
                break;
            case Direction.SE:
                newChunk = new Chunk(
                    Random.Range(xPos - (this.chunkWidth - 2), xPos + (this.chunkWidth - 2)),
                    yPos,
                    width, height);
                break;
            case Direction.SW:
                newChunk = new Chunk(
                    xPos,
                    Random.Range(yPos - (this.chunkHeight - 2), yPos + (this.chunkHeight - 2)),
                    width, height);
                break;
            default:
                break;
        }



        return newChunk;
    }
}
