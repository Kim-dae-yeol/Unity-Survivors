using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class EnterDungeonButtonUi : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(ShowDungeonUi);
    }

    private void ShowDungeonUi()
    {
        
        Debug.Log("show Dungeon ui");
    }
}