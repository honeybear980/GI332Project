using System;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleTon : MonoBehaviour
{
    
    private static HostSingleTon instance;

    public HostGameManager GameManager { get; private set; }
    public static HostSingleTon Instance
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindFirstObjectByType<HostSingleTon>();

            if (instance == null)
            {
                Debug.LogError("No HostSingleton in the scene!");
                return null;
            }
            return instance;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }

    private void OnDestroy()
    {
        GameManager?.Dispose();
    }
}
