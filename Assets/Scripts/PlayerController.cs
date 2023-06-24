using System.Net.Mime;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;

    //角色移动速度
    public float speed = 5.0f;
    //角色当前生命值
    private float currentHealth;
    //角色最大生命值
    public float maxHealth = 100f;
    public float health
    {
        get
        {
            return currentHealth;
        }
    }
    //最大无敌时间，角色首次生命值等于0时的触发
    public float invincibleTime = 3f;
    //当前无敌时间
    private float _invincibleTime = 0;
    //角色是否处于无敌状态
    private bool invincible = false;
    //碰到敌人后，角色的后退距离
    public float recedingDistance = 1.0f;
    private Vector2 receding;
    //角色朝向，-1为左，1为右
    private int _direction = -1;
    public int direction
    {
        get
        {
            return _direction;
        }
    }
    //角色技能
    public GameObject skillPrefab;
    //技能冷却时间
    public float skillTime = 1.0f;
    private float time1 = 0;
    private float time2 = 0;
    //角色死亡粒子特效
    public GameObject deathEffect;
    //角色显示血量
    public Image healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        Orientation();
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), transform.position.y);
        if (invincible)
        {
            _invincibleTime -= Time.deltaTime;
            if (_invincibleTime < 0)
            {
                invincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            time1 = Time.fixedTime;
            if (time1 - time2 > skillTime)
            {
                Lauch(_direction);
                time2 = Time.fixedTime;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * speed);
    }

    /// <summary>
    /// 修改角色生命值
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
            // if (invincible)
            //     return;

            // invincible = true;
            // _invincibleTime = invincibleTime;
            // currentHealth += 10.0f;
        }

        Debug.Log("角色生命值：" + currentHealth + "/" + maxHealth);
    }

    /// <summary>
    /// 角色朝向，左转朝左，右转朝右
    /// </summary>
    private void Orientation()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            _direction = 1;
            transform.localScale = new Vector3(-7, 7, 7);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            _direction = -1;
            transform.localScale = new Vector3(7, 7, 7);
        }
    }

    /// <summary>
    /// 角色碰到敌人，后退
    /// </summary>
    /// <param name="direction"></param>
    public void Recoil(int direction)
    {
        if (direction == -1)
        {
            rb.position = new Vector2(rb.position.x - recedingDistance, rb.position.y);

        }
        else
        {
            rb.position = new Vector2(rb.position.x + recedingDistance, rb.position.y);
        }
    }

    /// <summary>
    /// 角色释放技能
    /// /// </summary>
    /// <param name="direction"></param>
    private void Lauch(int direction)
    {
        Vector2 skillPosition = new Vector2(rb.position.x + (direction * 0.5f), rb.position.y);
        GameObject skillObject = Instantiate(skillPrefab, skillPosition, Quaternion.identity);
        skillObject.transform.localScale = new Vector3(skillObject.transform.localScale.x * direction, skillObject.transform.localScale.y, skillObject.transform.localScale.z);
        PlayerSkill skill = skillObject.GetComponent<PlayerSkill>();
        skill.Launch(direction * Vector2.right, 300.0f);
    }
}
