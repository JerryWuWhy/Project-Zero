using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoalMarketTrend : MaskableGraphic
{
    public float currentCoalPrice = 0.2f; // Initial price, modifiable at runtime
    private List<Vector3> lineInfo; // Stores line points for trend display
    private Vector3 coalPriceVector; // Stores current price vector for plotting
    private float elapsedTime; // Local timer for the component
    public float scrollThreshold = 10000f; // Threshold to start scrolling
    public float moveSpeed = 1f;

    private float _offset;

    protected override void Start()
    {
        base.Start();
        lineInfo = new List<Vector3>();
        coalPriceVector = new Vector3(0f, 0f, 0f);
        elapsedTime = 0f;
        _offset = 0f;
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        // Update the local timer only when the component is active
        elapsedTime += Time.deltaTime;

        // Generate a fluctuating price over time
        currentCoalPrice = Mathf.Cos(elapsedTime) * 0.5f;
        // Track the trend of coal prices over time
        coalPriceVector = new Vector3(elapsedTime * 5000f, currentCoalPrice * 50000f, 0f); // Adjust scaling as needed
        lineInfo.Add(coalPriceVector);

        // Check if the width of the line exceeds the scroll threshold
        if (coalPriceVector.x > scrollThreshold)
        {
            // Shift all points to the left by scrollSpeed to create scrolling effect
            _offset += moveSpeed * Time.deltaTime;
        }

        // Request UI update to redraw mesh based on new data
        SetVerticesDirty();
    }

    // Called when the mesh needs to be rebuilt (e.g., due to data change)
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        vh.Clear();

        if (lineInfo.Count < 2)
            return;

        // Generate lines connecting points in lineInfo
        for (int i = 0; i < lineInfo.Count - 1; i++)
        {
            Vector3 start = lineInfo[i];
            Vector3 end = lineInfo[i + 1];

            // Transform line points to fit canvas
            Vector2 startPos = new Vector2(start.x * 10f / Screen.width - _offset, start.y * 2f / Screen.height + 0.5f);
            Vector2 endPos = new Vector2(end.x * 10f / Screen.width - _offset, end.y * 2f / Screen.height + 0.5f);

            // Set up the vertices for each line segment
            AddLineSegment(vh, startPos, endPos, color);
        }
    }

    // Helper function to add a line segment as quads to the mesh
    private void AddLineSegment(VertexHelper vh, Vector2 start, Vector2 end, Color color)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        float lineWidth = 2f; // Line width in pixels

        Vector2 perpendicular = (end - start).normalized;
        perpendicular = new Vector2(-perpendicular.y, perpendicular.x) * lineWidth * 0.5f;

        // Define vertices for line thickness (quad shape)
        UIVertex v1 = UIVertex.simpleVert;
        v1.color = color;
        v1.position = start - perpendicular;

        UIVertex v2 = UIVertex.simpleVert;
        v2.color = color;
        v2.position = start + perpendicular;

        UIVertex v3 = UIVertex.simpleVert;
        v3.color = color;
        v3.position = end + perpendicular;

        UIVertex v4 = UIVertex.simpleVert;
        v4.color = color;
        v4.position = end - perpendicular;

        // Add vertices for the quad representing a line segment
        vh.AddUIVertexQuad(new[] { v1, v2, v3, v4 });
    }
}