
using System;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
	public class VoxelTerrainChunk
	{
	    public const int ChunkWidth = 16;
	    public const int ChunkHeight = 16;
	    public const int ChunkLength = 16;

	    private ModelActor _actor;
	    private Model _model;

	    private Voxel[,,] _voxels;

        internal VoxelTerrainChunk(Vector3Int worldPosition)
        {
            WorldPosition = worldPosition;
            OffsetPosition = new Vector3Int(worldPosition.X / ChunkWidth, worldPosition.Y / ChunkHeight, 
                worldPosition.Z / ChunkLength);

            _voxels = new Voxel[ChunkWidth, ChunkHeight, ChunkLength];

            _model = Content.CreateVirtualAsset<Model>();

            _actor = ModelActor.New();

	        _actor.Model = _model;
            _actor.LocalScale = new Vector3(100);
            _actor.Entries[0].Material = VoxelTerrainManager.Current.DefaultMaterial;

            // Set chunk's Actor as children of TerrainManager's actor (not required, but the scene actor list is a lot more clean)
            VoxelTerrainManager.Current.Actor.AddChild(_actor);
        }

	    internal void WorkerGenerateVoxels(IVoxelTerrainGenerator generator)
	    {
            generator.GenerateVoxels(WorldPosition, ref _voxels);
	    }

	    internal void WorkerGenerateMesh(IVoxelTerrainMesher mesher)
	    {
	        // Generate mesh
	        mesher.GenerateMesh(WorldPosition, this, ref _model);
        }

	    public Voxel GetVoxelFast(Vector3Int voxelPosition)
	    {
	        return _voxels[voxelPosition.X, voxelPosition.Y, voxelPosition.Z];
	    }

	    public void SetVoxelFast(Voxel voxel, Vector3Int voxelPosition)
	    {
	        _voxels[voxelPosition.X, voxelPosition.Y, voxelPosition.Z] = voxel;
	    }

	    public Voxel GetVoxel(Vector3Int voxelPosition)
	    {
	        if (voxelPosition.X >= 0 && voxelPosition.Y >= 0 && voxelPosition.Z >= 0 && 
	            voxelPosition.X < ChunkWidth && voxelPosition.Y < ChunkHeight && voxelPosition.Z < ChunkLength)
	            return GetVoxelFast(voxelPosition);

            return Voxel.Air; // TODO: Read from neighbor
	    }

	    public void SetVoxel(Voxel voxel, Vector3Int voxelPosition)
	    {
	        throw new NotImplementedException();
        }

        public Vector3Int WorldPosition { get; private set; }
	    public Vector3Int OffsetPosition { get; private set; }
	}
}
