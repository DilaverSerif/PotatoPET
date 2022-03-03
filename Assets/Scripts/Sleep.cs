using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Sleep : MonoBehaviour
{
    [SerializeField] private Image sleepImage;
    
    private void OnEnable()
    {
        Player.SleepEvent.AddListener(NowSleep);
    }

    private void OnDisable()
    {
        Player.SleepEvent.RemoveListener(NowSleep);
    }

    private void NowSleep()
    {
        Player.Instance.isSleep = !Player.Instance.isSleep;
        Player.Instance.sleepTime = DateTime.Now;

        DOTween.Kill("sleep");

        if (Player.Instance.isSleep)
        {
            sleepImage.DOFade(0.5F, 0.35F).SetId("sleep");
            GameBase.Dilaver.SoundSystem.PlaySound(Sounds.sleep);
            GameBase.Dilaver.MusicSystem.SetVolume(0.2f);
        }
        else
        {
            GameBase.Dilaver.SoundSystem.StopSound();
            sleepImage.DOFade(0, 0.35F).SetId("sleep");
            GameBase.Dilaver.MusicSystem.SetVolume(0.8f);
        }
    }
}
