using UnityEngine;
using UnityEngine.UI;

public class SleepButton : MonoBehaviour
{
    private Button sleepButton;
    private void Awake()
    {
        sleepButton = GetComponent<Button>();
    }

    private void Start()
    {
        sleepButton.onClick.AddListener(
            
            ()=> Player.SleepEvent.Invoke()
        );
    }
}
