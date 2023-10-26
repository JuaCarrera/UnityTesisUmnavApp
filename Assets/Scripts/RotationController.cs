using UnityEngine;
using UnityEngine.UI;

public class RotationController : MonoBehaviour
{
    public Transform targetCamera;
    public Scrollbar rotationScrollbar;

    private float currentRotationY = 0f;

    private void Start()
    {
        rotationScrollbar.onValueChanged.AddListener(OnRotationValueChanged);
    }

    private void OnRotationValueChanged(float value)
    {
        // Convert the scrollbar value (0 to 1) to an angle value (0 to 360)
        currentRotationY = (value * 360f) + 255f;
        targetCamera.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }
}
