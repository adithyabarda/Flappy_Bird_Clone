using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject coinPrefab;
    public float spawnRate = 1.5f;
    public float minHeight = -1f;
    public float maxHeight = 2f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        // Spawn pipes
        float randomY = Random.Range(minHeight, maxHeight);
        GameObject pipes = Instantiate(pipePrefab, transform.position + Vector3.up * randomY, Quaternion.identity);

        // Spawn one coin inside the pipe gap
        Transform scoringZone = pipes.transform.Find("Scoring");
        if (scoringZone != null && coinPrefab != null)
        {
            // Slight random offset within gap so they don’t overlap
            float randomYOffset = Random.Range(-0.5f, 0.5f);

            Vector3 coinPosition = scoringZone.position + Vector3.up * randomYOffset;

            GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);

            // Add movement & destroy behavior
            Coin coinScript = coin.AddComponent<Coin>();
            coinScript.speed = pipes.GetComponent<Pipes>().speed;
        }
    }
}
