using UnityEngine;

[CreateAssetMenu(menuName = "Buildables/BuildableCube")]
public class BuildableCube : BuildableBase
{
    public override Vector3 AllowedRotationAxes => new Vector3(0, 1, 0);

    protected override void OnSnap(RaycastHit hit, ref Vector3 position, out Quaternion rotation)
    {
        position.y = Mathf.Round(position.y);
        rotation = Quaternion.identity;
    }
}