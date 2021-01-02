using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material blockMaterial;
    public Block[,,] chunkBlocks;
    void Start()
    {
        StartCoroutine(GenerateChunk(16));
    }

    IEnumerator GenerateChunk(int chunkSize)
    {
        chunkBlocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                    chunkBlocks[x, y, z] = new Block(Block.BlockType.DIRT, this.gameObject, new Vector3(x,y,z), blockMaterial);

        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                    chunkBlocks[x, y, z].CreateBlock();
        CombineSides();
        yield return null;
    }

    // Combining sides into one object
    void CombineSides()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineSides = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineSides[i].mesh = meshFilters[i].sharedMesh;
            combineSides[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        MeshFilter blockMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        blockMeshFilter.mesh = new Mesh();
        blockMeshFilter.mesh.CombineMeshes(combineSides);

        MeshRenderer blockMeshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        blockMeshRenderer.material = blockMaterial;

        foreach (Transform side in this.transform)
            Destroy(side.gameObject);
    }
}
