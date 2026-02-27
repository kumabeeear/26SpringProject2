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
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        // 建议设置
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D音效
    }

    public void PlayPickup()
    {
        if (pickupClip != null) source.PlayOneShot(pickupClip);
    }

    public void PlayAllCollected()
    {
        if (allCollectedClip != null) source.PlayOneShot(allCollectedClip);
    }
}