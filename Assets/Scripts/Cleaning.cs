using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Cleaning : MonoBehaviour
{
    [SerializeField] private Transform dirt1;
    [SerializeField] private Transform balloon;
    [SerializeField] private Transform dirtsParent;

    private int baloonCount;

    [SerializeField] private int fullBalonCount;
    private List<Transform> baloons = new List<Transform>();
    public int BaloonCount => baloonCount;

    private void OnEnable()
    {
        CheckDirtsEvent.AddListener(CheckDirt);
    }

    private void OnDisable()
    {
        CheckDirtsEvent.RemoveListener(CheckDirt);
    }

    public void AddBalon(Vector3 pos)
    {
        if (baloonCount > fullBalonCount)
        {
            baloonCount = fullBalonCount;
        }
        else
        {
            baloonCount += 1;
            var balon = Instantiate(balloon, pos, Quaternion.identity);
            baloons.Add(balon);
            balon.SetParent(dirtsParent);
        }
    }

    public void Clear()
    {
        if (baloons.Count == 0) return;
        baloons[0].GetComponent<Balloon>().ByeBye();
        Player.ClearEvent.Invoke();
        CheckDirtsEvent.Invoke(Player.Instance.Dirty);
        baloons.Remove(baloons[0]);
    }

    private List<GameObject> dirtys = new List<GameObject>();
    public static CheckDirts CheckDirtsEvent = new CheckDirts();


    private void CheckDirt(float value)
    {
        float firstValue = value / 10; //9.8

        firstValue = 10 - firstValue; //0.2

        int turnValue = (int) Math.Floor((decimal) firstValue);
        turnValue *= 3;
        
        if (turnValue > 0)
        {
            if (dirtys.Count < turnValue)
            {
                for (int i = 0; i < turnValue-dirtys.Count; i++)
                {
                    Vector3 rand = General.RandomPointInArea2D(GetComponent<Collider2D>());
                    var dirt = Instantiate(dirt1,
                        new Vector3(transform.position.x + Random.Range(-0.58f,0.675f), Random.Range(-0.5f,2.15f), transform.position.z),
                        Quaternion.identity);

                    if (Random.Range(0, 10) > 5)
                    {
                        dirt.GetComponent<SpriteRenderer>().flipX = true;
                    }

                    if (Random.Range(0, 10) > 5)
                    {
                        dirt.GetComponent<SpriteRenderer>().flipY = true;
                    }

                    dirt.SetParent(dirtsParent);
                    dirtys.Add(dirt.gameObject);
                }
            }
        }

        if (dirtys.Count == 0) return;
        
        int remove = dirtys.Count - turnValue;
        
        if(remove <= 0) return;
        
        for (int i = 0; i < remove; i++)
        {
            var a = dirtys[0].gameObject;
            dirtys.Remove(a);
            Destroy(a);
        }
    }

    public class CheckDirts : UnityEvent<float>
    {
    }
}