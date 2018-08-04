// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski
// Erdroy's Cube meshing algorithm (c) 2016-2018 Damian 'Erdroy' Korczowski
// License: MIT

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlaxEngine;
using FlaxVoxels.Math;

namespace FlaxVoxels.Terrain.Meshing
{
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

        public void GenerateMesh(Vector3Int worldPosition, VoxelTerrainChunk chunk, ref Model chunkModel)
        {
            // Generate in order: Y [XZ]
            for (var y = 0; y < VoxelTerrainChunk.ChunkHeight; y++)
            for (var x = 0; x < VoxelTerrainChunk.ChunkWidth; x++)
            for (var z = 0; z < VoxelTerrainChunk.ChunkLength; z++)
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

                // Build triangles
                for (var i = 0; i < 36; i += 3)
                {
                    var corner0 = cornerData[i + 0];
                    if (corner0 == -1)
                        break;
                    var corner1 = cornerData[i + 1];
                    var corner2 = cornerData[i + 2];

                    var vertex0 = voxelOffsetFloat + CornerTable[corner0];
                    var vertex1 = voxelOffsetFloat + CornerTable[corner1];
                    var vertex2 = voxelOffsetFloat + CornerTable[corner2];

                    AddTriangle(_vertices.Count, baseVoxel, vertex0, vertex1, vertex2);
                }
            }

            // Check if we have something
            if (_vertices.Count == 0) return;

            Debug.Log("Chunk generated! Vertex count " + _vertices.Count);

            // Apply mesh
            chunkModel.LODs[0].Meshes[0].UpdateMesh(_vertices.ToArray(), _triangles.ToArray(), _normals.ToArray(), null,
                _uvs.ToArray(), _colors.ToArray());
        }

        public void Clear()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _colors.Clear();
            _triangles.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddTriangle(int baseIndex, Voxel voxel, Vector3 v0, Vector3 v1, Vector3 v2)
        {
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

            _uvs.Add(Vector2.Zero);
            _uvs.Add(Vector2.Zero);
            _uvs.Add(Vector2.Zero);

            _colors.Add(Color.DarkOliveGreen);
            _colors.Add(Color.DarkOliveGreen);
            _colors.Add(Color.DarkOliveGreen);

            // TODO: Set color as material base color + A is AO

            // TODO: Build UVs
            // TODO: Build AO
        }
    }
}