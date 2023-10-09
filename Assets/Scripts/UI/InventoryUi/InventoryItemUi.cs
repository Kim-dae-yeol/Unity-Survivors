using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class InventoryItemUi : UiPopup
{
    //todo extract SO 
    [field: SerializeField] public int Row { get; private set; }
    [field: SerializeField] public int Col { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    
    //todo click event handling
    
    //todo drag and drop impl
}