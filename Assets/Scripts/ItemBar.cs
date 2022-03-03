using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public  class ItemBar : MonoBehaviour
{
    private Image itemImage;
    private TextMeshProUGUI itemTitle;
    protected Button buyButton;

    public ItemData data;

    protected virtual string title
    {
        get
        {
            return data.ItemName + "\n" + "<sprite=0>" + data.Money;
        }
    }

    private void Awake()
    {
        itemImage = transform.Find("ItemSprite").GetComponent<Image>();
        itemTitle = GetComponentInChildren<TextMeshProUGUI>();
        buyButton = GetComponentInChildren<Button>();
    }
    
    public virtual void AlreadyUsing()
    {
        if(!data.Stackable)
        {
            buyButton.gameObject.SetActive(false);
        }
    }
    protected virtual void OnStart(){}
    private void Start()
    {
        OnStart();
        buyButton.onClick.AddListener(Use); 
        if(data == null) return;
        itemImage.sprite = data.ItemSprite;

        if (data.ItemType  == ItemType.clothe)
        {
            itemImage.transform.localScale = new Vector3(3, 3, 3);
            itemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 30);
            
        }
        else if (data.ItemType == ItemType.glasses)
        {
            itemImage.transform.localScale = new Vector3(3, 3, 3);
            itemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -110);
        }
        else if (data.ItemType == ItemType.hat)
        {
            itemImage.transform.localScale = new Vector3(3, 3, 3);
            itemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -180);
        }
        
        itemTitle.text = title;
    }

    protected virtual void Use()
    {
        AlreadyUsing();
        Shop.BuyEvent.Invoke(data);   
    }
}
