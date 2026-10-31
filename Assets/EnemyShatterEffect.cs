
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShatterEffect : MonoBehaviour
{
    [Header("Shatter Settings")]
    [Range(2, 12)]
    public int columns = 6;

    [Range(2, 12)]
    public int rows = 6;

    public float explosionForce = 3f;
    public float upwardForce = 0.5f;
    public float rotationSpeed = 360f;
    public float fragmentLifetime = 0.7f;

    [Header("References")]
    public SpriteRenderer targetSprite;

    private bool hasShattered;

    public void Shatter()
    {
        if (hasShattered)
            return;

        hasShattered = true;

        if (targetSprite == null)
            targetSprite = GetComponentInChildren<SpriteRenderer>();

        if (targetSprite == null || targetSprite.sprite == null)
        {
            Destroy(gameObject);
            return;
        }

        SpriteRenderer source = targetSprite;
        Sprite original = source.sprite;
        Texture2D texture = original.texture;
        Rect textureRect = original.textureRect;

        Vector2 boundsMin = original.bounds.min;
        float ppu = original.pixelsPerUnit;

        // Create a separate object for the fragments.
        GameObject fragmentParent = new GameObject("ShatteredFragments");
        fragmentParent.transform.position = source.transform.position;
        fragmentParent.transform.rotation = source.transform.rotation;
        fragmentParent.transform.localScale = source.transform.lossyScale;

        int sortingOrder = source.sortingOrder;
        string sortingLayer = source.sortingLayerName;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                float x0 = (float)x / columns;
                float x1 = (float)(x + 1) / columns;
                float y0 = (float)y / rows;
                float y1 = (float)(y + 1) / rows;

                Rect pieceRect = new Rect(
                    textureRect.x + x0 * textureRect.width,
                    textureRect.y + y0 * textureRect.height,
                    textureRect.width / columns,
                    textureRect.height / rows
                );

                Sprite piece = Sprite.Create(
                    texture,
                    pieceRect,
                    new Vector2(0.5f, 0.5f),
                    ppu
                );

                GameObject fragment = new GameObject("Fragment");
                fragment.transform.SetParent(
                    fragmentParent.transform,
                    false
                );

                fragment.transform.localPosition = new Vector3(
                    boundsMin.x + (x0 + x1) * 0.5f * textureRect.width / ppu,
                    boundsMin.y + (y0 + y1) * 0.5f * textureRect.height / ppu,
                    0f
                );

                SpriteRenderer renderer =
                    fragment.AddComponent<SpriteRenderer>();

                renderer.sprite = piece;
                renderer.color = source.color;
                renderer.sortingLayerName = sortingLayer;
                renderer.sortingOrder = sortingOrder;

                // Random outward direction.
                Vector2 direction = (
                    (Vector2)fragment.transform.position -
                    (Vector2)source.transform.position
                ).normalized;

                direction += Random.insideUnitCircle * 0.35f;
                direction.y += upwardForce;

                ShatterFragment motion =
                    fragment.AddComponent<ShatterFragment>();

                motion.Launch(
                    direction.normalized * explosionForce,
                    Random.Range(-rotationSpeed, rotationSpeed),
                    fragmentLifetime
                );
            }
        }

        // Hide the original enemy.
        source.enabled = false;

        // Remove the enemy immediately; fragments are independent.
        Destroy(gameObject);
        Destroy(fragmentParent, fragmentLifetime + 0.1f);
    }
}

public class ShatterFragment : MonoBehaviour
{
    private Vector2 velocity;
    private float spin;
    private float lifetime;
    private float elapsed;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Launch(
        Vector2 initialVelocity,
        float rotationSpeed,
        float duration)
    {
        velocity = initialVelocity;
        spin = rotationSpeed;
        lifetime = duration;

        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        transform.position +=
            (Vector3)(velocity * Time.deltaTime);

        transform.Rotate(
            0f, 0f, spin * Time.deltaTime
        );

        float t = Mathf.Clamp01(elapsed / lifetime);

        if (spriteRenderer != null)
        {
            Color color = startColor;
            color.a = startColor.a * (1f - t);
            spriteRenderer.color = color;
        }

        if (elapsed >= lifetime)
            Destroy(gameObject);
    }
}
