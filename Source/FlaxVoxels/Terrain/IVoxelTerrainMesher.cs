// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Terrain;

namespace FlaxVoxels
{
    internal interface IVoxelTerrainMesher
    {
        void GenerateMesh(VoxelTerrainChunk chunk);
        void Clear();
    }
}