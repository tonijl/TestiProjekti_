using System.Collections;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    [Header("Spawn Animation")]
    public float spawnDelay = 1f;
    public float animationDuration = 0.5f;
    public float startingScale = 0.2f;

    private SpriteRenderer[] sprites;
    private Vector3[] originalScales;
	
	private MonoBehaviour[] movementScripts;
	private bool[] wasEnabled;
	

    void Awake()
    {
        // Find the enemy's sprites, including child objects.
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalScales = new Vector3[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            originalScales[i] = sprites[i].transform.localScale;

            // Start invisible and small.
            sprites[i].transform.localScale =
                originalScales[i] * startingScale;

            Color color = sprites[i].color;
            color.a = 0f;
            sprites[i].color = color;
        }
		
		//Find enemy movement scripts.
		MonoBehaviour[] allScripts = GetComponentsInChildren<MonoBehaviour>(true);
		
		System.Collections.Generic.List<MonoBehaviour> movementList = new System.Collections.Generic.List<MonoBehaviour>();
		
		foreach (MonoBehaviour script in allScripts)
		{
			if(script is EnemyPathfinding || script is BouncingEnemy)
			{
				movementList.Add(script);
			}
		}
		
		movementScripts = movementList.ToArray();
		wasEnabled = new bool[movementScripts.Length];
		
		//Remember which scripts were enabled, then disable them.
		
		for(int i = 0; i < movementScripts.Length; i++)
		{
			wasEnabled[i] = movementScripts[i].enabled;
			movementScripts[i].enabled = false;
		}
		
		
    }

    IEnumerator Start()
    {
        // Wait before playing the animation.
        yield return new WaitForSeconds(spawnDelay);

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = animationDuration > 0f
                ? Mathf.Clamp01(elapsed / animationDuration)
                : 1f;

            // Smooth animation.
            float smoothT = t * t * (3f - 2f * t);

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null)
                    continue;

                // Grow the visual sprite.
                sprites[i].transform.localScale =
                    Vector3.Lerp(
                        originalScales[i] * startingScale,
                        originalScales[i],
                        smoothT
                    );

                // Fade in the sprite.
                Color color = sprites[i].color;
                color.a = t;
                sprites[i].color = color;
            }

            yield return null;
        }

        // Guarantee the final state is restored.
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] == null)
                continue;

            sprites[i].transform.localScale = originalScales[i];

            Color color = sprites[i].color;
            color.a = 1f;
            sprites[i].color = color;
        }
		
		// Allow movement after spawning finishes
		
		for (int i = 0; i < movementScripts.Length; i++)
		{
			if (movementScripts[i] != null)
			{
				movementScripts[i].enabled = wasEnabled[i];
			}
		}
		
		
    }
}