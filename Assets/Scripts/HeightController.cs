using UnityEngine;
using UnityEngine.UI;

public class HeightController : MonoBehaviour
{
    public LineRenderer targetLineRenderer;
    public Scrollbar heightScrollbar;

    public float minHeight = -10f;
    public float maxHeight = 10f;

    private void Start()
    {
        heightScrollbar.onValueChanged.AddListener(OnHeightValueChanged);
    }

    private void OnHeightValueChanged(float value)
    {
        // Convert the scrollbar value (0 to 1) to a height value (minHeight to maxHeight)
        float currentHeight = Mathf.Lerp(minHeight, maxHeight, value);

        // Assuming the LineRenderer has two points: start and end.
        Vector3 startPosition = targetLineRenderer.GetPosition(0);
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + currentHeight, startPosition.z);

        targetLineRenderer.SetPosition(1, endPosition);
    }
}
