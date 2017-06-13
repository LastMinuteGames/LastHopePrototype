﻿using Assets.Scripts.EnemySpawnSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    public ParticleSystem landingParticles;
    public float heightOffset;
    public float fallDuration;
    public GameObject enemy;
    public GameObject decal;

    private float destinationY;
    private Vector3 startOffset;
    private float interpolationTime = 0;
    private float currentHeight;
    private float capsuleHeight;
    private EnemyObserver observer = null;

    private bool spawned = false;

    private float timer = 0f;
    private float timeToLive = 1f;

    public EnemyObserver Observer
    {
        get
        {
            return observer;
        }

        set
        {
            observer = value;
        }
    }

    void Start()
    {
        startOffset = new Vector3(0, heightOffset, 0);
        destinationY = gameObject.transform.position.y;
        gameObject.transform.position += startOffset;
        capsuleHeight = GetComponentInParent<CapsuleCollider>().height;
    }

    void Update()
    {
        currentHeight = Mathf.Lerp(startOffset.y, destinationY, interpolationTime);
        if (interpolationTime <= 1)
        {
            interpolationTime += Time.deltaTime / fallDuration;
        }
        Vector3 currentPos = new Vector3(gameObject.transform.position.x, currentHeight, gameObject.transform.position.z);
        gameObject.transform.position = currentPos;
        if (currentHeight <= destinationY)
        {
            if (!spawned)
            {
                StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().Shake(0.3f, 1.75f,1,1,this.transform));
                SpawnParticles();
                SpawnDecal();
                spawned = true;
                timer = 0f;

            }else
            {
                timer += Time.deltaTime;
                if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().done || timer >= timeToLive)
                {
                    Destroy(gameObject);
                }
            }

        }
    }

    void SpawnParticles()
    {
        Vector3 landingPos = transform.position;
        Instantiate(landingParticles, transform.position, transform.rotation);
        GameObject go = Instantiate(enemy, landingPos, transform.rotation);
        Enemy instantiatedEnemy = go.GetComponent<Enemy>();
        if(instantiatedEnemy != null && observer != null)
        {
            observer.AddEnemy(instantiatedEnemy);
        }
    }

    void SpawnDecal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        Quaternion hitRotation = Quaternion.Euler(90, Random.Range(0, 360), 0);
        Instantiate(decal, hit.point + new Vector3(0, 0.001f, 0), hitRotation);
    }
}