using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    private Slider loadingSlider;
    private TextMeshProUGUI loadingText;
    private static string sceneName;
    private AsyncOperation loadingAsync;

    public static void LoadScene(string _sceneName)
    {
        sceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        if (sceneName == null) sceneName = "Game";
        
        loadingSlider = FindObjectOfType<Slider>();
        loadingSlider.value = 0;
        loadingText = FindObjectOfType<TextMeshProUGUI>();

        loadingAsync = SceneManager.LoadSceneAsync(sceneName);
        
        loadingAsync.allowSceneActivation = false;

        StartCoroutine("Loading");
    }

    
    private IEnumerator Loading()
    {
        while (loadingAsync.progress < 0.9f)
        {
            loadingText.text = "loading...%" + loadingAsync.progress * 100;
            loadingSlider.DOValue(loadingAsync.progress * 100, 0.15F);
            yield return new WaitForEndOfFrame();
        }
        
        loadingSlider.DOValue(100, 0.15F);
        loadingText.text = "loading...%100";
        yield return new WaitForSeconds(0.5f);
        loadingAsync.allowSceneActivation = true;
    }
}
