using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerScene
{
    public class TowerManager : MonoBehaviour
    {
        [NonSerialized]
        public HouseSlotManager houseSlotManager;

        public HouseInspectPopup houseInspectPopup;

        public static TowerManager instance = null;

        public Transform popupParent;

        public SideUIController sideUIController;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            houseSlotManager = GetComponent<HouseSlotManager>();
        }
    }
}
