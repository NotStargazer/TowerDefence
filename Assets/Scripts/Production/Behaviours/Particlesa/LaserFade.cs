using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserFade : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;
    private float fadeTime;

    LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        fadeTime = Time.time + lifetime;
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
            Destroy(gameObject);
        }
    }
}
