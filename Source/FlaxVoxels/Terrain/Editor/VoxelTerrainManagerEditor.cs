// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEngine;
using FlaxVoxels.Terrain;

namespace FlaxVoxels.Materials.Editor
{
    [CustomEditor(typeof(VoxelTerrainManager))]
    public class VoxelTerrainManagerEditor : GenericEditor
    {
        public override void Initialize(LayoutElementsContainer layout)
        {
            base.Initialize(layout);
            
            var saveButton = layout.Button("Create material set", Color.Green);
            saveButton.Button.Clicked += () =>
            {
                FlaxEditor.Editor.SaveJsonAsset("Content/VoxelMaterials.json", new VoxelMaterialSet());
            };
        }
    }
}
