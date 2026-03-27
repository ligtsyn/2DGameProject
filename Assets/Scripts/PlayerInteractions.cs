using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private GameObject scopeObject;
    [SerializeField] private LayerMask itemsLayer;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private float interactionDistance;
    [SerializeField] private float minScopeValue;
    [SerializeField] private float maxScopeValue;

    private Coroutine forceCoroutine;
    private ObjectScript objectScript;
    private PlayerMovement playerMovement;
    private GameObject heldItem;
    private Rigidbody2D itemRb;
    private Vector2 rayDirection;
    private float currentThrowingForce;
    private bool isForceCharging = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        scopeObject.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        rayDirection = playerMovement.GetRayDirection();

        if (heldItem != null) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 
            interactionDistance, itemsLayer);

        Debug.DrawRay(transform.position, interactionDistance * rayDirection, Color.green, 2f);

        if (hit.collider == null) return;
        if (hit.collider.isTrigger)
        {
            PickUpItem(hit.collider.gameObject);
        }
    }

    private void PickUpItem(GameObject itemObject)
    {
        heldItem = itemObject;
        itemRb = itemObject.GetComponent<Rigidbody2D>();

        if (itemRb != null)
            itemRb.simulated = false;

        itemObject.transform.SetParent(itemHolder);
        itemObject.transform.localPosition = Vector2.zero;
        objectScript = itemObject.GetComponent<ObjectScript>();
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (heldItem == null) return;

        if (context.performed)
        {
            ManageForceCoroutine();
        }
        if (context.canceled)
        {
            isForceCharging = false;
            objectScript.ThrowThis(rayDirection, currentThrowingForce);
            heldItem = null;
        }
    }

    private void ManageForceCoroutine()
    {
        if(forceCoroutine != null)
            StopCoroutine(forceCoroutine);
        currentThrowingForce = 0f;
        isForceCharging = true;
        forceCoroutine = StartCoroutine(ForceChargerCoroutine());
    }

    private IEnumerator ForceChargerCoroutine()
    {
        while (isForceCharging) {
            currentThrowingForce += 1 * Time.deltaTime;
            currentThrowingForce = Mathf.Clamp(currentThrowingForce, 0, 1f);
            Debug.Log(currentThrowingForce);
            yield return null;
        }
    }

    private Vector3 MouseDir()
    {
        return Vector3.zero;
    }
}