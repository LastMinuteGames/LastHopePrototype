﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BossManager : MonoBehaviour
{
    public static BossManager instance = null;

    public BossPhase[] bossPhases;
    public BossPhase currentPhase;

    public Slider hpSlider;
    public GameObject canvasGO;

    public GameObject bodyGO;
    public Texture emisiveIdle;
    public Texture emisiveMortar;
    public Texture emisiveFist;
    private Material bodyMaterial;
    //public GameObject rocketSpawnManager;
    

    private int currentPhaseId;
    private bool isDead = false;
    private bool isAwaken = false;
    private Animator animator;
    private FreeLookCam freeLookCam;
    private BossCam bossCam;

    private GameObject plasmaRayGO;
    private GameObject armAttackGO;
    private TurretsManager turretsManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //StartBossFight();
        animator = GetComponent<Animator>();
        freeLookCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeLookCam>();
        bossCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BossCam>();
        plasmaRayGO = transform.Find("Root/Neck/Head/PlasmaRay").gameObject;
        armAttackGO = transform.Find("Root/L_Clavicle/L_Biceps/L_Forearm/L_Hand/ArmAttack").gameObject;
        turretsManager = GameObject.FindGameObjectWithTag("TurretManager").GetComponent<TurretsManager>();
        bodyMaterial = bodyGO.GetComponent<SkinnedMeshRenderer>().material;
    }

    public void StartBossFight()
    {
        Debug.Log("start boss fight");
        turretsManager.RestartBossCombat();

        isAwaken = true;
        //if (rocketSpawnManager != null)
        //{
        //    GameObject prefab;
        //    prefab = (GameObject)Instantiate(rocketSpawnManager);
        //    prefab.name = "RocketSpawnManager";
        //}
        UpdateHpSlider();

        canvasGO.SetActive(true);
        if (bossCam)
        {
            bossCam.StartFollowing();
        }
        if (bossPhases.Length > 0)
        {
            currentPhaseId = 0;
            currentPhase = bossPhases[currentPhaseId];
            currentPhase.StartPhase();
        }
    }

    void Update()
    {
        if (isAwaken && !isDead)
        {
            currentPhase.UpdatePhase();
        }
    }

    void TerminateCurrentPhase()
    {
        currentPhase.TerminatePhase();
        currentPhaseId++;
        if (currentPhaseId < bossPhases.Length)
        {
            currentPhase = bossPhases[currentPhaseId];
            currentPhase.StartPhase();
        }
        else
        {
            BossDeath();
        }
    }

    public void TurretAttack()
    {
        if (isDead)
        {
            return;
        }
        TerminateCurrentPhase();
        animator.SetTrigger("damaged");
        UpdateHpSlider();
    }

    void BossDeath()
    {
        animator.SetTrigger("dead");
        isDead = true;
        isAwaken = false;
        canvasGO.SetActive(false);
        Invoke("Dead", 4.0f);
    }

    void UpdateHpSlider()
    {
        hpSlider.value = 100 * (bossPhases.Length - currentPhaseId) / (float)bossPhases.Length;
    }

    public void Dead()
    {
        SceneManager.LoadScene("WinScreen");
    }



    #region Emisive management functions
    public void SetEmisiveIddle()
    {
        bodyMaterial.SetTexture("_EmissionMap", emisiveIdle);
    }

    public void SetEmisiveMortar()
    {
        bodyMaterial.SetTexture("_EmissionMap", emisiveMortar);
    }

    public void SetEmisiveFist()
    {
        bodyMaterial.SetTexture("_EmissionMap", emisiveFist);
    }
    #endregion



    #region EventAttacks animation-related functions

    public void StartRay()
    {
        plasmaRayGO.SetActive(true);
    }
    public void EndRay()
    {
        plasmaRayGO.SetActive(false);
    }
    public void EnableArmAttackCollider()
    {
        armAttackGO.SetActive(true);
    }
    public void DisableArmAttackCollider()
    {
        armAttackGO.SetActive(false);
    }
    #endregion


}
