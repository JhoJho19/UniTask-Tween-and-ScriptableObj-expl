using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerInfoManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform objWithText;

    void Start()
    {
        objWithText.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        objWithText.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objWithText.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
    }
}