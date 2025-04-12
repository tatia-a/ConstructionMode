using UnityEngine;

public interface IBuildable
{
    GameObject GetPreviewPrefab();
    GameObject GetFinalPrefab();
    bool TryGetSnapPosition(RaycastHit hit, out Vector3 position, out Quaternion rotation);
}