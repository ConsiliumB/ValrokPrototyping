public class Coordinate
{
    private readonly int x, y;

    public static readonly Coordinate North = new Coordinate(0, 1);
    public static readonly Coordinate East = new Coordinate(1, 0);
    public static readonly Coordinate South = new Coordinate(0, -1);
    public static readonly Coordinate West = new Coordinate(-1, 0);

    public static readonly Coordinate[] directions = {
        North, East, South, West, North+East, East+South, South+West, West+North
        };

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return x; }
    }

    public static int Distance(Coordinate c1, Coordinate c2)
    {
        //NB! This distance calculation doesnt reflect the shorter distance of diagonal movement
        return System.Math.Abs(c1.x - c2.x) + System.Math.Abs(c1.y - c2.y);
    }

    public bool Equals(Coordinate other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object other)
    {
        return other is Coordinate && Equals((Coordinate)other);
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", x, y);
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
        return new Coordinate(c1.x + c2.x, c1.y + c2.y);
    }

    public static Coordinate operator -(Coordinate c1, Coordinate c2)
    {
        return new Coordinate(c1.x - c2.x, c1.y - c2.y);
    }

    public static bool operator ==(Coordinate c1, Coordinate c2)
    {
        return c1.x == c2.x && c1.y == c2.y;
    }

    public static bool operator !=(Coordinate c1, Coordinate c2)
    {
        return !(c1 == c2);
    }
}
