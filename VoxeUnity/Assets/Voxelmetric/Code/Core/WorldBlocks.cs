﻿using System;
using Voxelmetric.Code.Common;
using Voxelmetric.Code.Data_types;
using Voxelmetric.Code.Load_Resources.Blocks;
using Voxelmetric.Code.Utilities;

namespace Voxelmetric.Code.Core
{
    public class WorldBlocks
    {
        World world;

        public WorldBlocks(World world)
        {
            this.world = world;
        }

        /// <summary>
        /// Gets the chunk and retrives the block data at the given coordinates
        /// </summary>
        /// <param name="pos">Global position of the block data</param>
        public BlockData Get(Vector3Int pos)
        {
            // Return air for chunk that do not exist
            Chunk chunk = world.chunks.Get(pos);
            if (chunk==null)
                return BlockProvider.AirBlock;

            int xx = pos.x&Env.ChunkMask;
            int yy = pos.y&Env.ChunkMask;
            int zz = pos.z&Env.ChunkMask;

            return chunk.blocks.Get(Helpers.GetChunkIndex1DFrom3D(xx, yy, zz));
        }

        /// <summary>
        /// Retrives the block at given world coordinates
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        public Block GetBlock(Vector3Int pos)
        {
            // Return air for chunk that do not exist
            Chunk chunk = world.chunks.Get(pos);
            if (chunk == null)
                return world.blockProvider.BlockTypes[BlockProvider.AirType];

            int xx = pos.x&Env.ChunkMask;
            int yy = pos.y&Env.ChunkMask;
            int zz = pos.z&Env.ChunkMask;

            BlockData blockData = chunk.blocks.Get(Helpers.GetChunkIndex1DFrom3D(xx, yy, zz));
            return world.blockProvider.BlockTypes[blockData.Type];
        }

        /// <summary>
        /// Sets the block data at given world coordinates
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        /// <param name="blockData">A block to be placed on a given position</param>
        public void Set(Vector3Int pos, BlockData blockData)
        {
            Chunk chunk = world.chunks.Get(pos);
            if (chunk==null)
                return;

            int xx = pos.x&Env.ChunkMask;
            int yy = pos.y&Env.ChunkMask;
            int zz = pos.z&Env.ChunkMask;

            chunk.blocks.SetInner(Helpers.GetChunkIndex1DFrom3D(xx, yy, zz), blockData);
        }

        /// <summary>
        /// Sets the block data at given world coordinates. It does not perform any logic. It simply sets to block
        /// Use this function only for generating your terrain and structures
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        /// <param name="blockData">A block to be placed on a given position</param>
        public void SetRaw(Vector3Int pos, BlockData blockData)
        {
            Chunk chunk = world.chunks.Get(pos);
            if (chunk == null)
                return;

            int xx = pos.x & Env.ChunkMask;
            int yy = pos.y & Env.ChunkMask;
            int zz = pos.z & Env.ChunkMask;

            chunk.blocks.SetRaw(Helpers.GetChunkIndex1DFrom3D(xx, yy, zz), blockData);
        }

