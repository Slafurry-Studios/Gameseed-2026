using UnityEngine;
using Game.Core;

[System.Serializable]
public struct Building
{
    public Sprite buildingSprite;

    [Tooltip("Jumlah hit sebelum building ini hancur")]
    public int hitThreshold;
}


[RequireComponent(typeof(SpriteRenderer))]
public class BuildingSwitcher : MonoBehaviour, IDamageable
{
    [Header("Building")]
    public Building[] buildingPrefabs;

    [Header("Particle")]
    public GameObject destroyParticlePrefab;
    
    private SpriteRenderer spriteRenderer;

    private int currentIndex = 0;
    private int hitCount = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (buildingPrefabs != null && buildingPrefabs.Length > 0)
        {
            spriteRenderer.sprite =
                buildingPrefabs[currentIndex].buildingSprite;
        }
    }

    public void TakeDamage(float damage)
    {
        hitCount++;
        Debug.Log(gameObject.name + " terkena hit : " + hitCount);

        CheckThreshold();
    }

    void CheckThreshold()
    {
        if (currentIndex >= buildingPrefabs.Length)
            return;

        if (hitCount >= buildingPrefabs[currentIndex].hitThreshold)
        {
            ChangeBuilding();
        }
    }

    void ChangeBuilding()
    {
        PlayDestroyEffect();

        currentIndex++;

        if (currentIndex < buildingPrefabs.Length)
        {
            spriteRenderer.sprite =
                buildingPrefabs[currentIndex].buildingSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayDestroyEffect()
    {
        if (destroyParticlePrefab == null)
            return;

        GameObject effect = Instantiate(
            destroyParticlePrefab,
            transform.position,
            Quaternion.identity
        );

        ParticleSystem ps =
            effect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();

            Destroy(
                effect,
                ps.main.duration +
                ps.main.startLifetime.constantMax
            );
        }
        else
        {
            Destroy(effect, 3f);
        }
    }
}