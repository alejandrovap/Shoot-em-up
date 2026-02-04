using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Velocidad de caída de la nave enemiga
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject explosionPrefab; // Prefab de la explosión
    
    // Puntos que vale este enemigo
    [SerializeField]
    int points = 10;

    // Altura a la que se destruirá la nave enemiga si no la matan
    const float DESTROY_HEIGHT = -6f;

    void Update()
    {
        // Movimiento hacia abajo
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destruir la nave enemiga cuando se sale de la pantalla por abajo
        if (transform.position.y < DESTROY_HEIGHT)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shoot")) 
        {
            if (GameManager.GetInstance() != null)
            {
                GameManager.GetInstance().AddScore(points);
            }

            Destroy(other.gameObject);

            DestroyEnemy();
        }
        
        else if (other.CompareTag("Player"))
        {
            if (GameManager.GetInstance() != null)
            {
                GameManager.GetInstance().LoseLife();
            }

            DestroyEnemy();
        }
    }

    // Destruir la nave enemiga y crear una explosión
    void DestroyEnemy()
    {
        // Instanciar la animación de la explosión
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        
        // Destruir la nave enemiga
        Destroy(gameObject);
    }
}