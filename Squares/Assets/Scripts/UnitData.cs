using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Squares/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Basic Info")]
    public string unitName;
    public Sprite icon;
    public Color color;

    [Header("Stats")]
    public int maxHealth;
    public float moveSpeed;
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;

    [Header("Special")]
    public bool isKing;

    [Header("Sprites / Animation Frames")]
    public Sprite idleSprite;            // default frame
    public Sprite attackFrame1;          // first attack motion
    public Sprite attackFrame2;          // second attack motion
    public Sprite attackFinalFrame;      // final hit frame
}
