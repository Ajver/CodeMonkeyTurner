using UnityEngine;

public class InitialLoadSystem: MonoBehaviour
{
    private void Awake()
    {
        // We need external loader so it can trigger load
        // in static class, when the Menu is loaded
        MissionsStateStore.Load();
    }
}
