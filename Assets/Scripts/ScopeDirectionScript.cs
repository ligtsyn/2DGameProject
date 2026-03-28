using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeDirectionScript : MonoBehaviour
{
    // max scope 5 min scope 2

    private Camera cam;

    private void Start()
    {
        gameObject.SetActive(false);
        cam = Camera.main;
    }

    public void ScopePosition(Vector2 origin, float itemWheight, float maxThrowingForce, float forceMultiplier)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        Vector2 direction = (MouseDir() - origin).normalized;
    
        float distance = (forceMultiplier * maxThrowingForce) / itemWheight;

        Vector2 scopeLocalPos = origin + distance * direction;
        transform.position = (Vector3)scopeLocalPos;
    }

    public void DeActivateScope() 
    { 
        gameObject.SetActive(false);
    }

    public Vector2 MouseDir()
    {
        Vector3 mousePositionInScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mousePositionInScreen);

        Vector2 mouseVector = (Vector2)mouseWorldPosition;

        return mouseVector;
    }
}