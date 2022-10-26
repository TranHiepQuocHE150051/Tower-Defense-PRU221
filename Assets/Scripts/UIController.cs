using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject btnBuyTower;
    public GameObject btnUpgradeAndSellTower;
    public GameObject btnUpgradeTower;
    public GameObject btnSellTower;
    [SerializeField]
    int towerPlacementIndex;
    GameObject currentTower;

    //public Text txtWave;
    //public Text txtHealth;
    //public Text txtWood;

    public GameObject btnBuyCanonTower;
    public GameObject btnBuyArcherTower;
    public GameObject btnBuyMagicTower;
    public GameObject btnBuyLightningTower;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void OpenBtnBuyTower(Transform targetPosition, int placementIndex)
    {
        //CloseBtnUpgradeAndSellTower();
        //CheckBtnBuy();

        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
        btnBuyTower.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, btnBuyTower.transform.position.z);
        btnBuyTower.transform.localScale = new Vector3(0, 0, 0);
        btnBuyTower.SetActive(true);
        btnBuyTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        towerPlacementIndex = placementIndex;
    }
    public void CloseBtnBuyTower()
    {
        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
    }
    public void ButtonBuyTower(int index)
    {      
        CloseBtnBuyTower();
        Debug.Log(index);
        switch (index)
        {
            case 1:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.archerTower, towerPlacementIndex));
                break;
            case 2:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.canonTower, towerPlacementIndex));
                break;
            case 3:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.barackTower, towerPlacementIndex));
                break;
            case 4:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.magicTower, towerPlacementIndex));
                break;
        }
    }
    public void OpenBtnUpgradeAndSellTower(Transform targetPosition, GameObject tower)
    {
        CloseBtnBuyTower();
        //CloseAttackRange();

        //SetPrice(tower);
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
        btnUpgradeAndSellTower.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, btnUpgradeAndSellTower.transform.position.z);
        btnUpgradeAndSellTower.transform.localScale = new Vector3(0, 0, 0);
        btnUpgradeAndSellTower.SetActive(true);
        btnUpgradeAndSellTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        currentTower = tower;

        towerPlacementIndex = tower.GetComponent<TowerController>().towerPlacementIndex;

        //OpenAttackRange();
    }
    public void CloseBtnUpgradeAndSellTower()
    {
        //CloseAttackRange();
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
    }
}
