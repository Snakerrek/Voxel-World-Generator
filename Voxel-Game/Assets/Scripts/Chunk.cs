using UnityEngine;

public class Chunk
{
    public Block[,,] chunkBlocks;
    public GameObject chunkObject;
    Material blockMaterial;

    public Chunk(string name, Vector3 position, Material material)
    {
        this.chunkObject = new GameObject(name);
        this.chunkObject.transform.position = position;
        this.blockMaterial = material;
        GenerateChunk(16);
    }

    void GenerateChunk(int chunkSize)
    {
        chunkBlocks = new Block[chunkSize, chunkSize, chunkSize];

        for (int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
                for (int x = 0; x < chunkSize; x++)
                    chunkBlocks[x, y, z] = new Block((Block.BlockType)Random.Range(0, 4), this, new Vector3(x, y, z), World.atlasDictionary);
    }

    public void DrawChunk(int chunkSize)
    {
        for(int z = 0; z < chunkSize; z++)
            for (int y = 0; y < chunkSize; y++)
            for (int x = 0; x < chunkSize; x++)
                chunkBlocks[x, y, z].CreateBlock();
        CombineSides();
    }

    // Combining sides into one object
    void CombineSides()
    {
        MeshFilter[] meshFilters = chunkObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineSides = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineSides[i].mesh = meshFilters[i].sharedMesh;
            combineSides[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        MeshFilter blockMeshFilter = chunkObject.AddComponent<MeshFilter>();
        blockMeshFilter.mesh = new Mesh();
        blockMeshFilter.mesh.CombineMeshes(combineSides);

        MeshRenderer blockMeshRenderer = chunkObject.AddComponent<MeshRenderer>();
        blockMeshRenderer.material = blockMaterial;

        foreach (Transform side in chunkObject.transform)
           GameObject.Destroy(side.gameObject);
    }
}
