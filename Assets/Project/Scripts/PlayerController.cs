using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    
    private Camera playerCamera;
    private CharacterController controller;
    private PlayerInputActions input;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch;
    
    [Inject]
    public void Construct(PlayerInputActions input, Camera camera)
    {
        this.input = input;
        this.playerCamera = camera;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * lookInput.x * settings.lookSpeed);
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y * settings.lookSpeed, settings.minCameraPitch,
            settings.maxCameraPitch);
        playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }

    private void Move()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.SimpleMove(move * settings.moveSpeed);
    }
}