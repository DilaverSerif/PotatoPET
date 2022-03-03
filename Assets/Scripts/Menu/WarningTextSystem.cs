using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WarningTextm
{
    public class WarningTextSystem : MonoBehaviour
    {
        private Text warningText;

        private void Awake()
        {
            warningText = transform.GetChild(0).GetComponent<Text>();
        }

        private void OnEnable()
        {
            //MenuSystem.OpenWarningText.AddListener(Open);
        }

        private void OnDisable()
        {
            //MenuSystem.OpenWarningText.RemoveListener(Open);
        }

        private void Open(string context,float time)
        {
            DOTween.Kill("warningText");
            warningText.text = context;
            warningText.color = new Color(1f, 1f, 1f, 0);
            warningText.gameObject.SetActive(true);
            warningText.DOFade(1f, 0.2f).SetId("warningText").SetDelay(0.5f).SetUpdate(true).
                OnComplete(()=>
                    warningText.DOFade(0,2f).SetDelay(time).SetId("warningText").SetUpdate(true)
                    );
        }
    }
    
}
