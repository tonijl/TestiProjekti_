using UnityEngine;

public class GunAim : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		Vector2 direction = mousePosition - transform.position;
		
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		
		transform.rotation = Quaternion.Euler(0f, 0f, angle);
		
    }
}
