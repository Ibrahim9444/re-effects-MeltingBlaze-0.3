using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    public float lifeTime;
    private float currentLifeTime;
    private float damage;
    private WeaponStatsSO.WeaponElementType weaponElement;
    private float fireDamage;
    private float toxicDamage;
    private float acidDamage;

    void Start()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

        if (currentLifeTime <= 0)
        {
            destroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit enemy!");
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage);

            WeaponController weaponController = GetComponentInParent<WeaponController>();
            if (weaponController != null)
            {
                float triggerChance = weaponController.currentElementTriggerChance;

                if (Random.value <= triggerChance)
                {
                    WeaponController.ActiveElementCombination activeCombination = weaponController.GetActiveElementCombination();

                    Debug.Log($"Active Combination: {activeCombination}, Triggered!");

                    //Set elemental damage before switch statement.
                    SetElementalDamage(weaponController.GetCurrentFireDamage(), weaponController.GetCurrentToxicDamage(), weaponController.GetCurrentAcidDamage());

                    switch (activeCombination)
                    {
                        case WeaponController.ActiveElementCombination.FireFire:
                            Debug.Log("Applying FireFire effect!");
                            ApplyInfernoPlague(enemyController);
                            break;
                        case WeaponController.ActiveElementCombination.FireToxic:
                            Debug.Log("Applying FireToxic effect!");
                            ApplyInfernoPlague(enemyController);
                            break;
                        case WeaponController.ActiveElementCombination.None:
                            Debug.Log("No elemental effect to apply.");
                            break;
                        default:
                            Debug.Log("Unknown elemental combination.");
                            break;
                        case WeaponController.ActiveElementCombination.FireAcid:
                        case WeaponController.ActiveElementCombination.AcidFire:
                            Debug.Log("Applying Melting Blaze effect!");
                            ApplyMeltingBlaze(enemyController);
                            break;
                    }
                }
                else
                {
                    Debug.Log("Elemental effect trigger failed.");
                }
            }
            else
            {
                Debug.LogWarning("WeaponController not found in parent.");
            }

            destroyProjectile();
        }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetEffect(WeaponStatsSO.WeaponElementType weaponElement)
    {
        this.weaponElement = weaponElement;
    }

    public void SetElementalDamage(float fireDamage, float toxicDamage, float acidDamage)
    {
        this.fireDamage = fireDamage;
        this.toxicDamage = toxicDamage;
        this.acidDamage = acidDamage;
    }

    // ProjectileController.cs
    private void ApplyInfernoPlague(EnemyController enemy)
    {
        Debug.Log("ApplyInfernoPlague called!");
        Debug.Log($"Fire Damage: {fireDamage}, Toxic Damage: {toxicDamage}");
        InfernoPlagueEffect effect = enemy.gameObject.AddComponent<InfernoPlagueEffect>();
        effect.ApplyEffect(enemy, fireDamage, toxicDamage);
    }
    private void ApplyMeltingBlaze(EnemyController enemy)
    {
        MeltingBlazeEffect meltingBlazeEffect = enemy.gameObject.AddComponent<MeltingBlazeEffect>();
        meltingBlazeEffect.ApplyEffect(enemy, fireDamage, acidDamage);
    }

    void destroyProjectile()
    {
        Destroy(gameObject);
    }
}