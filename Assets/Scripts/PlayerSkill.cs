using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.ChangeHealth(-10.0f);
        }
        Destroy(gameObject);
    }
}
