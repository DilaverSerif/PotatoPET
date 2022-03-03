using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Image sleep, hungry, morale, dirt,levelBar;
    [SerializeField] private TextMeshProUGUI moneyText;
    private TextMeshProUGUI ageText;

    public static UnityEvent SetStatsEvent = new UnityEvent();
    public static UnityEvent MoneyShowEvent = new UnityEvent();
    
    public class MoneyShow:UnityEvent<int>{}

    private void Awake()
    {
        ageText = levelBar.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Player.GiveMoney.AddListener(GoldParticle);

        SetStatsEvent.AddListener(SetStats);
        MoneyShowEvent.AddListener(ShowMoney);
    }
    
    private void OnDisable()
    {
        Player.GiveMoney.RemoveListener(GoldParticle);
        SetStatsEvent.RemoveListener(SetStats);
        MoneyShowEvent.RemoveListener(ShowMoney);
    }

    private void GoldParticle(int _value)
    {
        if (_value > 20) _value = 20;

        var pos = Camera.main.WorldToScreenPoint(levelBar.transform.position);
        
        for (int i = 0; i < _value; i++)
        {
            GameBase.Dilaver.ParticlePlaySystem.SetDestory(false).PlayParticle(Particles.gold,new Vector3(pos.x + Random.Range(-20,20),pos.y + Random.Range(-20,20),0));
        }
    }
    
    private void ShowMoney()
    {
        moneyText.text = "<color=yellow>"+Player.Instance.money+" <sprite=0>";
    }

    private void SetStats()
    {
        hungry.DOFillAmount(Player.Instance.Hunger * 0.01f,0.2f).SetId("stats");
        sleep.DOFillAmount(Player.Instance.Sleep * 0.01f,0.2f).SetId("stats");
        morale.DOFillAmount(Player.Instance.Morale * 0.01f,0.2f).SetId("stats");
        dirt.DOFillAmount(Player.Instance.Dirty * 0.01f,0.2f).SetId("stats");
        float level = (float)(Player.Instance.AvailableExp) / (Player.Instance.Level * 20);
        levelBar.DOFillAmount( level,0.2f).SetId("stats");
        ageText.text = "LEVEL:" + Player.Instance.Level;
    }
}
