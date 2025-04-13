using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PreviewObject : MonoBehaviour
{
    [SerializeField] private LayerMask checkLayers;

    public bool CanBePlaced { get; private set; } = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (IsLayerChecked(other.gameObject.layer))
        {
            CanBePlaced = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsLayerChecked(other.gameObject.layer))
        {
            CanBePlaced = true;
        }
    }

    private bool IsLayerChecked(int layer)
    {
        return (checkLayers.value & (1 << layer)) != 0;
    }
}