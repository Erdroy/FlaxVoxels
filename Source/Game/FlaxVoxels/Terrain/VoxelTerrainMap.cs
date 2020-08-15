// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

using System.Collections.Generic;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
    internal class VoxelTerrainMap
    {
        /// <summary>
        ///     ChunkMap class. Provides fast chunk map, which can be used for mapping chunks - obvious, huh?
        /// </summary>
        public class ChunkMap
        {
            /// <summary>
            ///     The amount of chunks in dimension 0 (X).
            /// </summary>
            public const int MapWidth = 512;

            /// <summary>
            ///     The amount of chunks in dimension 1 (Y).
            /// </summary>
            public const int MapHeight = 8;

            /// <summary>
            ///     The amount of chunks in dimension 2 (Z).
            /// </summary>
            public const int MapLength = 512;

            public readonly VoxelTerrainChunk[,,] Chunks =
                new VoxelTerrainChunk[MapWidth, MapHeight, MapLength];

            /// <summary>
            ///     Default constructor.
            /// </summary>
            /// <param name="offset">The ChunkMap base offset, this is used to determine offset from other ChunkMaps.</param>
            public ChunkMap(Vector3Int offset)
            {
                Offset = offset;
                WorldPosition = offset *
                                new Vector3Int(MapWidth, MapHeight, MapLength) *
                                new Vector3Int(VoxelTerrainChunk.Width, VoxelTerrainChunk.Height,
                                    VoxelTerrainChunk.Length);
            }

            /// <summary>
            ///     Gets chunk at given world position.
            /// </summary>
            /// <param name="worldPosition">The world position.</param>
            /// <returns>The selected chunk or null when chunk is not generated.</returns>
            public VoxelTerrainChunk GetChunk(Vector3Int worldPosition)
            {
                var offset = WorldToLocalChunk(worldPosition);
                return Chunks[offset.X, offset.Y, offset.Z];
            }

            /// <summary>
            ///     Sets chunk at given world position (will be snapped to chunk offset position).
            /// </summary>
            /// <param name="chunk">The chunk instance.</param>
            /// <param name="worldPosition">The world position.</param>
            public void SetChunk(VoxelTerrainChunk chunk, Vector3Int worldPosition)
            {
                var offset = WorldToLocalChunk(worldPosition);
                Chunks[offset.X, offset.Y, offset.Z] = chunk;
            }

            /// <summary>
            ///     Converts world position to chunk offset, but also snapped to the origin of this chunk map.
            /// </summary>
            /// <param name="worldPosition">The world position.</param>
            /// <returns>The snapped local chunk offset.</returns>
            public Vector3Int WorldToLocalChunk(Vector3Int worldPosition)
            {
                var chunkOffset = WorldToChunkOffset(worldPosition);

                return new Vector3Int
                {
                    X = -(WorldPosition.X - chunkOffset.X * VoxelTerrainChunk.Width) /
                        VoxelTerrainChunk.Width,
                    Y = -(WorldPosition.Y - chunkOffset.Y * VoxelTerrainChunk.Height) /
                        VoxelTerrainChunk.Height,
                    Z = -(WorldPosition.Z - chunkOffset.Z * VoxelTerrainChunk.Length) /
                        VoxelTerrainChunk.Length
                };
            }

            /// <summary>
            ///     Converts chunk offset position to offset snapped to the origin of this chunk map.
            /// </summary>
            /// <param name="chunkOffset">The chunk offset position.</param>
            /// <returns>The snapped local chunk offset.</returns>
            public Vector3Int ChunkToLocalChunk(Vector3Int chunkOffset)
            {
                return new Vector3Int
                {
                    X = -(WorldPosition.X - chunkOffset.X * VoxelTerrainChunk.Width) /
                        VoxelTerrainChunk.Width,
                    Y = -(WorldPosition.Y - chunkOffset.Y * VoxelTerrainChunk.Height) /
                        VoxelTerrainChunk.Height,
                    Z = -(WorldPosition.Z - chunkOffset.Z * VoxelTerrainChunk.Length) /
                        VoxelTerrainChunk.Length
                };
            }

            /// <summary>
            ///     Snaps world position to chunk grid.
            /// </summary>
            /// <param name="worldPosition">The world position.</param>
            /// <returns>The snapped world position.</returns>
            public static Vector3Int WorldToChunkWorld(Vector3Int worldPosition)
            {
                var offset = WorldToChunkOffset(worldPosition);

                return new Vector3Int
                {
                    X = offset.X * VoxelTerrainChunk.Width,
                    Y = offset.Y * VoxelTerrainChunk.Height,
                    Z = offset.Z * VoxelTerrainChunk.Length
                };
            }

            /// <summary>
            ///     Converts world position to chunk offset.
            /// </summary>
            /// <param name="worldPosition">The world position.</param>
            /// <returns>The chunk offset position.</returns>
            public static Vector3Int WorldToChunkOffset(Vector3Int worldPosition)
            {
                // Snap world position components
                if (worldPosition.X < 0)
                    worldPosition.X -= VoxelTerrainChunk.Width - 1;

                if (worldPosition.Y < 0)
                    worldPosition.Y -= VoxelTerrainChunk.Height - 1;

                if (worldPosition.Z < 0)
                    worldPosition.Z -= VoxelTerrainChunk.Length - 1;

                return new Vector3Int
                {
                    X = worldPosition.X / VoxelTerrainChunk.Width,
                    Y = worldPosition.Y / VoxelTerrainChunk.Height,
                    Z = worldPosition.Z / VoxelTerrainChunk.Height
                };
            }

            /// <summary>
            ///     Converts world position to map offset.
            /// </summary>
            /// <param name="worldPosition">The world position.</param>
            /// <returns>The map offset position.</returns>
            public static Vector3Int WorldToMapOffset(Vector3Int worldPosition)
            {
                var chunkOffset = WorldToChunkOffset(worldPosition);

                // Snap chunk offset components
                if (chunkOffset.X < 0)
                    chunkOffset.X -= MapWidth - 1;

                if (chunkOffset.Y < 0)
                    chunkOffset.Y -= MapHeight - 1;

                if (chunkOffset.Z < 0)
                    chunkOffset.Z -= MapLength - 1;

                return new Vector3Int
                {
                    X = chunkOffset.X / MapWidth,
                    Y = chunkOffset.Y / MapHeight,
                    Z = chunkOffset.Z / MapLength
                };
            }

            /// <summary>
            ///     The base offset of this map.
            /// </summary>
            public Vector3Int Offset { get; }

            /// <summary>
            ///     The world position of this map (snapped with chunk map grid).
            /// </summary>
            public Vector3Int WorldPosition { get; }
        }

        private readonly VoxelTerrainChunkCache _chunkCache;

        private readonly Dictionary<Vector3Int, ChunkMap> _chunkMaps;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public VoxelTerrainMap()
        {
            _chunkCache = new VoxelTerrainChunkCache();
            _chunkMaps = new Dictionary<Vector3Int, ChunkMap>();
        }

        /// <summary>
        ///     Updates this voxel terrain map and chunk cache.
        /// </summary>
        public void Update()
        {
            // Update chunk cache
            _chunkCache.Update();
        }

        /// <summary>
        ///     Updates view actors, generates new chunks in it's view ranges
        ///     and destroys (caches) chunks which are out of view.
        /// </summary>
        /// <param name="viewActors">The read-only list of view actors.</param>
        public void UpdateViewActors(IReadOnlyList<Actor> viewActors)
        {
            // TODO: Auto generation implementation
        }

        /// <summary>
        ///     Creates chunk at given world position, if chunk already exists, the current chunk is being returned
        ///     and no voxel/mesh generation is required.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The created/found chunk.</returns>
        public VoxelTerrainChunk CreateChunk(Vector3Int worldPosition)
        {
            var chunkMap = FindOrAddChunkMap(worldPosition);

            // Try to find chunk first
            var chunk = chunkMap.GetChunk(worldPosition);
            if (chunk != null)
            {
                // Queue for mesh generation
                if(chunk.HasVoxels && !chunk.HasMesh)
                    VoxelTerrainChunkGenerator.EnqueueMeshGeneration(chunk);

                return chunk;
            }

            // Create temporary test chunk
            chunk = new VoxelTerrainChunk(this, worldPosition);
            chunkMap.SetChunk(chunk, worldPosition);
            
            // Generate whole chunk, this probably will also generate all neighbors
            VoxelTerrainChunkGenerator.EnqueueGeneration(chunk);

            return chunk;
        }

        /// <summary>
        ///     Looks for chunk map at given world position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The chunk map (can be null when doesn't exist).</returns>
        public ChunkMap FindChunkMap(Vector3Int worldPosition)
        {
            // Convert worldPosition to chunk map offset
            var offset = ChunkMap.WorldToMapOffset(worldPosition);
            return _chunkMaps.TryGetValue(offset, out var map) ? map : null;
        }

        /// <summary>
        ///     Looks for chunk at given world position. Returns null when chunk or chunk map doesn't exist.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The chunk (can be null).</returns>
        public VoxelTerrainChunk FindChunk(Vector3Int worldPosition)
        {
            var chunkMap = FindChunkMap(worldPosition);
            return chunkMap?.GetChunk(worldPosition);
        }

        /// <summary>
        ///     Looks for chunk map at given world position,
        ///     when chunk map is not found, this function creates one.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The chunk map.</returns>
        public ChunkMap FindOrAddChunkMap(Vector3Int worldPosition)
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

            // Check if this chunk should be always loaded, if true, skip caching and leave it as-is.
            if (!chunk.CanUnload)
                return;

            // Add chunk to cache
            _chunkCache.Add(chunk);
        }

        /// <summary>
        ///     Destroys given chunk. Must be called from main thread.
        /// </summary>
        /// <param name="chunk">The chunk which will be destroyed.</param>
        public void DestroyChunkNow(VoxelTerrainChunk chunk)
        {
            // Check if this chunk should be always loaded, if true, leave it as-is.
            if (!chunk.CanUnload)
                return;

            Object.Destroy(chunk.Actor);
        }
    }
}