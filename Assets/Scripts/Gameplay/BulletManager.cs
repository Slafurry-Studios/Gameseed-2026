using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Game.Core;

namespace Game.Gameplay
{
    public class BulletManager : MonoBehaviour
    {
        public static BulletManager Instance { get; private set; }

        private Dictionary<int, ObjectPool<Bullet>> pools = new Dictionary<int, ObjectPool<Bullet>>();
        private List<Bullet> activeBullets = new List<Bullet>();
        private Collider2D[] hitResults = new Collider2D[1];

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FireBullet(Bullet prefab, Vector2 startPos, Vector2 direction, float damage, float speed, float maxDistance, LayerMask targetMask, float hitRadius)
        {
            if (prefab == null) return;

            int prefabID = prefab.gameObject.GetInstanceID();

            if (!pools.ContainsKey(prefabID))
            {
                CreatePoolForPrefab(prefab, prefabID);
            }

            Bullet bullet = pools[prefabID].Get();
            bullet.transform.position = startPos;
            bullet.Setup(prefabID, damage, speed, maxDistance, direction, targetMask, hitRadius);
            
            activeBullets.Add(bullet);
        }

        private void CreatePoolForPrefab(Bullet prefab, int prefabID)
        {
            ObjectPool<Bullet> newPool = new ObjectPool<Bullet>(
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: (b) => b.gameObject.SetActive(true),
                actionOnRelease: (b) => b.gameObject.SetActive(false),
                actionOnDestroy: (b) => Destroy(b.gameObject),
                collectionCheck: true,
                defaultCapacity: 50,
                maxSize: 1000
            );
            pools.Add(prefabID, newPool);
        }

        private void Update()
        {
            for (int i = activeBullets.Count - 1; i >= 0; i--)
            {
                Bullet b = activeBullets[i];
                
                b.transform.position += (Vector3)(b.direction * b.speed * Time.deltaTime);

                int hits = Physics2D.OverlapCircleNonAlloc(b.transform.position, b.hitRadius, hitResults, b.targetMask);
                if (hits > 0)
                {
                    IDamageable damageable = hitResults[0].GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(b.damage);
                    }
                    
                    ReturnBullet(b, i);
                    continue; 
                }

                if (Vector2.Distance(b.startPosition, b.transform.position) >= b.maxDistance)
                {
                    ReturnBullet(b, i);
                }
            }
        }

        private void ReturnBullet(Bullet b, int index)
        {
            activeBullets.RemoveAt(index);
            if (pools.TryGetValue(b.prefabID, out ObjectPool<Bullet> pool))
            {
                pool.Release(b);
            }
            else
            {
                b.gameObject.SetActive(false); // Fallback
            }
        }
    }
}
