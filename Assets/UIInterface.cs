using UnityEngine;
using UnityEngine.InputSystem;

public class UIInterface : MonoBehaviour
{
    public GameObject tower;
    GameObject focusObj;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit))
            {
                focusObj = Instantiate(tower, hit.point, tower.transform.rotation);
            }
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            
        }
    }
}
