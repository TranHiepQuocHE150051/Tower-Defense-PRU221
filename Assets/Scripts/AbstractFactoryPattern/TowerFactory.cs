using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : ITowerFactory
{

    public ITower CreateTower(int level, int type , int index)
    {
        switch (type)
        {
            case 1:
                return new ArcherTower();
                break;
            case 2:
                return new CannonTower();
                break;
            case 3:
                return new MagicTower();
                break;
            case 4:
                return new LightingTower();
                break;
        }
        return null;
    }
}
