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
        public const int ChunkWidth = 16;
        public const int ChunkHeight = 16;
        public const int ChunkLength = 16;

        private readonly VoxelTerrainMap _terrainMap;

        private Voxel[,,] _voxels;

        internal VoxelTerrainChunk(VoxelTerrainMap terrainMap, Vector3Int worldPosition)
        {
            WorldPosition = worldPosition;
            OffsetPosition = new Vector3Int(worldPosition.X / ChunkWidth, worldPosition.Y / ChunkHeight,
                worldPosition.Z / ChunkLength);

            _terrainMap = terrainMap;
            _voxels = new Voxel[ChunkWidth, ChunkHeight, ChunkLength];

            // Create model
            Model = Content.CreateVirtualAsset<Model>();

            // Create chunk actor
            Actor = ModelActor.New();
            Actor.Model = Model;
            Actor.LocalScale = new Vector3(100);
            Actor.LocalPosition = new Vector3(worldPosition.X, worldPosition.Y, worldPosition.Z) * 100.0f;
            Actor.Entries[0].Material = VoxelTerrainManager.Current.DefaultMaterial;

            // Add mesh collider
            Collider = Actor.AddChild<MeshCollider>();

            // Set chunk's Actor as children of TerrainManager's actor (not required, but the scene actor list is a lot more clean)
            VoxelTerrainManager.Current.Actor.AddChild(Actor);
        }

        internal void WorkerGenerateVoxels(IVoxelTerrainGenerator generator)
        {
            generator.GenerateVoxels(WorldPosition, ref _voxels);
        }

        internal void WorkerGenerateMesh(IVoxelTerrainMesher mesher)
        {
            // Generate mesh
            mesher.GenerateMesh(this);

            // Update collision info
            UpdateCollision();
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

            return Voxel.Air; // TODO: Read from neighbor
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

            throw new NotImplementedException();
        }

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
        public ModelActor Actor { get; }

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
    }
}