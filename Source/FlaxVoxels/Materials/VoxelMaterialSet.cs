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
            [EditorOrder(0)]
	        public int Id;

	        [EditorOrder(1)]
            public string Name;

	        [EditorOrder(2), Range(0, 255)]
	        public int Hardness = 16;

            [EditorOrder(3)]
            public Texture Texture;

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
