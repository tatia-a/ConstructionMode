using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    
    private Camera playerCamera;
    private CharacterController controller;
    private PlayerInputActions input;
    
    private float cameraPitch;
    
    // inputs
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool canRotate = true;
    
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
        
        input.Player.UnlockCursor.started += ctx => canRotate = false; 
        input.Player.UnlockCursor.canceled += ctx => canRotate = true; 
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        Move();
        if(canRotate) Rotate();
    }
    private void Move()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.SimpleMove(move * settings.moveSpeed);
    }
    private void Rotate()
    {
        // Горизонтальный поворот
        float yawRotation = lookInput.x * settings.lookSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, yawRotation);
        
        // Вертикальный поворот камеры
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y * settings.lookSpeed * Time.deltaTime,
            settings.minCameraPitch, settings.maxCameraPitch);
        playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }
}