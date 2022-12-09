using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }
    
    [Header("Settings to adjust")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private AnimationCurve fadeOutCurve;

    [SerializeField] private bool fadeInOnStart = true;
    
    [Header("Don't touch")]
    [SerializeField] private Image background;

    [SerializeField] private GraphicRaycaster raycaster;
    
    private enum FadingDir
    {
        In,
        Out,
    }

    private FadingDir fadingDir;

    private float timer;
    private float currentFadingDuration;

    private string sceneNameToFade;
    
    private void Awake()
    {
        Instance = this;

        if (fadeInOnStart)
        {
            StartFadeIn();
        }
        else
        {
            enabled = false;
        }
    }

    private void StartFadeIn()
    {
        raycaster.enabled = true;
        enabled = true;
        fadingDir = FadingDir.In;
        currentFadingDuration = fadeInDuration;
        timer = currentFadingDuration;
    }

    private void StartFadeOut()
    {
        raycaster.enabled = true;
        enabled = true;
        fadingDir = FadingDir.Out;
        currentFadingDuration = fadeOutDuration;
        timer = currentFadingDuration;
    }

    private void Update()
    {
        AnimationCurve curve;
        
        if (fadingDir == FadingDir.In) 
        {
            curve = fadeInCurve;
        }
        else
        {
            curve = fadeOutCurve;
        }
        
        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0f);
        
        float curveRatioNormalized = (currentFadingDuration - timer) / currentFadingDuration;
        float alpha = curve.Evaluate(curveRatioNormalized);

        Color color = background.color;
        color.a = alpha;
        background.color = color;
        
        if (timer <= 0f)
        {
            enabled = false;
            
            if (fadingDir == FadingDir.In) 
            {
                FadeInCmplete();
            }
            else
            {
                FadeOutComplete();
            }
        }
    }

    private void FadeInCmplete()
    {
        // Disable blocking interactions by the canvas
        raycaster.enabled = false;
    }
    
    private void FadeOutComplete()
    {
        SceneManager.LoadScene(sceneNameToFade);
    }

    public void FadeToScene(string sceneName)
    {
        sceneNameToFade = sceneName;
        StartFadeOut();
    }
}
