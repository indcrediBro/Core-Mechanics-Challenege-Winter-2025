using System;
using UnityEngine;

public class DisableVFXOnDestroy : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;

    private void OnEnable()
    {
        trail.emitting = true;
    }

    private void OnDisable()
    {
        trail.emitting = false;
    }
}
