using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
	
	[Header("Health")]
	public int maxHealth = 100;
	private int currentHealth;
	private EnemyDamageFlash damageFlash;
	
	[Header("Healt Bar")]
	public Slider healthBar;
	public GameObject healthBarObject;
	public Vector3 healthBarOffset = new Vector3(0, 0.8f, 0);
	
	
	
	void Awake()
	{
		damageFlash = GetComponent<EnemyDamageFlash>();
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
		
		if(healthBarObject != null)
		{
			healthBarObject.SetActive(false);
		}
		
		
    }
	
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		
		
		if(healthBar != null)
		{
			healthBar.value = currentHealth;
		}
		
		if(healthBarObject != null)
		{
			healthBarObject.SetActive(currentHealth < maxHealth);
		}
		
		
		if(damageFlash != null)
		{
			damageFlash.FlashRed();
		}
		
		
		
		//Debug.Log("EnemyHealth: "+currentHealth);
		
		if(currentHealth <= 0)
		{
			Die();
		}
		
	}
	
	private void LateUpdate()
	{
		if(healthBarObject != null)
		{
			healthBarObject.transform.position = transform.position + healthBarOffset;
			
			healthBarObject.transform.rotation = Quaternion.identity;
		}
	}
	
	public void Heal(int amount)
	{
		currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
		
		if (healthBar != null)
		{
			healthBar.value = currentHealth;
		}
		
		if (healthBarObject != null)
		{
			healthBarObject.SetActive(currentHealth < maxHealth);
		}
		
	}
	
	void Die()
	{
		
		EnemyShatterEffect shatter = GetComponent<EnemyShatterEffect>();
		
		if(shatter != null)
		{
			shatter.Shatter();
		}
		else
		{
			Destroy(gameObject);
		}
		
		ScoreManager.instance.AddScore(10); //1
		//Destroy(gameObject);
	}

}
