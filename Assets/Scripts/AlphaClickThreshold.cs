using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaClickThreshold : MonoBehaviour, ICanvasRaycastFilter
{
    [Range(0, 1)]
    public float alphaThreshold = 0.1f;

    private Image _image;

    void Start() => _image = GetComponent<Image>();

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _image.rectTransform, screenPoint, eventCamera, out Vector2 local);

        Rect spriteRect = _image.sprite.rect;
        Rect uvRect = _image.rectTransform.rect;

        float x = (local.x / uvRect.width + 0.5f) * spriteRect.width + spriteRect.x;
        float y = (local.y / uvRect.height + 0.5f) * spriteRect.height + spriteRect.y;

        return _image.sprite.texture.GetPixel((int)x, (int)y).a > alphaThreshold;
    }
}
