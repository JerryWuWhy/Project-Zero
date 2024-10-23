using System;
using UnityEngine;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
using TMPro;           // Required if using TextMeshPro
using System.Collections.Generic;
public class Gashpon : MonoBehaviour
{
    private GameObject _clickedObject;
    private Vector3Int _clickedPos;
    public RectTransform uiRoot; // If not used, you can remove this line
    public GameObject panel;
    public TextMeshProUGUI resultText;      // Make sure this is assigned in the Inspector
    public Button playButton;    // Ensure the button is assigned in the Inspector
    public List<GameObject> objectsToSpawn;  // 要生成的对象
    public Camera mainCamera;         // 主相机
    public float spawnDistance = 10f; // 生成的距离
    public float fixedY = 0.0f;
    private string[] prizes = { "1", "2", "3", "4", "5" };

    private void Start()
    {
        // Ensure the button is not null before adding a listener
        if (playButton != null)
        {
            playButton.onClick.AddListener(Next);
        }
        // Ensure the text field is not null before assigning text
        if (resultText != null)
        {
            resultText.text = "Press Play to try your luck!";
        }
        else
        {
            Debug.LogError("ResultText is not assigned!");
        }
    }

    public void Next()
    {
        int randomIndex = UnityEngine.Random.Range(0, prizes.Length);
        string prize = prizes[randomIndex];

        // Display the prize in the text field, ensuring resultText is not null
        if (resultText != null)
        {
            resultText.text = $"You got: {prize}!";
            SpawnObject();
        }
    }
    void SpawnObject()
    {
        int randomIndex = UnityEngine.Random.Range(0, objectsToSpawn.Count);

        // In the viewport, choose a random point for x and z
        float randomX = UnityEngine.Random.Range(0.1f, 0.9f);
        float randomY = UnityEngine.Random.Range(0.1f, 0.9f);
        Vector3 randomViewportPoint = new Vector3(randomX, randomY, spawnDistance);

        // Convert viewport coordinates to world coordinates
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(randomViewportPoint);

        // Set the y-axis to the fixed height
        worldPosition.y = fixedY;

        // Instantiate the object from the list based on the random index
        GameObject objectToSpawn = objectsToSpawn[randomIndex];
        Instantiate(objectToSpawn, worldPosition, Quaternion.identity);
    }
    void Update()
    {
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
                if (clickedObject.CompareTag("Gashpon"))
                {
                    panel.GetComponent<RectTransform>().position = uiPos;
                    panel.SetActive(true);
                    _clickedObject = clickedObject;
                }

            }
        }
    }
}