using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject btnBuyTower;
    [SerializeField]
    int towerPlacementIndex;
    //GameObject currentTower;

    //public Text txtWave;
    //public Text txtHealth;
    //public Text txtWood;

    //public GameObject btnBuyCanonTower;
    //public GameObject btnBuyArcherTower;
    //public GameObject btnBuyMagicTower;
    //public GameObject btnBuyLightningTower;
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
}
