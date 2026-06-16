using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class FoodSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [Tooltip("List of food prefabs to randomly spawn from.")]
        [SerializeField] private List<GameObject> foodPrefabs = new List<GameObject>();
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10, -10);
        [SerializeField] private Vector2 spawnAreaMax = new Vector2(10, 10);
        
        [Header("Limits")]
        [SerializeField] private int maxFoodOnScreen = 5;

        private List<GameObject> spawnedFoods = new List<GameObject>();
        private Transform foodContainer;
        private float timer = 0f;

        private void Start()
        {
            foodContainer = new GameObject("FoodContainer").transform;
            foodContainer.SetParent(transform);
        }

        private void Update()
        {
            if (foodPrefabs == null || foodPrefabs.Count == 0) return;

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                
                spawnedFoods.RemoveAll(item => item == null);
                
                if (spawnedFoods.Count < maxFoodOnScreen)
                {
                    SpawnFood();
                }
            }
        }

        public void SpawnFood()
        {
            if (foodPrefabs.Count == 0) return;

            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            int randomIndex = Random.Range(0, foodPrefabs.Count);
            GameObject prefabToSpawn = foodPrefabs[randomIndex];

            GameObject newFood = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, foodContainer);
            newFood.tag = "Food";
            
            spawnedFoods.Add(newFood);
        }

        public void AddFoodPrefab(GameObject prefab)
        {
            if (!foodPrefabs.Contains(prefab))
                foodPrefabs.Add(prefab);
        }

        public void RemoveFoodPrefab(GameObject prefab)
        {
            foodPrefabs.Remove(prefab);
        }

        public void ClearAllFood()
        {
            foreach (var food in spawnedFoods)
            {
                if (food != null) Destroy(food);
            }
            spawnedFoods.Clear();
        }
    }
}
