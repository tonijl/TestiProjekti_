using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerScript : MonoBehaviour
{

	[Header("Health")]
	public int maxHealth = 3;
	private int currentHealth;
	public Slider healthBar;
	
	[Header("Invincibility Frames")]
	public float invincibilityDuration = 1f;
	private bool isInvincible = false;
	
	[Header("Flashing Effect")]
	public float flashInterval = 0.1f;
	private SpriteRenderer spriteRenderer;


	public float movementSpeed = 5f;

	private Rigidbody2D rb;
	private Vector2 movement;
	
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
		
		if(healthBar != null)
		{
			healthBar.maxValue = maxHealth;
			healthBar.value = currentHealth;
		}
		
		
		spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	public void TakeDamage(int damage)
	{
		if(isInvincible)
		{
			return;
		}
		
		currentHealth -= damage;
		Debug.Log("Player health: "+ currentHealth);
		
		if(healthBar != null)
		{
			healthBar.value = currentHealth;
		}
		
		
		if(currentHealth <= 0)
		{
			Die();
			return;
		}
		
		StartCoroutine(InvincibilityFrames());
	}
	
	private IEnumerator InvincibilityFrames()
	{
		isInvincible = true;
		
		float elapsed = 0f;
		
		while (elapsed < invincibilityDuration)
		{
			if(spriteRenderer != null)
			{
				spriteRenderer.enabled = !spriteRenderer.enabled;
			}
				
			yield return new WaitForSeconds(flashInterval);
			elapsed += flashInterval;
		}
		
		if(spriteRenderer!= null)
		{
			spriteRenderer.enabled = true;
		}
		
		isInvincible = false;
		
	}
		
	private void Die()
	{
		FindFirstObjectByType<GameManager>().GameOver();	
	}
	
	

    // Update is called once per frame
    void Update()
    {

	//float horizontal = 0f;
	//float vertical = 0f;



	
	/*if(Input.GetKey(KeyCode.LeftArrow)){ horizontal = -1f;}
	if(Input.GetKey(KeyCode.RightArrow)){ horizontal = 1f;}
	if(Input.GetKey(KeyCode.UpArrow)){ vertical = 1f;}
	if(Input.GetKey(KeyCode.DownArrow)){ vertical = -1f;}
	


	Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;

	transform.position += movement * movementSpeed * Time.deltaTime;*/

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
		
        FixedUpdate();
    }
	
	void FixedUpdate()
	{
		rb.linearVelocity = movement * movementSpeed;
	}
	
	
}
