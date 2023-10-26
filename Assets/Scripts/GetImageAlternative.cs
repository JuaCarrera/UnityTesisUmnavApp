using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using ZXing;

public class GetlmageAlternative : MonoBehaviour
{
    [SerializeField]
    private ARCameraBackground arCameraBackground;
    [SerializeField]
    private RenderTexture targetRenderTexture;
    [SerializeField]
    private TextMeshProUGUI qrCodeText;

    private Texture2D cameralmageTexture;
    private IBarcodeReader reader = new BarcodeReader(); // create a barcode reader instance

    private void Update()
    {
        Graphics.Blit(null, targetRenderTexture, arCameraBackground.material);
        cameralmageTexture = new Texture2D(targetRenderTexture.width, targetRenderTexture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(targetRenderTexture, cameralmageTexture);

        // Detect and decode the barcode inside the bitmap
        var result = reader.Decode(cameralmageTexture.GetPixels32(), cameralmageTexture.width, cameralmageTexture.height);
        
        // Do something with the result
        if (result != null)
        {
            qrCodeText.text = result.Text;
        }
    }
}