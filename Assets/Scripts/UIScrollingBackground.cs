using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIScrollingBackground : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.0f);
    private RawImage rawImage;
    private Material runtimeMat;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        runtimeMat = Instantiate(rawImage.material);
        rawImage.material = runtimeMat;
    }

    void Update()
    {
        Vector2 offset = runtimeMat.mainTextureOffset;
        offset += scrollSpeed * Time.deltaTime;
        runtimeMat.mainTextureOffset = offset;
    }
}
