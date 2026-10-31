using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunShoot : MonoBehaviour
{
	
	public GameObject bulletPrefab;
	public Transform firePoint;
	public float bulletSpeed = 15f;
	
	public int maxAmmo = 12;
	public float reloadTime = 100f;
	public Slider ammoBar;
	
	private int currentAmmo = 12;
	private float reloadTimer = 0f;
	
	//[Header("Ammo Bar")]
	
	
	
	void Start()
	{
		 currentAmmo = maxAmmo;
		
		if(ammoBar != null)
		{
			ammoBar.maxValue = maxAmmo;
			ammoBar.value = currentAmmo;
		}
		

	}

    // Update is called once per frame
    void Update()
    {
			if(currentAmmo>0){
		
			if(Input.GetMouseButtonDown(0))
			{
				Shoot();
				currentAmmo--;
				ammoBar.value = currentAmmo;
			}
			
			
			
			if(currentAmmo == 0)
			{
				Reload();
			}
			
		}
		
    }
	
	
	void FixedUpdate()
	{


			if(reloadTimer > 0 && currentAmmo == 0)
			{
				ammoBar.value = (1 - (reloadTimer / reloadTime)) * maxAmmo;
			}
			else
			{
				ammoBar.value = currentAmmo;
			}				

			reloadTimer--;
			
			if(reloadTimer == 0)
			{
				currentAmmo = maxAmmo;
			}
		
	}
	
	
	private void Reload()
	{
		currentAmmo = 0;
		reloadTimer = reloadTime;
		Debug.Log("reloadTimer: "+reloadTimer.ToString());
	}
	
	void Shoot()
	{
		
		GameObject bullet = Instantiate(
			bulletPrefab,
			firePoint.position,
			firePoint.rotation
		);
		
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
		
		direction.Normalize();
		
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		rb.linearVelocity = direction * bulletSpeed;
		
		
		
		
	}
	
}
