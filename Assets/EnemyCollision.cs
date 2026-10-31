using UnityEngine;

public class EnemyCollision : MonoBehaviour
{

	public int damage = 25;

	
	//private void OnCollisionEnter2D(Collision2D other)
	private void OnCollisionEnter2D(Collision2D collision)
	{
		
		//PlayerHealth player = collision.GetComponent<PlayerHealth>();
		playerScript player = collision.gameObject.GetComponent<playerScript>();
		
		if(player != null)
		{
			player.TakeDamage(damage);
		}
		
		/*if(collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("enemy colliding with player!");
			FindFirstObjectByType<GameManager>().GameOver();
		}*/
		
	}

}
