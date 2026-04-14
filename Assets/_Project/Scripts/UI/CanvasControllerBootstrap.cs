using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControllerBootstrap : MonoBehaviour
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Button jumpButton;

    private BallController ballController;
    private bool runtimeJumpSubscribed;

    private void Awake()
    {
        ballController = FindFirstObjectByType<BallController>();

        bool hasBallController = ballController != null;
        SetGameplayControlsVisible(hasBallController);

        if (!hasBallController)
        {
            return;
        }

        ballController.SetJoystick(variableJoystick);

        if (jumpButton != null && jumpButton.onClick.GetPersistentEventCount() == 0)
        {
            jumpButton.onClick.AddListener(ballController.Jump);
            runtimeJumpSubscribed = true;
        }
    }

    private void OnDestroy()
    {
        if (!runtimeJumpSubscribed || jumpButton == null || ballController == null)
        {
            return;
        }

        jumpButton.onClick.RemoveListener(ballController.Jump);
    }

    private void SetGameplayControlsVisible(bool visible)
    {
        if (variableJoystick != null)
        {
            variableJoystick.gameObject.SetActive(visible);
        }

        if (jumpButton != null)
        {
            jumpButton.gameObject.SetActive(visible);
        }
    }
}
