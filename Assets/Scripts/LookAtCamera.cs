using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    [SerializeField] private bool invert;
    
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position - dirToCamera);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
