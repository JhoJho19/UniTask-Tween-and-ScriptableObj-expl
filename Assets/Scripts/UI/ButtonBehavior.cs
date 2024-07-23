using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    private BallController ballController;
    private DeathZoneLogic deathZoneLogic;
    [SerializeField] GameObject button;

    private void OnEnable()
    {
        ballController = FindObjectOfType<BallController>();
        if (ballController != null)
            ballController.OnMovementStart.AddListener(Hide);

        deathZoneLogic = FindObjectOfType<DeathZoneLogic>();
        if (deathZoneLogic != null)
            deathZoneLogic.OnDeathZone.AddListener(Show);
    }

    private void OnDisable()
    {
        if (ballController != null)
            ballController.OnMovementStart.RemoveListener(Hide);

        if (deathZoneLogic != null)
            deathZoneLogic.OnDeathZone.RemoveListener(Show);
    }

    private void Hide()
    {
        button.SetActive(false);
    }

    private void Show()
    {
        button.SetActive(true);
    }
}
