// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain.Generator
{
    public class DefaultVoxelGenerator : IVoxelTerrainGenerator
    {
        public void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels)
        {
            for (var x = 0; x < VoxelTerrainChunk.ChunkWidth; x++)
            for (var y = 0; y < VoxelTerrainChunk.ChunkHeight; y++)
            for (var z = 0; z < VoxelTerrainChunk.ChunkLength; z++)
            {
                var voxel = Voxel.Air;

                // Generate voxel
                if (y <= 4)
                    voxel = new Voxel(1);

                // Set voxel
                voxels[x, y, z] = voxel;
            }

            voxels[8, 6, 8] = Voxel.Solid;
            voxels[8, 5, 8] = Voxel.Solid;
            voxels[9, 5, 8] = Voxel.Solid;
            voxels[9, 5, 9] = Voxel.Solid;
        }
    }
}