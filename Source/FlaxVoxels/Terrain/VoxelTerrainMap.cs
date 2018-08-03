
using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
	internal class VoxelTerrainMap
	{
	    private struct ChunkTable
	    {
	        public readonly Vector2Int Offset;
            public readonly VoxelTerrainChunk[,,] Chunks;

            public ChunkTable(VoxelTerrainChunk[,,] chunks, Vector2Int offset)
	        {
	            Chunks = chunks;
	            Offset = offset;
	        }
	    }

        /// <summary>
        /// The amount of chunks in single dimension.
        /// </summary>
	    public const int ChunkTableSize = 512;

	    private List<ChunkTable> _chunkTables;
	    private readonly VoxelTerrainManager _manager;

	    public VoxelTerrainMap(VoxelTerrainManager manager)
        {
            _manager = manager;
            _chunkTables = new List<ChunkTable>();
        }

	    public void Update()
	    {
	    }

	    public void UpdateActorViews(IReadOnlyList<Actor> viewActors)
	    {

	    }

	    public VoxelTerrainChunk CreateChunk(Vector2Int worldPosition)
	    {
            // TODO: Create chunk and queue for meshing

            throw new NotImplementedException();
	    }

	    public void DestroyChunk(VoxelTerrainChunk chunk)
	    {
            // TODO: cache chunk for specified time
	    }

	    public void DestroyChunkNow(VoxelTerrainChunk chunk)
	    {
            // TODO: delete chunk now
	    }
	}
}
