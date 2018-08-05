// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;
using Simplex;

namespace FlaxVoxels.Terrain.Generator
{
    /// <summary>
    ///     DefaultVoxelGenerator class. Provides basic IVoxelTerrainGenerator implementation with 
    ///     single-octave simplex noise-based terrain generation.
    /// </summary>
    public class DefaultVoxelGenerator : IVoxelTerrainGenerator
    {
        public void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels)
        {
            Noise.Seed = VoxelTerrainManager.Current.WorldSeed;
            Noise.OffsetX = worldPosition.X;
            Noise.OffsetY = worldPosition.Z;
            Noise.OffsetZ = worldPosition.Y;

            var noise2D = Noise.Calc2D(VoxelTerrainChunk.ChunkWidth, VoxelTerrainChunk.ChunkLength, 0.025f);

            const float baseLevel = 10.0f;
            const float hillLevel = 8.0f;

            for (var y = 0; y < VoxelTerrainChunk.ChunkHeight; y++)
            for (var x = 0; x < VoxelTerrainChunk.ChunkWidth; x++)
            for (var z = 0; z < VoxelTerrainChunk.ChunkLength; z++)
            {
                var voxelHeight = worldPosition.Y + y;

                var noise = noise2D[x, z] / 256.0f * hillLevel;
                noise += baseLevel;

                if (voxelHeight > noise)
                    continue;

                var surfaceDistance = System.Math.Abs(voxelHeight - noise);

                ushort voxelId;
                if (surfaceDistance > 6)
                    voxelId = 1;
                else if (surfaceDistance > 2)
                    voxelId = 2;
                else
                    voxelId = 4;

                voxels[x, y, z] = new Voxel(voxelId); // TODO: Use proper material
            }
        }
    }
}