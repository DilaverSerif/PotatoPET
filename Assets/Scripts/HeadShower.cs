using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeadShower : Draggable
{
    private float time;
    private Transform anim;

    public override void Awake()
    {
        base.Awake();
        anim = transform.GetChild(0);
    }

    public override void Drag()
    {
        if (hit)
        {
            var check = hit.collider.gameObject.GetComponent<Cleaning>();
            
            if (check != null)
            {
                if(!anim.gameObject.activeSelf) GameBase.Dilaver.SoundSystem.PlaySound(Sounds.shower);
                anim.gameObject.SetActive(true);
                if (time < 0.1f) time += Time.deltaTime;
                else
                {
                    time = 0;
                    check.Clear();
                }
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        GameBase.Dilaver.SoundSystem.StopSound();
        anim.gameObject.SetActive(false);

        time = 0;
    }
}
