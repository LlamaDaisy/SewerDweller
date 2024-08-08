using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    /// <summary>
    /// scriptable object holding loot drop info
    /// </summary>
    /// <param name="lootName">Name of object</param>
    /// <param name="dropChance">drop &</param>
    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
