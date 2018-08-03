
using FlaxEngine;
using FlaxVoxels.Math;
using FlaxVoxels.Terrain;

namespace FlaxVoxels
{
    internal interface IVoxelTerrainMesher
    {
        void GenerateMesh(Vector3Int worldPosition, VoxelTerrainChunk chunk, ref Model chunkModel);
        void Clear();
    }
}
