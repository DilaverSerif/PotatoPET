using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    private Image warningPanel;
    private TextMeshProUGUI warningText;

    private string content;
    private Button closeButton;

    private void Awake()
    {
        warningPanel = transform.Find("WarningPanel").GetComponent<Image>();
        warningText = warningPanel.GetComponentInChildren<TextMeshProUGUI>();
        closeButton = warningPanel.GetComponentInChildren<Button>();

        

        closeButton.onClick.AddListener(CloseButton);    
    }

    private void OnEnable()
    {
        MenuSystem.OpenWarning.AddListener(OpenWarning);
    }

    private void OnDisable()
    {
        MenuSystem.OpenWarning.RemoveListener(OpenWarning);
    }

    private bool showing;
    private void OpenWarning(string context)
    {
        if (showing)
        {
            return;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        showing = true;
        warningText.text = context;
        warningPanel.gameObject.SetActive(true);
        warningPanel.color = new Color(1, 1, 1, 0);
        warningPanel.DOFade(1, 0.2F).SetUpdate(true);
    }

    private void CloseButton()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        showing = false;
        warningPanel.gameObject.SetActive(false);
    }
}



