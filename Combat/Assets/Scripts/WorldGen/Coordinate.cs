using UnityEngine;

public class Coordinate
{
    public int Y { get; private set; }
    public int X { get; private set; }

    public static readonly Coordinate North = new Coordinate(0, 1);
    public static readonly Coordinate East = new Coordinate(1, 0);
    public static readonly Coordinate South = new Coordinate(0, -1);
    public static readonly Coordinate West = new Coordinate(-1, 0);

    public static readonly Coordinate[] directions = {
        North, East, South, West, North+East, East+South, South+West, West+North
        };

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static int Distance(Coordinate c1, Coordinate c2)
    {
        //NB! This distance calculation doesnt reflect the shorter distance of diagonal movement
        return System.Math.Abs(c1.X - c2.X) + System.Math.Abs(c1.Y - c2.Y);
    }

    public static int DistanceSquared(Coordinate c1, Coordinate c2)
    {
        return (int)(Mathf.Pow(Mathf.Abs(c1.X - c2.X),2) + Mathf.Pow(Mathf.Abs(c1.Y - c2.Y),2));
    }

    public bool Equals(Coordinate other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object other)
    {
        return other is Coordinate && Equals((Coordinate)other);
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", X, Y);
    }

    public override int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }

    public static Coordinate operator +(Coordinate c1, Coordinate c2)
    {
        return new Coordinate(c1.X + c2.X, c1.Y + c2.Y);
    }

    public static Coordinate operator -(Coordinate c1, Coordinate c2)
    {
        return new Coordinate(c1.X - c2.X, c1.Y - c2.Y);
    }

    public static Coordinate operator *(Coordinate c1, int i1)
    {
        return new Coordinate(c1.X * i1, c1.Y * i1);
    }

    public static bool operator ==(Coordinate c1, Coordinate c2)
    {
        //Null-comparisons.
        if (ReferenceEquals(c1, c2))
        {
            return true;
        }
        if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
        {
            return false;
        }
        return c1.X == c2.X && c1.Y == c2.Y;
    }

    public static bool operator !=(Coordinate c1, Coordinate c2)
    {
        return !(c1 == c2);
    }
}
