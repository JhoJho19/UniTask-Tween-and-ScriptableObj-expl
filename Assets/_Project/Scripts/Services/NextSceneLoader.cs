using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public class NextSceneLoader
    {
        private readonly PlayerProgressService progressService;

        public NextSceneLoader(PlayerProgressService progressService)
        {
            this.progressService = progressService;
        }

        public async UniTask LoadNextScene(CancellationToken cancellationToken)
        {
            if (progressService.CurrentLevelBuildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                return;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(progressService.CurrentLevelBuildIndex);
            loadOperation.allowSceneActivation = false;

            while (!loadOperation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                loadOperation.allowSceneActivation = loadOperation.progress >= 0.9f;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}
