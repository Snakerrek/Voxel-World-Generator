using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public enum BlockSide { FRONT, BACK, LEFT, RIGHT, TOP, BOTTOM };

    BlockType blockType;
    Chunk chunkParent;
    Vector3 blockPosition;

    static int[] triangles = new int[] { 3, 1, 0, 3, 2, 1 };

    static Vector3[] vertices = new Vector3[8] {  new Vector3(-0.5f, -0.5f,  0.5f),
                                                  new Vector3( 0.5f, -0.5f,  0.5f),
                                                  new Vector3( 0.5f, -0.5f, -0.5f),
                                                  new Vector3(-0.5f, -0.5f, -0.5f),
                                                  new Vector3(-0.5f,  0.5f,  0.5f),
                                                  new Vector3( 0.5f,  0.5f,  0.5f),
                                                  new Vector3( 0.5f,  0.5f, -0.5f),
                                                  new Vector3(-0.5f,  0.5f, -0.5f) };

    static Vector3[] frontVertices = new Vector3[] { vertices[4], vertices[5], vertices[1], vertices[0] };
    static Vector3[] backVertices = new Vector3[] { vertices[6], vertices[7], vertices[3], vertices[2] };
    static Vector3[] leftVertices = new Vector3[] { vertices[7], vertices[4], vertices[0], vertices[3] };
    static Vector3[] rightVertices = new Vector3[] { vertices[5], vertices[6], vertices[2], vertices[1] };
    static Vector3[] topVertices = new Vector3[] { vertices[7], vertices[6], vertices[5], vertices[4] };
    static Vector3[] bottomVertices = new Vector3[] { vertices[0], vertices[1], vertices[2], vertices[3] };

    public Block(BlockType blockType, Chunk chunkParent, Vector3 blockPosition)
    {
        this.blockType = blockType;
        this.chunkParent = chunkParent;
        this.blockPosition = blockPosition;
    }

    // Creating all cube sides
    public void CreateBlock()
    {
        if (blockType.isTransparent) return;

        foreach(BlockSide blockSide in Enum.GetValues(typeof(BlockSide)))
        {
            if (HasTransparentNeighbour(blockSide))
                GenerateBlockSide(blockSide);
        }
    }

    // Checking if neighbour block is transparent
    bool HasTransparentNeighbour(BlockSide blockSide)
    {
        Vector3 neighbourPosition = new Vector3(0,0,0);

        switch(blockSide)
        {
            case BlockSide.FRONT:
                neighbourPosition = new Vector3(blockPosition.x, blockPosition.y, blockPosition.z + 1);
                break;
            case BlockSide.BACK:
                neighbourPosition = new Vector3(blockPosition.x, blockPosition.y, blockPosition.z - 1);
                break;
            case BlockSide.LEFT:
                neighbourPosition = new Vector3(blockPosition.x - 1, blockPosition.y, blockPosition.z);
                break;
            case BlockSide.RIGHT:
                neighbourPosition = new Vector3(blockPosition.x + 1, blockPosition.y, blockPosition.z);
                break;
            case BlockSide.TOP:
                neighbourPosition = new Vector3(blockPosition.x, blockPosition.y + 1, blockPosition.z);
                break;
            case BlockSide.BOTTOM:
                neighbourPosition = new Vector3(blockPosition.x, blockPosition.y - 1, blockPosition.z);
                break;
        }

        Block[,,] chunkBlocks = chunkParent.chunkBlocks;

        if(neighbourPosition.x < 0 || neighbourPosition.x >= World.chunkSize ||
           neighbourPosition.y < 0 || neighbourPosition.y >= World.chunkSize ||
           neighbourPosition.z < 0 || neighbourPosition.z >= World.chunkSize)
        {
            Vector3 neighbourChunkPosition = this.chunkParent.chunkObject.transform.position;
            neighbourChunkPosition.x += (neighbourPosition.x - blockPosition.x) * World.chunkSize;
            neighbourChunkPosition.y += (neighbourPosition.y - blockPosition.y) * World.chunkSize;
            neighbourChunkPosition.z += (neighbourPosition.z - blockPosition.z) * World.chunkSize;

            string neighboutChunkName = World.GenerateChunkName(neighbourChunkPosition);

            Chunk neighboutChunk;
            if (World.chunks.TryGetValue(neighboutChunkName, out neighboutChunk))
            {
                chunkBlocks = neighboutChunk.chunkBlocks;
            }
            else 
                return true;
        }

        if (neighbourPosition.x < 0) neighbourPosition.x = World.chunkSize - 1;
        if (neighbourPosition.y < 0) neighbourPosition.y = World.chunkSize - 1;
        if (neighbourPosition.z < 0) neighbourPosition.z = World.chunkSize - 1;
        if (neighbourPosition.x >= World.chunkSize) neighbourPosition.x = 0;
        if (neighbourPosition.y >= World.chunkSize) neighbourPosition.y = 0;
        if (neighbourPosition.z >= World.chunkSize) neighbourPosition.z = 0;

        var neighbourBlockType = chunkBlocks[(int)neighbourPosition.x, (int)neighbourPosition.y, (int)neighbourPosition.z].blockType;

        if (neighbourBlockType.isTranslucent && !neighbourBlockType.isTransparent && this.blockType.isTranslucent)
            return false;

        return neighbourBlockType.isTransparent || neighbourBlockType.isTranslucent;
    }

    // Generating mesh values on side of the cube
    void GenerateBlockSide(BlockSide side)
    {
        switch (side)
        {
            case BlockSide.FRONT:
                foreach(Vector3 vertex in frontVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSide.BACK:
                foreach (Vector3 vertex in backVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSide.LEFT:
                foreach (Vector3 vertex in leftVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSide.RIGHT:
                foreach (Vector3 vertex in rightVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSide.TOP:
                foreach (Vector3 vertex in topVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
            case BlockSide.BOTTOM:
                foreach (Vector3 vertex in bottomVertices)
                {
                    chunkParent.vertices.Add(blockPosition + vertex);
                }
                break;
        }

        foreach(Vector2 blockUV in blockType.GetBlockUVs(side))
        {
            chunkParent.uvs.Add(blockUV);
        }

        foreach(int triangle in triangles)
        {
            if (this.blockType.isLiquid())
                chunkParent.liquidTriangles.Add(chunkParent.vertexIndex + triangle);
            else if (this.blockType.isTransparent || this.blockType.isTranslucent)
                chunkParent.transparentTriangles.Add(chunkParent.vertexIndex + triangle);
            else
                chunkParent.triangles.Add(chunkParent.vertexIndex + triangle);
        }

        chunkParent.vertexIndex += 4;
    }

    public BlockType GetBlockType()
    {
        return this.blockType;
    }
}
