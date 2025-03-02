using UnityEngine;
using System.Collections;

public class InfernoPlagueEffect : MonoBehaviour, IElementalEffect
{
    public float duration = 5f;
    public float baseDamage = 10f;

    public void ApplyEffect(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        Debug.Log("InfernoPlagueEffect applied!");

        if (enemy.enemyStats.isSpectral || enemy.enemyStats.isRobot)
        {
            if (enemy.enemyStats.isSpectral) Debug.Log("InfernoPlague has no effect on spectral enemies.");
            if (enemy.enemyStats.isRobot) Debug.Log("InfernoPlague has no effect on Robots.");
            Destroy(this);
            return;
        }

        StartCoroutine(ApplyDamageOverTime(enemy, fireBonus, toxicBonus));
    }

    private IEnumerator ApplyDamageOverTime(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        float duration = 4f;
        float tickInterval = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            float damagePerTick = (fireBonus + toxicBonus) / 4f;

            enemy.enemyCurrentHealth -= damagePerTick;
            Debug.Log($"Inferno Plague Damage: {damagePerTick}. Remaining Health: {enemy.enemyCurrentHealth}");

            if (enemy.enemyCurrentHealth <= 0)
            {
                Debug.Log("Enemy destroyed by Inferno Plague.");
                Destroy(enemy.gameObject);
                break;
            }

            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        Destroy(this);
    }
}