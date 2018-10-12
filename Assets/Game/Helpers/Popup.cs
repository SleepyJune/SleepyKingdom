using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject panel;

    public void Show()
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
        }
    }

    public void Close()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }
}