        /// <summary>
        /// Sets blocks to a given value in a given range
        /// </summary>
        /// <param name="posFrom">Starting position in local chunk coordinates</param>
        /// <param name="posTo">Ending position in local chunk coordinates</param>
        /// <param name="blockData">A block to be placed on a given position</param>
        public void SetRange(Vector3Int posFrom, Vector3Int posTo, BlockData blockData)
        {
            Vector3Int chunkPosFrom = Chunk.ContainingCoordinates(posFrom);
            Vector3Int chunkPosTo = Chunk.ContainingCoordinates(posTo);

            // Update all chunks in range
            int minY = (posFrom.y+chunkPosFrom.y)&Env.ChunkMask;
            for (int cy = chunkPosFrom.y; cy<=chunkPosTo.y; cy += Env.ChunkSize, minY = 0)
            {
                int maxY = ((minY+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                maxY = Math.Min(maxY+cy-1, posTo.y)&Env.ChunkMask;
                int minZ = (posFrom.z+chunkPosFrom.z)&Env.ChunkMask;

                for (int cz = chunkPosFrom.z; cz<=chunkPosTo.z; cz += Env.ChunkSize, minZ = 0)
                {
                    int maxZ = ((minZ+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                    maxZ = Math.Min(maxZ+cz-1, posTo.z)&Env.ChunkMask;
                    int minX = (posFrom.x+chunkPosFrom.x)&Env.ChunkMask;

                    for (int cx = chunkPosFrom.x; cx<=chunkPosTo.x; cx += Env.ChunkSize, minX = 0)
                    {
                        Chunk chunk = world.chunks.Get(new Vector3Int(cx, cy, cz));
                        if (chunk==null)
                            continue;

                        int maxX = ((minX+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                        maxX = Math.Min(maxX+cx-1, posTo.x)&Env.ChunkMask;

                        Vector3Int from = new Vector3Int(minX, minY, minZ);
                        Vector3Int to = new Vector3Int(maxX, maxY, maxZ);
                        chunk.blocks.SetRange(from, to, blockData);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the block data at given world coordinates, updates the chunk and its
        /// neighbors if the Update chunk flag is true or not set.
        /// </summary>
        /// <param name="pos">Global position of the block</param>
        /// <param name="blockData">The block be placed</param>
        /// <param name="setBlockModified">Set to true to mark chunk data as modified</param>
        public void Modify(Vector3Int pos, BlockData blockData, bool setBlockModified)
        {
            Chunk chunk = world.chunks.Get(pos);
            if (chunk==null)
                return;

            Vector3Int vector3Int = new Vector3Int(
                pos.x & Env.ChunkMask,
                pos.y & Env.ChunkMask,
                pos.z & Env.ChunkMask
                );

            chunk.blocks.Modify(vector3Int, blockData, setBlockModified);
        }

        /// <summary>
        /// Queues a modification of blocks in a given range
        /// </summary>
        /// <param name="posFrom">Starting positon in local chunk coordinates</param>
        /// <param name="posTo">Ending position in local chunk coordinates</param>
        /// <param name="blockData">BlockData to place at the given location</param>
        /// <param name="setBlockModified">Set to true to mark chunk data as modified</param>
        public void ModifyRange(Vector3Int posFrom, Vector3Int posTo, BlockData blockData, bool setBlockModified)
        {
            Vector3Int chunkPosFrom = Chunk.ContainingCoordinates(posFrom);
            Vector3Int chunkPosTo = Chunk.ContainingCoordinates(posTo);

            // Update all chunks in range
            int minY = (posFrom.y+chunkPosFrom.y)&Env.ChunkMask;
            for (int cy = chunkPosFrom.y; cy<=chunkPosTo.y; cy += Env.ChunkSize, minY = 0)
            {
                int maxY = ((minY+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                maxY = Math.Min(maxY+cy-1, posTo.y)&Env.ChunkMask;
                int minZ = (posFrom.z+chunkPosFrom.z)&Env.ChunkMask;

                for (int cz = chunkPosFrom.z; cz<=chunkPosTo.z; cz += Env.ChunkSize, minZ = 0)
                {
                    int maxZ = ((minZ+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                    maxZ = Math.Min(maxZ+cz-1, posTo.z)&Env.ChunkMask;
                    int minX = (posFrom.x+chunkPosFrom.x)&Env.ChunkMask;

                    for (int cx = chunkPosFrom.x; cx<=chunkPosTo.x; cx += Env.ChunkSize, minX = 0)
                    {
                        Chunk chunk = world.chunks.Get(new Vector3Int(cx, cy, cz));
                        if (chunk==null)
                            continue;

                        int maxX = ((minX+Env.ChunkSize)>>Env.ChunkPow)<<Env.ChunkPow;
                        maxX = Math.Min(maxX+cx-1, posTo.x)&Env.ChunkMask;

                        Vector3Int from = new Vector3Int(minX, minY, minZ);
                        Vector3Int to = new Vector3Int(maxX, maxY, maxZ);
                        chunk.blocks.ModifyRange(from, to, blockData, setBlockModified);
                    }
                }
            }
        }
    }
}
