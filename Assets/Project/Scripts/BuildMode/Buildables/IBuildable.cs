using UnityEngine;

public interface IBuildable
{
    LayerMask AllowedLayers { get; }
    Vector3 AllowedRotationAxes { get; }
    GameObject GetPreviewPrefab();
    GameObject GetFinalPrefab();
    bool TryGetSnapPosition(RaycastHit hit, out Vector3 position, out Quaternion rotation);
    
}