using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Food : Draggable
{
    public ItemData food;
    private PlayerAnimationController player;
    [SerializeField] private TextMeshProUGUI stackUI;

    public override void OnDrag(PointerEventData eventData)
    {
        if (food == null) return;
        base.OnDrag(eventData);

        if (!hit & player != null) player.Nam(false);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (food == null)
        {
            return;
        }

        base.OnBeginDrag(eventData);
    }

    public void CheckStack()
    {
        if (food == null)
        {
            stackUI.gameObject.SetActive(false);
            return;
        }

        if (food.Stack > 0)
        {
            stackUI.gameObject.SetActive(true);
            stackUI.text = "X" + food.Stack;
        }
        else stackUI.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        base.OnEndDrag(null);
    }

    public override void Drag()
    {
        var check = hit.collider.gameObject.GetComponent<PlayerAnimationController>();

        if (check == null)
        {
            return;
        }

        if (player == null) player = check;
        check.Nam(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (hit)
        {
            if (food == null) return;
            if (Eating.CheckSameFood(food)) Player.EetFoodEvent.Invoke(food.value);
            else
            {
                MenuSystem.OpenWarning.Invoke("DOESNT WANT :/");
                GameBase.Dilaver.SoundSystem.PlaySound(Sounds.dontwant);
                base.OnEndDrag(eventData);
                return;
            }
            
            hit.collider.GetComponent<PlayerAnimationController>().NamNam(food);
            GameBase.Dilaver.SoundSystem.PlaySound(Sounds.eat);
            PlayerInventory.RemoveItemEvent.Invoke(food);
            player.Nam(false);
            Kitchen.GetFoodEvent.Invoke();
        }

        if (player != null)
        {
            player.Nam(false);
            player = null;
        }

        base.OnEndDrag(eventData);
    }
}