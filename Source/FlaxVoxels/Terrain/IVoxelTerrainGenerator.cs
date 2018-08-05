// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
    /// <summary>
    ///     IVoxelTerrainGenerator interface. Provides IVoxelTerrainGenerator interface for terrain generation algorithms.
    /// </summary>
    internal interface IVoxelTerrainGenerator
    {
        /// <summary>
        ///     Generates voxels for given chunk's world position and outputs the result in referenced voxels array.
        /// </summary>
        /// <param name="worldPosition">The chunk world position.</param>
        /// <param name="voxels">The voxel array reference.</param>
        void GenerateVoxels(Vector3Int worldPosition, ref Voxel[,,] voxels);
    }
}