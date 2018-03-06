using UnityEngine;

public class Coordinate
{
    public float X { get { return Vector.x; } }
    public float Y { get { return Vector.y; } }

    public Vector2 Vector { get; private set; }

    public static readonly Coordinate North = new Coordinate(0, 1);
    public static readonly Coordinate East = new Coordinate(1, 0);
    public static readonly Coordinate South = new Coordinate(0, -1);
    public static readonly Coordinate West = new Coordinate(-1, 0);

    public static readonly Coordinate[] directions = {
        North, East, South, West, North+East, East+South, South+West, West+North
        };

    public Coordinate(int x, int y)
    {
        Vector = new Vector2(x, y);
    }

    public Coordinate(float x, float y)
    {
        Vector = new Vector2(x, y);
    }

    public Coordinate(Vector2 vector)
    {
        Vector = vector;
    }

    public static float Distance(Coordinate c1, Coordinate c2)
    {
        //NB! This distance calculation doesnt reflect the shorter distance of diagonal movement
        return Mathf.Abs(c1.X - c2.X) + Mathf.Abs(c1.Y - c2.Y);
    }

    public static float DistanceSquared(Coordinate c1, Coordinate c2)
    {
        return Mathf.Pow(Mathf.Abs(c1.X - c2.X),2) + Mathf.Pow(Mathf.Abs(c1.Y - c2.Y),2);
    }

    public bool Equals(Coordinate other)
    {
        return Vector == other.Vector;
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
        return Vector.GetHashCode();
    }

    public static Coordinate operator +(Coordinate lhs, Coordinate rhs)
    {
        return new Coordinate(lhs.Vector + rhs.Vector);
    }

    public static Coordinate operator -(Coordinate lhs, Coordinate rhs)
    {
        return new Coordinate(lhs.Vector - rhs.Vector);
    }

    public static Coordinate operator *(Coordinate lhs, int i1)
    {
        return new Coordinate(lhs.Vector * i1);
    }

    public static bool operator ==(Coordinate lhs, Coordinate rhs)
    {
        //Null-comparisons.
        if (ReferenceEquals(lhs, rhs))
        {
            return true;
        }
        if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
        {
            return false;
        }
        return lhs.Vector == rhs.Vector;
    }

    public static bool operator !=(Coordinate lhs, Coordinate rhs)
    {
        return !(lhs == rhs);
    }

    public static implicit operator Vector2(Coordinate c)
    {
        return c.Vector;
    }
}
