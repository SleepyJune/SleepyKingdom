using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class AnimalCapturePopup : Popup
{
    [NonSerialized]
    public AnimalUnit animal;

    public Image faceImage;

    public void SetAnimal(AnimalUnit newAnimal)
    {
        animal = newAnimal;
                
        faceImage.sprite = animal.faceImage;
    }

    public void OnButtonPressed()
    {
        if (animal != null)
        {
            animal.unitManager.actionBar.SetTarget(animal);
            Close();
        }
    }
}
