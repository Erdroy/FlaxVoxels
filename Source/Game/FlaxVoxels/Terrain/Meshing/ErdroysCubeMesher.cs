// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski
// Erdroy's Cube meshing algorithm (c) 2016-2018 Damian 'Erdroy' Korczowski
// License: MIT

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain.Meshing
{
    /// <inheritdoc />
    /// <summary>
    ///     ErdroysCubeMesher class. Provides Erdroy's Cube meshing algorithm implementation.
    /// </summary>
    public class ErdroysCubeMesher : IVoxelTerrainMesher
    {
        private static readonly Vector3[] CornerTable =
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f)
        };

        private static readonly Vector2[] UVTable =
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
        };

        private static readonly Vector3Int[] FaceTable =
        {
            new Vector3Int(0, 0, 1), // Front face
            new Vector3Int(0, 0, -1), // Back face
            new Vector3Int(-1, 0, 0), // Left face
            new Vector3Int(1, 0, 0), // Right face
            new Vector3Int(0, 1, 0), // Top face
            new Vector3Int(0, -1, 0) // Bottom face
        };

        private readonly List<Color32> _colors;
        private readonly List<Vector3> _normals;
        private readonly List<int> _triangles;
        private readonly List<Vector2> _uvs;
        private readonly List<Vector3> _vertices;

        public ErdroysCubeMesher()
        {
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _uvs = new List<Vector2>();
            _colors = new List<Color32>();
            _triangles = new List<int>();
        }

        /// <inheritdoc />
        public void GenerateMesh(VoxelTerrainChunk chunk)
        {
            // Generate in order: Y [XZ]
            for (var y = 0; y < VoxelTerrainChunk.Height; y++)
            for (var x = 0; x < VoxelTerrainChunk.Width; x++)
            for (var z = 0; z < VoxelTerrainChunk.Length; z++)
            {
                var voxelOffset = new Vector3Int(x, y, z);
                var voxelOffsetFloat = new Vector3(x, y, z);
                var baseVoxel = chunk.GetVoxelFast(voxelOffset);

                // Skip if current voxel is empty
                if (baseVoxel.IsAir)
                    continue;

                // Build case code
                var caseCode = 0;
                for (var i = 0; i < 6; i++)
                {
                    var voxel = chunk.GetVoxel(voxelOffset + FaceTable[i]);
                    if (voxel.IsAir)
                        caseCode |= 1 << i;
                }

                // There is no need to process this cell, 
                // as there is no empty space or only empty space.
                // When caseCode is 0, the cell is empty.
                // When caseCode is == 64 (>= 64 - just sanity check...), the cell is full.
                if (caseCode == 0 || caseCode >= 64)
                    continue;

                // TODO: Different/Custom shapes

                var cornerData = ErdroysCube.ErdroyTable[caseCode];
                var uvData = ErdroysCube.ErdroyUVTable[caseCode];

                // Build triangles
                for (var i = 0; i < 36; i += 3)
                {
                    var corner0 = cornerData[i + 0];
                    if (corner0 == -1)
                        break;
                    var corner1 = cornerData[i + 1];
                    var corner2 = cornerData[i + 2];

                    var uv0 = uvData[i + 0];
                    var uv1 = uvData[i + 1];
                    var uv2 = uvData[i + 2];

                    AddTriangle(i, _vertices.Count, baseVoxel, voxelOffsetFloat, corner0, corner1, corner2, uv0, uv1, uv2);
                }
            }

            // Check if we have something
            if (_vertices.Count == 0)
            {
                Clear();
                return;
            }
            
            // Apply mesh
            chunk.Model.LODs[0].Meshes[0].UpdateMesh(_vertices.ToArray(), _triangles.ToArray(), _normals.ToArray(),
                null,
                _uvs.ToArray(), _colors.ToArray());

            // oof?
            chunk.Model.MaterialSlots[0].Material = VoxelTerrainManager.Current.DefaultMaterial;

            // Clear mesher state
            Clear();
        }

        private void Clear()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _colors.Clear();
            _triangles.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddTriangle(int i, int baseIndex, Voxel voxel, Vector3 voxelOffsetFloat, int corner0, int corner1, int corner2, int uv0, int uv1, int uv2)
        {
            var v0 = voxelOffsetFloat + CornerTable[corner0];
            var v1 = voxelOffsetFloat + CornerTable[corner1];
            var v2 = voxelOffsetFloat + CornerTable[corner2];

            _vertices.Add(v0);
            _vertices.Add(v1);
            _vertices.Add(v2);

            _triangles.Add(baseIndex + 0);
            _triangles.Add(baseIndex + 1);
            _triangles.Add(baseIndex + 2);

            // Calculate normal
            var normal = Vector3.Cross(v1 - v0, v2 - v0).Normalized;

            _normals.Add(normal);
            _normals.Add(normal);
            _normals.Add(normal);

            const int atlasSize = 256;
            const int textureSize = 16;
            const float textureSizeUv = textureSize / (float)atlasSize;

            var texture = (int)voxel.VoxelId;
            var cordX = texture % textureSize;
            var cordY = texture / textureSize;

            var offset = new Vector2(cordX * textureSizeUv, cordY * textureSizeUv);

            _uvs.Add(UVTable[uv0] * textureSizeUv + offset);
            _uvs.Add(UVTable[uv1] * textureSizeUv + offset);
            _uvs.Add(UVTable[uv2] * textureSizeUv + offset);

            // Select material based on the voxel id
            var material = VoxelTerrainManager.Current.VoxelMaterials[voxel.VoxelId];

            // Add material color
            _colors.Add(material.BaseColor);
            _colors.Add(material.BaseColor);
            _colors.Add(material.BaseColor);
            
            // TODO: Build AO
            // TODO: Set color as material base color + A is AO
        }
    }
}