using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndCreditsPager : MonoBehaviour
{
    [Header("UI")]
    public GameObject root;          // 黑色Panel的根（整个UI）
    public TMP_Text textLine;        // 显示文字的TMP

    [Header("Pages")]
    [TextArea(2, 6)]
    public List<string> pages = new List<string>();

    [Header("Input")]
    public KeyCode nextKey = KeyCode.Mouse0; // 左键翻页

    int index = 0;
    bool playing = false;

    void Awake()
    {
        if (root == null) root = gameObject;
        root.SetActive(false);
    }

    public void Play()
    {
        if (pages == null || pages.Count == 0)
        {
            Debug.LogWarning("EndCreditsPager: pages is empty.");
            return;
        }

        playing = true;
        index = 0;
        root.SetActive(true);
        ShowCurrent();
    }

    void Update()
    {
        if (!playing) return;

        // 最后一页停住，不再翻页
        if (index >= pages.Count - 1) return;

        if (Input.GetKeyDown(nextKey))
        {
            index++;
            ShowCurrent();
        }
    }

    void ShowCurrent()
    {
        if (textLine == null) return;
        textLine.text = pages[index];
    }
}