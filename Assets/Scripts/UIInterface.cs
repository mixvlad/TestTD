using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIInterface : MonoBehaviour
{
    public Shoot rocket;
    public Shoot gattling;
    public Shoot flamer;
    public TMP_Text waveText;
    public TMP_Text moneyText;
    public TMP_Text healthText;
    public Button pauseButton;
    public Button normalSpeedButton;
    public Button fastSpeedButton;
    public Button veryFastSpeedButton;
    public AudioSource wrongSound;
    public TMP_Text upgradeButtonText;

    public GameObject turretMenu;
    GameObject itemPrefab;
    GameObject focusObj;
    private Camera mainCamera;
    private Shoot currentClickedOnTurret;


    public void CreateRocket()
    {
        if (LevelManager.totalMoney >= rocket.turretData.price)
        {
            LevelManager.totalMoney -= rocket.turretData.price;
            itemPrefab = rocket.gameObject;
            CreateItem();
        }
        else
        {
            wrongSound.Play();
        }
    }
    public void CreateFlamer()
    {
        if (LevelManager.totalMoney >= flamer.turretData.price)
        {
            LevelManager.totalMoney -= flamer.turretData.price;
            itemPrefab = flamer.gameObject;
            CreateItem();
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateGatling()
    {
        if (LevelManager.totalMoney >= gattling.turretData.price)
        {
            LevelManager.totalMoney -= gattling.turretData.price;
            itemPrefab = gattling.gameObject;
            CreateItem();
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
        currentClickedOnTurret = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        Time.timeScale = 1;
        LevelManager.levelEnded = false;
        LevelManager.playerHealth = LevelManager.playerMaxHealth;
        LevelManager.totalMoney = 200;
        LevelManager.wavesEmitted = 0;
        LevelManager.numberOfWaves = 3;
        LevelManager.totalEnemies = 0;
        LevelManager.nextWaveReady = false;
        Time.timeScale = 1;
    }

    public void UpgradeTurret()
    {
        if (LevelManager.totalMoney >= currentClickedOnTurret.turretData.price)
        {
            LevelManager.totalMoney -= currentClickedOnTurret.turretData.price;
            currentClickedOnTurret.turretData.damage *= 2;
            currentClickedOnTurret.turretData.price *= 2;
            upgradeButtonText.text = "Upgrade " + currentClickedOnTurret.turretData.price + "$";
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void SellTurret()
    {
        LevelManager.totalMoney += currentClickedOnTurret.turretData.price / 2;
        Destroy(currentClickedOnTurret.gameObject, 0.1f);
        CloseTurretMenu();
    }

    void CreateItem()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit))
        {
            focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        pauseButton.onClick.AddListener(() => LevelManager.OnSpeedChanged(0));
        normalSpeedButton.onClick.AddListener(() => LevelManager.OnSpeedChanged(1));
        fastSpeedButton.onClick.AddListener(() => LevelManager.OnSpeedChanged(5));
        veryFastSpeedButton.onClick.AddListener(() => LevelManager.OnSpeedChanged(10));
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.wavesEmitted < LevelManager.numberOfWaves)
        {
            waveText.text = "🌊 " + (LevelManager.wavesEmitted + 1) + " / " + LevelManager.numberOfWaves;
        }
        moneyText.text = LevelManager.totalMoney + "$";
        if (LevelManager.playerHealth <= 0)
        {
            healthText.text = "💀";
        }
        else
        {
            healthText.text = "❤️ " + LevelManager.playerHealth + " / " + LevelManager.playerMaxHealth;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("turret"))
            {
                currentClickedOnTurret = hit.collider.gameObject.GetComponent<Shoot>();
                turretMenu.transform.position = Mouse.current.position.ReadValue();
                upgradeButtonText.text = "Upgrade " + currentClickedOnTurret.turretData.price + "$";
                turretMenu.SetActive(true);
            }
        }
        else if (focusObj != null && Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 9)))
            {
                focusObj.transform.position = hit.point;
            }
        }
        else if (focusObj != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 9)) && hit.collider.gameObject.CompareTag("platform") && hit.normal == new Vector3(0, 1, 0))
            {
                hit.collider.gameObject.tag = "occupied";
                focusObj.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, hit.collider.gameObject.transform.localScale.y, 0);
                focusObj.GetComponent<BoxCollider>().enabled = true;
                focusObj.GetComponent<SphereCollider>().enabled = true;
                focusObj.GetComponent<Shoot>().enabled = true;
            }
            else
            {
                Destroy(focusObj);
                LevelManager.totalMoney += focusObj.GetComponent<Shoot>().turretData.price;
            }

            focusObj = null;
        }
    }
}
