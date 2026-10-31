using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage = 25;
	public float lifetime = 3f;

	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
	
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		
		EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
		
		if(enemy != null)
		{
			//Debug.Log("Bullet collided with enemy!");
			enemy.TakeDamage(damage);
			Destroy(gameObject);
		}

		
	}


}
