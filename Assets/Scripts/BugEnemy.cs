using UnityEngine;
using System.Collections;

public class BugEnemy : MonoBehaviour
{
    public float speed = 3f;
    public int health = 100;
    public int damage = 10;

    private int maxHealth;
    private Transform firewall;
    private Animator animator;
    private EnemyHealthBar healthBar;
    private bool isDead = false;
    private bool hasAttacked = false;

    private enum State { Walking, Attacking, Dead }
    private State currentState = State.Walking;

    void Start()
    {
        maxHealth = health;
        firewall = GameObject.FindWithTag("Firewall").transform;
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        if (healthBar != null)
            healthBar.SetHealth(health, maxHealth);
    }

    void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case State.Walking:
                MoveTowardFirewall();
                break;
            case State.Attacking:
                AttackFirewall();
                break;
            case State.Dead:
                Die();
                break;
        }
    }

    void MoveTowardFirewall()
    {
        Vector3 targetPos = new Vector3(firewall.position.x,
            transform.position.y, firewall.position.z);

        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (animator != null)
            animator.SetBool("Walk", true);

        if (Vector3.Distance(transform.position, targetPos) < 2f)
            currentState = State.Attacking;
    }

    void AttackFirewall()
    {
        if (hasAttacked) return;
        hasAttacked = true;

        if (animator != null)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Attack");
        }

        FirewallHealth fw = firewall.GetComponent<FirewallHealth>();
        if (fw != null)
            fw.TakeDamage(damage);

        StartCoroutine(DieAfterAttack());
    }

    IEnumerator DieAfterAttack()
    {
        yield return new WaitForSeconds(1.5f);
        Die();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        health -= amount;

        if (healthBar != null)
            healthBar.SetHealth(health, maxHealth);

        if (animator != null)
            animator.SetTrigger("TakeDamage");

        if (health <= 0)
        {
            if (GameManager.instance != null)
                GameManager.instance.AddScore(10);
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        currentState = State.Dead;

        if (healthBar != null)
            healthBar.SetHealth(0, maxHealth);

        if (animator != null)
            animator.SetBool("Death", true);

        Destroy(gameObject, 2f);
    }
}
