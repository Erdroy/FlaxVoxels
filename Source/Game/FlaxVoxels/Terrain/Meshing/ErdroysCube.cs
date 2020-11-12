public static class ErdroysCube
{
	public static readonly short[][] ErdroyTable = 
	{
		new short[] { -1 },
		new short[] { 6, 2, 3, 3, 7, 6, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, -1 },
		new short[] { 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, -1 },
		new short[] { 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, -1 },
		new short[] { 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0, -1 },
		new short[] { 6, 2, 3, 3, 7, 6, 5, 1, 0, 0, 4, 5, 6, 4, 0, 0, 2, 6, 7, 3, 1, 1, 5, 7, 7, 5, 4, 4, 6, 7, 0, 1, 3, 3, 2, 0 },
	};
}