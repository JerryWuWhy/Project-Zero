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
    private int price = 0;
    
    public RectTransform uiRoot;
    public PanelController panel; 
    public StructureManager st;

    public GameObject upgradeButton; 
    public GameObject priceLabel;

    public TMP_Text pricetext;

    private GameObject _clickedObject;
    private Vector3Int _clickedPos;
    
    private void Update()
    {
        // 检测鼠标点击
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 如果点击到了某个物体
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Vector3 clickPosition = hit.point;
                _clickedPos = Vector3Int.RoundToInt(hit.point);
                
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
        var houseData = DataManager.Inst.GetHouseData(_clickedPos);
        var houseConfig = ConfigManager.Inst.GetHouseConfig(houseData.houseId);
        var upgradedConfig = ConfigManager.Inst.GetHouseConfig(houseConfig.series, houseConfig.level + 1);

        if (upgradedConfig != null)
        {
            Instantiate(upgradedConfig.prefab, _clickedObject.transform.position, _clickedObject.transform.rotation);
            Destroy(_clickedObject);
            DataManager.Inst.SetHouseData(_clickedPos, upgradedConfig.id);

            upgradeButton.SetActive(false);
        }
    }
}