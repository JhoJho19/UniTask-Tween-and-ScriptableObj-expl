using Data;
using NonMonobech;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AutoStart : MonoBehaviour
{
    NextSceneLoader loader;
    private CancellationTokenSource cancellationTokenSource;

    private void Start()
    {
        loader = new NextSceneLoader();
        cancellationTokenSource = new CancellationTokenSource();
        GameProgressManager.Instance.LoadProgress();

        if (GameProgressManager.Instance.LvlCount > 1)
            GameProgressManager.Instance.LvlCount--; // Метод LoadNextScene делает LvlCount++ для загрузки следующей сцены.
                                                     // Поэтому для корректной загрузки сохраненной сцены, а не следующей за ней, нам необходимо сделать LvlCount-- перед вызовом LoadNextScene().

        LoadNextSceneAsync(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid LoadNextSceneAsync(CancellationToken cancellationToken)
    {
        await loader.LoadNextScene(cancellationToken);
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
