using UnityEngine;

[CreateAssetMenu(menuName = "Buildables/BuildableCircle")]
public class BuildableCircle : BuildableBase
{
    public override Vector3 AllowedRotationAxes => new Vector3(0, 0, 1);

    protected override void OnSnap(RaycastHit hit, ref Vector3 position, out Quaternion rotation)
    {
        rotation = Quaternion.LookRotation(-hit.normal);
    }
}