using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    // Sounds
    public AudioSource wingSound;
    public AudioSource hitSound;

    // Boundaries
    public float upperBound = 5f;
    public float lowerBound = -5f;

    // Start position reset
    private Vector3 startPosition;

    // Smoke Puff Prefabs
    public GameObject smallPuffPrefab;
    public GameObject largePuffPrefab;
    public Transform puffSpawnPoint;

    private float puffTimer;
    public float puffInterval = 0.2f;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    void Start()
    {
        InvokeRepeating("AnimateSprite", 0.15f, 0.15f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
            if (wingSound != null) wingSound.Play();

            SpawnPuff(largePuffPrefab);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
                if (wingSound != null) wingSound.Play();
                SpawnPuff(largePuffPrefab);
            }
        }

        // Apply gravity
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Continuous small smoke trail
        puffTimer += Time.deltaTime;
        if (puffTimer >= puffInterval)
        {
            puffTimer = 0f;
            SpawnPuff(smallPuffPrefab);
        }

        // Clamp plane within screen bounds
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, lowerBound, upperBound);
        transform.position = pos;

        // GameOver if hitting bounds
        if (transform.position.y >= upperBound || transform.position.y <= lowerBound)
        {
            if (hitSound != null) hitSound.Play();
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }

    private void SpawnPuff(GameObject puffPrefab)
    {
        if (puffPrefab != null && puffSpawnPoint != null)
        {
            // spawn slightly behind the plane
            GameObject puff = Instantiate(puffPrefab, puffSpawnPoint.position, Quaternion.identity, transform);

            // detach so it doesn’t move with plane
            puff.transform.SetParent(null);

            // ensure puff moves & fades
            if (puff.GetComponent<PuffMovement>() == null)
                puff.AddComponent<PuffMovement>();
        }
    }

    public void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length) spriteIndex = 0;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (hitSound != null) hitSound.Play();
            FindFirstObjectByType<GameManager>().GameOver();
        }
        else if (other.CompareTag("Scoring"))
        {
            FindFirstObjectByType<GameManager>().IncreaseScore();
        }
        else if (other.CompareTag("Coin"))
        {
            FindFirstObjectByType<GameManager>().AddCoinScore(1);
            Destroy(other.gameObject);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        direction = Vector3.zero;
    }
}
