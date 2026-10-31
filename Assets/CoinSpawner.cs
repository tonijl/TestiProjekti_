using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	
	public GameObject coinPrefab;
	
	public Vector2 minPosition = new Vector2(-8f, -4f);
	public Vector2 maxPosition = new Vector2(8f, 4f);
	
	public void SpawnCoin()
	{
		Vector2 randomPosition = new Vector2(
			Random.Range(minPosition.x, maxPosition.x),
			Random.Range(minPosition.y, maxPosition.y)
			);
			
			Instantiate(coinPrefab, randomPosition, Quaternion.identity);
	}
	

}
