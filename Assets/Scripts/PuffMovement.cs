using UnityEngine;

public class PuffMovement : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 1.5f;
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // move left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // fade out gradually
        timer += Time.deltaTime;
        if (spriteRenderer != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
        }

        // destroy after lifetime
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
