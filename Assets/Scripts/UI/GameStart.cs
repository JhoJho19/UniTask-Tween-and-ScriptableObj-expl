using UnityEngine;
using Cysharp.Threading.Tasks;
using NonMonobech;
using Data;
using System.Threading;

namespace UI
{
    public class GameStart : MonoBehaviour
    {
        private NextSceneLoader loader;
        private GameProgressManager progressManager;
        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            progressManager = FindObjectOfType<GameProgressManager>();
            loader = new NextSceneLoader(progressManager);
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartGameButton() // сейчас старт игры срабатывает автоматически,
                                      // но вот метод для случая с наличием стартового меню
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
            progressManager.LvlCount = 1;
            progressManager.SaveProgress();
            await loader.LoadNextScene(cancellationTokenSource.Token);
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
