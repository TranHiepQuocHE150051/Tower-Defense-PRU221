using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public Transform towerParent;
    public Transform towerPlacementParent;

    public GameObject barackTower;
    public GameObject archerTower;
    public GameObject canonTower;
    public GameObject magicTower;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    public IEnumerator SpawnTower(GameObject tower, int towerPlacementIndex)
    {
        Transform towerPlacement = towerPlacementParent.GetChild(towerPlacementIndex);
       
        yield return new WaitForSeconds(0.4f);

        GameObject newTower = Instantiate(tower, towerParent);
        Debug.Log("Spawn Tower");
        //towerPlacement.gameObject.SetActive(false);
        newTower.transform.position = towerPlacement.position;
        newTower.GetComponent<TowerController>().towerPlacementIndex = towerPlacementIndex;      
    }
}
