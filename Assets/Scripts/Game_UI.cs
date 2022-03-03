using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI : MonoBehaviour
{
    [SerializeField] private Button bedroomButton, kitchenButton, livingroomButton, bathroomButton;
    [SerializeField] private Button shopButton, inventryButton;
    [SerializeField] private Transform kitchenPanel, bedroomPanel, livingPanel, bathPanel;
    [SerializeField] private Transform inventoryPanel, shopPanel;
    private Transform player;
    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        
        switch (player.transform.position.x)
        {
            case -50:
                ChangeRoom(rooms.bath);
                break;
            case -25:
                ChangeRoom(rooms.kitchen);
                break;
            case 0:
                ChangeRoom(rooms.bed);
                break;
            case 25:
                ChangeRoom(rooms.living);
                break;
                
        }
    }

    private void OnEnable()
    {
        bedroomButton.onClick.AddListener(()=> ChangeRoom(rooms.bed));
        kitchenButton.onClick.AddListener(()=> ChangeRoom(rooms.kitchen));
        livingroomButton.onClick.AddListener(()=> ChangeRoom(rooms.living));
        bathroomButton.onClick.AddListener(()=> ChangeRoom(rooms.bath));
        
        inventryButton.onClick.AddListener(()=> inventoryPanel.gameObject.SetActive(true));
        shopButton.onClick.AddListener(()=> shopPanel.gameObject.SetActive(true));
    }

    private void OnDisable()
    {
        bedroomButton.onClick.RemoveListener(()=> ChangeRoom(rooms.bed));
        kitchenButton.onClick.RemoveListener(()=> ChangeRoom(rooms.kitchen));
        livingroomButton.onClick.RemoveListener(()=> ChangeRoom(rooms.living));
        bathroomButton.onClick.RemoveListener(()=> ChangeRoom(rooms.bath));
    }

    enum rooms
    {
        bath,
        kitchen,
        living,
        bed
    }

    private void ChangeRoom(rooms room)
    {
        if (Player.Instance.isSleep)
        {
            MenuSystem.OpenWarning.Invoke("POTATO IS SLEEPING!");
            return;
        }
        
        bedroomPanel.gameObject.SetActive(false);
        kitchenPanel.gameObject.SetActive(false);
        livingPanel.gameObject.SetActive(false);
        bathPanel.gameObject.SetActive(false);
        bathroomButton.interactable = true;
        kitchenButton.interactable = true;
        livingroomButton.interactable = true;
        bedroomButton.interactable = true;

        inventryButton.gameObject.SetActive(true);
        DOTween.Kill("camera");

        int x = 0;
        
        switch (room)
        {
            case rooms.bath:
                inventryButton.gameObject.SetActive(false);
                bathPanel.gameObject.SetActive(true);
                x = -50;
                bathroomButton.interactable = false;
                break;
            case rooms.kitchen:
                kitchenPanel.gameObject.SetActive(true);
                x = -25;
                kitchenButton.interactable = false;
                break;
            case rooms.living:
                livingPanel.gameObject.SetActive(true);
                x = 25;
                livingroomButton.interactable = false;
                break;
            case rooms.bed:
                bedroomPanel.gameObject.SetActive(true);
                x = 0;
                bedroomButton.interactable = false;

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(room), room, null);
        }
        
        if(player.transform.position.x == x) return;
        Camera.main.transform.DOMoveX(x, 0.25F).SetId("camera");
        GameBase.Dilaver.SoundSystem.PlaySound(Sounds.move);
        player.DOMoveX(x, 0.30F).SetId("camera");
    }
}
