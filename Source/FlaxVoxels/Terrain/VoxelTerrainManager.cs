// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

using System.Collections.Generic;
using FlaxEngine;
using FlaxVoxels.Materials;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain
{
    /// <summary>
    ///     Voxel terrain manager. Manages all voxel-terrain related stuff, chunk spawning etc.
    /// </summary>
    public class VoxelTerrainManager : Script
    {
        private readonly List<Actor> _views = new List<Actor>();

        private VoxelTerrainMap _terrainMap;

        private void Start()
        {
            Current = this;
            _terrainMap = new VoxelTerrainMap();
            
            // Build material set
            VoxelMaterials = new VoxelMaterialSet.VoxelMaterial[ushort.MaxValue];

            foreach (var entry in ((VoxelMaterialSet) MaterialSet.CreateInstance()).Materials)
            {
                var material = entry.Material;
                if (material != null)
                    VoxelMaterials[material.Id] = material;
            }

            // Build initial chunks
            for (var y = 0; y < 4; y++)
            for (var x = -8; x < 8; x++)
            for (var z = -8; z < 8; z++)
                _terrainMap.CreateChunk(new Vector3Int(x * 16, y * 16, z * 16));
        }

        private void Update()
        {
            _terrainMap.Update();
            _terrainMap.UpdateViewActors(_views);
        }

        /// <summary>
        ///     Adds actor to track view. This sets where the terrain is generated etc.
        /// </summary>
        /// <param name="viewActor">The view actor.</param>
        /// <param name="viewRangeMul">The view-range multiplier. (Default: 1.0)</param>
        public void AddActorView(Actor viewActor, float viewRangeMul = 1.0f)
        {
            _views.Add(viewActor);
        }

        /// <summary>
        ///     Clears view track list.
        /// </summary>
        public void ClearActorViews()
        {
            _views.Clear();
        }

        internal VoxelMaterialSet.VoxelMaterial[] VoxelMaterials { get; private set; }

        /// <summary>
        ///     Voxel material set that will be used to map all voxels with proper materials.
        /// </summary>
        [AssetReference(typeof(VoxelMaterialSet))]
        public JsonAsset MaterialSet { get; set; }

        /// <summary>
        ///     The default material used for all solid blocks.
        /// </summary>
        public Material DefaultMaterial { get; set; }

        /// <summary>
        ///     The maximal amount of time, that chunk can be cached i.e. not visible, but not unloaded.
        ///     After this time, all resources of a cached chunk will be released.
        /// </summary>
        public int MaxChunkCacheTime { get; set; } = 60;

        /// <summary>
        ///     The world seed.
        /// </summary>
        public int WorldSeed { get; set; } = 0xDEAD;

        /// <summary>
        ///     The current active voxel terrain manager.
        /// </summary>
        public static VoxelTerrainManager Current { get; private set; }
    }
}