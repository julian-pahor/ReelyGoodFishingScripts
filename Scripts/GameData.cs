using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int HighScore;

    public GameData(GameManager gm)
    {
        HighScore = gm.highScore;
    }

}
