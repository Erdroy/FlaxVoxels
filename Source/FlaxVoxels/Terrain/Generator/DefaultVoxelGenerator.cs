// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;
using Simplex;

namespace FlaxVoxels.Terrain.Generator
{
    /// <inheritdoc />
    /// <summary>
    ///     DefaultVoxelGenerator class. Provides basic IVoxelTerrainGenerator implementation with 
    ///     single-octave simplex noise-based terrain generation.
    /// </summary>
    public class DefaultVoxelGenerator : IVoxelTerrainGenerator
    {
        /// <inheritdoc />
        public void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels)
        {
            Noise.Seed = VoxelTerrainManager.Current.WorldSeed;
            Noise.OffsetX = worldPosition.X;
            Noise.OffsetY = worldPosition.Z;
            Noise.OffsetZ = worldPosition.Y;
            
            // Calculate noise
            var noise2D = Noise.Calc2D(VoxelTerrainChunk.Width, VoxelTerrainChunk.Length, 0.025f);

            const float baseLevel = 16.0f;
            const float hillLevel = 12.0f;

            for (var y = 0; y < VoxelTerrainChunk.Height; y++)
            for (var x = 0; x < VoxelTerrainChunk.Width; x++)
            for (var z = 0; z < VoxelTerrainChunk.Length; z++)
            {
                // Calculate world space voxel Y/height
                var voxelHeight = worldPosition.Y + y;

                // Calculate noise value in range [0...1]
                var noiseValue = noise2D[x, z] / 256.0f;
                
                // Calculate terrain height TODO: This can be optimized, use cached values after first XZ iteration
                var terrainHeight = noiseValue * hillLevel;
                terrainHeight += baseLevel;

                // Calculate distance to the surface
                var surfaceDistance = terrainHeight - voxelHeight;

                // Check if we are still under surface, if not, then continue to the next voxel.
                if (surfaceDistance < 0)
                    continue;
                
                // Select which block should we used based on distance from surface
                ushort voxelId;
                if (surfaceDistance > 6)
                    voxelId = 1; // We are deep, use stone.
                else if (surfaceDistance > 2)
                    voxelId = 2; // Well a lil bit under surface, use dirt.
                else
                    voxelId = 4; // Otherwise, use sand.

                voxels[x, y, z] = new Voxel(voxelId);
            }
        }
    }
}