using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] Material material = null;

    enum BlockSide { FRONT, BACK, LEFT, RIGHT, TOP, BOTTOM};
    Vector3[] vertices = new Vector3[8] { new Vector3(-0.5f, -0.5f,  0.5f),
                                          new Vector3( 0.5f, -0.5f,  0.5f),
                                          new Vector3( 0.5f, -0.5f, -0.5f),
                                          new Vector3(-0.5f, -0.5f, -0.5f),
                                          new Vector3(-0.5f,  0.5f,  0.5f),
                                          new Vector3( 0.5f,  0.5f,  0.5f),
                                          new Vector3( 0.5f,  0.5f, -0.5f),
                                          new Vector3(-0.5f,  0.5f, -0.5f) };
    Vector2[] uv = new Vector2[4] { new Vector2(0f, 0f),
                                    new Vector2(1f, 0f),
                                    new Vector2(0f, 1f),
                                    new Vector2(1f, 1f) };

    private void Start()
    {
        CreateBlock();
    }

    // Creating all cube sides
    void CreateBlock()
    {
        foreach(BlockSide blockSide in Enum.GetValues(typeof(BlockSide)))
        {
            CreateBlockSide(blockSide);
        }
    }

    void CreateBlockSide(BlockSide side)
    {
        Mesh mesh = new Mesh();
        mesh = GenerateBlockSide(mesh, side);

        GameObject blockSide = new GameObject("Block_side");
        blockSide.transform.parent = this.gameObject.transform;

        MeshFilter meshFilter = blockSide.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = blockSide.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    // Generating mesh values on side of the cube
    Mesh GenerateBlockSide(Mesh mesh, BlockSide side)
    {
        switch (side)
        {
            case BlockSide.FRONT:
                mesh.vertices = new Vector3[] { vertices[4], vertices[5], vertices[1], vertices[0] };
                mesh.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSide.BACK:
                mesh.vertices = new Vector3[] { vertices[6], vertices[7], vertices[3], vertices[2] };
                mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSide.LEFT:
                mesh.vertices = new Vector3[] { vertices[7], vertices[4], vertices[0], vertices[3] };
                mesh.normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSide.RIGHT:
                mesh.vertices = new Vector3[] { vertices[5], vertices[6], vertices[2], vertices[1] };
                mesh.normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSide.TOP:
                mesh.vertices = new Vector3[] { vertices[7], vertices[6], vertices[5], vertices[4] };
                mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case BlockSide.BOTTOM:
                mesh.vertices = new Vector3[] { vertices[0], vertices[1], vertices[2], vertices[3] };
                mesh.normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                mesh.uv = new Vector2[] { uv[3], uv[2], uv[0], uv[1] };
                mesh.triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }
        return mesh;
    }
}
