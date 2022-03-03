using UnityEngine;
using UnityEngine.EventSystems;

public class Soap : Draggable
{
    private float time;
    public override void Drag()
    {
        var check = hit.collider.gameObject.GetComponent<Cleaning>();
        
        if (check == null) return;
        if (time < 0.15f) time += Time.deltaTime;
        else
        {
            if(Random.Range(0,10)>5)GameBase.Dilaver.SoundSystem.PlaySound(Sounds.soapwiping1);
            else GameBase.Dilaver.SoundSystem.PlaySound(Sounds.soapwiping2);

            time = 0;
            check.AddBalon(new Vector3(hit.point.x,hit.point.y,check.transform.position.z));
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GameBase.Dilaver.SoundSystem.PlaySound(Sounds.takeSoap);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        time = 0;
    }
}
