using UnityEngine;
//using Unity.AI;
//using Unity.AI.Navigation;

public class EnemyChase : MonoBehaviour
{
	
	public float moveSpeed = 2f;
	
	private Transform player;
	private Rigidbody2D rb;
	
	//private UnityEngine.AI.NavMeshAgent agent;
	
	/*void Awake()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}*/
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
		rb = GetComponent<Rigidbody2D>();
		
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		
		if(playerObject != null)
		{
			player = playerObject.transform;
		}
		else
		{
			Debug.LogWarning("Player with tag 'Player' not found!");
		}			
		
    }

	void FixedUpdate()
	{
		if(player == null) return;
		
		Vector2 direction = ((Vector2)player.position - rb.position).normalized;
		
		rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
		
	}
	
	/*
	void Update()
	{
		if(player == null)
		{
			return;
		}
		
		float distance = Vector3.Distance(
			transform.position,
			player.position
		);

			if(distance < 10f)
			{
				agent.isStopped = false;
				agent.SetDestination(player.position);
			}else{
				agent.isStopped = true;
			}
		
		
		agent.SetDestination(player.position);
	}*/



}
