﻿using Voxelmetric.Code.Data_types;

namespace Voxelmetric.Code.Utilities
{
    public static class Env
    {
        public const int ChunkPow = 5;
        public const int ChunkPow2 = 2*ChunkPow;
        public const int ChunkSize = 1 << ChunkPow;
        public const int ChunkSizeHalf = ChunkSize>>1;
        public const int ChunkSizePow2 = ChunkSize*ChunkSize;
        public const int ChunkSizePow3 = ChunkSize*ChunkSizePow2;
        public const int ChunkMask = ChunkSize-1;


        //! Internal chunk size including room for edge fields as well so that we do not have to check whether we are within chunk bounds.
        //! This means we will ultimately consume a bit more memory in exchange for more performance
        public const int ChunkPadding = 1;
        public const int ChunkSizePlusPadding = ChunkSize + ChunkPadding;
        public const int ChunkSizeWithPadding = ChunkSize + ChunkPadding * 2;
        public const int ChunkSizeWithPaddingPow2 = ChunkSizeWithPadding*ChunkSizeWithPadding;
        public const int ChunkSizeWithPaddingPow3 = ChunkSizeWithPadding*ChunkSizeWithPaddingPow2;

        public const float BlockSize = 1f;
        public const float BlockSizeInv = 1f / BlockSize;
        public static readonly Vector3Int ChunkSizePos = Vector3Int.one * ChunkSize;

        // Padding added to the size of block faces to fix floating point issues
        // where tiny gaps can appear between block faces
        public const float BlockFacePadding = 0.0005f;
    }

    public static class Core
    {
        public const bool UseThreadPool = false;
        public const bool UseThreadedIO = true;
    }

    public static class Directories
    {
        public const string SaveFolder = "VoxelSaves";
    }
}
