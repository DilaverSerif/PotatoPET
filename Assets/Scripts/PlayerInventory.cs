using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class SaveFile
{
    [SerializeField] public List<ItemData> ItemDatas = new List<ItemData>();
    public ItemData clothe, glass, hat;
}

public class PlayerInventory : CanSave
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();
    private GameObject inventory;

    public List<ItemData> Items
    {
        get { return items; }
    }

    public ItemData clothes, glasses, hat;

    [SerializeField] private Transform clothesPoint, glassesPoint, hatPoint;

    public static Wear WearEvent = new Wear();
    public static Action<ItemData> ItemRemove;
    public static Wear AddItemEvent = new Wear();
    public static Wear RemoveItemEvent = new Wear();


    private static PlayerInventory _instance;

    public static PlayerInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerInventory>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("PlayeInventory");
                    _instance = container.AddComponent<PlayerInventory>();
                }
            }

            return _instance;
        }
    }
    

    private void CheckEquiment()
    {
        if (glasses != null) glassesPoint.GetComponent<SpriteRenderer>().sprite = glasses.ItemSprite;

        if (hat != null) hatPoint.GetComponent<SpriteRenderer>().sprite = hat.ItemSprite;

        if (clothes != null) clothesPoint.GetComponent<SpriteRenderer>().sprite = clothes.ItemSprite;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        WearEvent.AddListener(Wear);
        ItemRemove += RemoveWear;
        AddItemEvent.AddListener(AddItem);
        RemoveItemEvent.AddListener(RemoveItem);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        AddItemEvent.RemoveListener(AddItem);
        WearEvent.RemoveListener(Wear);
        ItemRemove -= RemoveWear;

        RemoveItemEvent.RemoveListener(RemoveItem);
    }

    public override void FileSave()
    {
        var file = SaveSystem._SaveFile;

        file.ItemDatas = items;
        file.clothe = clothes;
        file.hat = hat;
        file.glass = glasses;
    }

    public override void SaveLoad()
    {
        
        if (SaveSystem.haveSave)
        {
            var file = SaveSystem._SaveFile;
            items = file.ItemDatas;
            clothes = file.clothe;
            glasses = file.glass;
            hat = file.hat;

            CheckEquiment();
        }
    }
    
    public ItemData GetFood(int index = 0)
    {
        List<ItemData> foods = new List<ItemData>();

        foreach (var food in items)
        {
            if (food.ItemType == ItemType.food)
            {
                foods.Add(food);
            }
        }

        if (index == foods.Count)
        {
            index = 0;
        }

        if (index < 0)
        {
            index = foods.Count - 1;
        }


        Kitchen.foodIndex = index;
        if (foods.Count == 0) return null;

        return foods[index];
    }

    private void RemoveWear(ItemData data)
    {
        switch (data.ItemType)
        {
            case ItemType.glasses:
                glassesPoint.GetComponent<SpriteRenderer>().sprite = null;
                glasses = null;
                break;
            case ItemType.hat:
                hatPoint.GetComponent<SpriteRenderer>().sprite = null;
                hat = null;
                break;
            case ItemType.clothe:
                clothesPoint.GetComponent<SpriteRenderer>().sprite = null;
                clothes = null;
                break;
        }
        
        GameBase.Dilaver.SoundSystem.SetPitch().PlaySound(Sounds.zipper);
        if ( inventory == null)
        {
            inventory = FindObjectOfType<InventoryPanel>().transform.parent.gameObject;
        }
        inventory.SetActive(false);
    }
    
    private void Wear(ItemData data)
    {
        switch (data.ItemType)
        {
            case ItemType.glasses:
                glassesPoint.GetComponent<SpriteRenderer>().sprite = data.ItemSprite;
                glasses = data;
                break;
            case ItemType.hat:
                hatPoint.GetComponent<SpriteRenderer>().sprite = data.ItemSprite;
                hat = data;
                break;
            case ItemType.clothe:
                clothesPoint.GetComponent<SpriteRenderer>().sprite = data.ItemSprite;
                clothes = data;
                break;
        }
        
        GameBase.Dilaver.SoundSystem.SetPitch().PlaySound(Sounds.zipper);
        if ( inventory == null)
        {
            inventory = FindObjectOfType<InventoryPanel>().transform.parent.gameObject;
        }
        inventory.SetActive(false);
    }

    private void AddItem(ItemData data)
    {
        foreach (var a in items)
        {
            if (data.Stackable)
            {
                if (a.ItemName == data.ItemName)
                {
                    if (data.Stack == 0)
                    {
                        data.Stack = 1;
                    }
                    data.Stack = 1;
                    return;
                }
            }
   
        }
        
        items.Add(data);
    }

    private void RemoveItem(ItemData data)
    {
        if (data.Stackable)
        {
            foreach (var a in items)
            {
                if (a.ItemName == data.ItemName)
                {
                    if (a.Stack > 0) data.Stack = -1;
                    if (a.Stack <= 0) items.Remove(a);
                    break;
                }
            }

            return;
        }

        foreach (var a in items)
        {
            if (a.ItemName == data.ItemName)
            {
                items.Remove(a);
                break;
            }
        }
    }
}

public class Wear : UnityEvent<ItemData>
{
}