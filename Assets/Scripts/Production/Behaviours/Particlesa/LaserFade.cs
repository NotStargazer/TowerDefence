using UnityEngine;

/// <summary>
/// Creates a fading effect on the laser fired by the FreezeTower.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LaserFade : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;
    private float fadeTime;
    Gradient m_StartColor;

    LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        fadeTime = Time.time + lifetime;
        m_StartColor = line.colorGradient;
    }

    private void OnEnable()
    {
        fadeTime = Time.time + lifetime;
        Gradient grad = m_StartColor;
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[line.colorGradient.alphaKeys.Length];
        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i] = new GradientAlphaKey(1, i);
        }
        grad.SetKeys(line.colorGradient.colorKeys, alphaKeys);
        line.colorGradient = grad;
    }

    void Update()
    {
        Gradient grad = line.colorGradient;
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[line.colorGradient.alphaKeys.Length];
        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i] = new GradientAlphaKey(1 - ((lifetime - (fadeTime - Time.time)) / lifetime), i);
        }
        grad.SetKeys(line.colorGradient.colorKeys, alphaKeys);
        line.colorGradient = grad;

        if (Time.time > fadeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
