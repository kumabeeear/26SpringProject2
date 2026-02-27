using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    [Tooltip("出生点物体名字（在场景中查找）")]
    public string spawnPointName = "PlayerSpawn";

    void Start()
    {
        GameObject spawn = GameObject.Find(spawnPointName);
        if (spawn == null) return;

        // CharacterController 需要先关再移动
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.position = spawn.transform.position;
        transform.rotation = spawn.transform.rotation;

        if (cc != null) cc.enabled = true;
    }
}