using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance { get; private set; }

    [Header("Audio")]
    public AudioSource source;

    [Header("Clips")]
    public AudioClip pickupClip;
    public AudioClip allCollectedClip;

    void Awake()
    {
        // 单例：只保留一个
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 跨场景保留
        DontDestroyOnLoad(gameObject);

        // 确保有 AudioSource
        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f; // 2D
    }

    public void PlayPickup()
    {
        if (pickupClip != null) source.PlayOneShot(pickupClip);
        else Debug.LogWarning("SFXPlayer: pickupClip not assigned");
    }

    public void PlayAllCollected()
    {
        if (allCollectedClip != null) source.PlayOneShot(allCollectedClip);
        else Debug.LogWarning("SFXPlayer: allCollectedClip not assigned");
    }
}