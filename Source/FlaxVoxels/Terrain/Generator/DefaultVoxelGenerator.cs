// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain.Generator
{
    public class DefaultVoxelGenerator : IVoxelTerrainGenerator
    {
        public void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels)
        {
            Simplex.Noise.Seed = VoxelTerrainManager.Current.WorldSeed;
            Simplex.Noise.OffsetX = worldPosition.X;
            Simplex.Noise.OffsetY = worldPosition.Z;
            Simplex.Noise.OffsetZ = worldPosition.Y;

            var noise2D = Simplex.Noise.Calc2D(VoxelTerrainChunk.ChunkWidth, VoxelTerrainChunk.ChunkLength, 0.025f);

            const float baseLevel = 10.0f;
            const float hillLevel = 8.0f;

            for (var y = 0; y < VoxelTerrainChunk.ChunkHeight; y++)
            for (var x = 0; x < VoxelTerrainChunk.ChunkWidth; x++)
            for (var z = 0; z < VoxelTerrainChunk.ChunkLength; z++)
            {
                var noise = (noise2D[x, z] / 256.0f) * hillLevel;
                noise += baseLevel;

                if (worldPosition.Y + y > noise)
                    continue;
                
                voxels[x, y, z] = new Voxel(1); // TODO: Use proper material
            }
        }
    }
}