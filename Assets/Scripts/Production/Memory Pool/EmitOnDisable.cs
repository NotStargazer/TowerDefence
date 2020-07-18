using System;
using UnityEngine;

namespace Tools 
{
    public class EmitOnDisable : MonoBehaviour
    {
        public event Action<GameObject> OnDisableGameObject;
        private void OnDisable()
        {
            OnDisableGameObject?.Invoke(gameObject);
        }
    }
}