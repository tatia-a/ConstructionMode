using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public Camera playerCamera;
    public BuildModeController buildModeController;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(playerCamera).AsSingle();
        Container.Bind<BuildModeController>().FromInstance(buildModeController).AsSingle();
        Container.Bind<PlayerInputActions>().AsSingle();
        Container.Bind<BuildableFactory>().AsSingle();
    }
}