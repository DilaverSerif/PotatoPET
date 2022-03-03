using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    public static UnityEvent CheckAnim = new UnityEvent();
    
    [SerializeField] private Animator bodyAnim,mouthAnim,eyeAnim,handAnim;
    
    private static readonly int Sleep = Animator.StringToHash("sleep");
    private static readonly int Dirt = Animator.StringToHash("dirt");
    private static readonly int Hunger = Animator.StringToHash("hunger");
    private static readonly int Morale = Animator.StringToHash("morale");

    private void Awake()
    {
        bodyAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CheckAnim.AddListener(UpdateAnimFloats);
        Player.SleepEvent.AddListener(SleepAnim);
    }

    private void OnDisable()
    {
        CheckAnim.RemoveListener(UpdateAnimFloats);
        Player.SleepEvent.RemoveListener(SleepAnim);

    }

    private void SleepAnim()
    {
        mouthAnim.SetBool("isSleep",Player.Instance.isSleep);
        bodyAnim.SetBool("isSleep",Player.Instance.isSleep);
        eyeAnim.SetBool("isSleep",Player.Instance.isSleep);
    }

    private void UpdateAnimFloats()
    {
        bodyAnim.SetFloat(Sleep,Player.Instance.Sleep);
        mouthAnim.SetFloat(Sleep,Player.Instance.Sleep);
        eyeAnim.SetFloat(Sleep,Player.Instance.Sleep);
        
        bodyAnim.SetFloat(Dirt,Player.Instance.Dirty);
        mouthAnim.SetFloat(Dirt,Player.Instance.Dirty);
        eyeAnim.SetFloat(Dirt,Player.Instance.Dirty);
        
        bodyAnim.SetFloat(Hunger,Player.Instance.Hunger);
        mouthAnim.SetFloat(Hunger,Player.Instance.Hunger);
        eyeAnim.SetFloat(Hunger,Player.Instance.Hunger);
        handAnim.SetFloat(Hunger,Player.Instance.Hunger);
        
        bodyAnim.SetFloat(Morale,Player.Instance.Morale);
        mouthAnim.SetFloat(Morale,Player.Instance.Morale);
        eyeAnim.SetFloat(Morale,Player.Instance.Morale);
    }

    public void Nam(bool value)
    {
        mouthAnim.SetBool("nam",value);
    }

    public void NamNam(ItemData item)
    {
        mouthAnim.SetTrigger("namnam");
    }
    
}
