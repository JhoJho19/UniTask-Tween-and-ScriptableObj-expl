using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Services;
using UnityEngine;

public class AutoStart : MonoBehaviour
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

        LoadNextSceneAsync(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid LoadNextSceneAsync(CancellationToken cancellationToken)
    {
        await loader.LoadNextScene(cancellationToken);
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
