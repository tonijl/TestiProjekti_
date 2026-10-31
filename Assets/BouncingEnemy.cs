using UnityEngine;

public class BouncingEnemy : MonoBehaviour
{
	
	public float moveSpeed = 3f;
	
	private Rigidbody2D rb;
	private Vector2 moveDirection;
	
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		
		//chooses a random starting direction
		
		moveDirection = Random.insideUnitCircle.normalized;
		
		if(moveDirection == Vector2.zero)
		{
			moveDirection = Vector2.right;
		}
		
	}
	
	void FixedUpdate()
	{
		rb.linearVelocity = moveDirection * moveSpeed;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		//Reflect the movement direction off the wall
		
		if(collision.contactCount > 0)
		{
			Vector2 normal = collision.GetContact(0).normal;
			
			moveDirection = Vector2.Reflect(
				moveDirection,
				normal
			).normalized;
		}
		
	}
	
}
