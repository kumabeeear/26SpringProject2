using UnityEngine;
using TMPro;

public class IntroPages : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject root;   // Panel(含文字)的根物体，用来整体隐藏
    [SerializeField] private TMP_Text bodyText;

    [Header("Pages")]
    [TextArea(3, 8)]
    [SerializeField] private string[] pages;

    [Header("Input")]
    [SerializeField] private KeyCode nextKey = KeyCode.Mouse0; // 左键翻页

    [Header("Behavior")]
    [SerializeField] private bool showOnStart = true;
    [SerializeField] private bool hideAfterLastPage = true;

    private int index = 0;
    public bool IsShowing { get; private set; }

    void Start()
    {
        if (root == null) root = gameObject;
        if (showOnStart) Show();
        else Hide();
    }

    void Update()
    {
        if (!IsShowing) return;

        if (Input.GetKeyDown(nextKey))
        {
            NextPage();
        }
    }

    public void Show()
    {
        IsShowing = true;
        root.SetActive(true);
        index = Mathf.Clamp(index, 0, pages.Length > 0 ? pages.Length - 1 : 0);
        Refresh();
    }

    public void Hide()
    {
        IsShowing = false;
        root.SetActive(false);
    }

    private void NextPage()
    {
        if (pages == null || pages.Length == 0)
        {
            if (hideAfterLastPage) Hide();
            return;
        }

        index++;

        if (index >= pages.Length)
        {
            if (hideAfterLastPage) Hide();
            else index = pages.Length - 1;
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        if (bodyText != null && pages != null && pages.Length > 0)
        {
            bodyText.text = pages[index];
        }
    }
}