// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using System.Collections.Generic;
using FlaxEngine;
using FlaxVoxels.Math;
using FlaxVoxels.Terrain.Generator;
using FlaxVoxels.Terrain.Meshing;

namespace FlaxVoxels.Terrain
{
    internal class VoxelTerrainMap
    {
        public class ChunkMap
        {
            /// <summary>
            ///     The amount of chunks in dimension 0 (X).
            /// </summary>
            public const int ChunkMapWidth = 512;

            /// <summary>
            ///     The amount of chunks in dimension 1 (Y).
            /// </summary>
            public const int ChunkMapHeight = 32;

            /// <summary>
            ///     The amount of chunks in dimension 2 (Z).
            /// </summary>
            public const int ChunkMapLength = 512;

            public readonly VoxelTerrainChunk[,,] Chunks =
                new VoxelTerrainChunk[ChunkMapWidth, ChunkMapHeight, ChunkMapLength];

            public ChunkMap(Vector3Int offset)
            {
                Offset = offset;
                WorldPosition = offset *
                                new Vector3Int(ChunkMapWidth, ChunkMapHeight, ChunkMapLength) *
                                new Vector3Int(VoxelTerrainChunk.ChunkWidth, VoxelTerrainChunk.ChunkHeight,
                                    VoxelTerrainChunk.ChunkLength);
            }

            public VoxelTerrainChunk GetChunk(Vector3Int worldPosition)
            {
                var offset = WorldToLocalChunk(worldPosition);
                return Chunks[offset.X, offset.Y, offset.Z];
            }

            public void SetChunk(VoxelTerrainChunk chunk, Vector3Int worldPosition)
            {
                var offset = WorldToLocalChunk(worldPosition);
                Chunks[offset.X, offset.Y, offset.Z] = chunk;
            }

            public Vector3Int WorldToLocalChunk(Vector3Int worldPosition)
            {
                var chunkOffset = WorldToChunkOffset(worldPosition);

                return new Vector3Int
                {
                    X = -(WorldPosition.X - chunkOffset.X * VoxelTerrainChunk.ChunkWidth) / VoxelTerrainChunk.ChunkWidth,
                    Y = -(WorldPosition.Y - chunkOffset.Y * VoxelTerrainChunk.ChunkHeight) / VoxelTerrainChunk.ChunkHeight,
                    Z = -(WorldPosition.Z - chunkOffset.Z * VoxelTerrainChunk.ChunkLength) / VoxelTerrainChunk.ChunkLength,
                };
            }

            public Vector3Int ChunkToLocalChunk(Vector3Int chunkOffset)
            {
                return new Vector3Int
                {
                    X = -(WorldPosition.X - chunkOffset.X * VoxelTerrainChunk.ChunkWidth) / VoxelTerrainChunk.ChunkWidth,
                    Y = -(WorldPosition.Y - chunkOffset.Y * VoxelTerrainChunk.ChunkHeight) / VoxelTerrainChunk.ChunkHeight,
                    Z = -(WorldPosition.Z - chunkOffset.Z * VoxelTerrainChunk.ChunkLength) / VoxelTerrainChunk.ChunkLength,
                };
            }

            public static Vector3Int WorldToChunkWorld(Vector3Int worldPosition)
            {
                var offset = WorldToChunkOffset(worldPosition);

                return new Vector3Int
                {
                    X = offset.X * VoxelTerrainChunk.ChunkWidth,
                    Y = offset.Y * VoxelTerrainChunk.ChunkHeight,
                    Z = offset.Z * VoxelTerrainChunk.ChunkLength,
                };
            }

            public static Vector3Int WorldToChunkOffset(Vector3Int worldPosition)
            {
                // Snap world position components
                if (worldPosition.X < 0)
                    worldPosition.X -= VoxelTerrainChunk.ChunkWidth - 1;

                if (worldPosition.Y < 0)
                    worldPosition.Y -= VoxelTerrainChunk.ChunkHeight - 1;

                if (worldPosition.Z < 0)
                    worldPosition.Z -= VoxelTerrainChunk.ChunkLength - 1;

                return new Vector3Int
                {
                    X = worldPosition.X / VoxelTerrainChunk.ChunkWidth,
                    Y = worldPosition.Y / VoxelTerrainChunk.ChunkHeight,
                    Z = worldPosition.Z / VoxelTerrainChunk.ChunkHeight
                };
            }

            public static Vector3Int WorldToMapOffset(Vector3Int worldPosition)
            {
                var chunkOffset = WorldToChunkOffset(worldPosition);

                // Snap chunk offset components
                if (chunkOffset.X < 0)
                    chunkOffset.X -= ChunkMapWidth - 1;

                if (chunkOffset.Y < 0)
                    chunkOffset.Y -= ChunkMapHeight - 1;

                if (chunkOffset.Z < 0)
                    chunkOffset.Z -= ChunkMapLength - 1;

                return new Vector3Int
                {
                    X = chunkOffset.X / ChunkMapWidth,
                    Y = chunkOffset.Y / ChunkMapHeight,
                    Z = chunkOffset.Z / ChunkMapLength
                };
            }

            public Vector3Int Offset { get; }
            public Vector3Int WorldPosition { get; }
        }

        private readonly VoxelTerrainChunkCache _chunkCache;
        
        private readonly Dictionary<Vector3Int, ChunkMap> _chunkMaps;

        private readonly IVoxelTerrainGenerator _currentGenerator;
        private readonly IVoxelTerrainMesher _currentMesher;

        public VoxelTerrainMap()
        {
            _chunkCache = new VoxelTerrainChunkCache();
            _chunkMaps = new Dictionary<Vector3Int, ChunkMap>();

            _currentMesher = new ErdroysCubeMesher();
            _currentGenerator = new DefaultVoxelGenerator();
        }

        public void Update()
        {
            // Update chunk cache
            _chunkCache.Update();
        }

        public void UpdateActorViews(IReadOnlyList<Actor> viewActors)
        {
        }

        private ChunkMap FindChunkMap(Vector3Int worldPosition)
        {
            // Convert worldPosition to chunk map offset
            var offset = ChunkMap.WorldToMapOffset(worldPosition);
            return _chunkMaps[offset];
        }

        private ChunkMap FindOrAddChunkMap(Vector3Int worldPosition)
        {
            // Convert worldPosition to chunk map offset
            var offset = ChunkMap.WorldToMapOffset(worldPosition);

            // Try to find the chunk map with the same calculated map offset
            if (!_chunkMaps.TryGetValue(offset, out var map))
            {
                // Not found, create and add
                map = new ChunkMap(offset);

                // Add chunk map
                _chunkMaps.Add(offset, map);
            }

            return map;
        }

        public VoxelTerrainChunk CreateChunk(Vector3Int worldPosition)
        {
            var chunkMap = FindOrAddChunkMap(worldPosition);

            // Create temporary test chunk
            var chunk = new VoxelTerrainChunk(this, worldPosition);
            chunkMap.SetChunk(chunk, worldPosition);

            chunk.WorkerGenerateVoxels(_currentGenerator);
            chunk.WorkerGenerateMesh(_currentMesher);

            // Clear mesher
            _currentMesher.Clear();

            // TODO: Queue for generation and meshing

            return chunk;
        }

        public VoxelTerrainChunk FindChunk(Vector3Int worldPosition)
        {
            var chunkMap = FindOrAddChunkMap(worldPosition);
            return chunkMap.GetChunk(worldPosition);
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