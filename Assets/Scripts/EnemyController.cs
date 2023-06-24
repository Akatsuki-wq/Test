using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    //敌人移动速度
    public float speed = 3.0f;
    //敌人当前生命值
    private float currentHealth;
    //敌人最大生命值
    public float maxHealth = 100.0f;
    public float health
    {
        get
        {
            return currentHealth;
        }
    }
    //移动距离
    private float travelDistance = 1.0f;
    private Vector2 movement;
    //敌人朝向,1为右，-1为左
    private int _direction = -1;
    //敌人移动时间（单程）
    public float travelTime = 5.0f;
    private float _travelTime;
    //敌人死亡粒子特效
    public GameObject deathEffect;
    //敌人显示血量
    public Image healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        movement = new Vector2(travelDistance, 0);

        _travelTime = travelTime;
    }

    void Update()
    {
        Orientation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 修改敌人生命值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(float amount)
    {

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        float percent = currentHealth / maxHealth;
        healthBar.fillAmount = percent;

        if (currentHealth == 0)
        {
            GameObject effect = Instantiate(deathEffect, rb.position, Quaternion.identity);
            Destroy(gameObject);
        }

        Debug.Log("敌人生命值：" + currentHealth + "/" + maxHealth);
    }

    /// <summary>
    /// 修改敌人朝向
    /// </summary>
    private void Orientation()
    {
        if (_direction == -1)
        {
            _travelTime -= Time.deltaTime;

            if (_travelTime < 0)
            {
                _direction = 1;

                _travelTime = travelTime;

                transform.localScale = new Vector3(-7, 7, 7);
            }
        }
        else
        {
            _travelTime -= Time.deltaTime;

            if (_travelTime < 0)
            {
                _direction = -1;

                _travelTime = travelTime;

                transform.localScale = new Vector3(7, 7, 7);
            }
        }
    }

    /// <summary>
    /// 敌人移动
    /// /// </summary>
    private void Move()
    {
        if (_direction == -1)
        {
            rb.MovePosition(rb.position - movement * Time.fixedDeltaTime * speed);
        }
        else
        {
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * speed);
        }
    }

    /// <summary>
    /// 敌人碰到角色后，角色后退并掉血
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController controller = other.gameObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (_direction == -1)
            {
                controller.ChangeHealth(-10.0f);
                controller.Recoil(_direction);
            }
            else
            {
                controller.ChangeHealth(-10.0f);
                controller.Recoil(_direction);
            }
        }
    }
}
