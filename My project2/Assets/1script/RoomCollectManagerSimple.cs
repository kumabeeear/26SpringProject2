using System.Collections.Generic;
using UnityEngine;

public class RoomCollectManagerSimple : MonoBehaviour
{
    public static RoomCollectManagerSimple Instance { get; private set; }

    [Header("Drag the ITEM OBJECTS that will be hidden on pickup")]
    public List<GameObject> requiredObjects = new List<GameObject>();

    [Header("Debug")]
    public bool allCollected;

    private bool playedAllSfx;

    void Awake()
    {
        // 简单单例（每个场景一个也可以；如果你想跨场景只留一个再告诉我）
        Instance = this;

        // 进新场景时重置一次，避免上个场景已经播过导致本场景不播
        playedAllSfx = false;
        allCollected = false;
    }

    void Update()
    {
        allCollected = CheckAllCollected();

        // ✅ 第一次达成“全收集”时播放一次
        if (allCollected && !playedAllSfx)
        {
            playedAllSfx = true;
            SFXPlayer.Instance?.PlayAllCollected();
        }
    }

    public bool CheckAllCollected()
    {
        if (requiredObjects == null || requiredObjects.Count == 0) return false;

        for (int i = 0; i < requiredObjects.Count; i++)
        {
            var obj = requiredObjects[i];
            if (obj == null) continue;

            // 只要还有一个是激活的，就没收集齐
            if (obj.activeInHierarchy) return false;
        }
        return true;
    }
}