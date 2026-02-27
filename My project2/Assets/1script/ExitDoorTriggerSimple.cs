using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTriggerSimple : MonoBehaviour
{
    public KeyCode leaveKey = KeyCode.F;
    public string nextSceneName = "";

    [TextArea] public string notReadyText = "Collect all items first.";
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
            if (RoomCollectManagerSimple.Instance != null &&
                RoomCollectManagerSimple.Instance.CheckAllCollected())
            {
                LoadNext();
            }
        }
    }

    void RefreshUI()
    {
        bool ready = RoomCollectManagerSimple.Instance != null &&
                     RoomCollectManagerSimple.Instance.CheckAllCollected();

        ItemPromptUI.Instance?.ShowName(ready ? readyText : notReadyText);
    }

    void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}