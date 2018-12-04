// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

namespace FlaxVoxels.Terrain
{
	public enum VoxelTerrainChunkState
	{
	    Uncompleted,
        QueuedForGeneration,
        QueuedForVoxelGeneration,
        QueuedForMeshGeneration,
        GeneratingVoxels,
        GeneratingMesh,
        Complete
	}
}
