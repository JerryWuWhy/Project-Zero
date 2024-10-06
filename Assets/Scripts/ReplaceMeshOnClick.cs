using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System;
using TMPro;

public class ReplaceMeshOnClick : MonoBehaviour
{
    // 要替换的新模型的 Prefab
    public GameObject newPrefab1;
    public GameObject newPrefab2;
    public GameObject newPrefab3;
    public GameObject newPrefab4;
    public GameObject newPrefab5;
    private int price = 0;

    // 引用自定义的 Panel 类
    public RectTransform uiRoot;
    public PanelController panel; // 确保这个 Panel 在 Inspector 中已经设置
    public StructureManager st;

    public GameObject upgradeButton; // 预制的按钮
    public GameObject priceLabel;

    public TMP_Text pricetext;
    // public GameObject buttonPrefab; // 预制的按钮
    // private GameObject spawnedButton; // 生成的按钮
    private GameObject _clickedObject;
    

    private void Update()
    {
        // 检测鼠标点击
        if (Input.touchCount > 0) // 0 表示左键
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 如果点击到了某个物体
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Vector3 clickPosition = hit.point;


                Vector3 screenPosition = Camera.main.WorldToScreenPoint(clickPosition);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRoot,
                    screenPosition, null, out var uiPos
                );

                // 如果点击到带有 "building" 标签的物体
                if (clickedObject.CompareTag("building"))
                {
                    upgradeButton.GetComponent<RectTransform>().position = uiPos;
                    pricetext.text = price + " K";
                    upgradeButton.SetActive(true);
                    priceLabel.SetActive(true);
                    _clickedObject = clickedObject;
                    
                }
                else
                {
                    upgradeButton.SetActive(false);
                    priceLabel.SetActive(false);
                }
            }
            else
            {
                upgradeButton.SetActive(false);
                priceLabel.SetActive(false);
            }
        }
    }

    public void OnUpgradeClick()
    {
        // 判断 selectedSkin 的值并替换相应的模型
 
        if (panel.selectedSkin == 0)
        {
            Instantiate(newPrefab1, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            price = 1;
        }
        else if (panel.selectedSkin == 1)
        {
            Instantiate(newPrefab2, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            price = 2;
        }
        else if (panel.selectedSkin == 2)
        {
            Instantiate(newPrefab3, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            price = 3;
        }
        else if (panel.selectedSkin == 3)
        {
            Instantiate(newPrefab4, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            price = 4;
        }
        else if (panel.selectedSkin == 4)
        {
            Instantiate(newPrefab5, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            price = 5;
        }
        else
        {
            price = 0;
        }

        upgradeButton.SetActive(false);
    }
}