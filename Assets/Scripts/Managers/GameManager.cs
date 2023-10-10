using System;
using System.Collections;
using System.Collections.Generic;
using DE.Util;
using Di;
using Managers;
using Model.Item;
using Model.LevelDesign;
using UnityEngine;
using Util;

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private UiManager _uiManager;
    public InventoryManager InventoryManager { get; private set; }

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
        _uiManager.ShowPopupByName("InteractableUi");
        LoadDataSetFromCsv();
    }

    private static void LoadDataSetFromCsv()
    {
        List<TestData> testDataSet = CsvReader.ReadCsv<TestData>("test.csv", 1);
        foreach (var testData in testDataSet)
        {
            Debug.Log($"testData is {testData.ToStringForLogging()}");
        }
        //todo 
    }

    private List<InventoryItem> GetItems()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        for (int i = 0; i < 2; i++)
        {
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Row = 0;
            inventoryItem.Col = i;
            Item item = new Item(
                type: Item.ItemType.Weapon,
                width: 1,
                height: 3,
                itemName: "Long Sword",
                itemDesc: "very long sword",
                minDamage: 1,
                maxDamage: 3
            );
            inventoryItem.Item = item;
            items.Add(inventoryItem);
        }

        return items;
    }
}