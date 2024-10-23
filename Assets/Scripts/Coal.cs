using System;
using UnityEngine;
using UnityEngine.UI;  
using TMPro;

public class Coal : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // TextMeshPro component if you use TextMeshPro
    public TextMeshProUGUI tmpTextsingle;
    public float interval = 1f;     // Time in seconds between increments
    public float increaseAmount = 0.1f; // How much to increase per interval
    public float increaseAmountSingle = 0.1f;
    private float counter = 0f;     // The float value to increase
    private float countersingle = 0f;
    public string unit = " T"; // The unit to display after the number
    private GameObject _clickedObject;
    private float timer = 0f;
    private Vector3Int _clickedPos;
    public RectTransform uiRoot;
    public GameObject panel;
    public void Iron()
    {
        increaseAmountSingle = 0f;
        Debug.Log("1");
    }

    public void Lithium()
    {
        increaseAmountSingle = 0f;
        Debug.Log("1");
    }
    
    void Update()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj1 in allObjects)
        {
            if (obj1.name == "CoalFactory(Clone)")
            {
                count++;
            }
        }
        GameObject obj = GameObject.Find("CoalFactory(Clone)");
        increaseAmount = 0.1f * count;

        if (obj != null)
        {
            // Increment the timer based on time passed since last frame
            timer += Time.deltaTime;

            // Check if enough time has passed based on the interval
            if (timer >= interval)
            {
                // Reset the timer
                timer = 0f;

                // Increment the counter by the desired amount
                counter += increaseAmount;
                countersingle += increaseAmountSingle;

                // Update the UI text with float value formatted to 2 decimal places and add the unit
                if (tmpText != null)
                    tmpText.text = counter.ToString("F2") + unit;
                if (tmpTextsingle != null)
                    tmpTextsingle.text = increaseAmountSingle + unit;
            }
        }

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If a raycast hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Vector3 clickPosition = hit.point;
                _clickedPos = Vector3Int.RoundToInt(hit.point);

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(clickPosition);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRoot, screenPosition, null, out var uiPos);

                // If the clicked object has the "Coal" tag
                if (clickedObject.CompareTag("Coal"))
                {
                    panel.GetComponent<RectTransform>().position = uiPos;
                    panel.SetActive(true);
                    //panel1.blocksRaycasts = false; // Disable raycasts when the panel is active
                    _clickedObject = clickedObject;
                }
                else
                {
                    panel.SetActive(false);
                    //panel1.blocksRaycasts = true; // Enable raycasts when the panel is inactive
                }
            }
            else
            {
                panel.SetActive(false);
                //panel1.blocksRaycasts = true; // Enable raycasts when the panel is inactive
            }
        }
    }
}