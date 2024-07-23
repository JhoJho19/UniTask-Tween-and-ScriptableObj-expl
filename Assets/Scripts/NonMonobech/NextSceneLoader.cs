using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using System.Threading;

namespace NonMonobech
{
    public class NextSceneLoader
    {
        public async UniTask LoadNextScene(CancellationToken cancellationToken)
        {
            int nextScene = GameProgressManager.Instance.LvlCount++;

            if (nextScene <= SceneManager.sceneCountInBuildSettings) // если текущая сцена не является последней - загружаем следующую
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextScene);
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
                        GameProgressManager.Instance.LvlCount--;
                        return;
                    }
                }
            }
        }
    }
}
