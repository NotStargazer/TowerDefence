using System;
using TMPro;
using UnityEngine;

/// <summary>
/// UIElement, updates player health UI.
/// </summary>
public class UIHealth : MonoBehaviour, IObserver<int>
{
    private TextMeshProUGUI m_Text;

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(int value)
    {
        m_Text.text = $"{value}";
    }

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }
}
