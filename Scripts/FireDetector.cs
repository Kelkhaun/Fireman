using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FireDetector : MonoBehaviour
{
    private BoxCollider _boxCollider;

    public event Action FireFound;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            FireFound?.Invoke();
            DeactivateCollider();
        }
    }

    private void DeactivateCollider()
    {
        _boxCollider.enabled = false;
    }
}
