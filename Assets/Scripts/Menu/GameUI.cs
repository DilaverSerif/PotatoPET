using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI comboText;

    private Vector3 scorePos;
    private void ScoreWriter(int score)
    {
        DOTween.Kill("SCORE");
        scoreText.transform.DOShakePosition(0.15f, Vector3.one * 5, 5).SetId("SCORE").OnComplete(()=> scoreText.GetComponent<RectTransform>().anchoredPosition = scorePos);
        scoreText.DOColor(Color.yellow, 0.15F).SetId("SCORE").OnComplete(()=> scoreText.DOColor(Color.white, 0.15f).SetId("SCORE"));
        scoreText.text = "<sprite=0>" + score.ToString();
    }
    
    private Vector3 comboPos;
    private void ComboWriter(int combo)
    {
        DOTween.Kill("COMBO");
        comboText.transform.DOShakePosition(0.15f, Vector3.one * 5, 5).SetId("COMBO").OnComplete(()=> comboText.GetComponent<RectTransform>().anchoredPosition = comboPos);
        comboText.DOColor(Color.red, 0.15F).SetId("COMBO").OnComplete(()=> comboText.DOColor(Color.white, 0.15f).SetId("COMBO"));
        comboText.text = "X" + combo.ToString();
    }

    private void LevelWriter(int score)
    {
        DOTween.Kill("LEVEL");
        levelText.DOColor(Color.green, 0.15F).SetId("LEVEL").OnComplete(()=> levelText.DOColor(Color.white, 0.15f).SetId("LEVEL"));
        levelText.text = "LEVEL " + score.ToString();
    }

    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.text = "<sprite=0>" + "0";
        scorePos = scoreText.GetComponent<RectTransform>().anchoredPosition;
        
        levelText = transform.Find("Level").GetComponent<TextMeshProUGUI>();
        levelText.text = "LEVEL 1";
        
        comboText = transform.Find("Combo").GetComponent<TextMeshProUGUI>();
        comboText.text = "X0";
        comboPos = comboText.GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnEnable()
    {
        MenuSystem.ScoreTextWriter.AddListener(ScoreWriter);
        MenuSystem.ComboTextWriter.AddListener(ComboWriter);
        MenuSystem.LevelTextWriter.AddListener(LevelWriter);
    }

    private void OnDisable()
    {
        MenuSystem.ScoreTextWriter.RemoveListener(ScoreWriter);
        MenuSystem.ComboTextWriter.RemoveListener(ComboWriter);
        MenuSystem.LevelTextWriter.RemoveListener(LevelWriter);
    }
}


