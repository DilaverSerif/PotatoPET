using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinShop : MonoBehaviour
{
    [SerializeField] private Transform coinShopPanel;
    private Button thisButton;
    [SerializeField] private Button _500Button, _1250Button, _2000Button, _3000Button, _6500Button, _13500Button,close;
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OpenClose);
    }

    private void Start()
    {
        _500Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold500"));
        _1250Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold1250"));

        _2000Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold2000"));
        _3000Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold3000"));

        _6500Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold6500"));
        _13500Button.onClick.AddListener(()=> Purchaser.Instance.BuyConsumable("gold13500"));
        close.onClick.AddListener(()=> coinShopPanel.gameObject.SetActive(false));
    }

    private bool open;
    private void OpenClose()
    {
        open = !open;

        coinShopPanel.gameObject.SetActive(open);
    }
}
