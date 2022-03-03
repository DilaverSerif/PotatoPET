using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : ItemBar
{
    [SerializeField] private Button itemRemove;
    
    protected override string title
    {
        get
        {
            if (data.Stackable)
            {
                buyButton.gameObject.SetActive(false);
                if (data.Stack == 0)
                {
                   return data.ItemName + "\n" + 1+ "X";
                }
                return data.ItemName + "\n" + data.Stack + "X";
            }
            return data.ItemName + "\n" + "1X";
        }
    }

    protected override void OnStart()
    {
        itemRemove.onClick.AddListener(()=> PlayerInventory.ItemRemove.Invoke(data));
    }

    public override void AlreadyUsing()
    {
        base.AlreadyUsing();
        itemRemove.gameObject.SetActive(true);
    }

    protected override void Use()
    {
        if (data.ItemType != ItemType.food)
        {
            buyButton.gameObject.SetActive(false);
            itemRemove.gameObject.SetActive(true);
            PlayerInventory.WearEvent.Invoke(data);
        }
    }

    private void RemoveButon()
    {
        if (data.ItemType == ItemType.food) return;
            PlayerInventory.WearEvent.Invoke(data);
    }
    
}
