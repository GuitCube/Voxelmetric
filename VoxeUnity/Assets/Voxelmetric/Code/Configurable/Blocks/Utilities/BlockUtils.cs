using UnityEngine;
using Voxelmetric.Code.Core;
using Voxelmetric.Code.Data_types;
using Voxelmetric.Code.Load_Resources.Textures;
using Voxelmetric.Code.Rendering;
using Voxelmetric.Code.Utilities;

namespace Voxelmetric.Code.Configurable.Blocks.Utilities
{
    public static class BlockUtils
    {
        //Adding a tiny overlap between block meshes may solve floating point imprecision
        //errors causing pixel size gaps between blocks when looking closely
        public static readonly float halfBlock = (Env.BlockSize/2)+Env.BlockFacePadding;

        public static readonly Vector3 HalfBlockVector = new Vector3(halfBlock, halfBlock, halfBlock);

        public static readonly Vector3[][] HalfBlockOffsets =
        {
            new[]
            {
                // Direction.up
                new Vector3(-halfBlock, +halfBlock, +halfBlock),
                new Vector3(+halfBlock, +halfBlock, +halfBlock),
                new Vector3(+halfBlock, +halfBlock, -halfBlock),
                new Vector3(-halfBlock, +halfBlock, -halfBlock)
            },
            new[]
            {
                // Direction.down
                new Vector3(-halfBlock, -halfBlock, -halfBlock),
                new Vector3(+halfBlock, -halfBlock, -halfBlock),
                new Vector3(+halfBlock, -halfBlock, +halfBlock),
                new Vector3(-halfBlock, -halfBlock, +halfBlock),
            },

            new[]
            {
                // Direction.north
                new Vector3(+halfBlock, -halfBlock, +halfBlock),
                new Vector3(+halfBlock, +halfBlock, +halfBlock),
                new Vector3(-halfBlock, +halfBlock, +halfBlock),
                new Vector3(-halfBlock, -halfBlock, +halfBlock)
            },
            new[]
            {
                // Direction.south
                new Vector3(-halfBlock, -halfBlock, -halfBlock),
                new Vector3(-halfBlock, +halfBlock, -halfBlock),
                new Vector3(+halfBlock, +halfBlock, -halfBlock),
                new Vector3(+halfBlock, -halfBlock, -halfBlock),
            },

            new[]
            {
                // Direction.east
                new Vector3(+halfBlock, -halfBlock, -halfBlock),
                new Vector3(+halfBlock, +halfBlock, -halfBlock),
                new Vector3(+halfBlock, +halfBlock, +halfBlock),
                new Vector3(+halfBlock, -halfBlock, +halfBlock)
            },
            new[]
            {
                // Direction.west
                new Vector3(-halfBlock, -halfBlock, +halfBlock),
                new Vector3(-halfBlock, +halfBlock, +halfBlock),
                new Vector3(-halfBlock, +halfBlock, -halfBlock),
                new Vector3(-halfBlock, -halfBlock, -halfBlock),
            }
        };

