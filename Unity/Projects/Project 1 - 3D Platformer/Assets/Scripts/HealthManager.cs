using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Scene refs (rebound each scene)")]
    public CharacterController player;

    private bool isRespawning = false;
    public Vector3 respawnPoint;

    [Header("Health")]
    public int maxHealth = 2;
    public int currentHealth;

    private void Awake()
    {
        // Singleton + persist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Only initialize health once
        if (currentHealth <= 0) currentHealth = maxHealth;

        // Try bind in the first scene
        BindPlayerIfNeeded();
        if (player != null && respawnPoint == Vector3.zero)
        {
            respawnPoint = player.transform.position;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // New scene = new player instance (usually)
        BindPlayerIfNeeded();

        // Optional: if you want respawn point per-scene default:
        // if (player != null) respawnPoint = player.transform.position;
    }

    private void BindPlayerIfNeeded()
    {
        if (player != null) return;

        // Find the CharacterController on the object tagged Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<CharacterController>();

            // If this is the first time we ever bound a player, set default respawn
            if (player != null && respawnPoint == Vector3.zero)
            {
                respawnPoint = player.transform.position;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        isRespawning = true;

        BindPlayerIfNeeded();
        if (player == null)
        {
            isRespawning = false;
            yield break;
        }

        player.enabled = false;
        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        // Rebind again in case scene swapped while waiting
        BindPlayerIfNeeded();
        if (player == null)
        {
            isRespawning = false;
            yield break;
        }

        player.transform.position = respawnPoint;

        player.gameObject.SetActive(true);
        player.enabled = true;

        var tpc = player.GetComponent<StarterAssets.ThirdPersonController>();
        if (tpc != null)
        {
            tpc.ResetControllerState();
        }

        currentHealth = maxHealth;
        isRespawning = false;
    }

    public void setRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}
