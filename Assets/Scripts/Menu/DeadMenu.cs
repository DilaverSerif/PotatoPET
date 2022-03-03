using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeadMenu : MonoBehaviour
{
    private Transform deadPanel;
    private Transform backGround;
    private Button continueButton,claimButton;
    private void Awake()
    {
        deadPanel = transform.Find("DeadPanel");

        backGround = transform.Find("Background");
        
        continueButton = deadPanel.Find("Continue").GetComponent<Button>();
        continueButton.onClick.AddListener(()=> StartCoroutine(ContinueButton()));

        claimButton = deadPanel.Find("Claim").GetComponent<Button>();
        claimButton.onClick.AddListener(()=> ClaimButton());
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
        MenuSystem.OpenDead.AddListener(() =>
        {
            StartCoroutine(OpenAnim());
        });
        
        AdsSystem.AdsPrize.AddListener(Reward);
    }

    private IEnumerator OpenAnim()
    {
        Time.timeScale = 0;
        MusicSystem.SetMusicOption.Invoke(true);
        //GameBase.Dilaver.SoundSystem.PlaySound(Sounds.cantbuy);
        yield return new WaitForSecondsRealtime(1f);
        backGround.DOScale(new Vector3(25,25,1),1.25f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.25f);
        Time.timeScale = 0;
        for (int i = 0; i < deadPanel.childCount; i++)
        {
            deadPanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        MenuSystem.OpenDead.RemoveListener(() =>
        {
            StartCoroutine(OpenAnim());
        });    
        
        AdsSystem.AdsPrize.RemoveListener(Reward);
    }

    private IEnumerator ContinueButton()
    {
        AdsSystem.ShowFScreenAdsEvent.Invoke();
        yield return new WaitForSeconds(0.15f);
        LoadingScreen.LoadScene("Game");
    }

    private void ClaimButton()
    {
        AdsSystem.ShowRewardEvent.Invoke();
    }

    private void Reward()
    {
        var saveScore = GameBase.Dilaver.ScoreSystem.TotalScore / 2;
        
        PlayerPrefs.SetInt("prize",saveScore);
        
        LoadingScreen.LoadScene("Game");
    }
}



