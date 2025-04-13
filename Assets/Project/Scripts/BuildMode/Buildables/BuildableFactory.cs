using UnityEngine;
using Zenject;

public class BuildableFactory : IFactory<IBuildable, GameObject>
{
    public GameObject Create(IBuildable buildable)
    {
        return Object.Instantiate(buildable.GetPreviewPrefab());
    }
}