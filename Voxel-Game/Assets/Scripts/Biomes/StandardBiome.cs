using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBiome : Biome
{
    protected override BlockType Generate2ndLayer()
    {
        if(typeProbability < 0.1f)
        {
            return World.blockTypes[BlockType.Type.DIAMOND];
        }

        if(typeProbability < 0.25f)
        {
            return World.blockTypes[BlockType.Type.COAL];
        }

        return World.blockTypes[BlockType.Type.STONE];
    }
}
