using System;
using System.Collections;
using UnityEngine;

public class ParticleDestory : MonoBehaviour
{
    public float lifeTime;
    private ParticleSystem ps;
    private void Awake() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (lifeTime == 0)
        {
            ps.Stop();

            var main = ps.main;

            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Destroy;

            ps.Play();
        }
        else StartCoroutine(Counter());
    }


    private IEnumerator Counter()
    {
        var main = ps.main;

        main.loop = true;
        main.stopAction = ParticleSystemStopAction.Destroy;
        
        yield return new WaitForSeconds(lifeTime);
        main.loop = false;
        ps.Stop();
        Destroy(gameObject);
        
    }

}
