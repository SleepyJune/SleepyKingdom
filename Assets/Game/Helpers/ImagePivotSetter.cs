using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ImagePivotSetter : MonoBehaviour
{   

    // Use this for initialization
    void Start()
    {
        Vector2 size = GetComponent<RectTransform>().sizeDelta;
        size *= GetComponent<Image>().pixelsPerUnit;
        Vector2 pixelPivot = GetComponent<Image>().sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        GetComponent<RectTransform>().pivot = percentPivot;
    }
}
