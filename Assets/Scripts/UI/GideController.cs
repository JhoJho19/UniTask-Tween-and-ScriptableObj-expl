using Gameplay;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GideController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI howToMoveText;
    [SerializeField] TextMeshProUGUI howToJumpText;
    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();

        howToMoveText.transform.localScale = Vector3.zero;
        howToMoveText.alpha = 0;
        howToJumpText.transform.localScale = Vector3.zero;
        howToJumpText.alpha = 0;

        howToMoveText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        howToMoveText.DOFade(1, 0.5f).SetEase(Ease.OutBack);
        howToJumpText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        howToJumpText.DOFade(1, 0.5f).SetEase(Ease.OutBack);

        BallController ballController = FindObjectOfType<BallController>();
        if (ballController != null)
            ballController.OnMovementStart.AddListener(GideOff);
    }

    private void GideOff()
    {
        HideGuideAsync(cts.Token).Forget();
    }

    private async UniTaskVoid HideGuideAsync(CancellationToken token)
    {
        await UniTask.Delay(1000, cancellationToken: token);

        howToMoveText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        howToMoveText.DOFade(0, 0.5f).SetEase(Ease.InBack);
        howToJumpText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        howToJumpText.DOFade(0, 0.5f).SetEase(Ease.InBack);

        await UniTask.Delay(500, cancellationToken: token);

        cts?.Cancel();
        cts?.Dispose();
    }
}
