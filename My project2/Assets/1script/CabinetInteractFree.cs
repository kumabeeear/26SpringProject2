using UnityEngine;

public class CabinetInteractFree : MonoBehaviour
{
    [Header("References")]
    public Transform doorLeft;
    public Transform doorRight;

    [Tooltip("射线对准的目标（柜子整体或任意带Collider的物体）。命中它或它的子物体才允许交互。")]
    public Transform aimRoot;

    public Camera playerCam;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.Mouse1; // 右键

    [Header("Door Rotation")]
    public float openAngle = 90f;
    public float openSpeed = 6f;

    bool playerInside;
    bool isOpen;

    Quaternion leftClosedRot, rightClosedRot;
    Quaternion leftOpenRot, rightOpenRot;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        // 防呆：没填就直接报错提示
        if (doorLeft == null || doorRight == null)
            Debug.LogError("CabinetInteractFree: Please assign doorLeft and doorRight in Inspector.");

        if (aimRoot == null)
            Debug.LogError("CabinetInteractFree: Please assign aimRoot (cabinet root / collider object) in Inspector.");

        if (doorLeft != null) leftClosedRot = doorLeft.localRotation;
        if (doorRight != null) rightClosedRot = doorRight.localRotation;

        if (doorLeft != null) leftOpenRot = leftClosedRot * Quaternion.Euler(0, -openAngle, 0);
        if (doorRight != null) rightOpenRot = rightClosedRot * Quaternion.Euler(0, openAngle, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        if (!playerInside) return;
        if (!IsAimingAtTarget()) return;

        if (Input.GetKeyDown(interactKey))
            isOpen = !isOpen;

        if (doorLeft != null)
            doorLeft.localRotation = Quaternion.Lerp(doorLeft.localRotation, isOpen ? leftOpenRot : leftClosedRot, Time.deltaTime * openSpeed);

        if (doorRight != null)
            doorRight.localRotation = Quaternion.Lerp(doorRight.localRotation, isOpen ? rightOpenRot : rightClosedRot, Time.deltaTime * openSpeed);
    }

    bool IsAimingAtTarget()
    {
        if (playerCam == null || aimRoot == null) return false;

        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // 命中 aimRoot 或它的子物体 collider 就算“对准”
            return hit.transform == aimRoot || hit.transform.IsChildOf(aimRoot);
        }
        return false;
    }
}