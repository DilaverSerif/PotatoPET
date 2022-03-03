using UnityEngine;

[CreateAssetMenu(fileName = "Clothes", menuName = "Items", order = 1)]
public  class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [SerializeField] private ItemType ıtemType;
    public ItemType ItemType => ıtemType;

    [SerializeField] private Sprite itemSprite;
    public Sprite ItemSprite => itemSprite;

    [SerializeField] private int money;
    public int Money => money;
    
    [SerializeField] private bool stackable;
    public bool Stackable => stackable;

    [SerializeField]private int stack;
    public int value;
    
    public int Stack
    {
        set
        {
            stack += value;
        }
        get
        {
            return stack;
        }
    }

}

public enum ItemType
{
    glasses,
    hat,
    clothe,
    food
}