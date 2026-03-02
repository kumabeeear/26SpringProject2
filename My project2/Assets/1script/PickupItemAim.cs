using UnityEngine;


public class PickupItemAim : MonoBehaviour
{
    public string itemId = "item_1";
    [Header("References")]
    public GameObject itemObjectToHide;     // 拾取后要消失的物品
    public Camera playerCam;               // 玩家相机（不填会自动找 Camera.main）
    [Tooltip("射线最远距离")]
    public float aimDistance = 3f;

    [Header("Texts")]
    public string itemName = "Item";
    [TextArea(2, 6)]
    public string thoughtAfterPickup = "";

    [Header("Aim Settings")]
    [Tooltip("射线必须打到这个Transform(不填则用 itemObjectToHide 的 transform)")]
    public Transform aimTarget;
    [Tooltip("允许打到子物体Collider")]
    public bool allowHitChildren = true;

    bool playerInside;
    bool picked;
    bool isAiming;

    void Start()
    {
        if (playerCam == null)
        {
            if (Camera.main != null) playerCam = Camera.main;
        }
        if (aimTarget == null && itemObjectToHide != null) aimTarget = itemObjectToHide.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (picked) return;
        if (!other.CompareTag("Player")) return;
        playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        isAiming = false;
        ItemPromptUI.Instance?.HideName();
    }

    void Update()
    {
        if (picked) return;

        if (!playerInside)
        {
            if (isAiming)
            {
                isAiming = false;
                ItemPromptUI.Instance?.HideName();
            }
            return;
        }

        bool aimingNow = CheckAiming();

        if (aimingNow && !isAiming)
        {
            isAiming = true;
            ItemPromptUI.Instance?.ShowName(itemName);
        }
        else if (!aimingNow && isAiming)
        {
            isAiming = false;
            ItemPromptUI.Instance?.HideName();
        }

        if (isAiming && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Pick();
        }
    }

    bool CheckAiming()
    {
        if (playerCam == null || aimTarget == null) return false;

        // 屏幕中心射线
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance))
        {
            Transform hitT = hit.transform;

            if (allowHitChildren)
            {
                // 命中物体或其子物体
                return hitT == aimTarget || hitT.IsChildOf(aimTarget);
            }
            else
            {
                return hitT == aimTarget;
            }
        }
        return false;
    }

    void Pick()
    {
        picked = true;

        ItemPromptUI.Instance?.HideName();
        if (!string.IsNullOrWhiteSpace(thoughtAfterPickup))
            ItemPromptUI.Instance?.ShowThought(thoughtAfterPickup);

        if (itemObjectToHide != null) itemObjectToHide.SetActive(false);
        gameObject.SetActive(false); // 关掉trigger避免重复
        
        SFXPlayer.Instance?.PlayPickup();
    }
}