using UnityEngine;
using Cysharp.Threading.Tasks;
using NonMonobech;
using Data;
using System.Threading;

namespace UI
{
    public class GameStart : MonoBehaviour
    {
        NextSceneLoader loader;
        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            loader = new NextSceneLoader();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTaskVoid StartTheGame()
        {
            GameProgressManager.Instance.LoadProgress();
            GameProgressManager.Instance.LvlCount--; // Метод LoadNextScene делает LvlCount++ для загрузки следующей сцены.
            // Поэтому для корректной загрузки сохраненной сцены, а не следующей за ней, нам необходимо сделать LvlCount-- перед вызовом LoadNextScene().
            await loader.LoadNextScene(cancellationTokenSource.Token);
        }

        public void Restart()
        {
            RestartTheGame().Forget();
        }

        private async UniTaskVoid RestartTheGame()
        {
            GameProgressManager.Instance.LvlCount = 1;
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
