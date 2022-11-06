using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int type;
    public int index;
    public int level;
    public int wave;
    public int health;
    public float coin;

    public SaveData(int type, int index, int level, int wave,int health, float coin)
    {
        this.type = type;
        this.index = index;
        this.level = level;
        this.wave = wave;
        this.health = health;
        this.coin = coin;
    }
}
