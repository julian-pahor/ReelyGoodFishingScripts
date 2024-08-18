using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish", order = 1)]
public class Fish : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    [Header("Fishs name + informative description")]
    public string fishName;
    public string description;
    public int pointScore;
    public Sprite fishPortrait;

    [Header("How large the catch bar will be.")]
    public float catchWidth;
    [Header("How many seconds to catch")]
    public float fillSpeed;

    [Header("Impacts spawn chance")]
    public Rarity rarity; // will fully replace spawnChance eventually (soon, very soon)

    [Header("Difficulty Settings for Catch Bar - 1 Easy - 10 Hard")]
    public float cbarDistance;
    public float cbarSpeed;
    public float cbarTime;
}
