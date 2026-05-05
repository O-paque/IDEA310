using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Scene refs (rebound each scene)")]
    public CharacterController player;

    [Header("UI")]
    [SerializeField] private GameOverMenu gameOverMenu;

    private bool isRespawning = false;
    private bool isGameOver = false;

    public Vector3 respawnPoint;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        if (currentHealth <= 0)
            currentHealth = maxHealth;

        BindPlayerIfNeeded();

        if (player != null && respawnPoint == Vector3.zero)
            respawnPoint = player.transform.position;

        BindGameOverUIIfNeeded();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = null;
        BindPlayerIfNeeded();
        BindGameOverUIIfNeeded();

        isGameOver = false;
    }

    private void BindPlayerIfNeeded()
    {
        if (player != null)
            return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<CharacterController>();

            if (player != null && respawnPoint == Vector3.zero)
                respawnPoint = player.transform.position;
        }
    }

    private void BindGameOverUIIfNeeded()
    {
        if (gameOverMenu != null)
            return;

        gameOverMenu = FindFirstObjectByType<GameOverMenu>();
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver)
            return;

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        GameManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isGameOver = true;

            if (gameOverMenu != null)
            {
                gameOverMenu.ShowGameOver();
            }
            else
            {
                Debug.LogWarning("GameOverMenu not found in scene.");
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        if (isGameOver)
            return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        GameManager.Instance.UpdateHealth(currentHealth, maxHealth);
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

        BindPlayerIfNeeded();
        if (player == null)
        {
            isRespawning = false;
            yield break;
        }

        player.transform.position = respawnPoint;

        player.gameObject.SetActive(true);
        player.enabled = true;

        currentHealth = maxHealth;
        isRespawning = false;
    }

    public void setRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void ResetAfterGameOver()
    {
        isGameOver = false;
        isRespawning = false;
        currentHealth = maxHealth;

        if (GameManager.Instance != null)
            GameManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }
}