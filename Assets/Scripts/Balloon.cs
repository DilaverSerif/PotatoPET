using UnityEngine;
using DG.Tweening;

public class Balloon : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void ByeBye()
    {
        body.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<SpriteRenderer>().DOFade(0, 0.05F).OnComplete(
            () =>
            {
                GameBase.Dilaver.SoundSystem.PlaySound(Sounds.bubble);
                Destroy(gameObject);
            }
        );
    }
}