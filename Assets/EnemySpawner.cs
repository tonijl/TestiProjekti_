using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	
	[Header("Enemy Types")]
	public GameObject ChasingEnemyPrefab;
	public GameObject BouncingEnemyPrefab;
	
	[Range(0f, 1f)]
	public float bouncingEnemyChance = 0.3f;
	
	
	[Header("References")]
	public GameObject enemyPrefab;
	public Transform player;
	public Transform[] spawnPoints;


    [Header("Starting Difficulty")]
    public float startingSpawnInterval = 3f;
    public int startingMaxEnemies = 2;

    [Header("Difficulty Scaling")]
    public int scorePerLevel = 100;
    public float intervalReductionPerLevel = 0.5f;
    public float minimumSpawnInterval = 1f;
    public int extraEnemiesPerLevel = 2;
    public int maximumEnemyLimit = 25;
	
	[Header("Spawn Area")]
	public Vector2 spawnMin = new Vector2(-4f, -4f);
	public Vector2 spawnMax = new Vector2(4f, 4f);
	public float safeDistance = 4f;
	public int maxAttempts = 30;
	
	[Header("Enemy Speed")]
	public float minSpeed = 0.5f;
	public float maxSpeed = 1.5f;
	public float speedIncreasePerLevel = 0.1f;
	
	private int nextSpawnScore = 1;
	private int spawnLevel = 0;

    [Header("Score")]
    public int currentScore = 0;

    private float spawnTimer = 0f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
	
	

    private float CurrentSpawnInterval
    {
        get
        {
            int level = currentScore / scorePerLevel;

            return Mathf.Max(
                minimumSpawnInterval,
                startingSpawnInterval -
                level * intervalReductionPerLevel
            );
        }
    }

    private int CurrentMaxEnemies
    {
        get
        {
            int level = currentScore / scorePerLevel;

            return Mathf.Min(
                maximumEnemyLimit,
                startingMaxEnemies +
                level * extraEnemiesPerLevel
            );
        }
    }

    private void Update()
    {
        // Remove enemies that have been destroyed
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= CurrentSpawnInterval)
        {
            spawnTimer = 0f;

            if (spawnedEnemies.Count < CurrentMaxEnemies)
            {
                SpawnEnemy();
            }
        }
    }



    // Call this whenever the player's score changes
    public void UpdateScore(int newScore)
    {
        currentScore = Mathf.Max(0, newScore);
		Debug.Log("currentScore: "+currentScore.ToString());
		
    }
	
	
	void SpawnEnemy()
	{
		
		Debug.Log("currentScore: "+currentScore.ToString());
		
		if (player == null)
		{
			Debug.LogError("EnemySpawner: Player is not assigned!");
			return;
		}

		if (ChasingEnemyPrefab == null ||
			BouncingEnemyPrefab == null)
		{
			Debug.LogError("EnemySpawner: Assign both enemy prefabs!");
			return;
		}

    // Continue with your existing spawn position code...
		Vector2 spawnPosition = Vector2.zero;
		bool validPosition = false;
		

		Debug.Log("spawning new enemy!");
		

		
		for(int i = 0; i < maxAttempts; i++)
		{
			spawnPosition = new Vector2(
				Random.Range(spawnMin.x, spawnMax.x),
				Random.Range(spawnMin.y, spawnMax.y)
			);
			
			if(Vector2.Distance(spawnPosition, player.position) >= safeDistance)
			{
				validPosition = true;
				break;
			}

		}
		
		if(!validPosition)
		{
			Debug.LogWarning("No safe enemy spawn position found!");
			return;
		}
		
		
		GameObject selectPrefab = Random.value < bouncingEnemyChance
			? BouncingEnemyPrefab
			: ChasingEnemyPrefab;
			
			GameObject enemy = Instantiate(
				selectPrefab,
				spawnPosition,
				Quaternion.identity
			);	
			
			spawnedEnemies.Add(enemy);
			
		//float levelBonus = (spawnLevel - 1) * speedIncreasePerLevel;
		
		int level = currentScore / Mathf.Max(1, scorePerLevel);
		float levelBonus = level * speedIncreasePerLevel;
		

		float speed = Random.Range(minSpeed, maxSpeed) + levelBonus;
		
		
		//EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
		EnemyChase movement = enemy.GetComponent<EnemyChase>();
		
		
		if(movement != null)
		{
			movement.moveSpeed = speed;
		}
		
		BouncingEnemy bouncing = enemy.GetComponent<BouncingEnemy>();
		
		if(bouncing != null)
		{
			bouncing.moveSpeed = speed * 4;
		}
		
		
	}
	
	
	
}




