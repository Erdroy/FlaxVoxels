// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
	internal interface IVoxelTerrainGenerator
	{
	    void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels);
	}
}
