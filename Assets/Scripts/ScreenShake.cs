using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance { get; private set; }
    
    private CinemachineImpulseSource cinemachineImpulseSource;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one ScreenShake in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
