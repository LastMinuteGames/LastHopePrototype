﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image currentHPBar;
    
    public int maxHP = 100;
    public int currentHP;
    public Material defaultMaterial;
    public Material dmgedMaterial;
    public float timeBetweenDmg = 0.5f;

    private PlayerMovement moveScript;
    private bool dmged;
    private bool dead;
    private MeshRenderer meshRenderer;
    private float timer;

    void Awake()
    {
        currentHP = maxHP;
        moveScript = GetComponent<PlayerMovement>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (dmged)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenDmg)
            {
                dmged = false;
                meshRenderer.material = defaultMaterial;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
            TakeDmg(10);

        if (Input.GetKeyDown(KeyCode.H))
            Heal(5);

        if (dead == true && Input.GetKeyDown(KeyCode.R))
            Respawn();

        UpdateHPBar();
    }

    public void TakeDmg(int nasusQ)
    {
        if (!dead)
        {
            dmged = true;
            timer = 0;
            currentHP -= nasusQ;
            meshRenderer.material = dmgedMaterial;
            if (currentHP <= 0 && !dead)
                Die();
        }
    }
    public void Heal(int hp)
    {
        if (!dead && currentHP < maxHP)
        {
            currentHP += hp;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }
    }

    void Die()
    {
        dead = true;
        moveScript.enabled = false;
        meshRenderer.enabled = false;
    }

    void Respawn()
    {
        currentHP = maxHP;
        dead = false;
        moveScript.enabled = true;
        meshRenderer.enabled = true;
    }

    void UpdateHPBar()
    {
        float ratio = (float)currentHP / maxHP;
        currentHPBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}