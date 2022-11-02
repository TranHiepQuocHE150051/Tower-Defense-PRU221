using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour,ITower
{    
    public void spawnTower(GameObject prefab, int level, int index)
    {
        StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.archerTower, index));
        Transform towerLevel = prefab.transform.GetChild(0);

        for (int i = 0; i < towerLevel.childCount - 1; i++)
        {
            if (towerLevel.GetChild(i).gameObject.activeSelf)
            {
                if (towerLevel.GetChild(i + 1) != null)
                {
                                  
                    towerLevel.GetChild(i).gameObject.SetActive(false);
                    towerLevel.GetChild(i + 1).gameObject.SetActive(true);
                    if (prefab.GetComponent<ArcherTowerController>() != null)
                    {
                        prefab.GetComponent<ArcherTowerController>().Level++;
                    }
                    else if (prefab.GetComponent<CanonTowerController>() != null)
                    {
                        prefab.GetComponent<CanonTowerController>().Level++;
                    }
                    else if (prefab.GetComponent<MagicTowerController>() != null)
                    {
                        prefab.GetComponent<MagicTowerController>().Level++;
                    }
                    else if (prefab.GetComponent<LightningTowerController>() != null)
                    {
                        prefab.GetComponent<LightningTowerController>().Level++;
                    }
                    break;
                }
            }
        }
    }
}
