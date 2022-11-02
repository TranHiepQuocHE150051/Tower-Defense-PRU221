using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    public Text txtWave;
    public Text txtHealth;
    public Text txtWood;

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
        CloseBtnUpgradeAndSellTower();
        CheckBtnBuy();

        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
        btnBuyTower.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, btnBuyTower.transform.position.z);
        btnBuyTower.transform.localScale = new Vector3(0, 0, 0);
        btnBuyTower.SetActive(true);
        btnBuyTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        towerPlacementIndex = placementIndex;
    }

    void CheckBtnBuy()
    {
        btnBuyArcherTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyArcherTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyCanonTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyCanonTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyMagicTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyMagicTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyLightningTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyLightningTower.GetComponent<AnimatedButton>().interactable = true;

        if (TowerManager.instance.archerPrice > PlayerSetting.instance.Coin)
        {
            btnBuyArcherTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyArcherTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.canonPrice > PlayerSetting.instance.Coin)
        {
            btnBuyCanonTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyCanonTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.magicPrice > PlayerSetting.instance.Coin)
        {
            btnBuyMagicTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyMagicTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.lightningPrice > PlayerSetting.instance.Coin)
        {
            btnBuyLightningTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyLightningTower.GetComponent<AnimatedButton>().interactable = false;
        }
    }

    public void CloseBtnBuyTower()
    {
        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
    }

    public void ButtonBuyTower(int index)
    {
        AudioController.instance.PlaySound("buyTower");
        CloseBtnBuyTower();
        switch (index)
        {
            case 1:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.barackTower, towerPlacementIndex));
                break;
            case 2:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.archerTower, towerPlacementIndex));
                break;
            case 3:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.canonTower, towerPlacementIndex));
                break;
            case 4:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.magicTower, towerPlacementIndex));
                break;
        }
    }

    float priceToUpgrade;
    float priceToSell;

    public void OpenBtnUpgradeAndSellTower(Transform targetPosition, GameObject tower)
    {
        CloseBtnBuyTower();
        CloseAttackRange();

        SetPrice(tower);
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
        btnUpgradeAndSellTower.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, btnUpgradeAndSellTower.transform.position.z);
        btnUpgradeAndSellTower.transform.localScale = new Vector3(0, 0, 0);
        btnUpgradeAndSellTower.SetActive(true);
        btnUpgradeAndSellTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        currentTower = tower;

        towerPlacementIndex = tower.GetComponent<TowerController>().towerPlacementIndex;

        OpenAttackRange();
    }

    void SetPrice(GameObject tower)
    {
        if (tower.GetComponent<ArcherTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<ArcherTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<ArcherTowerController>().price;
        }
        else if (tower.GetComponent<CanonTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<CanonTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<CanonTowerController>().price;
        }
        else if (tower.GetComponent<MagicTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<MagicTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<MagicTowerController>().price;
        }
        else if (tower.GetComponent<LightningTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<LightningTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<LightningTowerController>().price;
        }
        priceToSell *= 0.8f;
        btnUpgradeTower.transform.GetChild(2).GetComponent<Text>().text = priceToUpgrade.ToString();
        btnSellTower.transform.GetChild(2).GetComponent<Text>().text = priceToSell.ToString();

        if (PlayerSetting.instance.Coin < priceToUpgrade)
        {
            btnUpgradeTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnUpgradeTower.GetComponent<AnimatedButton>().interactable = false;
        }
        else
        {
            btnUpgradeTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            btnUpgradeTower.GetComponent<AnimatedButton>().interactable = true;
        }

        Transform towerLevel = tower.transform.GetChild(0);

        for (int i = 0; i < towerLevel.childCount; i++)
        {
            if (towerLevel.GetChild(i).gameObject.activeSelf)
            {
                if (i == towerLevel.childCount - 1)
                {
                    btnUpgradeTower.SetActive(false);
                }
                else
                {
                    btnUpgradeTower.SetActive(true);
                }
                break;
            }
        }
    }

    public void CloseBtnUpgradeAndSellTower()
    {
        CloseAttackRange();
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
    }

    public void ButtonUpgradeTower()
    {
        AudioController.instance.PlaySound("upgradeTower");
        PlayerSetting.instance.Coin -= priceToUpgrade;
        CloseBtnUpgradeAndSellTower();
        StartCoroutine(TowerManager.instance.UpgradeTower(currentTower));
    }

    public void ButtonSellTower()
    {
        AudioController.instance.PlaySound("sellTower");
        PlayerSetting.instance.Coin += priceToSell;
        CloseBtnUpgradeAndSellTower();
        Destroy(currentTower);
        TowerManager.instance.towerPlacementParent.GetChild(towerPlacementIndex).gameObject.SetActive(true);
    }

    void OpenAttackRange()
    {
        if (currentTower.GetComponent<ArcherTowerController>() != null)
        {
            currentTower.GetComponent<ArcherTowerController>().attackRange.GetChild(0).gameObject.SetActive(true);
        }
        else if (currentTower.GetComponent<CanonTowerController>() != null)
        {
            currentTower.GetComponent<CanonTowerController>().attackRange.GetChild(0).gameObject.SetActive(true);
        }
        else if (currentTower.GetComponent<MagicTowerController>() != null)
        {
            currentTower.GetComponent<MagicTowerController>().attackRange.GetChild(0).gameObject.SetActive(true);
        }
        else if (currentTower.GetComponent<LightningTowerController>() != null)
        {
            currentTower.GetComponent<LightningTowerController>().attackRange.GetChild(0).gameObject.SetActive(true);
        }
    }

    void CloseAttackRange()
    {
        if (currentTower != null)
        {
            if (currentTower.GetComponent<ArcherTowerController>() != null)
            {
                currentTower.GetComponent<ArcherTowerController>().attackRange.GetChild(0).gameObject.SetActive(false);
            }
            else if (currentTower.GetComponent<CanonTowerController>() != null)
            {
                currentTower.GetComponent<CanonTowerController>().attackRange.GetChild(0).gameObject.SetActive(false);
            }
            else if (currentTower.GetComponent<MagicTowerController>() != null)
            {
                currentTower.GetComponent<MagicTowerController>().attackRange.GetChild(0).gameObject.SetActive(false);
            }
            else if (currentTower.GetComponent<LightningTowerController>() != null)
            {
                currentTower.GetComponent<LightningTowerController>().attackRange.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    [Header("Panel Lose")]
    public GameObject pnlLose;
    public Transform background;
    public Transform btnHome;
    public Transform btnReplay;
    public Text txtHighScore;

    public void OpenPanelLose()
    {
        StartCoroutine(DelayOpenPanelLose());
    }

    IEnumerator DelayOpenPanelLose()
    {
        txtHighScore.text = "Score: " + MonsterSpawnController.instance.WaveSpawn + "\nHigh score: " + PlayerSetting.instance.HighScore;
        btnHome.localScale = Vector3.zero;
        btnReplay.localScale = Vector3.zero;
        background.localScale = Vector3.zero;
        pnlLose.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        pnlLose.SetActive(true);

        pnlLose.GetComponent<Image>().DOFade(0.5f, 0.5f);
        background.DOScale(1, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(1f);

        btnReplay.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        btnHome.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    [Header("Panel Home")]
    public GameObject pnlHome;
    public GameObject pnlIngame;
    //public GameObject pnlLoading;
    public GameObject gameplayPrefab;
    public Transform gameplayParent;

    public void ButtonPlay()
    {
        StartCoroutine(DelayPlay());
    }

    IEnumerator DelayPlay()
    {
        //pnlLoading.SetActive(true);
        //float defaultX1 = pnlLoading.transform.GetChild(0).localPosition.x;
        //float defaultX2 = pnlLoading.transform.GetChild(1).localPosition.x;
        //pnlLoading.transform.GetChild(0).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        AudioController.instance.PlaySound("ingame");
        GameObject gameplay = Instantiate(gameplayPrefab, gameplayParent);
        gameplay.transform.position = Vector3.zero;
        pnlHome.SetActive(false);
        pnlIngame.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //pnlLoading.transform.GetChild(0).DOLocalMoveX(defaultX1, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(defaultX2, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        //pnlLoading.SetActive(false);
    }

    public void ButtonHome()
    {
        StartCoroutine(DelayHome());
    }

    IEnumerator DelayHome()
    {
        Time.timeScale = 1;
        txtGameSpeed.text = "x1";

        //pnlLoading.SetActive(true);
        //float defaultX1 = pnlLoading.transform.GetChild(0).localPosition.x;
        //float defaultX2 = pnlLoading.transform.GetChild(1).localPosition.x;
        //pnlLoading.transform.GetChild(0).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        Destroy(gameplayParent.GetChild(0).gameObject);
        pnlHome.SetActive(true);
        pnlIngame.SetActive(false);
        pnlLose.SetActive(false);
        pnlPause.SetActive(false);
        AudioController.instance.PlaySound("mainMenu");

        yield return new WaitForSeconds(0.5f);

        //pnlLoading.transform.GetChild(0).DOLocalMoveX(defaultX1, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(defaultX2, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        //pnlLoading.SetActive(false);
    }

    public void ButtonRestart()
    {
        StartCoroutine(DelayRestart());
    }

    IEnumerator DelayRestart()
    {
        Time.timeScale = 1;
        txtGameSpeed.text = "x1";

        //pnlLoading.SetActive(true);
        //float defaultX1 = pnlLoading.transform.GetChild(0).localPosition.x;
        //float defaultX2 = pnlLoading.transform.GetChild(1).localPosition.x;
        //pnlLoading.transform.GetChild(0).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(0, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        pnlPause.SetActive(false);
        pnlLose.SetActive(false);

        Destroy(gameplayParent.GetChild(0).gameObject);

        yield return new WaitForSeconds(0.5f);

        Instantiate(gameplayPrefab, gameplayParent);

        yield return new WaitForSeconds(0.5f);

        //pnlLoading.transform.GetChild(0).DOLocalMoveX(defaultX1, 0.5f).SetEase(Ease.Linear);
        //pnlLoading.transform.GetChild(1).DOLocalMoveX(defaultX2, 0.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        //pnlLoading.SetActive(false);
    }

    [Header("Panel Setting")]
    public GameObject pnlSetting;
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibrateOn;
    public GameObject vibrateOff;

    public void ButtonOpenSetting()
    {
        pnlSetting.transform.GetChild(0).localScale = Vector3.zero;
        pnlSetting.SetActive(true);
        pnlSetting.transform.GetChild(0).DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    public void ButtonCloseSetting()
    {
        pnlSetting.transform.GetChild(0).DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            pnlSetting.SetActive(false);
        });
    }

    public void ButtonSound()
    {
        if (AudioController.instance.Sound == 1)
        {
            AudioController.instance.Sound = 0;
        }
        else
        {
            AudioController.instance.Sound = 1;
        }
    }

    public void ButtonVibrate()
    {
        if (AudioController.instance.Vibrate == 1)
        {
            AudioController.instance.Vibrate = 0;
        }
        else
        {
            AudioController.instance.Vibrate = 1;
        }
    }

    [Header("Panel Pause")]
    public GameObject pnlPause;

    public void OpenPanelPause()
    {
        Time.timeScale = 0;
        pnlPause.transform.GetChild(0).localScale = Vector3.zero;
        pnlPause.SetActive(true);
        pnlPause.transform.GetChild(0).DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void ButtonContinue()
    {
        pnlPause.transform.GetChild(0).DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            pnlPause.SetActive(false);
            Time.timeScale = 1;
            txtGameSpeed.text = "x1";
        });
    }

    [Header("Panel Ingame")]
    public Text txtGameSpeed;

    public void ButtonChangeGameSpeed()
    {
        switch (Time.timeScale)
        {
            case 1:
                Time.timeScale = 2;
                txtGameSpeed.text = "x2";
                break;
            case 2:
                Time.timeScale = 3;
                txtGameSpeed.text = "x3";
                break;
            case 3:
                Time.timeScale = 20;
                txtGameSpeed.text = "x20";
                break;
            case 20:
                Time.timeScale = 1;
                txtGameSpeed.text = "x1";
                break;
            default:
                Time.timeScale = 1;
                txtGameSpeed.text = "x1";
                break;
        }
    }
}
