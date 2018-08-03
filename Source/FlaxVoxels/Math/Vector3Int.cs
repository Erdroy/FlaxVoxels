
namespace FlaxVoxels.Math
{
	public struct Vector3Int
	{
	    public int X { get; set; }
	    public int Y { get; set; }
	    public int Z { get; set; }

	    public Vector3Int(int x, int y, int z)
	    {
	        X = x;
	        Y = y;
	        Z = z;
	    }

	    public static Vector3Int operator +(Vector3Int l, Vector3Int r)
	    {
	        return new Vector3Int(l.X + r.X, l.Y + l.Y, l.Z + r.Z);
	    }

	    public static Vector3Int operator -(Vector3Int l, Vector3Int r)
	    {
	        return new Vector3Int(l.X - r.X, l.Y - l.Y, l.Z - r.Z);
	    }
        
	    public static Vector3Int operator *(Vector3Int l, Vector3Int r)
	    {
	        return new Vector3Int(l.X * r.X, l.Y * l.Y, l.Z * r.Z);
	    }

	    public static Vector3Int operator /(Vector3Int l, Vector3Int r)
	    {
	        return new Vector3Int(l.X / r.X, l.Y / l.Y, l.Z / r.Z);
	    }

        public static bool operator ==(Vector3Int l, Vector3Int r)
	    {
	        return l.X == r.X && l.Y == r.Y && l.Z == r.Z;
	    }

	    public static bool operator !=(Vector3Int l, Vector3Int r)
	    {
	        return !(l == r);
	    }

	    public bool Equals(Vector3Int other)
	    {
	        return X == other.X && Y == other.Y && Z == other.Z;
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        return obj is Vector3Int i && Equals(i);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            var hashCode = X;
	            hashCode = (hashCode * 397) ^ Y;
	            hashCode = (hashCode * 397) ^ Z;
	            return hashCode;
	        }
	    }
    }
}
