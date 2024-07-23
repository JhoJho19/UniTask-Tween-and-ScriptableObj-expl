using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using NonMonobech;
using System.Threading;

namespace Gameplay
{
    public class FinishLogic : MonoBehaviour
    {
        public UnityEvent Finish;
        [SerializeField] private ParticleSystem[] particleSystemFirework;
        [SerializeField] AudioSource audioSource;
        private NextSceneLoader _nextSceneLoader;
        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            _nextSceneLoader = new NextSceneLoader();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                BallController ballController = other.GetComponent<BallController>();
                if (ballController != null)
                {
                    foreach (var particle in particleSystemFirework)
                    {
                        particle.Play();
                    }
                    audioSource.Play();
                    Finish.Invoke();
                    ballController.enabled = false;
                    WaitAndLoad(2f, cancellationTokenSource.Token).Forget();
                }
            }
        }

        private async UniTaskVoid WaitAndLoad(float time, CancellationToken cancellationToken)
        {
            await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            await _nextSceneLoader.LoadNextScene(cancellationToken);
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
        }
        private void OnApplicationQuit()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
