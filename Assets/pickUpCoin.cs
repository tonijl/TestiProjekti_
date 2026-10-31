using UnityEngine;

public class pickUpCoin : MonoBehaviour
{
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		

		if(other.CompareTag("Player") == true)
		{
			ScoreManager.instance.AddScore(1);
			
			CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();
			
			if(spawner != null)
			{
				spawner.SpawnCoin();
			}
			
			
			Destroy(gameObject);
			//Destroy(this);
		}
		
	}


	
	
}
