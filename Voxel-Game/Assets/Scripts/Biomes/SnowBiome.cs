using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBiome : Biome
{
    public override float firstLayerIncrement { get { return 0.04f; } }
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockType.Type.SNOW];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockType.Type.SNOW];
    }
}