        public static void PrepareColors(Chunk chunk, Vector3Int localPos, VertexData[] vertexData, Direction direction)
        {
            if (chunk.world.config.addAOToMesh)
            {
                bool nSolid = false;
                bool eSolid = false;
                bool sSolid = false;
                bool wSolid = false;

                bool wnSolid = false;
                bool neSolid = false;
                bool esSolid = false;
                bool swSolid = false;

                ChunkBlocks blocks = chunk.blocks;

                switch (direction)
                {
                    case Direction.up:
                        nSolid = blocks.Get(localPos.Add(0, 1, 1)).Solid;
                        eSolid = blocks.Get(localPos.Add(1, 1, 0)).Solid;
                        sSolid = blocks.Get(localPos.Add(0, 1, -1)).Solid;
                        wSolid = blocks.Get(localPos.Add(-1, 1, 0)).Solid;
                        
                        wnSolid = blocks.Get(localPos.Add(-1, 1, 1)).Solid;
                        neSolid = blocks.Get(localPos.Add(1, 1, 1)).Solid;
                        esSolid = blocks.Get(localPos.Add(1, 1, -1)).Solid;
                        swSolid = blocks.Get(localPos.Add(-1, 1, -1)).Solid;
                        break;
                    case Direction.down:
                        nSolid = blocks.Get(localPos.Add(0, -1, -1)).Solid;
                        eSolid = blocks.Get(localPos.Add(1, -1, 0)).Solid;
                        sSolid = blocks.Get(localPos.Add(0, -1, 1)).Solid;
                        wSolid = blocks.Get(localPos.Add(-1, -1, 0)).Solid;

                        wnSolid = blocks.Get(localPos.Add(-1, -1, -1)).Solid;
                        neSolid = blocks.Get(localPos.Add(1, -1, -1)).Solid;
                        esSolid = blocks.Get(localPos.Add(1, -1, 1)).Solid;
                        swSolid = blocks.Get(localPos.Add(-1, -1, 1)).Solid;
                        break;
                    case Direction.north:
                        nSolid = blocks.Get(localPos.Add(1, 0, 1)).Solid;
                        eSolid = blocks.Get(localPos.Add(0, 1, 1)).Solid;
                        sSolid = blocks.Get(localPos.Add(-1, 0, 1)).Solid;
                        wSolid = blocks.Get(localPos.Add(0, -1, 1)).Solid;

                        esSolid = blocks.Get(localPos.Add(-1, 1, 1)).Solid;
                        neSolid = blocks.Get(localPos.Add(1, 1, 1)).Solid;
                        wnSolid = blocks.Get(localPos.Add(1, -1, 1)).Solid;
                        swSolid = blocks.Get(localPos.Add(-1, -1, 1)).Solid;
                        break;
                    case Direction.east:
                        nSolid = blocks.Get(localPos.Add(1, 0, -1)).Solid;
                        eSolid = blocks.Get(localPos.Add(1, 1, 0)).Solid;
                        sSolid = blocks.Get(localPos.Add(1, 0, 1)).Solid;
                        wSolid = blocks.Get(localPos.Add(1, -1, 0)).Solid;

                        esSolid = blocks.Get(localPos.Add(1, 1, 1)).Solid;
                        neSolid = blocks.Get(localPos.Add(1, 1, -1)).Solid;
                        wnSolid = blocks.Get(localPos.Add(1, -1, -1)).Solid;
                        swSolid = blocks.Get(localPos.Add(1, -1, 1)).Solid;
                        break;
                    case Direction.south:
                        nSolid = blocks.Get(localPos.Add(-1, 0, -1)).Solid;
                        eSolid = blocks.Get(localPos.Add(0, 1, -1)).Solid;
                        sSolid = blocks.Get(localPos.Add(1, 0, -1)).Solid;
                        wSolid = blocks.Get(localPos.Add(0, -1, -1)).Solid;

                        esSolid = blocks.Get(localPos.Add(1, 1, -1)).Solid;
                        neSolid = blocks.Get(localPos.Add(-1, 1, -1)).Solid;
                        wnSolid = blocks.Get(localPos.Add(-1, -1, -1)).Solid;
                        swSolid = blocks.Get(localPos.Add(1, -1, -1)).Solid;
                        break;
                    case Direction.west:
                        nSolid = blocks.Get(localPos.Add(-1, 0, 1)).Solid;
                        eSolid = blocks.Get(localPos.Add(-1, 1, 0)).Solid;
                        sSolid = blocks.Get(localPos.Add(-1, 0, -1)).Solid;
                        wSolid = blocks.Get(localPos.Add(-1, -1, 0)).Solid;

                        esSolid = blocks.Get(localPos.Add(-1, 1, -1)).Solid;
                        neSolid = blocks.Get(localPos.Add(-1, 1, 1)).Solid;
                        wnSolid = blocks.Get(localPos.Add(-1, -1, 1)).Solid;
                        swSolid = blocks.Get(localPos.Add(-1, -1, -1)).Solid;
                        break;
                }

                SetColorsAO(vertexData, wnSolid, nSolid, neSolid, eSolid, esSolid, sSolid, swSolid, wSolid,
                            chunk.world.config.ambientOcclusionStrength, direction);
            }
            else
            {
                SetColors(vertexData, 1, 1, 1, 1, 1);
            }
        }

