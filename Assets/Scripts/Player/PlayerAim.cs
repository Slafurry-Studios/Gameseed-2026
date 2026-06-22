using UnityEngine;
using UnityEngine.InputSystem;
using Game.Gameplay;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour
    {
        [Header("Aim Settings")]
        [Tooltip("Assign the indicator or weapon object that will orbit the center.")]
        [SerializeField] private Transform aimIndicator;
        
        [Tooltip("The center point of the orbit. Usually the player's center. If empty, uses this object's transform.")]
        [SerializeField] private Transform orbitCenter;

        [Tooltip("Distance from the center point to the aim indicator.")]
        [SerializeField] private float orbitRadius = 1.5f;

        [Tooltip("Should the indicator also rotate to face the aim direction?")]
        [SerializeField] private bool rotateIndicator = true;
        
        [Tooltip("Offset rotation in degrees if the indicator's sprite doesn't point correctly. Example: -90 if sprite faces UP.")]
        [SerializeField] private float rotationOffset = -90f;
        
        [Header("Shooting Settings")]
        [SerializeField] private InputActionReference shootAction;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float bulletDamage = 10f;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float maxShootDistance = 10f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float hitRadius = 0.2f;
        
        private Camera mainCamera;
        private float nextFireTime;
        private Vector3 currentAimDirection;

        private void Awake()
        {
            mainCamera = Camera.main;
            
            if (orbitCenter == null)
            {
                orbitCenter = transform;
            }

            if (aimIndicator == null)
            {
                aimIndicator = transform;
            }
        }

        private void OnEnable()
        {
            if (shootAction != null) shootAction.action.Enable();
        }

        private void OnDisable()
        {
            if (shootAction != null) shootAction.action.Disable();
        }

        private void Update()
        {
            AimAtCursor();
            HandleShooting();
        }

        private void HandleShooting()
        {
            if (shootAction == null || bulletPrefab == null || BulletManager.Instance == null) return;

            if (shootAction.action.IsPressed() && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }

        private void Shoot()
        {
            if (currentAimDirection == Vector3.zero) return;
            // CameraShakeManager.Instance.Shake(1f, 1f);
            HitStopManager.Instance.Stop(0.1f);
            BulletManager.Instance.FireBullet(bulletPrefab, aimIndicator.position, currentAimDirection, bulletDamage, bulletSpeed, maxShootDistance, targetMask, hitRadius);
        }

        private void AimAtCursor()
        {
            if (mainCamera == null || aimIndicator == null || orbitCenter == null) return;
            if (Mouse.current == null) return;
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));
            mouseWorldPos.z = orbitCenter.position.z;

            Vector3 aimDirection = (mouseWorldPos - orbitCenter.position).normalized;
            currentAimDirection = aimDirection;
            
            if (aimDirection != Vector3.zero)
            {
                aimIndicator.position = orbitCenter.position + aimDirection * orbitRadius;

                if (rotateIndicator)
                {
                    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                    aimIndicator.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
                }
            }
        }
    }
}
