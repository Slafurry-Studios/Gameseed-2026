using UnityEngine;

[System.Serializable]
public struct Building
{
    public Sprite buildingSprite;

    [Tooltip("Jumlah hit sebelum building ini hancur")]
    public int hitThreshold;
}


[RequireComponent(typeof(SpriteRenderer))]
public class BuildingSwitcher : MonoBehaviour
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
        // otomatis mengambil SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        if (buildingPrefabs != null && buildingPrefabs.Length > 0)
        {
            spriteRenderer.sprite = buildingPrefabs[currentIndex].buildingSprite;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hit();
        }
    }


    void Hit()
    {
        hitCount++;

        Debug.Log("Total Hit : " + hitCount);

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
        DestroyCurrentBuilding();

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


    void DestroyCurrentBuilding()
    {
        if (destroyParticlePrefab == null)
            return;


        GameObject effect = Instantiate(
            destroyParticlePrefab,
            transform.position,
            Quaternion.identity
        );


        ParticleSystem ps = effect.GetComponent<ParticleSystem>();

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