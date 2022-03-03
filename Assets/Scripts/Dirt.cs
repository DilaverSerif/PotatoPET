using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    private void OnDestroy()
    {
        Player.GetExpEvent.Invoke(1);
    }
}
