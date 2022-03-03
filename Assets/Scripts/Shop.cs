using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Buy BuyEvent = new Buy();
    [SerializeField] private List<ItemData> foods, clothes;
    [SerializeField] private Transform itemBar;
    [SerializeField] private Transform clothesPanel, foodsPanel;
    [SerializeField] private Button closeButton;

     [Header("CLOTHES")] 
     [SerializeField]private Transform dressPanel;
     [SerializeField] private Transform glassPanel;
     [SerializeField] private Transform hatPanel;
     [SerializeField] private Button hatButton;
     [SerializeField] private Button derssButton;
     [SerializeField] private Button glassesButton;

     private List<ItemBar> ItemBars = new List<ItemBar>();

     private void CheckShop()
     {
         var a = PlayerInventory.Instance.Items;
         
         if (a.Count == 0 | ItemBars.Count == 0)
         {
             return;
         }

         for (int i = 0; i < a.Count; i++)
         {
             for (int j = 0; j < ItemBars.Count; j++)
             {
                 if (ItemBars[j].data.ItemName == a[i].ItemName)
                 {
                     ItemBars[j].AlreadyUsing();
                 }
             }
         }
         

     }
     
     private void CreateShop()
     {
         for (var i = 0; i < foods.Count; i++)
        {
            var item = Instantiate(itemBar);
            item.GetComponent<ItemBar>().data = foods[i];
            item.SetParent(foodsPanel.GetChild(0));
        }

        for (int i = 0; i < clothes.Count; i++)
        {
            var item = Instantiate(itemBar);
            item.GetComponent<ItemBar>().data = clothes[i];
            ItemBars.Add(item.GetComponent<ItemBar>());
            if (PlayerInventory.Instance.clothes != null)
            {
                if (clothes[i].ItemName == PlayerInventory.Instance.clothes.ItemName)
                {
                    item.GetComponent<ItemBar>().AlreadyUsing();
                }
            }

            if (PlayerInventory.Instance.hat != null)
            {
                if (clothes[i].ItemName == PlayerInventory.Instance.hat.ItemName)
                {
                    item.GetComponent<ItemBar>().AlreadyUsing();
                }

            }


            if (PlayerInventory.Instance.glasses != null)
            {
                if (clothes[i].ItemName == PlayerInventory.Instance.glasses.ItemName)
                {
                    item.GetComponent<ItemBar>().AlreadyUsing();
                }
            }
            
            
            switch (clothes[i].ItemType)
            {
                case ItemType.glasses:
                    item.SetParent(glassPanel);
                    break;
                case ItemType.hat:
                    item.SetParent(hatPanel);
                    break;
                case ItemType.clothe:
                    item.SetParent(dressPanel);
                    break;
            }
        }
    }

     private void Awake()
     {
         CreateShop();
     }

     private void Start()
    {
        
        hatButton.onClick.AddListener(()=> PanelOpen(Categorys.hats));
        derssButton.onClick.AddListener(()=> PanelOpen(Categorys.dress));
        glassesButton.onClick.AddListener(()=> PanelOpen(Categorys.glass));
        closeButton.onClick.AddListener(()=> closeButton.transform.parent.gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        OpenShop();
        CheckShop();
        BuyEvent.AddListener(BuyFunc);
    }

    private void OnDisable()
    {
        BuyEvent.RemoveListener(BuyFunc);
    }

    enum Shops
    {
        coin,
        food,
        clothes
    }

    private void OpenShop(bool gold = false)
    {
        clothesPanel.gameObject.SetActive(false);
        foodsPanel.gameObject.SetActive(false);
        hatButton.gameObject.SetActive(false);
        glassesButton.gameObject.SetActive(false);
        derssButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
        
        
        if (Player.Instance.transform.position.x != -25)
        {
            clothesPanel.gameObject.SetActive(true);
            glassesButton.gameObject.SetActive(true);
            hatButton.gameObject.SetActive(true);
            derssButton.gameObject.SetActive(true);
        } else
        {
            foodsPanel.gameObject.SetActive(true);
        }
        
        
        
    }
    
    private void PanelOpen(Categorys category)
    {
        glassPanel.gameObject.SetActive(false);
        hatPanel.gameObject.SetActive(false);
        dressPanel.gameObject.SetActive(false);
        glassesButton.interactable = true;
        hatButton.interactable = true;
        derssButton.interactable = true;
        
        
        switch (category)
        {
            case Categorys.glass:
                glassPanel.gameObject.SetActive(true);
                glassesButton.interactable = false;
                break;
            case Categorys.hats:
                hatPanel.gameObject.SetActive(true);
                hatButton.interactable = false;
                break;
            case Categorys.dress:
                dressPanel.gameObject.SetActive(true);
                derssButton.interactable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(category), category, null);
        }
        
    }

    enum Categorys
    {
        glass,
        hats,
        dress
    }
    
    private void BuyFunc(ItemData data)
    {
        if (Player.Instance.money >= data.Money)
        {
            GameBase.Dilaver.SoundSystem.PlaySound(Sounds.buy);
            Player.Instance.money -= data.Money;
            StatsUI.MoneyShowEvent.Invoke();
            PlayerInventory.AddItemEvent.Invoke(data);
            if(data.ItemType == ItemType.food) Kitchen.GetFoodEvent.Invoke();
            return;
        }

        MenuSystem.OpenWarning.Invoke("DONT HAVE ENOUGH MONEY :(");
        GameBase.Dilaver.SoundSystem.PlaySound(Sounds.cantbuy);

    }

    public class Buy : UnityEvent<ItemData>
    {
    }
}