using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Draggable : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    private Vector3 startPos;
    [SerializeField] private LayerMask patatoMask;
    public virtual void Awake()
    {
        startPos = GetComponent<RectTransform>().position;
    }

    public abstract void Drag();

    protected RaycastHit2D hit;
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = eventData.position;
        hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        
        if (hit)
        {
            Drag();
        }
    }
    
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.position = startPos;
        gameObject.transform.localScale = Vector3.one;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
