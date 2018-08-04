// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
    using Object = FlaxEngine.Object;

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
        ///     The amount of chunks in single dimension.
        /// </summary>
        public const int ChunkTableSize = 512;

        private readonly VoxelTerrainManager _manager;
        private readonly VoxelTerrainChunkCache _chunkCache;

        private List<ChunkTable> _chunkTables;

        public VoxelTerrainMap(VoxelTerrainManager manager)
        {
            _manager = manager;
            _chunkCache = new VoxelTerrainChunkCache();
            _chunkTables = new List<ChunkTable>();
        }

        public void Update()
        {
            // Update chunk cache
            _chunkCache.Update();
        }

        public void UpdateActorViews(IReadOnlyList<Actor> viewActors)
        {
        }
        
        public VoxelTerrainChunk CreateChunk(Vector3Int worldPosition)
        {
            // TODO: Create chunk and queue for meshing

            throw new NotImplementedException();
        }

        /// <summary>
        ///     Destroys given chunk after cache time. Must be called from main thread.
        /// </summary>
        /// <param name="chunk">The chunk which will be destroyed.</param>
        public void DestroyChunk(VoxelTerrainChunk chunk)
        {
            // Destroy chunk now, when cache time less or equal to 0.
            if (VoxelTerrainManager.Current.MaxChunkCacheTime <= 0)
            {
                DestroyChunkNow(chunk);
                return;
            }

            // Hide chunk
            chunk.Hide();

            // Add chunk to cache
            _chunkCache.Add(chunk);
        }

        /// <summary>
        ///     Destroys given chunk. Must be called from main thread.
        /// </summary>
        /// <param name="chunk">The chunk which will be destroyed.</param>
        public void DestroyChunkNow(VoxelTerrainChunk chunk)
        {
            Object.Destroy(chunk.Actor);
        }
    }
}