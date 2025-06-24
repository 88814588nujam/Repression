using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public enum CharaType
    {
        Player,
        Enemy
    }
    public float maxHealth = 1000.0f;
    public float currentHealth = 0.0f;
    public CharaType charaType = CharaType.Player;

    private Transform firstLayerChild;
    private Transform secondLayerChild;
    private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        firstLayerChild = transform.Find("Avatar");
        if (firstLayerChild != null) secondLayerChild = firstLayerChild.Find("Health");
        if (secondLayerChild != null) healthBar = secondLayerChild.GetComponent<HealthBar>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(Mathf.Round((currentHealth < 0.0f ? 0.0f : currentHealth) / maxHealth * 10.0f) / 10.0f);
        if (currentHealth <= 0.0f) Die();
    }

    private void Die()
    {
        switch (charaType)
        {
            case CharaType.Player:
                PlayerExplosion playerExplosion = GetComponent<PlayerExplosion>();
                if (playerExplosion != null) playerExplosion.isExplosion = true;
                break;
            case CharaType.Enemy:
                EnemyExplosion enemyExplosion = GetComponent<EnemyExplosion>();
                if (enemyExplosion != null) enemyExplosion.isExplosion = true;
                break;
        }
    }
}