using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button cubeButton;
    public Button circleButton;

    [Header("Buildable Prefabs")]
    public BuildableCube buildableCube;
    public BuildableCircle buildableCircle;

    private BuildModeController buildMode;

    [Inject]
    public void Construct(BuildModeController buildMode)
    {
        this.buildMode = buildMode;
    }

    private void Start()
    {
        cubeButton.onClick.AddListener(() => OnBuildButtonClicked(buildableCube));
        circleButton.onClick.AddListener(() => OnBuildButtonClicked(buildableCircle));
    }

    private void OnBuildButtonClicked(IBuildable buildable)
    {
        buildMode.EnterBuildMode(buildable);
    }
}