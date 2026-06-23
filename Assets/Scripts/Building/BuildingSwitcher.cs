using UnityEngine;

[System.Serializable]
public struct Building
{
    public GameObject buildingPrefab;

    [Tooltip("Jumlah hit sebelum building ini hancur")]
    public int hitThreshold;
}


public class BuildingSwitcher : MonoBehaviour
{
    [Header("Building")]
    public Building[] buildingPrefabs;


    [Header("Particle")]
    public GameObject destroyParticlePrefab;


    private GameObject currentBuilding;
    private int currentIndex = 0;

    private int hitCount = 0;


    void Start()
    {
        SpawnBuilding();
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


    void SpawnBuilding()
    {
        currentBuilding = Instantiate(
            buildingPrefabs[currentIndex].buildingPrefab,
            transform.position,
            transform.rotation,
            transform
        );
    }


    void ChangeBuilding()
    {
        DestroyCurrentBuilding();

        currentIndex++;

        if (currentIndex < buildingPrefabs.Length)
        {
            SpawnBuilding();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void DestroyCurrentBuilding()
    {
        if (currentBuilding == null)
            return;


        if (destroyParticlePrefab != null)
        {
            GameObject effect = Instantiate(
                destroyParticlePrefab,
                currentBuilding.transform.position,
                Quaternion.identity
            );


            ParticleSystem ps = effect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();

                Destroy(
                    effect,
                    ps.main.duration + ps.main.startLifetime.constantMax
                );
            }
            else
            {
                Destroy(effect, 3f);
            }
        }


        Destroy(currentBuilding);
    }
}