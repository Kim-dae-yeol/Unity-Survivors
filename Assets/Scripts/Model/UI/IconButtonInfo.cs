using System;
using UnityEngine;

namespace Model.UI
{
    [Serializable]
    public struct IconButtonInfo
    {
        public string name;
        public Sprite icon;
        public Sprite background;
        public string popupUiName;
    }
}