using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public Camera playerCamera;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(playerCamera).AsSingle();
        Container.Bind<PlayerInputActions>().AsSingle();
        Container.Bind<BuildableFactory>().AsSingle();
    }
}