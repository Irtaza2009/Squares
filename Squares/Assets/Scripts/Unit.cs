using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public UnitData data;   // Assign in Inspector

    private int currentHealth;
    private float attackTimer;

    private Vector3 targetPos;
    private bool hasTarget = false;
    private bool isPausedForCombat = false;

    private Unit targetEnemy; // Enemy currently engaged with
    private List<Unit> enemiesInRange = new List<Unit>();

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D attackRangeCollider;
    private BoxCollider2D bodyCollider;

    void Start()
    {
        currentHealth = data.maxHealth;

        // Visual color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = data.color;

        // Find child circle collider for attack range
        attackRangeCollider = GetComponentInChildren<CircleCollider2D>();
        if (attackRangeCollider != null)
        {
            attackRangeCollider.isTrigger = true;
            attackRangeCollider.radius = data.attackRange;
        }

        // Body collider (on root)
        bodyCollider = GetComponent<BoxCollider2D>();

        Debug.Log($"{data.unitName} spawned with {currentHealth} HP");
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        // Movement (pause if fighting)
        if (hasTarget && !isPausedForCombat)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, data.moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                hasTarget = false;
                Debug.Log($"{data.unitName} reached destination");
            }
        }

        // Attack logic
        if (targetEnemy != null)
        {
            isPausedForCombat = true;

            // Check if enemy is still inside my attack collider
            if (attackRangeCollider.bounds.Intersects(targetEnemy.bodyCollider.bounds))
            {
                if (CanAttack())
                {
                    Attack(targetEnemy);
                }
            }
            else
            {
                RemoveTarget(targetEnemy);
            }
        }
        else
        {
            if (enemiesInRange.Count > 0)
            {
                targetEnemy = enemiesInRange[0];
                Debug.Log($"{data.unitName} switched to new target: {targetEnemy.data.unitName}");
            }
            else
            {
                isPausedForCombat = false;
            }
        }
    }

    public void MoveTo(Vector3 pos)
    {
        targetPos = pos;
        hasTarget = true;
        Debug.Log($"{data.unitName} moving to {pos}");
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"{data.unitName} took {dmg} damage, {currentHealth} HP left");

        if (currentHealth <= 0)
        {
            Debug.Log($"{data.unitName} died!");
            Destroy(gameObject);
        }
    }

    public bool CanAttack() => attackTimer <= 0;

    public void Attack(Unit target)
    {
        if (CanAttack())
        {
            Debug.Log($"{data.unitName} attacks {target.data.unitName} for {data.attackDamage} damage");
            target.TakeDamage(data.attackDamage);
            attackTimer = data.attackCooldown;
        }
    }

    // Trigger (child circle collider = attack range)

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit otherUnit = other.GetComponent<Unit>();

        if (otherUnit != null && otherUnit != this && other == otherUnit.bodyCollider)
        {
            if (!enemiesInRange.Contains(otherUnit))
                enemiesInRange.Add(otherUnit);

            if (targetEnemy == null)
            {
                targetEnemy = otherUnit;
                Debug.Log($"{data.unitName} targeting {otherUnit.data.unitName}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Unit otherUnit = other.GetComponent<Unit>();
        if (otherUnit != null && otherUnit != this && other == otherUnit.bodyCollider)
        {
            RemoveTarget(otherUnit);
        }
    }

    private void RemoveTarget(Unit enemy)
    {
        if (enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);

        if (targetEnemy == enemy)
        {
            targetEnemy = null;
            isPausedForCombat = false;
        }
    }

    private void OnDestroy()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
                enemy.RemoveTarget(this);
        }
    }
}

