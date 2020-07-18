using Tools;
using UnityEngine;

[RequireComponent(typeof(UIController))]
public class Player : MonoBehaviour
{
    UIController m_UIController;

    [SerializeField] private int m_InitHealth;
    public ObservableProperty<int> Health { get; } = new ObservableProperty<int>();
    public ObservableProperty<string> Name { get; } = new ObservableProperty<string>();
    private void Awake()
    {
        m_UIController = GetComponent<UIController>();
        Health.Value = m_InitHealth;
        m_UIController.Bind(Health, typeof(UIHealth));
    } 

    public void ReduceHealth(int amount)
    {
        Health.Value -= amount;
    }
}