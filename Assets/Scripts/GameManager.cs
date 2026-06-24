using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int killsPerSpin = 10;
    public GameObject racketPrefab;
    public float racketDuration = 5f;

    private int killCount = 0;
    private int spinsAvailable = 0;
    private bool racketActive = false;
    private GameObject racketInstance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (racketPrefab != null)
        {
            racketInstance = Instantiate(racketPrefab);
            racketInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (racketActive && racketInstance != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            racketInstance.transform.position = mousePos;
        }
    }

    public void AddKill()
    {
        killCount++;
        if (killCount % killsPerSpin == 0)
            spinsAvailable++;
    }

    public bool CanSpin()
    {
        return spinsAvailable > 0 && !racketActive;
    }

    public void UseSpin()
    {
        if (spinsAvailable > 0)
        {
            spinsAvailable--;
            killCount = 0;
        }
    }

    public int GetSpinsAvailable()
    {
        return spinsAvailable;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public int GetKillsUntilSpin()
    {
        if (spinsAvailable > 0)
            return 0;

        return killsPerSpin - (killCount % killsPerSpin);
    }

    public void ActivateRacket()
    {
        if (racketInstance == null) return;
        racketActive = true;
        racketInstance.SetActive(true);
        Cursor.visible = false;
        Invoke(nameof(DeactivateRacket), racketDuration);
    }

    void DeactivateRacket()
    {
        racketActive = false;
        if (racketInstance != null)
            racketInstance.SetActive(false);
        Cursor.visible = true;
    }

    public bool IsRacketActive()
    {
        return racketActive;
    }
}
