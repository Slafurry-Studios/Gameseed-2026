using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class LootLauncher : MonoBehaviour
    {
        [Header("Launch Animation")]
        public float launchDuration = 0.4f;
        public float launchHeight = 1.5f;

        private Coroutine launchRoutine;
        private FloatingMovement floatingMovement;

        private void Awake()
        {
            floatingMovement = GetComponent<FloatingMovement>();
        }

        public void Launch(Vector3 startPos, Vector3 endPos)
        {
            // Matikan floating dulu biar nggak rebutan posisi
            if (floatingMovement != null)
                floatingMovement.enabled = false;

            transform.position = startPos;

            if (launchRoutine != null)
                StopCoroutine(launchRoutine);

            launchRoutine = StartCoroutine(LaunchRoutine(startPos, endPos));
        }

        private IEnumerator LaunchRoutine(Vector3 startPos, Vector3 endPos)
        {
            float elapsed = 0f;

            while (elapsed < launchDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / launchDuration);

                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
                float arc = Mathf.Sin(t * Mathf.PI) * launchHeight;
                currentPos.y += arc;

                transform.position = currentPos;

                yield return null;
            }

            transform.position = endPos;
            launchRoutine = null;

            if (floatingMovement != null)
            {
                floatingMovement.SetStartPosition(endPos);
                floatingMovement.enabled = true;
            }
        }
    }
}