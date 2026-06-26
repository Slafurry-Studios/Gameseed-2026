using UnityEngine;
using Game.Gameplay; 

namespace Game.AI
{
    public class AttackState : EntityState
    {
        [Header("Combat Settings")]
        public float attackRadius = 10f;
        public float fireRate = 0.5f;

        [Header("Bullet Settings")]
        [Tooltip("Drag the Bullet prefab here (it must have the Bullet script attached)")]
        public Bullet bulletPrefab;
        public Transform firePoint;
        
        public float bulletDamage = 1f;
        public float bulletSpeed = 10f;
        public float bulletMaxDistance = 20f;
        public float bulletHitRadius = 0.2f;
        [Tooltip("What should this bullet hit? (Set to 'Player')")]
        public LayerMask targetMask; 

        [Header("Line of Sight (LoS)")]
        public bool requiresLineOfSight = true;
        public LayerMask obstacleLayer;

        private float lastFireTime;

        public override bool CheckConditions(EntityBrain brain)
        {
            if (brain.Target == null) return false;

            float distance = Vector2.Distance(transform.position, brain.Target.position);
            
            if (distance > attackRadius) return false;

            if (requiresLineOfSight)
            {
                Vector2 directionToTarget = (brain.Target.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distance, obstacleLayer);
                
                if (hit.collider != null) return false;
            }

            return true;
        }

        public override void EnterState(EntityBrain brain)
        {
            brain.Movement.SetMovement(Vector2.zero, 0f);
        }

        public override void UpdateState(EntityBrain brain)
        {

            Vector2 aimDirection = (brain.Target.position - transform.position).normalized;

            if (Time.time >= lastFireTime + fireRate)
            {
                if (bulletPrefab != null && firePoint != null)
                {

                    BulletManager.Instance.FireBullet(
                        bulletPrefab, 
                        firePoint.position, 
                        aimDirection, 
                        bulletDamage, 
                        bulletSpeed, 
                        bulletMaxDistance, 
                        targetMask, 
                        bulletHitRadius
                    );
                }
                else
                {
                    Debug.LogWarning($"{gameObject.name} is missing a Bullet Prefab or Fire Point!");
                }
                
                lastFireTime = Time.time;
            }
        }

        public override void ExitState(EntityBrain brain) { }
    }
}