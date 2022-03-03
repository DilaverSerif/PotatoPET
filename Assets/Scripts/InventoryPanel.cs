using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    private List<ItemData> inventory = new List<ItemData>();
    [SerializeField] private Transform itemBar;
    [SerializeField] private Transform foodParent, clothesParent;
    [SerializeField] private Button foodButton, clothesButton, closeButton;


    private void Start()
    {
        foodButton.onClick.AddListener(() => SetPanels(Categorys.foods));
        clothesButton.onClick.AddListener(() => SetPanels(Categorys.clothes));
        closeButton.onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }

    enum Categorys
    {
        foods,
        clothes
    }

    private void SetPanels(Categorys category)
    {
        foodParent.gameObject.SetActive(false);
        clothesParent.gameObject.SetActive(false);

        switch (category)
        {
            case Categorys.foods:
                foodParent.gameObject.SetActive(true);
                break;
            case Categorys.clothes:
                clothesParent.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(category), category, null);
        }
    }

    private void OnEnable()
    {
        GetItems();
    }


    private void OnDisable()
    {
        if (foodParent.childCount != 0)
        {
            foreach (Transform child in foodParent)
            {
                Destroy(child.gameObject);
            }
        }

        if (clothesParent.childCount != 0)
        {
            foreach (Transform child in clothesParent)
            {
                Destroy(child.gameObject);
            }
        }
    }


    private void GetItems()
    {
        if (PlayerInventory.Instance.Items == null) return;

        inventory = PlayerInventory.Instance.Items;

        int clothes = 0, food = 0;

        foreach (var item in inventory)
        {
            var spawnItem = Instantiate(itemBar);
            spawnItem.gameObject.SetActive(false);
            spawnItem.GetComponent<ItemBar>().data = item;
            spawnItem.gameObject.SetActive(true);
            if (item.ItemType == ItemType.food)
            {
                food += 1;
                spawnItem.SetParent(foodParent);
            }
            else
            {
                if (PlayerInventory.Instance.clothes != null)
                {
                    if (item.ItemName == PlayerInventory.Instance.clothes.ItemName)
                    {
                        spawnItem.GetComponent<ItemInventory>().AlreadyUsing();
                    }
                }

                if (PlayerInventory.Instance.hat != null)
                {
                    if (item.ItemName == PlayerInventory.Instance.hat.ItemName)
                    {
                        spawnItem.GetComponent<ItemInventory>().AlreadyUsing();
                    }
                }

                if (PlayerInventory.Instance.glasses != null)
                {
                    if (item.ItemName == PlayerInventory.Instance.glasses.ItemName)
                    {
                        spawnItem.GetComponent<ItemInventory>().AlreadyUsing();
                    }
                }

                spawnItem.SetParent(clothesParent);
                clothes += 1;
            }
        }

        //6440


        if (clothes > 12)
        {
            clothes -= 12;
            var carp = Math.Ceiling((decimal) clothes / 3);
            var rectClot = clothesParent.GetComponent<RectTransform>();
            rectClot.offsetMax = Vector2.zero;
            rectClot.offsetMin = new Vector2(0, (int) carp * -365);
        }

        if (food > 12)
        {
            food -= 12;
            var carp = Math.Ceiling((decimal) food / 3);
            var rectClot = foodParent.GetComponent<RectTransform>();
            rectClot.offsetMax = Vector2.zero;
            rectClot.offsetMin = new Vector2(0, (int) carp * -365);
        }
    }
}