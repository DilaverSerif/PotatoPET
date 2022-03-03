using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV : MonoBehaviour
{
    private Button tvButton;

    private void Awake()
    {
        tvButton = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        tvButton.onClick.AddListener(TvWatch);
    }

    private void TvWatch()
    {
        AdsSystem.ShowRewardEvent.Invoke();
    }
    
}
