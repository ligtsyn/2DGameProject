using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float staminaDrain;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegen;

    private Rigidbody2D rigidBody;
    private Vector2 direction;
    private Vector2 rayDirection;
    private float currentSpeed;
    private float stamina;
    private bool isRunning;

    private Coroutine staminaRoutine;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentSpeed = walkingSpeed;
        isRunning = false;
        stamina = maxStamina;
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = direction.normalized * currentSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (direction != Vector2.zero) { rayDirection = direction; }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (stamina == maxStamina && !isRunning)
            {
                isRunning = true;
                StartStaminaRoutineManagment();
            }
        }
        if (context.canceled)
        {
            isRunning = false;
            StartStaminaRoutineManagment();
        }
    }

    private void StartStaminaRoutineManagment()
    {
        if (staminaRoutine != null)
        {
            StopCoroutine(staminaRoutine);
        }
        staminaRoutine = StartCoroutine(StaminaCoroutine());
    }

    private IEnumerator StaminaCoroutine()
    {
        while (isRunning && stamina > 0)
        {
            currentSpeed = sprintSpeed;
            stamina -= staminaDrain * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                isRunning = false;
            }
            yield return null;
        }
        while (!isRunning && stamina < maxStamina)
        {
            currentSpeed = walkingSpeed;
            stamina += staminaRegen * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            yield return null;
        }
        staminaRoutine = null;
    }

    public void PlayerDeath()
    {
        Debug.Log("HA HA Skill issue");
    }

    public Vector2 GetRayDirection()
    {
        if (rayDirection != Vector2.zero)
            return rayDirection;
        else 
            return Vector2.zero;
    }
}