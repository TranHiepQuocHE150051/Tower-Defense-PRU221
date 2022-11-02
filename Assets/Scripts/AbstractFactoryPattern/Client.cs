using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public int level;
    public int type;
    public int towerPlacementIndex;
    public GameObject archer;
    public GameObject cannon;
    public GameObject magic;
    public GameObject lighting;
    public void ClientMethod(ITowerFactory factory)
    {
        ITower tower = factory.CreateTower(level, type, towerPlacementIndex);
        switch (type)
        {
            case 1:
                tower.spawnTower(archer, level, towerPlacementIndex);
                break;
            case 2:
                tower.spawnTower(cannon, level, towerPlacementIndex);
                break;
            case 3:
                tower.spawnTower(magic, level, towerPlacementIndex);
                break;
            case 4:
                tower.spawnTower(lighting, level, towerPlacementIndex);
                break;
        }

    }
       

        


}
    

