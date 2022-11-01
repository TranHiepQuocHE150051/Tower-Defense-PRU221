using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public Transform towerParent;
    public Transform towerPlacementParent;

    public GameObject barackTower;
    public GameObject archerTower;
    public GameObject canonTower;
    public GameObject magicTower;

    public GameObject effectSpawnTower;

    public float canonPrice;
    public float archerPrice;
    public float magicPrice;
    public float lightningPrice;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadPriceTower();
    }

    public JSONNode loadTextData(string path)
    {
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));

        return JSONArray.Parse(txt.text);
    }

    public void LoadPriceTower()
    {
        JSONNode jsonNode = loadTextData("Data/CanonTower");
        foreach (JSONNode node in jsonNode)
        {
            if (node["Level"].AsInt == 1)
            {
                canonPrice = node["Price"].AsFloat;
                UIController.instance.btnBuyCanonTower.transform.GetChild(2).GetComponent<Text>().text = canonPrice.ToString();
                break;
            }
        }
        
        jsonNode = loadTextData("Data/MagicTower");
        foreach (JSONNode node in jsonNode)
        {
            if (node["Level"].AsInt == 1)
            {
                magicPrice = node["Price"].AsFloat;
                UIController.instance.btnBuyMagicTower.transform.GetChild(2).GetComponent<Text>().text = magicPrice.ToString();
                break;
            }
        }
        
        jsonNode = loadTextData("Data/ArcherTower");
        foreach (JSONNode node in jsonNode)
        {
            if (node["Level"].AsInt == 1)
            {
                archerPrice = node["Price"].AsFloat;
                UIController.instance.btnBuyArcherTower.transform.GetChild(2).GetComponent<Text>().text = archerPrice.ToString();
                break;
            }
        }
        
        jsonNode = loadTextData("Data/LightningTower");
        foreach (JSONNode node in jsonNode)
        {
            if (node["Level"].AsInt == 1)
            {
                lightningPrice = node["Price"].AsFloat;
                UIController.instance.btnBuyLightningTower.transform.GetChild(2).GetComponent<Text>().text = lightningPrice.ToString();
                break;
            }
        }
    }

    public IEnumerator SpawnTower(GameObject tower, int towerPlacementIndex)
    {
        Transform towerPlacement = towerPlacementParent.GetChild(towerPlacementIndex);

        GameObject effectSpawn = Instantiate(effectSpawnTower);
        effectSpawn.transform.position = towerPlacement.position;

        yield return new WaitForSeconds(0.4f);

        GameObject newTower = Instantiate(tower, towerParent);
        towerPlacement.gameObject.SetActive(false);
        newTower.transform.position = towerPlacement.position;
        newTower.GetComponent<TowerController>().towerPlacementIndex = towerPlacementIndex;

        if (tower.GetComponent<ArcherTowerController>() != null)
        {
            if (archerPrice <= PlayerSetting.instance.Coin)
            {
                PlayerSetting.instance.Coin -= archerPrice;
            }
        }
        else if (tower.GetComponent<CanonTowerController>() != null)
        {
            if (canonPrice <= PlayerSetting.instance.Coin)
            {
                PlayerSetting.instance.Coin -= canonPrice;
            }
        }
        else if (tower.GetComponent<MagicTowerController>() != null)
        {
            if (magicPrice <= PlayerSetting.instance.Coin)
            {
                PlayerSetting.instance.Coin -= magicPrice;
            }
        }
        else if (tower.GetComponent<LightningTowerController>() != null)
        {
            if (lightningPrice <= PlayerSetting.instance.Coin)
            {
                PlayerSetting.instance.Coin -= lightningPrice;
            }
        }
    }

    public IEnumerator UpgradeTower(GameObject tower)
    {
        Transform towerLevel = tower.transform.GetChild(0);
        for (int i = 0; i < towerLevel.childCount - 1; i++)
        {
            if (towerLevel.GetChild(i).gameObject.activeSelf)
            {
                if (towerLevel.GetChild(i + 1) != null)
                {
                    GameObject effectSpawn = Instantiate(effectSpawnTower);
                    effectSpawn.transform.position = tower.transform.position;

                    yield return new WaitForSeconds(0.4f);

                    towerLevel.GetChild(i).gameObject.SetActive(false);
                    towerLevel.GetChild(i + 1).gameObject.SetActive(true);
                    if (tower.GetComponent<ArcherTowerController>() != null)
                    {
                        tower.GetComponent<ArcherTowerController>().Level++;
                    }
                    else if (tower.GetComponent<CanonTowerController>() != null)
                    {
                        tower.GetComponent<CanonTowerController>().Level++;
                    }
                    else if (tower.GetComponent<MagicTowerController>() != null)
                    {
                        tower.GetComponent<MagicTowerController>().Level++;
                    }
                    else if (tower.GetComponent<LightningTowerController>() != null)
                    {
                        tower.GetComponent<LightningTowerController>().Level++;
                    }
                    break;
                }
            }
        }
    }
}
