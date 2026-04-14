using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Gameplay
{
    public class BallController : MonoBehaviour
    {
        public UnityEvent OnMovementStart;
        [SerializeField] private Rigidbody ballRigidbody;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private AudioSource jumpSound;
        private VariableJoystick _joystick;
        private bool _isMoving;
        private bool _isOnGround;
        private float _moveHorizontal;
        private float _moveVertical;
        private bool _shouldJump;
        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            var finishLogic = FindFirstObjectByType<FinishLogic>();
            if (finishLogic != null)
            {
                finishLogic.Finish.AddListener(StopTheBall);
            }

            _cancellationTokenSource = new CancellationTokenSource();
            UpdateLoop(_cancellationTokenSource.Token).Forget();
            FixedUpdateLoop(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (this == null || ballRigidbody == null) break;

                    _moveHorizontal = GetMovementAxis(Input.GetAxis("Horizontal"), _joystick != null ? _joystick.Horizontal : 0f);
                    _moveVertical = GetMovementAxis(Input.GetAxis("Vertical"), _joystick != null ? _joystick.Vertical : 0f);

                    if (!_isMoving && (_moveHorizontal != 0 || _moveVertical != 0))
                    {
                        _isMoving = true;
                        OnMovementStart.Invoke();
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _shouldJump = true;
                    }

                    await UniTask.Yield();
                }
            }
            catch (MissingReferenceException)
            {
                Debug.Log("мячик уничтожили вместе со сценой(");
            }
        }

        private async UniTaskVoid FixedUpdateLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (this == null || ballRigidbody == null) break;

                    _isOnGround = Physics.Raycast(transform.position, Vector3.down, 0.5f);

                    Vector3 movement = new Vector3(_moveHorizontal, 0.0f, _moveVertical);
                    ballRigidbody.AddForce(movement * moveSpeed);

                    if (_shouldJump && _isOnGround)
                    {
                        JumpLogic();
                        _shouldJump = false;
                    }

                    await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
                }
            }
            catch (MissingReferenceException)
            {
                Debug.Log("мячик уничтожили вместе со сценой(");
            }
        }

        public void Jump()
        {
            _shouldJump = true;
        }

        public void SetJoystick(VariableJoystick joystick)
        {
            _joystick = joystick;
        }

        public void JumpLogic()
        {
            jumpSound.Play();
            ballRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private float GetMovementAxis(float keyboardAxis, float joystickAxis)
        {
            return Mathf.Abs(joystickAxis) > 0.01f ? joystickAxis : keyboardAxis;
        }

        private void StopTheBall()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void OnDestroy()
        {
            StopTheBall();
        }

        private void OnApplicationQuit()
        {
            StopTheBall();
        }
    }
}
