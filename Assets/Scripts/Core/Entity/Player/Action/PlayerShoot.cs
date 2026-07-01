using UnityEngine;
using UnityEngine.InputSystem;
using Game.Gameplay;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerAim))]
    public class PlayerShoot : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float bulletNormalRadius = 0.2f;
        [SerializeField] private Bullet bulletBiggerDakkaPrefab;
        [SerializeField] private float bulletBiggerDakkaRadius = 0.5f;
        [SerializeField] private Bullet bulletAverageBulletEnjoyerPrefab;
        [SerializeField] private float bulletAverageBulletEnjoyerPrefabRadius = 1f;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float bulletDamage = 10f;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float maxShootDistance = 10f;
        [SerializeField] private LayerMask targetMask;

        private PlayerAim playerAim;
        private float nextFireTime;
        private bool doubleBullet;
        private bool tripleBullet;
        private bool fiveBullet;
        private bool isExplosive = false;
        private bool isRichochet = false;

        private void Awake()
        {
            playerAim = GetComponent<PlayerAim>();
        }

        private void Update()
        {
            if (bulletPrefab == null || BulletManager.Instance == null)
                return;

            if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
        private void Shoot()
        {
            Vector3 currentAimDirection = playerAim.CurrentAimDirection;
            if (currentAimDirection == Vector3.zero) return;

            if (!doubleBullet && !tripleBullet && !fiveBullet)
            {
                SpawnBullet(currentAimDirection);
            }
            else if (doubleBullet)
            {
                Vector3 leftDir = Quaternion.Euler(0, -5f, 0) * currentAimDirection;
                Vector3 rightDir = Quaternion.Euler(0, 5f, 0) * currentAimDirection;

                SpawnBullet(leftDir);
                SpawnBullet(rightDir);
            }
            else if (tripleBullet)
            {
                Vector3 leftDir = Quaternion.Euler(0, -5f, 0) * currentAimDirection;
                Vector3 midDir = Quaternion.Euler(0, 0f, 0) * currentAimDirection;
                Vector3 rightDir = Quaternion.Euler(0, 5f, 0) * currentAimDirection;

                SpawnBullet(leftDir);
                SpawnBullet(midDir);
                SpawnBullet(rightDir);
            }
            else if (fiveBullet)
            {
                Vector3 leftDir = Quaternion.Euler(0, -10f, 0) * currentAimDirection;
                Vector3 midLeftDir = Quaternion.Euler(0, -5f, 0) * currentAimDirection;
                Vector3 midDir = Quaternion.Euler(0, 0f, 0) * currentAimDirection;
                Vector3 midRightDir = Quaternion.Euler(0, 5f, 0) * currentAimDirection;
                Vector3 rightDir = Quaternion.Euler(0, 10f, 0) * currentAimDirection;

                SpawnBullet(leftDir);
                SpawnBullet(midLeftDir);
                SpawnBullet(midDir);
                SpawnBullet(midRightDir);
                SpawnBullet(rightDir);
            }
        }

        private void SpawnBullet(Vector3 dir)
        {
            BulletManager.Instance.FireBullet(
                bulletPrefab,
                playerAim.AimIndicatorPosition,
                dir,
                bulletDamage,
                bulletSpeed,
                maxShootDistance,
                targetMask,
                bulletNormalRadius,
                isExplosive,
                isRichochet);
            SoundManager.Instance.PlaySound2D("Shotgun_Punchy");
        }

        public void BiggerDakka()
        {
            bulletPrefab = bulletBiggerDakkaPrefab;
            bulletNormalRadius = bulletBiggerDakkaRadius;
        }

        public void MoreDakka()
        {
            doubleBullet = true;
        }

        public void DakkaEverywhere()
        {
            doubleBullet = false;
            tripleBullet = true;
        }

        public void MoreEspresso()
        {
            fireRate = fireRate * 1.15f;
        }

        public void FrameRateKiller()
        {
            fireRate = fireRate * 1.3f;
        }

        public void AverageBulletEnjoyer()
        {
            bulletPrefab = bulletAverageBulletEnjoyerPrefab;
            bulletNormalRadius = bulletAverageBulletEnjoyerPrefabRadius;
        }

        public void ExplosiveAmmo()
        {
            isExplosive = true;
        }

        public void RichochetBullet()
        {
            isRichochet = true;
        }

        public void ApocalypseStream()
        {
            fiveBullet = true;
            tripleBullet = false;
            doubleBullet = false;
            fireRate = fireRate * 1.5f;
        }
    }
}