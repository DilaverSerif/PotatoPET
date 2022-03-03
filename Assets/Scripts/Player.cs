using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public partial class SaveFile
{
    public string last;
    public int money, availableExp;
    public float hungry, morale, dirt, sleep;
    public bool isSleep;
}

public class Player : CanSave
{
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }

            return _instance;
        }
    }

    private static Player _instance;

    public static EetFood EetFoodEvent = new EetFood();
    public static UnityEvent SleepEvent = new UnityEvent();
    public static Exp GetExpEvent = new Exp();
    public static ParametreWithEvent<int> GiveMoney = new ParametreWithEvent<int>();
    public static UnityEvent ClearEvent = new UnityEvent();
    public static ParametreWithEvent<int> MoraleEvent = new ParametreWithEvent<int>();

    enum Room
    {
        living,
        bath,
        kitchen,
        bed
    }

    public class EetFood : UnityEvent<float>
    {
    }

    public class ParametreWithEvent<T> : UnityEvent<T>
    {
    }

    public class Exp : UnityEvent<int>
    {
    }

    public DateTime sleepTime;
    public bool isSleep;

    private float morale, sleep, hunger, dirty;

    public float Morale => morale;

    public float Sleep => sleep;

    public float Hunger
    {
        get { return hunger; }
        set { hunger += value; }
    }

    public float Dirty => dirty;

    public int money;

    private int level = 1;
    public int Level => level;
    private int availableExp;

    public int AvailableExp
    {
        get { return availableExp; }
    }

    private void GetExp(int _exp)
    {
        availableExp += _exp * level;

        if (availableExp >= level * 30)
        {
            availableExp = 0;
            level += 1;
            GiveMoney.Invoke(level * 10);
        }

        StatsUI.SetStatsEvent.Invoke();
    }

    private void MoneyGive(int _money)
    {
        money += _money;
        GameBase.Dilaver.SoundSystem.PlaySound(Sounds.gold);
        StatsUI.MoneyShowEvent.Invoke();
    }

    private void AddMorale(int _morale)
    {
        morale += _morale;
        if (morale > 100) morale = 100;
        StatsUI.SetStatsEvent.Invoke();
    }

    private void Clear()
    {
        dirty += 5;
        StatsUI.SetStatsEvent.Invoke();
    }

    private void Awake()
    {
        morale = 100;
        sleep = 100;
        hunger = 75;
        dirty = 50;
        money = 100;
    }


    private void Eat(float value)
    {
        if (hunger < 100)
        {
            hunger += value;
            if (hunger > 100) hunger = 100;
        }

        StatsUI.SetStatsEvent.Invoke();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SleepEvent.AddListener(IsSleepNow);
        GiveMoney.AddListener(MoneyGive);
        AdsSystem.AdsPrize.AddListener(
            () =>
            {
                MoraleEvent.Invoke(10); 
                GiveMoney.Invoke(200);
            }
        );
        MoraleEvent.AddListener(AddMorale);
        EetFoodEvent.AddListener(Eat);
        GetExpEvent.AddListener(GetExp);
        ClearEvent.AddListener(Clear);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SleepEvent.RemoveListener(IsSleepNow);

        GiveMoney.RemoveListener(MoneyGive);

        AdsSystem.AdsPrize.RemoveListener(
            () => { MoraleEvent.Invoke(20); }
        );
        MoraleEvent.RemoveListener(AddMorale);
        EetFoodEvent.RemoveListener(Eat);
        GetExpEvent.RemoveListener(GetExp);
        ClearEvent.RemoveListener(Clear);
    }

    public override void FileSave()
    {
        var file = SaveSystem._SaveFile;
        file.sleep = sleep;
        file.dirt = dirty;
        file.morale = morale;
        file.hungry = hunger;

        file.money = money;
        file.availableExp = availableExp;
        file.level = level;
        file.isSleep = isSleep;

        file.last = DateTime.Now.ToString();
    }

    private void IsSleepNow()
    {
        if(transform.position == new Vector3(0, transform.position.y, transform.position.z)) return;
        MenuSystem.OpenWarning.Invoke("POTATO WOKE UP :)");
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
    
    public override void SaveLoad()
    {
        if (SaveSystem.haveSave)
        {
            var file = SaveSystem._SaveFile;

            isSleep = file.isSleep;
            sleep = file.sleep;
            dirty = file.dirt;
            morale = file.morale;
            hunger = file.hungry;

            money = file.money;
            availableExp = file.availableExp;
            level = file.level;

            sleepTime = Convert.ToDateTime(file.last);
        }
    }
    

    private void CalculateTime()
    {
        int passTime = (int) DateTime.Now.Subtract(sleepTime).TotalMinutes;
        
        if(passTime <= 0) return;
        
        if (isSleep)
        {
            var _sleep = sleep;

            sleep += passTime * 0.6f;
            hunger -= passTime * 0.1f;
            dirty -= passTime * 0.1f;
            morale -= passTime * 0.1f;

            _sleep -= sleep;

            if (_sleep > 100) _sleep = 100;
            if (_sleep > 0)
            {
                GetExpEvent.Invoke((int) _sleep);
            }
            
            SleepEvent.Invoke();

        }
        else
        {
            sleep -= passTime * 0.2f;
            hunger -= passTime * 0.2f;
            dirty -= passTime * 0.2f;
            morale -= passTime * 0.2f;
        }
    }

    private IEnumerator Start()
    {
        if(SaveSystem.haveSave) CalculateTime();
        StatsUI.MoneyShowEvent.Invoke();
        int sleeping = 0;
        while (true)
        {
            if (sleep > 100) sleep = 100;
            if (morale > 100) morale = 100;
            if (dirty > 100) dirty = 100;
            if (hunger > 100) hunger = 100;

            if (isSleep)
            {
                if (sleep > 0) sleep += 0.5f;
                if (sleeping % 3 == 0) GetExp(1);
                if (morale > 0) morale -= 0.1f;
                if (hunger > 0) hunger -= 0.3f;
                if (dirty > 0) dirty -= 0.1f;
            }
            else
            {
                if (sleep > 0) sleep -= 0.25f;
                if (morale > 0) morale -= 0.25f;
                if (hunger > 0) hunger -= 1;
                if (dirty > 0) dirty -= 0.5f;
            }

            if (sleep < 0) sleep = 0;
            if (morale < 0) morale = 0;
            if (hunger < 0) hunger = 0;
            if (dirty < 0) dirty = 0;

            PlayerAnimationController.CheckAnim.Invoke();
            Cleaning.CheckDirtsEvent.Invoke(dirty);
            StatsUI.SetStatsEvent.Invoke();
            yield return new WaitForSeconds(15f);
            if (isSleep) sleeping += 1;
        }
    }
}