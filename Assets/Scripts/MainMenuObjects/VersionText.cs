using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    void Start()
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        text.text = "Version " + Application.version;
    }
    
}
