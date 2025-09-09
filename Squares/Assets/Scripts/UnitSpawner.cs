using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject infantryPrefab;
    public GameObject archerPrefab;

    [Header("Spawn Settings")]
    public int unitsPerType = 5;
    public float spacing = 1.5f;   // space between units
    public float bottomY = -4f;    // y position for Player1
    public float topY = 4f;        // y position for Enemy
    public float startX = -4f;     // leftmost x for spawning

    void Start()
    {
        SpawnArmy("Player1", infantryPrefab, archerPrefab, bottomY);
        SpawnArmy("Enemy", infantryPrefab, archerPrefab, topY);
    }

    void SpawnArmy(string tagName, GameObject infantry, GameObject archer, float yPos)
    {
        // Infantry row
        for (int i = 0; i < unitsPerType; i++)
        {
            Vector3 pos = new Vector3(startX + i * spacing, yPos, 0f);
            GameObject unit = Instantiate(infantry, pos, Quaternion.identity);
            unit.tag = tagName;
        }

        // Archers row (slightly behind infantry)
        for (int i = 0; i < unitsPerType; i++)
        {
            Vector3 pos = new Vector3(startX + i * spacing, yPos + (tagName == "Enemy" ? -2f : 2f), 0f);
            GameObject unit = Instantiate(archer, pos, Quaternion.identity);
            unit.tag = tagName;
        }
    }
}
