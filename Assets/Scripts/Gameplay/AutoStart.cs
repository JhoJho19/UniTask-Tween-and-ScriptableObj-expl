using Data;
using NonMonobech;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AutoStart : MonoBehaviour
{
    private NextSceneLoader loader;
    private GameProgressManager progressManager;
    private CancellationTokenSource cancellationTokenSource;

    private void Start()
    {
        progressManager = FindObjectOfType<GameProgressManager>();
        loader = new NextSceneLoader(progressManager);
        cancellationTokenSource = new CancellationTokenSource();

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
