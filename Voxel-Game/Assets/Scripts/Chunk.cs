using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Texture2D[] atlasTextures;
    public Block[,,] chunkBlocks;

    Dictionary<string, Rect> atlasDictionary = new Dictionary<string, Rect>();

    Material blockMaterial;
    void Start()
    {
        Texture2D atlas = GetTextureAtlas();
        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = atlas;
        blockMaterial = material;
        StartCoroutine(GenerateChunk(16));
    }

    //Generating texture atlas
    Texture2D GetTextureAtlas()
    {
        Texture2D textureAtlas = new Texture2D(8192, 8192);
        Rect[] rectCoordinates = textureAtlas.PackTextures(atlasTextures, 0, 8192, false);
        textureAtlas.Apply();

        for(int i = 0; i < rectCoordinates.Length; i++)
        {
            atlasDictionary.Add(atlasTextures[i].name.ToLower(), rectCoordinates[i]);
        }
        return textureAtlas;
    }
    IEnumerator GenerateChunk(int chunkSize)
    {
        chunkBlocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                    chunkBlocks[x, y, z] = new Block((Block.BlockType)Random.Range(0,4), this.gameObject, new Vector3(x,y,z), atlasDictionary);

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
