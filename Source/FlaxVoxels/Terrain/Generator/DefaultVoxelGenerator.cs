
using System;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain.Generator
{
	public class DefaultVoxelGenerator : IVoxelTerrainGenerator
	{
	    public void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels)
	    {
	        for (var x = 0; x < VoxelTerrainChunk.ChunkWidth; x++)
	        {
	            for (var y = 0; y < VoxelTerrainChunk.ChunkHeight; y++)
	            {
	                for (var z = 0; z < VoxelTerrainChunk.ChunkLength; z++)
	                {
	                    var voxel = Voxel.Air;

                        // Generate voxel
	                    if (y <= 4)
	                        voxel = new Voxel(1);

                        // Set voxel
	                    voxels[x, y, z] = voxel;
	                }
                }
            }
	    }
    }
}
