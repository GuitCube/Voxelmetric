﻿using Voxelmetric.Code.Data_types;

namespace Voxelmetric.Code.Core
{
    public class EmptyChunk: Chunk
    {
        public new static EmptyChunk Create(World world, BlockPos pos)
        {
            EmptyChunk chunk = Globals.MemPools.EmptyChunkPool.Pop();
            chunk.Init(world, pos);
            return chunk;
        }

        public new static void Remove(Chunk chunk)
        {
            EmptyChunk emptyChunk = (EmptyChunk)chunk;
            emptyChunk.Reset();
            Globals.MemPools.EmptyChunkPool.Push(emptyChunk);
        }

        public override void RequestGenerate()
        {
            base.RequestGenerate();
            blocks = new EmptyChunkBlocks(this);
        }
    }

    public class EmptyChunkBlocks: ChunkBlocks
    {
        public EmptyChunkBlocks(Chunk chunk): base(chunk)
        {
        }

        public override Block Get(BlockPos blockPos)
        {
            return chunk.world.Air;
        }

        public override Block LocalGet(BlockPos localBlockPos)
        {
            return chunk.world.Air;
        }

        public override void Set(BlockPos blockPos, Block block, bool updateChunk = true, bool setBlockModified = true)
        {
        }
    }
}