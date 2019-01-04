using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Game/Helmet Object")]
public class HelmetObject : GameDataObject
{
    public Sprite image;

    public Sprite imageBack;

    public int defense = 1;
}
