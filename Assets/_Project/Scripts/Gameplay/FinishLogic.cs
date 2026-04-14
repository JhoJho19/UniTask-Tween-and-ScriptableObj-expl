using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class FinishLogic : MonoBehaviour
    {
        public UnityEvent Finish;
        [SerializeField] private ParticleSystem[] particleSystemFirework;
        [SerializeField] AudioSource audioSource;
        private NextSceneLoader nextSceneLoader;
        private PlayerProgressService progressService;
        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            progressService = FindFirstObjectByType<PlayerProgressService>();
            if (progressService == null)
            {
                enabled = false;
                return;
            }

            nextSceneLoader = new NextSceneLoader(progressService);
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
            try
            {
                await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            progressService.CurrentLevelBuildIndex++;
            progressService.SaveProgress();
            await nextSceneLoader.LoadNextScene(cancellationToken);
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
        }
        private void OnApplicationQuit()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}
