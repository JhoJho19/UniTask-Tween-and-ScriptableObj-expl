using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Services;
using UnityEngine;

namespace UI
{
    public class GameStart : MonoBehaviour
    {
        private NextSceneLoader loader;
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

            loader = new NextSceneLoader(progressService);
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartGameButton()
        {
            RestartTheGame().Forget();
        }

        private async UniTaskVoid StartTheGame()
        {
            await loader.LoadNextScene(cancellationTokenSource.Token);
        }

        public void Restart()
        {
            RestartTheGame().Forget();
        }

        private async UniTaskVoid RestartTheGame()
        {
            progressService.CurrentLevelBuildIndex = 1;
            progressService.SaveProgress();
            await loader.LoadNextScene(cancellationTokenSource.Token);
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
