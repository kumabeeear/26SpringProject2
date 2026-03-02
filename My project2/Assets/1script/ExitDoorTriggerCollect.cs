using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTriggerCollect : MonoBehaviour
{
    [Header("Scene")]
    public string nextSceneName = "";

    [Header("Input")]
    public KeyCode leaveKey = KeyCode.F;

    [Header("UI Text")]
    [TextArea] public string notReadyText = "Still forget something.";
    [TextArea] public string readyText = "Press F to leave";

    bool playerInside;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        RefreshUI();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        ItemPromptUI.Instance?.HideName();
    }

    void Update()
    {
        if (!playerInside) return;

        RefreshUI();

        if (Input.GetKeyDown(leaveKey))
        {
            if (IsReady())
            {
                LoadNext();
            }
        }
    }

    bool IsReady()
    {
        return RoomCollectManagerSimple.Instance != null &&
               RoomCollectManagerSimple.Instance.CheckAllCollected();
    }

    void RefreshUI()
    {
        if (ItemPromptUI.Instance == null) return;
        ItemPromptUI.Instance.ShowName(IsReady() ? readyText : notReadyText);
    }

    void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}