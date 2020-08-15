// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

using FlaxVoxels.Terrain;

namespace FlaxVoxels
{
    /// <summary>
    ///     IVoxelTerrainMesher interface. Provides IVoxelTerrainMesher interface for terrain mesh generation algorithms.
    /// </summary>
    internal interface IVoxelTerrainMesher
    {
        /// <summary>
        ///     Generates mesh for given chunk.
        /// </summary>
        /// <param name="chunk">The terrain chunk.</param>
        void GenerateMesh(VoxelTerrainChunk chunk);
    }
}