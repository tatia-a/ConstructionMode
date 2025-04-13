using UnityEngine;

public abstract class BuildableBase : ScriptableObject, IBuildable
{
    [SerializeField] protected GameObject previewPrefab; 
    [SerializeField] protected GameObject finalPrefab; 
    [SerializeField] protected LayerMask allowedLayers; 

    public abstract Vector3 AllowedRotationAxes { get; }
    public GameObject GetPreviewPrefab() => previewPrefab;
    public GameObject GetFinalPrefab() => finalPrefab;
    public LayerMask AllowedLayers => allowedLayers;

    public bool TryGetSnapPosition(RaycastHit hit, out Vector3 position, out Quaternion rotation)
    {
        if (((1 << hit.collider.gameObject.layer) & allowedLayers) != 0)
        {
            position = hit.point;
            OnSnap(hit, ref position, out rotation);
            return true;
        }

        position = default;
        rotation = default;
        return false;
    }
    

    protected abstract void OnSnap(RaycastHit hit, ref Vector3 position, out Quaternion rotation);
}