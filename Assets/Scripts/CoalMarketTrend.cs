// using System.Collections.Generic;
// using System.Timers;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class CoalMarketTrend : MaskableGraphic
// {
//     public MarketPanel marketpanel;
//     public float currentCoalPrice; // Initial price, modifiable at runtime
//     private List<Vector3> lineInfo; // Stores line points for trend display
//     private Vector3 coalPriceVector; // Stores current price vector for plotting
//     public float scrollThreshold = 10000f; // Threshold to start scrolling
//     public float moveSpeed = 40f;
//     public TimeManager timemanager;
//     private float _offset;
//
//     private float timer = 0f;
//     // 新增 Text 组件
//     public Text xAxisLabel;
//     public Text yAxisLabel;
//
//     protected override void Start()
//     {
//         base.Start();
//         lineInfo = new List<Vector3>();
//         coalPriceVector = new Vector3(0f, 0f, 0f);
//         _offset = 0f;
//         currentCoalPrice = (marketpanel.coalpurchaseprice) / (marketpanel.coalpurchaseamount);
//     }
//
//     void Update()
//     {
//         var targetPrice = (marketpanel.coalpurchaseprice) / (marketpanel.coalpurchaseamount);
//         currentCoalPrice += (targetPrice - currentCoalPrice) * 0.1f;
//
//         // 添加随机小浮动（例如在 -0.1% 到 +0.1% 的范围内）
//         float fluctuationRange = Random.Range(0.005f, 0.06f); // 0.05% 到 0.2% 的范围
//         float randomFluctuation = Random.Range(-fluctuationRange, fluctuationRange); // 在动态波动范围内随机
//
//         currentCoalPrice *= (1 + randomFluctuation);
//
//         if (!Application.isPlaying)
//         {
//             return;
//         }
//
//         
//         // Update text labels with current values
//         if (timemanager.enableRealTimeUpdate)
//         {
//             timer += Time.deltaTime;
//             coalPriceVector = new Vector3(timer * 5000f, currentCoalPrice * 500000f, 0f);
//             lineInfo.Add(coalPriceVector);
//             xAxisLabel.text = $"Time: {timer:F1}s";
//             yAxisLabel.text = $"Price: {currentCoalPrice:F2}";
//         }
//         
//
//         // Check if the width of the line exceeds the scroll threshold
//         if (coalPriceVector.x > scrollThreshold)
//         {
//             if (timemanager.enableRealTimeUpdate)
//             {
//                 // Shift all points to the left by scrollSpeed to create scrolling effect
//                 _offset += moveSpeed * Time.deltaTime;
//             }
//             
//         }
//
//         if (timemanager.enableRealTimeUpdate)
//         {
//             SetVerticesDirty();
//         }
//         // Request UI update to redraw mesh based on new data
//         
//     }
//
//     // Called when the mesh needs to be rebuilt (e.g., due to data change)
//     protected override void OnPopulateMesh(VertexHelper vh)
//     {
//         if (!Application.isPlaying)
//         {
//             return;
//         }
//
//         vh.Clear();
//
//         // Draw X and Y Axes
//         DrawAxes(vh);
//
//         if (lineInfo.Count < 2)
//             return;
//
//         // Generate lines connecting points in lineInfo
//         for (int i = 0; i < lineInfo.Count - 1; i++)
//         {
//             Vector3 start = lineInfo[i];
//             Vector3 end = lineInfo[i + 1];
//
//             // Transform line points to fit canvas
//             Vector2 startPos = new Vector2(start.x * 10f / Screen.width - _offset, start.y * 2f / Screen.height + 0.5f);
//             Vector2 endPos = new Vector2(end.x * 10f / Screen.width - _offset, end.y * 2f / Screen.height + 0.5f);
//
//             // Set up the vertices for each line segment
//             AddLineSegment(vh, startPos, endPos, color);
//         }
//     }
//
//     // Helper function to draw the X and Y Axes
//     // Helper function to draw the X and Y Axes
//     private void DrawAxes(VertexHelper vh)
//     {
//         // 定义 Y 轴的长度
//         float yAxisLength = Screen.height / 2; // 调整为所需的 Y 轴长度
//         float yInterval = 50f; // 每个水平分隔线之间的距离
//
//         // 固定的 Y 轴（保持在屏幕中心位置）
//         Vector2 fixedYAxisStart = new Vector2(0f, -yAxisLength);
//         Vector2 fixedYAxisEnd = new Vector2(0f, yAxisLength);
//         AddLineSegment(vh, fixedYAxisStart, fixedYAxisEnd, Color.green); // 使用蓝色表示固定的 Y 轴
//
//         // 移动的 Y 轴（根据 _offset 进行偏移）
//         Vector2 movingYAxisStart = new Vector2(-_offset, -yAxisLength);
//         Vector2 movingYAxisEnd = new Vector2(-_offset, yAxisLength);
//         AddLineSegment(vh, movingYAxisStart, movingYAxisEnd, Color.green); // 使用绿色表示移动的 Y 轴
//
//         // 无限长的 X 轴
//         float infiniteLength = 100000f; // 一个非常大的值，用于模拟无限长的 X 轴
//         Vector2 xAxisStart = new Vector2(-infiniteLength - _offset, 0f); // X 轴起点，应用偏移量
//         Vector2 xAxisEnd = new Vector2(infiniteLength - _offset, 0f);    // X 轴终点，应用偏移量
//         AddLineSegment(vh, xAxisStart, xAxisEnd, Color.green); // 绘制绿色的 X 轴
//
//         // 沿固定的 Y 轴绘制水平分隔线
//         for (float y = -yAxisLength; y <= yAxisLength; y += yInterval)
//         {
//             Vector2 separatorStart = new Vector2(-25f, y);
//             Vector2 separatorEnd = new Vector2(25f, y);
//             AddLineSegment(vh, separatorStart, separatorEnd, Color.green);
//         }
//     }
//
//     // Helper function to add a line segment as quads to the mesh
//     private void AddLineSegment(VertexHelper vh, Vector2 start, Vector2 end, Color color)
//     {
//         if (!Application.isPlaying)
//         {
//             return;
//         }
//
//         float lineWidth = 2f; // Line width in pixels
//
//         Vector2 perpendicular = (end - start).normalized;
//         perpendicular = new Vector2(-perpendicular.y, perpendicular.x) * lineWidth * 0.5f;
//
//         // Define vertices for line thickness (quad shape)
//         UIVertex v1 = UIVertex.simpleVert;
//         v1.color = color;
//         v1.position = start - perpendicular;
//
//         UIVertex v2 = UIVertex.simpleVert;
//         v2.color = color;
//         v2.position = start + perpendicular;
//
//         UIVertex v3 = UIVertex.simpleVert;
//         v3.color = color;
//         v3.position = end + perpendicular;
//
//         UIVertex v4 = UIVertex.simpleVert;
//         v4.color = color;
//         v4.position = end - perpendicular;
//
//         // Add vertices for the quad representing a line segment
//         vh.AddUIVertexQuad(new[] { v1, v2, v3, v4 });
//     }
//
//     // Helper function to set up a Text component
//     private void SetupLabel(Text label, string initialText, Vector2 position)
//     {
//         label.text = initialText;
//         label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
//         label.fontSize = 14;
//         label.alignment = TextAnchor.MiddleCenter;
//         label.color = Color.black;
//
//         // Set the initial position of the label
//         RectTransform rectTransform = label.GetComponent<RectTransform>();
//         rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
//         rectTransform.anchoredPosition = position;
//     }
// }