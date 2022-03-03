using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Joystick joystick;
    private Animator anim;
    private Rigidbody body;
    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    private Vector3 Target;
    private void Update()
    {
        anim.SetFloat("speed",body.velocity.magnitude);

        var rot = Mathf.Atan2(joystick.Output.y, joystick.Output.x) * Mathf.Rad2Deg;
        
        //transform.rotation = Quaternion.Euler(0,rot,0);
    }
}
