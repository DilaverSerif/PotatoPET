using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SaveFile
{
    public List<ItemData> eatingFoods = new List<ItemData>();
}

public class Eating : CanSave
{
    public override void OnEnable()
    {
        base.OnEnable();
        Player.EetFoodEvent.AddListener(Eat);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Player.EetFoodEvent.RemoveListener(Eat);
    }

    public override void FileSave()
    {
        var file = SaveSystem._SaveFile;

        file.eatingFoods = foods;
    }

    public override void SaveLoad()
    {
        if (SaveSystem.haveSave)
        {
            var file = SaveSystem._SaveFile;

            foods = file.eatingFoods;
        }
    }

    private void Start()
    {
        StartCoroutine(DeleteFood());
    }

    private IEnumerator DeleteFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(1200);

            if (foods.Count > 0)
                foods.RemoveAt(0);
        }
    }

    //100-75 = 25
    private static List<ItemData> foods = new List<ItemData>();

    public static bool CheckSameFood(ItemData food)
    {
        // int same = 0;
        // if (foods.Count > 0)
        // {
        //     foreach (var a in foods)
        //     {
        //         if (a.ItemName == food.ItemName)
        //         {
        //             same += 1;
        //         }
        //     }
        // }
        //
        // if (same > 6 | Player.Instance.Hunger >= 100)
        // {
        //     return false;
        // }

        if (Player.Instance.Hunger >= 100)
        {
            return false;
        }

        foods.Add(food);
        return true;
    }

    private void Eat(float value)
    {
        if (Player.Instance.Hunger < 100)
        {
            float exp = 0;

            float x = 100 - Player.Instance.Hunger;

            if (value < x)
            {
                exp = value;
            }
            else exp = x;

            exp = (float) Math.Floor(exp);

            Player.GetExpEvent.Invoke((int) (exp));
        }
    }
}