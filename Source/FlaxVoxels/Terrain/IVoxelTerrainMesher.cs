// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxVoxels.Math;
using FlaxVoxels.Terrain;

namespace FlaxVoxels
{
    internal interface IVoxelTerrainMesher
    {
        void GenerateMesh(Vector3Int worldPosition, VoxelTerrainChunk chunk);
        void Clear();
    }
}