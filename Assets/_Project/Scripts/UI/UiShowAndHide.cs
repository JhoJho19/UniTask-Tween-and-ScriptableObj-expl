using UnityEngine;
using DG.Tweening;
using UI;

public class UiShowAndHide : MonoBehaviour
{
    [SerializeField] RectTransform uiImage;

    private bool isUiImageVisible = false;

    void Start()
    {
        uiImage.localScale = Vector3.zero;
    }

    public void UIShowOrHide()
    {
        if (isUiImageVisible)
        {
            uiImage.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }
        else
        {
            uiImage.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            FindObjectOfType<ScoreField>().UpdateCoinCounterText();
        }
        isUiImageVisible = !isUiImageVisible;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
