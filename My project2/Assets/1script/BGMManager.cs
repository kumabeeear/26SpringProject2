using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    [Header("Audio")]
    public AudioSource source;

    [Header("Clips")]
    public AudioClip room1and2Clip;
    public AudioClip room3Clip;

    [Header("Scene Names (must match exactly)")]
    public string room1SceneName = "1";
    public string room2SceneName = "2";
    public string room3SceneName = "3";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void Start()
    {
        // 开局（Room1）先播对的歌
        ApplyBGMForScene(SceneManager.GetActiveScene().name);
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyBGMForScene(scene.name);
    }

    void ApplyBGMForScene(string sceneName)
    {
        AudioClip target = null;

        if (sceneName == room3SceneName)
            target = room3Clip;
        else if (sceneName == room1SceneName || sceneName == room2SceneName)
            target = room1and2Clip;

        if (target == null) return;

        // 如果还是同一首，就别重播（Room1 -> Room2 不断）
        if (source.clip == target && source.isPlaying) return;

        source.clip = target;
        source.Play();
    }
}