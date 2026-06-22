using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }

    [Header("Impulse Source")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Shake(float amplitude, float frequency)
    {
        if (impulseSource == null)
        {
            Debug.LogWarning("[CameraShakeManager] Impulse Source not assigned!");
            return;
        }

        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = frequency;

        impulseSource.GenerateImpulse();
    }
}