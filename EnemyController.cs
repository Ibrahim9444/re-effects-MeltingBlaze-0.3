using UnityEngine;
using System.Collections;
using static EnemyStatsSO;

public class EnemyController : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    public EnemyType enemyType;

    [Header("Enemy Stats")]
    private string enemyCurrentName;
    [HideInInspector] public float enemyCurrentHealth;
    [HideInInspector] public float enemyCurrentArmor;
    private float enemyCurrentMovementSpeed;
    private float enemyCurrentBaseDamage;

    private float debugTimer = 0f;
    private float debugInterval = 5f; // Log every 5 seconds

    void Start()
    {
        References();
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        Debug.Log("Enemy Type: " + enemyStats.enemyType);

        if (enemyStats.healingGen)
        {
            StartCoroutine(HealRegeneration());
        }

        if (enemyStats.armorGen)
        {
            StartCoroutine(ArmorRegeneration());
        }
    }

    void Update()
    {
        debugTimer += Time.deltaTime;
        if (debugTimer >= debugInterval)
        {
            Debug.Log($"Current health: {enemyCurrentHealth}, Current armor: {enemyCurrentArmor}");
            debugTimer -= debugInterval;
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy {enemyCurrentName} taking {damage} damage");

        if (enemyStats.isRobot) // Check enemyStats.isRobot
        {
            // For robots, damage armor directly
            if (enemyCurrentArmor > 0)
            {
                float damageToArmor = Mathf.Min(damage, enemyCurrentArmor);
                enemyCurrentArmor -= damageToArmor;
                Debug.Log($"Damage absorbed by armor: {damageToArmor}. Remaining Armor: {enemyCurrentArmor}");
            }

            if (enemyCurrentArmor <= 0)
            {
                Debug.Log("Robot destroyed.");
                Destroy(gameObject);
            }
        }
        else
        {
            // For non-robots, damage armor then health
            if (enemyCurrentArmor > 0)
            {
                float damageToArmor = Mathf.Min(damage, enemyCurrentArmor);
                enemyCurrentArmor -= damageToArmor;
                damage -= damageToArmor;
                Debug.Log($"Damage absorbed by armor: {damageToArmor}. Remaining Armor: {enemyCurrentArmor}");
            }

            if (damage > 0)
            {
                enemyCurrentHealth -= damage;
                Debug.Log($"Damage taken: {damage}. Remaining Health: {enemyCurrentHealth}");
            }

            if (enemyCurrentHealth <= 0)
            {
                Debug.Log("Enemy destroyed.");
                Destroy(gameObject);
            }
        }
    }

    void References()
    {
        enemyCurrentName = enemyStats.enemyName;
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        enemyCurrentMovementSpeed = enemyStats.MovementSpeed;
        enemyCurrentBaseDamage = enemyStats.BaseDamage;
    }

    private IEnumerator HealRegeneration()
    {
        while (true)
        {
            if (enemyCurrentHealth < enemyStats.Health)
            {
                float healAmount = enemyStats.Health * (enemyStats.healingRegenPercentage / 100f);
                enemyCurrentHealth += healAmount;
                enemyCurrentHealth = Mathf.Min(enemyCurrentHealth, enemyStats.Health);
                Debug.Log($"{enemyCurrentName} healing: +{healAmount}. Current Health: {enemyCurrentHealth}");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ArmorRegeneration()
    {
        while (true)
        {
            if (enemyCurrentArmor < enemyStats.Armor)
            {
                float armorRegenAmount = enemyStats.Armor * (enemyStats.armorRegenPercentage / 100f);
                enemyCurrentArmor += armorRegenAmount;
                enemyCurrentArmor = Mathf.Min(enemyCurrentArmor, enemyStats.Armor);
                Debug.Log($"{enemyCurrentName} regenerating armor: +{armorRegenAmount}. Current Armor: {enemyCurrentArmor}");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}