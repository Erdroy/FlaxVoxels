// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxEngine;

namespace FlaxVoxels.Materials
{
    /// <summary>
    ///     VoxelMaterialSet class. Provides API for creating voxel material sets.
    /// </summary>
	public class VoxelMaterialSet
	{
        /// <summary>
        ///     VoxelMaterial set class.
        /// </summary>
        public class VoxelMaterial
	    {
            /// <summary>
            ///     The voxel id, which gets this material assigned.
            /// </summary>
            [EditorOrder(0)]
	        public int Id;

            /// <summary>
            ///     The name of this material.
            /// </summary>
	        [EditorOrder(1)]
            public string Name;

            /// <summary>
            ///     The material hardness of this material, hardness which is 0, 
            ///     is indestructible (through dig function, but still can be set with Get/SetVoxel)!
            /// </summary>
	        [EditorOrder(2), Range(0, 255)]
	        public int Hardness = 16;

            /// <summary>
            ///     The texture of this material, this will be packed into atlas if texturing is enabled.
            /// </summary>
            [EditorOrder(3)]
            public Texture Texture;

            /// <summary>
            ///     The base color of this material, this won't be used when texturing is enabled.
            /// </summary>
	        [EditorOrder(4)]
            public Color BaseColor;
        }

        /// <summary>
        ///     Wrapper for VoxelMaterial class (to make the editor layout a bit cleaner).
        /// </summary>
	    public struct VoxelMaterialEntry
	    {
            public VoxelMaterial Material;
	    }

        /// <summary>
        ///     The unordered list of materials.
        /// </summary>
        public VoxelMaterialEntry[] Materials;
	}
}
