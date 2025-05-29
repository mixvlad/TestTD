using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIInterface : MonoBehaviour
{
    public GameObject rocket;
    public GameObject gattling;
    public GameObject flamer;
    
    public GameObject turretMenu;
    GameObject itemPrefab;
    GameObject focusObj;
    private Camera mainCamera;

    public void CreateRocket()
    {
        itemPrefab = rocket;
        CreateItem();
    }
    public void CreateFlamer()
    {
        itemPrefab = flamer;
        CreateItem();
    }

    public void CreateGatling()
    {
        itemPrefab = gattling;
        CreateItem();
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }

    void CreateItem()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit))
        {
            focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
            focusObj.GetComponent<Collider>().enabled = false;
        }
    }

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
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("turret"))
            {
                turretMenu.transform.position = Mouse.current.position.ReadValue();
                turretMenu.SetActive(true);
            }
        }
        else if (focusObj != null && Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit))
            {
                focusObj.transform.position = hit.point;
            }
        }
        else if (focusObj != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("platform") && hit.normal == new Vector3(0, 1, 0))
            {
                hit.collider.gameObject.tag = "occupied";
                focusObj.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, hit.collider.gameObject.transform.localScale.y / 2, 0);
                focusObj.GetComponent<Collider>().enabled = true;
            }
            else
            {
                Destroy(focusObj);
            }

            focusObj = null;
        }
    }
}
