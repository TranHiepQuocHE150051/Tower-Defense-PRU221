using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;
    public LayerMask towerPlacementLayer;
    public LayerMask towerLayer;
    private bool isMultiTouch = false;
    private bool isDragging = false;
    Vector3 touchStart;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            isMultiTouch = false;
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging && !isMultiTouch)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, towerPlacementLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.tag.Equals("TowerPlacement"))
                    {
                        Debug.Log("position: "+hit.collider.transform.position.x+" "+ hit.collider.transform.position.y);
                        UIController.instance.OpenBtnBuyTower(hit.collider.transform, hit.collider.transform.parent.GetSiblingIndex());
                    }
                }
                else
                {
                    UIController.instance.CloseBtnBuyTower();

                    hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, towerLayer);
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag.Equals("Tower"))
                        {
                            UIController.instance.OpenBtnUpgradeAndSellTower(hit.collider.transform, hit.collider.gameObject.transform.parent.gameObject);
                        }
                    }
                    else
                    {
                        UIController.instance.CloseBtnUpgradeAndSellTower();
                    }
                }


            }
        }
    }
}
