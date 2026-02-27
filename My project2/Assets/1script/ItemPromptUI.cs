using UnityEngine;
using TMPro;

public class ItemPromptUI : MonoBehaviour
{
    public static ItemPromptUI Instance { get; private set; }

    [Header("UI Refs")]
    public GameObject root;       // 整个UI的根（Panel也行），可以为空则用自身
    public TMP_Text nameLine;     // 显示物品名字
    public TMP_Text thoughtLine;  // 显示心声（可选）

    [Header("Thought Settings")]
    public float thoughtHideAfter = 5f; // 心声多久后消失（public可调）

    float thoughtTimer;

    void Awake()
    {
        Instance = this;
        if (root == null) root = gameObject;
        HideName();
        HideThought();
    }

    void Update()
    {
        if (thoughtLine != null && thoughtLine.gameObject.activeSelf)
        {
            thoughtTimer -= Time.deltaTime;
            if (thoughtTimer <= 0f) HideThought();
        }
    }

    // ✅ 只显示物品名字（已去掉 Left Click 提示 + 富文本 alpha）
    public void ShowName(string itemName)
    {
        if (root != null) root.SetActive(true);
        if (nameLine != null)
        {
            nameLine.gameObject.SetActive(true);
            nameLine.text = itemName;
        }
    }

    public void HideName()
    {
        if (nameLine != null) nameLine.gameObject.SetActive(false);
        TryDisableRoot();
    }

    public void ShowThought(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || thoughtLine == null) return;

        if (root != null) root.SetActive(true);
        thoughtLine.gameObject.SetActive(true);
        thoughtLine.text = text;
        thoughtTimer = thoughtHideAfter;
    }

    public void HideThought()
    {
        if (thoughtLine != null) thoughtLine.gameObject.SetActive(false);
        TryDisableRoot();
    }

    void TryDisableRoot()
    {
        if (root == null) return;
        bool nameOn = nameLine != null && nameLine.gameObject.activeSelf;
        bool thoughtOn = thoughtLine != null && thoughtLine.gameObject.activeSelf;
        if (!nameOn && !thoughtOn) root.SetActive(false);
    }
}