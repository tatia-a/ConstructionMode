using UnityEngine;
using Zenject;

public class BuildModeController : MonoBehaviour
{
    private Camera playerCamera;
    private BuildableFactory factory;
    private PlayerInputActions input;

    public LayerMask surfaceMask;
    public float buildDistance = 5f;

    private GameObject previewInstance;
    private PreviewObject currentPreview;
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
    }
    
    public void EnterBuildMode(IBuildable buildable)
    {
        if (previewInstance != null) Destroy(previewInstance);
        currentBuildable = buildable;
        previewInstance = factory.Create(buildable); // сюда же добавить, чтобы фабрика выкидывала и ссылку на компонент PreviewObject
        currentPreview = previewInstance.GetComponent<PreviewObject>();
        currentRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (previewInstance == null || currentBuildable == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, buildDistance, surfaceMask))
        {
            if (currentPreview.CanBePlaced && currentBuildable.TryGetSnapPosition(hit, out Vector3 pos, out Quaternion rot))
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

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (currentPreview.CanBePlaced && Physics.Raycast(ray, out RaycastHit hit, buildDistance, surfaceMask))
        {
            if (currentBuildable.TryGetSnapPosition(hit, out Vector3 pos, out Quaternion rot))
            {
                Instantiate(currentBuildable.GetFinalPrefab(), pos, rot * currentRotation);
                Destroy(previewInstance);
                currentBuildable = null;
            }
            else
            {
                Debug.Log("Нельзя разместить объект здесь");
            }
        }
    }


    private void RotatePreview(float scroll)
    {
        if (Mathf.Abs(scroll) > 0.1f && currentBuildable != null)
        {
            Vector3 axes = currentBuildable.AllowedRotationAxes;

            Quaternion delta = Quaternion.identity;
            var angle = 45f * Mathf.Sign(scroll);
            if (axes.x == 1) delta *= Quaternion.Euler(angle, 0, 0);
            if (axes.y == 1) delta *= Quaternion.Euler(0, angle, 0);
            if (axes.z == 1) delta *= Quaternion.Euler(0, 0, angle);

            currentRotation *= delta;
        }
    }

}