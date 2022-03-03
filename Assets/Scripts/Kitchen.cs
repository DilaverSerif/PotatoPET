using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Kitchen : MonoBehaviour
{
    private Image foodPlace;
    [SerializeField] private Food food;
    [SerializeField] private Button left, right;
    public static UnityEvent GetFoodEvent = new UnityEvent();
    [SerializeField] private Sprite empty;
    
    private void Awake()
    {
        food = FindObjectOfType<Food>();
        foodPlace = food.gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        right.onClick.AddListener(()=>
        {
            foodIndex += 1;
            GetFood();
        });
        
        left.onClick.AddListener(()=>
        {
            foodIndex -= 1;
            GetFood();
        });
    }

    private void OnEnable()
    {
        GetFood();
        GetFoodEvent.AddListener(() =>
        {
            GetFood();
            
        });

    }

    private void OnDisable()
    {
        GetFoodEvent.RemoveListener(() =>
        {
            GetFood();
        });
    }

    public static int foodIndex = 0;
    private void GetFood()
    {
        var getFood = PlayerInventory.Instance.GetFood(foodIndex);
        if(getFood != null)
        {
            food.food = getFood;
            foodPlace.sprite = food.food.ItemSprite;
            food.gameObject.SetActive(true);
        }
        else
        {
            foodPlace.sprite = empty;
            food.food = null;
        }
        
        food.CheckStack();
    }
    
    
}
