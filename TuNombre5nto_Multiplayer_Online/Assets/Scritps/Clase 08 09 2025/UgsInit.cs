using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using Unity.Services.Core;
using Unity.Services.Authentication;
public class UgsInit : MonoBehaviour
{
    static readonly SemaphoreSlim Gate = new SemaphoreSlim(1, 1);
    static Task initTask;
    static bool loggedOnce;
    async void Awake()
    {
        await InitAsync();
    }
    public static async Task InitAsync()
    {
        // Fast path: already ready
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn)
        {
            LogOnce("[UGS] Already signed in :" + AuthenticationService.Instance.PlayerId);
            return;
        }

        //someone else is already initializing? wait for it 
        if (initTask != null)
        {
            await initTask;
            LogOnce("[Ugs] Signed in:" + AuthenticationService.Instance.PlayerId);
            return;
        }

        await Gate.WaitAsync();
        try
        {
            if (initTask == null)
                initTask = InitInnerAsync();
        }
        finally { Gate.Release(); }
        await initTask;
        LogOnce("[UGS] SIGNED IN :" + AuthenticationService.Instance.PlayerId);
    }
    private static async Task InitInnerAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            if (AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
                catch
                {
                    //if another instance us racing us , swallow and let the other one finish.
                    await Task.Yield();
                }
            }
        }
    }
    private static void LogOnce(string msg)
    {
        if (loggedOnce) return;
        loggedOnce = true;
        Debug.Log(msg);
    }
}
