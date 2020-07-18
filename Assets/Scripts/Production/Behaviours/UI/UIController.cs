using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

/// <summary>
/// Dynamic component, let's you bind IObserver UI Elements.
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> m_UIElements;

    /// <summary>
    /// Searches all attached UIElements for the first instance of type UIElement.
    /// </summary>
    /// <param name="UIElement">The type of UIElement to search for. Element must have IObserver interface.</param>
    public void Bind<T>(ObservableProperty<T> property, Type UIElement)
    {
        Component element = null;
        foreach (RectTransform rt in m_UIElements)
        {
            element = rt.GetComponent(UIElement);
            if ((element != null) && (element is IObserver<T>))
            {
                property.Subscribe((IObserver<T>)element);
                break;
            }
        }
        if (element == null)
        {
            throw new DataMisalignedException($"UIElement is missing IOberserver<{typeof(T)}> interface");
        }
    }
}
