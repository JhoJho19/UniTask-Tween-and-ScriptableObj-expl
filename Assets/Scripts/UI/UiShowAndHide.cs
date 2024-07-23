using UnityEngine;
using DG.Tweening;

public class UiShowAndHide : MonoBehaviour
{
    [SerializeField] RectTransform uiImage;

    private bool isUiImageyisible = false;

    void Start()
    {
        uiImage.localScale = Vector3.zero;
    }

    public void UIShowOrHide()
    {
        if (isUiImageyisible)
        {
            uiImage.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }
        else
        {
            uiImage.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        isUiImageyisible = !isUiImageyisible;
    }
}
