using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ResourceButtonUI : MonoBehaviour
{
    public Sprite resourceImage;

    public Text resourceName;
    public Text amount;

    public void UpdateResource(int value)
    {
        amount.text = value.ToString();
    }
}