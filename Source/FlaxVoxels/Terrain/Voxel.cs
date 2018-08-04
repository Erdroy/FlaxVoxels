// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

namespace FlaxVoxels.Terrain
{
	public struct Voxel
	{
	    public ushort VoxelId;

	    public Voxel(ushort voxelId)
	    {
	        VoxelId = voxelId;
	    }

	    public bool IsAir => VoxelId == 0u;

        public static Voxel Air => new Voxel(0);
        public static Voxel Solid => new Voxel(1);
        public static Voxel Mask => new Voxel(ushort.MaxValue);
	}
}
