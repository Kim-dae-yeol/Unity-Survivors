using System;
using System.Collections;
using System.Collections.Generic;
using DE.Util;
using Di;
using Managers;
using Model.Item;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private UiManager _uiManager;
    public InventoryManager InventoryManager { get; private set; }
    private DataManager _dataManager = new DataManager();

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
        var items = GetItems();
        InventoryManager = new InventoryManager(items);
        _uiManager = Container.RequireUiManager(this);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //todo load ui per scene
        SceneManager.sceneLoaded += ShowUi;
    }

    private void ShowUi(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScene")
        {
            _uiManager.ShowPopupByName(nameof(HomeUi));
        }
    }

    private List<InventoryItem> GetItems()
    {
        List<InventoryItem> items = _dataManager.LoadTestDataSet();

        return items;
    }

    public void DropInventoryItem(int index)
    {
        //todo if dungeon drop item shown
        InventoryManager.RemoveItemAt(index);
    }
}