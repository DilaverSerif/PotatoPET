using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GoldLootAnim : MonoBehaviour
{
    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("GoldIcon").transform;
        transform.SetParent(FindObjectOfType<GraphicRaycaster>().transform);
    }

    private void Start()
    {
        transform.DOMove(target.position,0.65f).SetDelay(Random.Range(0, 0.45f)).OnComplete(()=> Destroy(gameObject));
    }
}
