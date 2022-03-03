using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public partial class SaveFile
{
    public bool music, effect;
}
public class PauseMenu : CanSave
{
    private Button pauseButton, soundButton, musicButton;

    private RectTransform pausePanel;

    private void Awake()
    {
        pausePanel = transform.Find("PausePanel").GetComponent<RectTransform>();

        pauseButton = GetComponent<Button>();
        soundButton = pausePanel.Find("Sound").GetComponent<Button>();
        musicButton = pausePanel.Find("Music").GetComponent<Button>();
        
        pauseButton.onClick.AddListener(PauseButton);
        soundButton.onClick.AddListener(SoundButton);
        musicButton.onClick.AddListener(MusicButton);
    }

    private void Start()
    {
        SaveLoad();
    }

    private bool pauseStat;
    private void PauseButton()
    {
        pauseStat = !pauseStat;
        StartCoroutine(AnimationThePanel(pauseStat));
    }

    private IEnumerator AnimationThePanel(bool value)
    {
        if (!value)
        {
            pausePanel.DOLocalMoveY(0, 0.2F).SetUpdate(true);
            pausePanel.DOSizeDelta(new Vector2(100, 100), 0.2f).SetUpdate(true).OnComplete(()=> pausePanel.gameObject.SetActive(false));
        }
        else
        {
            pausePanel.gameObject.SetActive(true);
            pausePanel.DOLocalMoveY(-270, 0.2F).SetUpdate(true);
            pausePanel.DOSizeDelta(new Vector2(100, 400), 0.2f).SetUpdate(true);
        }

        yield return new WaitForSecondsRealtime(0.21f);
        soundButton.gameObject.SetActive(value);
        musicButton.gameObject.SetActive(value);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        
        if (soundButton)
        {
            soundButton.GetComponent<Image>().DOColor(Color.white, 0.2f);
            
        }
        else
        {
            soundButton.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f, 1), 0.2f);
        }
        
        if (!musicStat)
        {
            musicButton.GetComponent<Image>().DOColor(Color.white, 0.2f);
            
        }
        else
        {
            musicButton.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f, 1), 0.2f);
        }
    }

    private bool effectStat;
    private void SoundButton()
    {
        effectStat = !effectStat;
        SoundSystem.SetSoundOption.Invoke(effectStat);
        if (!effectStat)
        {
            soundButton.GetComponent<Image>().DOColor(Color.white, 0.2f);
            
        }
        else
        {
            soundButton.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f, 1), 0.2f);
        }
    }

    private bool musicStat;
    private void MusicButton()
    {
        
        musicStat = !musicStat;
        MusicSystem.SetMusicOption.Invoke(musicStat);
        if (!musicStat)
        {
            musicButton.GetComponent<Image>().DOColor(Color.white, 0.2f);
            
        }
        else
        {
            musicButton.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f, 1), 0.2f);
        }
        
    }

    public override void FileSave()
    {
        var a = SaveSystem._SaveFile;
        a.effect = effectStat;
        a.music = musicStat;
    }

    public override void SaveLoad()
    {
        
        if (SaveSystem.haveSave)
        {
            var a = SaveSystem._SaveFile;

            effectStat = a.effect;
            musicStat = a.music;
            
            MusicSystem.SetMusicOption.Invoke(musicStat);
            SoundSystem.SetSoundOption.Invoke(effectStat);
        }

    }
}
