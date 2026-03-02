using UnityEngine;

public class ExitDoorTriggerFinalRoom : MonoBehaviour
{
    [Header("UI Prompt (uses your ItemPromptUI)")]
    [TextArea] public string notReadyText = "Collect all items first.";
    [TextArea] public string readyText = "Press F to leave";
    public KeyCode leaveKey = KeyCode.F;

    [Header("End Sequence")]
    public EndCreditsPager endPager;                 // 拖你的 EndCreditsPager
    public Transform playerRoot;                     // 拖玩家根物体（First Person Controller）
    public Vector3 voidTeleportPosition = new Vector3(0, -500, 0);

    [Tooltip("玩家控制脚本（有的话拖进来，结尾时禁用）")]
    public Behaviour playerLookScript;               // 例如 FirstPersonController / PlayerInput / MouseLook 等
    public Behaviour playerMoveScript;               // 例如 CharacterController 驱动脚本

    bool playerInside;
    bool ended;

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
        if (ended) return;
        if (!playerInside) return;

        RefreshUI();

        if (Input.GetKeyDown(leaveKey))
        {
            if (IsReady())
            {
                StartEnd();
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
        ItemPromptUI.Instance?.ShowName(IsReady() ? readyText : notReadyText);
    }

    void StartEnd()
    {
        ended = true;

        // 1) 隐藏门口提示
        ItemPromptUI.Instance?.HideName();

        // 2) 禁用玩家控制（可选但强烈建议）
        if (playerLookScript != null) playerLookScript.enabled = false;
        if (playerMoveScript != null) playerMoveScript.enabled = false;

        // 3) 传送到虚空（并避免 CharacterController 卡住）
        if (playerRoot != null)
        {
            CharacterController cc = playerRoot.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            playerRoot.position = voidTeleportPosition;

            if (cc != null) cc.enabled = true;
        }

        // 4) 开始黑屏翻页
        if (endPager != null)
            endPager.Play();
        else
            Debug.LogWarning("ExitDoorTriggerFinalRoom: endPager not assigned.");
    }
}