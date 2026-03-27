using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask itemsLayer;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private float interactionDistance;
    [SerializeField] private float maxThrowingForce;

    private ObjectScript objectScript;
    private ScopeDirectionScript scopeScript;
    private PlayerMovement playerMovement;
    private Coroutine forceCoroutine;
    private GameObject heldItem;
    private Rigidbody2D itemRb;
    private Vector2 rayDirection;
    private float forceMultiplier;
    private bool isForceCharging = false;

    void Start()
    {
        scopeScript = GetComponentInChildren<ScopeDirectionScript>(true);
        playerMovement = GetComponent<PlayerMovement>();
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

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (heldItem == null) return;
        Vector2 mouseDir = (scopeScript.MouseDir() - (Vector2)transform.position).normalized;

        if (context.performed)
        {
            ManageForceCoroutine();
        }
        if (context.canceled)
        {
            isForceCharging = false;
            objectScript.ThrowThis(mouseDir, maxThrowingForce, forceMultiplier);
            heldItem = null;
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

        objectScript = heldItem.GetComponent<ObjectScript>();
    }

    private void ManageForceCoroutine()
    {
        if(forceCoroutine != null)
            StopCoroutine(forceCoroutine);
        forceMultiplier = 0f;
        isForceCharging = true;
        forceCoroutine = StartCoroutine(ForceChargerCoroutine());
    }

    private IEnumerator ForceChargerCoroutine()
    {
        while (isForceCharging) {
            forceMultiplier += 1 * Time.deltaTime;
            forceMultiplier = Mathf.Clamp(forceMultiplier, 0, 1f);

            scopeScript.ScopePosition(gameObject.transform.position, heldItem.GetComponent<ObjectScript>().GetItemWheight(),
                maxThrowingForce,forceMultiplier);

            yield return null;
        }
        scopeScript.DeActivateScope();
    }
}