using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using System.Threading;

namespace NonMonobech
{
    public class NextSceneLoader
    {
        private GameProgressManager progressManager;

        public NextSceneLoader(GameProgressManager progressManager)
        {
            this.progressManager = progressManager;
        }

        public async UniTask LoadNextScene(CancellationToken cancellationToken)
        {
            if (progressManager.LvlCount <= SceneManager.sceneCountInBuildSettings)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(progressManager.LvlCount);
                loadOperation.allowSceneActivation = false;

                while (!loadOperation.isDone)
                {
                    if (loadOperation.progress >= 0.9f)
                    {
                        loadOperation.allowSceneActivation = true;
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }
    }
}
