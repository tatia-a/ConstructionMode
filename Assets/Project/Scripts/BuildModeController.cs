using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BuildModeController : MonoBehaviour
{
    private Camera playerCamera;
    private BuildableFactory factory;
    private PlayerInputActions input;

    public LayerMask surfaceMask;
    public float buildDistance = 5f;

    private GameObject previewInstance;
    private IBuildable currentBuildable;
    private Quaternion currentRotation = Quaternion.identity;

    [Inject]
    public void Construct(Camera camera, BuildableFactory factory, PlayerInputActions input)
    {
        this.playerCamera = camera;
        this.factory = factory;
        this.input = input;
    }

    private void OnEnable()
    {
        input.Player.PlaceObject.performed += ctx => TryPlaceObject();
        input.Player.RotateObject.performed += ctx => RotatePreview(ctx.ReadValue<float>());
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterBuildMode(IBuildable buildable)
    {
        if (previewInstance != null) Destroy(previewInstance);
        currentBuildable = buildable;
        previewInstance = factory.Create(buildable);
        currentRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (previewInstance == null || currentBuildable == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, buildDistance, surfaceMask))
        {
            if (currentBuildable.TryGetSnapPosition(hit, out Vector3 pos, out Quaternion rot))
            {
                previewInstance.transform.position = pos;
                previewInstance.transform.rotation = rot * currentRotation;
                SetPreviewColor(true);
            }
            else
            {
                previewInstance.transform.position = hit.point;
                SetPreviewColor(false);
            }
        }
    }

    private void SetPreviewColor(bool valid)
    {
        var renderers = previewInstance.GetComponentsInChildren<Renderer>();
        Color color = valid ? Color.green : Color.red;
        foreach (var r in renderers)
        {
            foreach (var mat in r.materials)
                mat.color = color;
        }
    }

    private void TryPlaceObject()
    {
        if (previewInstance == null || currentBuildable == null) return;

        Vector3 pos = previewInstance.transform.position;
        Quaternion rot = previewInstance.transform.rotation;
        Instantiate(currentBuildable.GetFinalPrefab(), pos, rot);
        Destroy(previewInstance);
        currentBuildable = null;
    }

    private void RotatePreview(float scroll)
    {
        if (Mathf.Abs(scroll) > 0.1f)
        {
            currentRotation *= Quaternion.Euler(0, Mathf.Sign(scroll) * 45f, 0);
        }
    }
}