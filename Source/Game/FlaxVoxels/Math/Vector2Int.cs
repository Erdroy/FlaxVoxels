// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

namespace FlaxVoxels.Math
{
    public struct Vector2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int l, Vector2Int r)
        {
            return new Vector2Int(l.X + r.X, l.Y + r.Y);
        }

        public static Vector2Int operator -(Vector2Int l, Vector2Int r)
        {
            return new Vector2Int(l.X - r.X, l.Y - r.Y);
        }

        public static Vector2Int operator *(Vector2Int l, Vector2Int r)
        {
            return new Vector2Int(l.X * r.X, l.Y * r.Y);
        }

        public static Vector2Int operator /(Vector2Int l, Vector2Int r)
        {
            return new Vector2Int(l.X / r.X, l.Y / r.Y);
        }

        public static Vector2Int operator *(Vector2Int l, int r)
        {
            return new Vector2Int(l.X * r, l.Y * r);
        }

        public static bool operator ==(Vector2Int l, Vector2Int r)
        {
            return l.X == r.X && l.Y == r.Y;
        }

        public static bool operator !=(Vector2Int l, Vector2Int r)
        {
            return !(l == r);
        }

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2Int i && Equals(i);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}