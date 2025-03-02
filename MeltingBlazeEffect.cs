using UnityEngine;
using System.Collections;

public class MeltingBlazeEffect : MonoBehaviour, IElementalEffect
{
    public void ApplyEffect(EnemyController enemy, float fireBonus, float acidBonus)
    {
        Debug.Log($"MeltingBlazeEffect applied! Fire Bonus: {fireBonus}, Acid Bonus: {acidBonus}"); // Add this line

        Debug.Log("MeltingBlazeEffect applied!");

        if (enemy.enemyStats.isSpectral)
        {
            Debug.Log("Melting Blaze has no effect on spectral enemies.");
            Destroy(this);
            return;
        }

        StartCoroutine(ApplyDamageOverTime(enemy, fireBonus, acidBonus));
    }

    private IEnumerator ApplyDamageOverTime(EnemyController enemy, float fireBonus, float acidBonus)
    {
        float duration = 4f;
        float tickInterval = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            float healthDamage = fireBonus / 15f;
            float armorDamage = acidBonus / 15f;

            Debug.Log($"Melting Blaze: acidBonus: {acidBonus}");
            Debug.Log($"Melting Blaze: armorDamage BEFORE Mathf.Min: {armorDamage}");

            // Apply Armor Damage (Always)
            if (armorDamage > 0)
            {
                float damageToArmor = Mathf.Min(armorDamage, enemy.enemyCurrentArmor);

                Debug.Log($"Melting Blaze: damageToArmor: {damageToArmor} AFTER Mathf.Min");

                enemy.enemyCurrentArmor -= damageToArmor;
                Debug.Log($"Melting Blaze Armor Damage: {damageToArmor}. Remaining Armor: {enemy.enemyCurrentArmor}");
            }

            // Apply Health Damage (Only if not a robot)
            if (!enemy.enemyStats.isRobot)
            {
                enemy.enemyCurrentHealth -= healthDamage;
                Debug.Log($"Melting Blaze Health Damage: {healthDamage}. Remaining Health: {enemy.enemyCurrentHealth}");
            }
            else
            {
                Debug.Log("Melting Blaze Health damage has no effect on robots");
            }

            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        Destroy(this);
    }
}