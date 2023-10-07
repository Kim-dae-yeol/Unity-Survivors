using System;
using System.Collections;
using System.Collections.Generic;
using DE.Util;
using Di;
using Managers;
using Model.LevelDesign;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private UiManager _uiManager;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = FindObjectOfType<GameManager>();
            if (_instance != null)
            {
                return _instance;
            }

            GameObject go = new GameObject();
            _instance = go.AddComponent<GameManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _uiManager = Container.RequireUiManager(this);
    }

    private void Start()
    {
        _uiManager.ShowPopupByName("InteractableUi");

    }
}