        public static void PrepareTexture(Chunk chunk, Vector3Int localPos, VertexData[] vertexData, Direction direction, TextureCollection textureCollection)
        {
            Rect texture = textureCollection.GetTexture(chunk, localPos, direction);

            vertexData[3].UV = new Vector2(texture.x+texture.width, texture.y);
            vertexData[2].UV = new Vector2(texture.x+texture.width, texture.y+texture.height);
            vertexData[1].UV = new Vector2(texture.x, texture.y+texture.height);
            vertexData[0].UV = new Vector2(texture.x, texture.y);
        }

        public static void PrepareTexture(Chunk chunk, Vector3Int localPos, VertexData[] vertexData, Direction direction, TextureCollection[] textureCollections)
        {
            Rect texture = textureCollections[(int)direction].GetTexture(chunk, localPos, direction);

            vertexData[3].UV = new Vector2(texture.x+texture.width, texture.y);
            vertexData[2].UV = new Vector2(texture.x+texture.width, texture.y+texture.height);
            vertexData[1].UV = new Vector2(texture.x, texture.y+texture.height);
            vertexData[0].UV = new Vector2(texture.x, texture.y);
        }

        public static void PrepareVertices(Vector3Int localPos, VertexData[] vertexData, Direction direction)
        {
            Vector3 vPos = localPos;
            int d = DirectionUtils.Get(direction);

            vertexData[0].Vertex = vPos+HalfBlockOffsets[d][0];
            vertexData[1].Vertex = vPos+HalfBlockOffsets[d][1];
            vertexData[2].Vertex = vPos+HalfBlockOffsets[d][2];
            vertexData[3].Vertex = vPos+HalfBlockOffsets[d][3];
        }

        private static void SetColorsAO(VertexData[] vertexData, bool wnSolid, bool nSolid, bool neSolid, bool eSolid, bool esSolid, bool sSolid, bool swSolid, bool wSolid, float strength, Direction direction)
        {
            float ne = 1f;
            float es = 1f;
            float sw = 1f;
            float wn = 1f;

            strength *= 0.5f;

            if (nSolid)
            {
                wn -= strength;
                ne -= strength;
            }

            if (eSolid)
            {
                ne -= strength;
                es -= strength;
            }

            if (sSolid)
            {
                es -= strength;
                sw -= strength;
            }

            if (wSolid)
            {
                sw -= strength;
                wn -= strength;
            }

            if (neSolid)
                ne -= strength;

            if (swSolid)
                sw -= strength;

            if (wnSolid)
                wn -= strength;

            if (esSolid)
                es -= strength;

            SetColors(vertexData, ne, es, sw, wn, 1, direction);
        }

        public static void SetColors(VertexData[] data, float ne, float es, float sw, float wn, float light, Direction direction=Direction.up)
        {
            wn = (wn * light);
            ne = (ne * light);
            es = (es * light);
            sw = (sw * light);

            switch (direction)
            {
                case Direction.down:
                    data[0].Color = new Color(wn, wn, wn);
                    data[3].Color = new Color(ne, ne, ne);
                    data[2].Color = new Color(es, es, es);
                    data[1].Color = new Color(sw, sw, sw);
                    break;
                case Direction.up:
                    data[1].Color = new Color(wn, wn, wn);
                    data[2].Color = new Color(ne, ne, ne);
                    data[3].Color = new Color(es, es, es);
                    data[0].Color = new Color(sw, sw, sw);
                    break;
                case Direction.north:
                case Direction.east:
                    data[0].Color = new Color(wn, wn, wn);
                    data[1].Color = new Color(ne, ne, ne);
                    data[2].Color = new Color(es, es, es);
                    data[3].Color = new Color(sw, sw, sw);
                    break;
                default: // east, south
                    data[3].Color = new Color(wn, wn, wn);
                    data[2].Color = new Color(ne, ne, ne);
                    data[1].Color = new Color(es, es, es);
                    data[0].Color = new Color(sw, sw, sw);
                    break;
            }
        }

        public static void SetColors(VertexData[] data, ref Color color)
        {
            data[0].Color = color;
            data[1].Color = color;
            data[2].Color = color;
            data[3].Color = color;
        }
    }
}