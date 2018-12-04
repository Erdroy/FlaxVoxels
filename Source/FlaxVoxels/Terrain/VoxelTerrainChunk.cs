// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using System;
using System.Runtime.CompilerServices;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
    /// <summary>
    ///     VoxelTerrainChunk class. Implements chunk management, including generating voxels, meshing etc.
    /// </summary>
    public class VoxelTerrainChunk
    {
        /// <summary>
        ///     Neighboring chunk direction lookup table.
        ///     Contains 26 possible direction in which a neighboring chunk can be found.
        /// </summary>
        public static Vector3Int[] NeighborChunkDirections =
        {
            // Bottom chunks (9 possible)
            new Vector3Int(-1, -1, -1),
            new Vector3Int( 0, -1, -1),
            new Vector3Int( 1, -1, -1),
            new Vector3Int(-1, -1,  0),
            new Vector3Int( 0, -1,  0),
            new Vector3Int( 1, -1,  0),
            new Vector3Int(-1, -1,  1),
            new Vector3Int( 0, -1,  1),
            new Vector3Int( 1, -1,  1),
            
            // Same level chunks (8 possible)
            new Vector3Int(-1,  0, -1),
            new Vector3Int( 0,  0, -1),
            new Vector3Int( 1,  0, -1),
            new Vector3Int(-1,  0,  0),
            // [0, 0, 0] (middle chunk)
            new Vector3Int( 1,  0,  0),
            new Vector3Int(-1,  0,  1),
            new Vector3Int( 0,  0,  1),
            new Vector3Int( 1,  0,  1),

            // Top chunks (9 possible)
            new Vector3Int(-1,  1, -1),
            new Vector3Int( 0,  1, -1),
            new Vector3Int( 1,  1, -1),
            new Vector3Int(-1,  1,  0),
            new Vector3Int( 0,  1,  0),
            new Vector3Int( 1,  1,  0),
            new Vector3Int(-1,  1,  1),
            new Vector3Int( 0,  1,  1),
            new Vector3Int( 1,  1,  1),
        };

        public const int ChunkWidth = 16;
        public const int ChunkHeight = 16;
        public const int ChunkLength = 16; // TODO: Unify all of these values into ChunkSize (?)

        private readonly VoxelTerrainMap _terrainMap;

        private Voxel[,,] _voxels;

        internal VoxelTerrainChunk(VoxelTerrainMap terrainMap, Vector3Int worldPosition)
        {
            State = VoxelTerrainChunkState.Uncompleted;

            WorldPosition = worldPosition;
            OffsetPosition = new Vector3Int(worldPosition.X / ChunkWidth, worldPosition.Y / ChunkHeight,
                worldPosition.Z / ChunkLength);

            _terrainMap = terrainMap;
            _voxels = new Voxel[ChunkWidth, ChunkHeight, ChunkLength];

            // Create model
            Model = Content.CreateVirtualAsset<Model>();

            // Create chunk actor
            Actor = StaticModel.New();
            Actor.Name = "VoxelTerrain Chunk";
            Actor.Model = Model;
            Actor.LocalScale = new Vector3(100);
            Actor.LocalPosition = new Vector3(worldPosition.X, worldPosition.Y, worldPosition.Z) * 100.0f;
            Actor.Entries[0].Material = VoxelTerrainManager.Current.DefaultMaterial;

            // Add mesh collider
            Collider = Actor.AddChild<MeshCollider>();

            // Set chunk's Actor as children of TerrainManager's actor (not required, but the scene actor list is a lot more clean)
            VoxelTerrainManager.Current.Actor.AddChild(Actor, true);
        }

        internal void WorkerGenerateVoxels(IVoxelTerrainGenerator generator)
        {
            generator.GenerateVoxels(WorldPosition, ref _voxels);
            HasVoxels = true;
        }

        internal void WorkerGenerateMesh(IVoxelTerrainMesher mesher)
        {
            // Update neighbors
            UpdateNeighbors();

            // Generate mesh
            mesher.GenerateMesh(this);

            // Update collision info
            UpdateCollision();
            HasMesh = true;
        }

        internal void UpdateNeighbors()
        {
            Neighbors = new VoxelTerrainChunk[NeighborChunkDirections.Length];

            for (var i = 0; i < NeighborChunkDirections.Length; i++)
            {
                Neighbors[i] = _terrainMap.FindChunk(WorldPosition + NeighborChunkDirections[i] * ChunkWidth);
            }
        }

        /// <summary>
        ///     Shows this chunk.
        /// </summary>
        public void Show()
        {
            Actor.IsActive = true;
        }

        /// <summary>
        ///     Hides this chunk.
        /// </summary>
        public void Hide()
        {
            Actor.IsActive = false;
        }

        /// <summary>
        ///     Updates chunk's collision data.
        /// </summary>
        public void UpdateCollision()
        {
            // TODO: Update collision info when there will be proper API for this.
        }

        /// <summary>
        ///     Deactivates and adds this chunk to cache.
        /// </summary>
        public void Destroy()
        {
            _terrainMap.DestroyChunk(this);
        }

        /// <summary>
        ///     Destroys this chunk.
        /// </summary>
        public void DestroyNow()
        {
            _terrainMap.DestroyChunkNow(this);
        }

        /// <summary>
        ///     Gets voxel from this chunk without any checks. Can only access this chunk's voxel data.
        /// </summary>
        /// <param name="voxelPosition">The local voxel position [X:0-15, Y:0-15, Z:0-15].</param>
        /// <returns>The voxel.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Voxel GetVoxelFast(Vector3Int voxelPosition)
        {
            return _voxels[voxelPosition.X, voxelPosition.Y, voxelPosition.Z];
        }

        /// <summary>
        ///     Sets voxel in this chunk without any checks. Can only access this chunk's voxel data.
        /// </summary>
        /// <param name="voxel">The voxel which will be set at given position .</param>
        /// <param name="voxelPosition">The local voxel position [X:0-15, Y:0-15, Z:0-15]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVoxelFast(Voxel voxel, Vector3Int voxelPosition)
        {
            _voxels[voxelPosition.X, voxelPosition.Y, voxelPosition.Z] = voxel;
        }

        /// <summary>
        ///     Gets voxel at given position, including all 26 neighbor chunks.
        /// </summary>
        /// <param name="voxelPosition">The local voxel position [X:0-15, Y:0-15, Z:0-15]</param>
        /// <returns>The voxel.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Voxel GetVoxel(Vector3Int voxelPosition)
        {
            if (voxelPosition.Y < 0)
                return Voxel.Mask; // Do not show bottom faces at chunks located at [Y: 0]

            if (voxelPosition.X >= 0 && voxelPosition.Y >= 0 && voxelPosition.Z >= 0 &&
                voxelPosition.X < ChunkWidth && voxelPosition.Y < ChunkHeight && voxelPosition.Z < ChunkLength)
                return GetVoxelFast(voxelPosition);

            var offsetX = voxelPosition.X < 0 ? -16 : voxelPosition.X >= ChunkWidth ? 16 : 0;
            var offsetY = voxelPosition.Y < 0 ? -16 : voxelPosition.Y >= ChunkWidth ? 16 : 0;
            var offsetZ = voxelPosition.Z < 0 ? -16 : voxelPosition.Z >= ChunkWidth ? 16 : 0;
            
            var chunk = _terrainMap.FindChunk(WorldPosition + new Vector3Int(offsetX, offsetY, offsetZ));

            if (offsetX != 0)
                voxelPosition.X += offsetX > 0 ? -16 : 16;

            if (offsetY != 0)
                voxelPosition.Y += offsetY > 0 ? -16 : 16;

            if (offsetZ != 0)
                voxelPosition.Z += offsetZ > 0 ? -16 : 16;

            return chunk?.GetVoxelFast(voxelPosition) ?? Voxel.Air;
        }

        /// <summary>
        ///     Sets voxel at given position, including all 26 neighbor chunks.
        /// </summary>
        /// <param name="voxel">The voxel which will be set at given position .</param>
        /// <param name="voxelPosition">The local voxel position [X:0-15, Y:0-15, Z:0-15]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVoxel(Voxel voxel, Vector3Int voxelPosition)
        {
            if (voxelPosition.Y < 0)
                return;

            if (voxelPosition.X >= 0 && voxelPosition.Y >= 0 && voxelPosition.Z >= 0 &&
                voxelPosition.X < ChunkWidth && voxelPosition.Y < ChunkHeight && voxelPosition.Z < ChunkLength)
                SetVoxelFast(voxel, voxelPosition);

            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns true when this chunk has generated voxels.
        /// </summary>
        public bool HasVoxels { get; internal set; }

        /// <summary>
        ///     Return true when this chunk has mesh.
        /// </summary>
        public bool HasMesh { get; internal set; }

        /// <summary>
        ///     Gets or sets unload flag. When false, this chunk will never unload.
        /// </summary>
        public bool CanUnload { get; set; } = true;

        /// <summary>
        ///     World position of this chunk.
        /// </summary>
        public Vector3Int WorldPosition { get; }

        /// <summary>
        ///     Offset position of this chunk (for ChunkTable).
        /// </summary>
        public Vector3Int OffsetPosition { get; }

        /// <summary>
        ///     The base actor of this chunk.
        /// </summary>
        public StaticModel Actor { get; }

        /// <summary>
        ///     This chunk's collider.
        /// </summary>
        public MeshCollider Collider { get; }

        /// <summary>
        ///     The chunk model.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        ///     Gets chunk visibility state. True when active and visible.
        /// </summary>
        public bool IsVisible => Actor.IsActive;

        /// <summary>
        ///     Gets chunk queue state. True when chunk is queued for any type of processing.
        /// </summary>
        public bool IsQueued => State == VoxelTerrainChunkState.QueuedForGeneration ||
                                State == VoxelTerrainChunkState.QueuedForVoxelGeneration || 
                                State == VoxelTerrainChunkState.QueuedForMeshGeneration;

        /// <summary>
        ///     Gets chunk processing state. True when chunk is being processed.
        /// </summary>
        public bool IsProcessing => State == VoxelTerrainChunkState.GeneratingVoxels ||
                                    State == VoxelTerrainChunkState.GeneratingMesh;

        /// <summary>
        ///     Gets complete state. True when chunk is complete.
        /// </summary>
        public bool IsComplete => State == VoxelTerrainChunkState.Complete;

        /// <summary>
        ///     The current chunk state.
        /// </summary>
        public VoxelTerrainChunkState State { get; set; }

        /// <summary>
        ///     Contains all neighboring chunks.
        /// </summary>
        public VoxelTerrainChunk[] Neighbors { get; set; }
    }
}