using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "ScriptableObjects/Location", order = 1)]
public class Locations : ScriptableObject
{
    public List<Fish> fishList = new List<Fish>();

    //SpawnChance Calculator using enum rarities from fishList

    private int[] spawnChances;

    private List<Fish> commonFish = new List<Fish>();
    private List<Fish> uncommonFish = new List<Fish>();
    private List<Fish> rareFish = new List<Fish>();
    private List<Fish> legendaryFish = new List<Fish>();

    /// <summary>
    /// Initialise to be called to setup spawn chances + scores
    /// </summary>
    public void Initialise()
    {
        commonFish.Clear();
        uncommonFish.Clear();
        rareFish.Clear();
        legendaryFish.Clear();

        foreach(Fish f in fishList)
        {
            switch(f.rarity)
            {
                case Fish.Rarity.Common:
                    commonFish.Add(f);
                    f.pointScore = 100;
                        break;
                case Fish.Rarity.Uncommon:
                    uncommonFish.Add(f);
                    f.pointScore = 250;
                    break;
                case Fish.Rarity.Rare:
                    rareFish.Add(f);
                    f.pointScore = 500;
                    break;
                case Fish.Rarity.Legendary:
                    legendaryFish.Add(f);
                    f.pointScore = 1000;
                    break;
            }
        }
    }

    public Fish GetFish()
    {
        int chance = Random.Range(0, 101);

        if(chance >= 90)
        {
            return legendaryFish[Random.Range(0, legendaryFish.Count)];
        }
        else if (chance >= 70)
        {
            return rareFish[Random.Range(0, rareFish.Count)];
        }
        else if (chance >= 40)
        {
            return uncommonFish[Random.Range(0, uncommonFish.Count)];
        }
        else
        {
            return commonFish[Random.Range(0, commonFish.Count)];
        }
    }
}
