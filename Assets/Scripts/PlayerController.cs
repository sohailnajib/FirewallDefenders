using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5f;

    private CharacterController charController;
    private Animator animator;
    private float verticalVelocity = 0f;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleAiming();  
        HandleMovement();
        HandleAnimations();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (animator != null) animator.SetBool("Aiming", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (animator != null) animator.SetBool("Aiming", false);
        }
    }

    void HandleMovement()
    {
        if (charController.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                if (animator != null) animator.SetTrigger("Roll");
            }
        }
        else
        {
            verticalVelocity -= 20f * Time.deltaTime;
        }

        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = verticalVelocity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        charController.Move(movement);
    }

    void HandleAnimations()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float currentSpeed = new Vector3(inputX, 0, inputZ).magnitude * speed;

        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("X", inputX);
            animator.SetFloat("Y", inputZ);
        }
    }
}