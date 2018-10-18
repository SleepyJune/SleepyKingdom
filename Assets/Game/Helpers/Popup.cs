using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject panel;

    public delegate void Callback(Popup popup);

    public event Callback OnShow;
    public event Callback OnClose;
    public event Callback OnHide;

    public void Show()
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);

            if(OnShow != null)
            {
                OnShow(this);
            }
        }
    }

    public void Hide()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);

            if(OnHide != null)
            {
                OnHide(this);
            }
        }
    }

    public void Close()
    {
        Destroy(gameObject);

        if (OnClose != null)
        {
            OnClose(this);
        }
    }
}