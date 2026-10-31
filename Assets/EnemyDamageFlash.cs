
using System.Collections;
using UnityEngine;

public class EnemyDamageFlash : MonoBehaviour
{
    [Header("Damage Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer[] sprites;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }
    }

    public void FlashRed()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }


IEnumerator FlashRoutine()
{
    // Flash red while preserving each sprite's transparency.
    foreach (SpriteRenderer sprite in sprites)
    {
        if (sprite != null)
        {
            Color color = flashColor;
            color.a = sprite.color.a;
            sprite.color = color;
        }
    }

    yield return new WaitForSeconds(flashDuration);

    // Restore the original RGB color but preserve current alpha.
    for (int i = 0; i < sprites.Length; i++)
    {
        if (sprites[i] != null)
        {
            Color color = originalColors[i];
            color.a = sprites[i].color.a;
            sprites[i].color = color;
        }
    }

    flashCoroutine = null;
}

}
