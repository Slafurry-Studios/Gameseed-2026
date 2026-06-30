using UnityEngine;
using Game.Core;
using Game.Gameplay;

[System.Serializable]
public struct Building
{
    public Sprite buildingSprite;
    public int hitThreshold;
}


[RequireComponent(typeof(SpriteRenderer))]
public class BuildingSwitcher : MonoBehaviour, IDamageable
{
    [Header("Only Player Bullet")]
    [SerializeField] private Bullet playerBullet;

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
        if (buildingPrefabs.Length > 0)
        {
            spriteRenderer.sprite =
                buildingPrefabs[currentIndex].buildingSprite;
        }
    }

    public void TakeDamage(float damage)
    {
        // fallback
        TakeDamage(damage, null);
    }

    public void TakeDamage(float damage, Bullet bullet)
    {
        // hanya bullet player
        if (playerBullet != null && bullet != playerBullet)
            return;


        hitCount++;

        Debug.Log(
            gameObject.name +
            " terkena player bullet : " +
            hitCount
        );
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
        if (!destroyParticlePrefab)
            return;

        GameObject effect = Instantiate(
            destroyParticlePrefab,
            transform.position,
            Quaternion.identity
        );

        ParticleSystem ps =
            effect.GetComponent<ParticleSystem>();


        if (ps)
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