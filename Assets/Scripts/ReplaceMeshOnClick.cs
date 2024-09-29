using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System;

public class ReplaceMeshOnClick : MonoBehaviour
{
    // 要替换的新模型的 Prefab
    public GameObject newPrefab1;
    public GameObject newPrefab2;
    public GameObject newPrefab3;
    public GameObject newPrefab4;
    public GameObject newPrefab5;

    // 引用自定义的 Panel 类
    public RectTransform uiRoot;
    public PanelController panel; // 确保这个 Panel 在 Inspector 中已经设置
    public StructureManager st;

    public GameObject upgradeButton; // 预制的按钮

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
                    // // 如果还没有按钮，生成一个按钮
                    // if (spawnedButton == null)
                    // {
                    //     // 生成按钮并设置父物体
                    //     spawnedButton = Instantiate(buttonPrefab, clickPosition, Quaternion.identity, uiRoot);
                    //     spawnedButton.GetComponent<RectTransform>().position = uiPos;
                    // }
                    // else
                    // {
                    //     // 如果按钮已经生成了，移动它到新的点击位置
                    //     spawnedButton.GetComponent<RectTransform>().position = uiPos;
                    // }

                    upgradeButton.GetComponent<RectTransform>().position = uiPos;
                    upgradeButton.SetActive(true);
                    _clickedObject = clickedObject;
                }
                else
                {
                    upgradeButton.SetActive(false);
                }
            }
            else
            {
                upgradeButton.SetActive(false);
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
        }
        else if (panel.selectedSkin == 1)
        {
            Instantiate(newPrefab2, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
        }
        else if (panel.selectedSkin == 2)
        {
            Instantiate(newPrefab3, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
        }
        else if (panel.selectedSkin == 3)
        {
            Instantiate(newPrefab4, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
        }
        else if (panel.selectedSkin == 4)
        {
            Instantiate(newPrefab5, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
        }

        upgradeButton.SetActive(false);
    }
}