﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.EnemySpawnSystem;

public enum EVENT_STATUS
{
    DISABLED,
    STARTED,
    FINISHED
}

public class GenericCombatEvent : MonoBehaviour, EnemyObserver
{
    public GameObject parentWallsEvent;
    public EnemySpawnManager manager;
    public GameObject reusableSpawnPointsParent;
    public float delayBetweenSpawns = 2.0f;
    protected EVENT_STATUS status = EVENT_STATUS.DISABLED;
    protected TargetController target;

    [SerializeField]
    protected EnemyBehaviour behaviour = EnemyBehaviour.EB_DEFAULT;

    [SerializeField]
    protected List<EnemySpawnPoint> reusableSpawnPoints = new List<EnemySpawnPoint>();
    protected List<GameObject> eventWalls = new List<GameObject>();
    protected List<Wave> waves = new List<Wave>();
    protected Wave currentWave;
    protected Dictionary<EnemyType, uint> enemiesPendingToSpawn = new Dictionary<EnemyType, uint>();
    protected List<Enemy> enemies = new List<Enemy>();
    protected float lastSpawnTime = 0;

    protected PlayerController player;

    public void AddEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.Autokill = false;
            enemy.behaviour = behaviour;
            enemies.Add(enemy);
        }
    }

    virtual protected void InitData()
    {
        status = EVENT_STATUS.DISABLED;

        lastSpawnTime = 0;
        enemiesPendingToSpawn.Clear();

        if (target != null)
            target.InitData();

        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();

        waves.Clear();


    }

    void Start()
    {

        target = transform.GetComponentInParent<TargetController>();

        foreach (Transform child in reusableSpawnPointsParent.transform)
        {
            reusableSpawnPoints.Add(child.GetComponent<EnemySpawnPoint>());
        }

        foreach (Transform child in parentWallsEvent.transform)
        {
            GameObject eventWall = child.gameObject;
            eventWall.SetActive(false);
            eventWalls.Add(eventWall);
        }

        InitData();
    }

    protected virtual void UpdateEvent()
    {
        if (currentWave.IsFinished()) //Next wave!
        {
            //currentWave.FinishDebug();
            waves.RemoveAt(0);
            if (waves.Count > 0)
            {
                currentWave = waves[0];
                List<Spawn> spawns = currentWave.StartWave();
                //Debug.Log("Spawns Start Wave: " + spawns.Count);
                AddSpawnsToPendingEnemies(spawns);
            }
            else
            {
                FinishedEvent();

                //string text = "Nice job! But the Colossal wall is under attack!";
                //string from = "";
                //DialogueSystem.Instance.AddDialogue(text, from, 3.5f);
            }
        }
        else //Update Waves!!!
        {
            Dictionary<EnemyType, uint> deadEnemies = CleanUpEnemies();
            AddSpawnsToPendingEnemies(currentWave.RemoveEnemies(deadEnemies));
            float seconds = Time.realtimeSinceStartup;
            List<EnemyType> keys = new List<EnemyType>(enemiesPendingToSpawn.Keys);
            for (int i = 0; i < keys.Count; ++i)
            {
                EnemyType type = keys[i];
                if (enemiesPendingToSpawn[type] > 0 && (lastSpawnTime == 0 || seconds - lastSpawnTime > delayBetweenSpawns))
                {
                    enemiesPendingToSpawn[type]--;
                    SpawnEnemyType(type);
                    lastSpawnTime = seconds;
                }
            }
        }
    }

    void Update()
    {
        if (status != EVENT_STATUS.STARTED || currentWave == null)
            return;

        if (player != null && player.IsDead())
        {
            InitData();
            return;
        }


        if (target == null || (target != null && target.alive))
        {
            UpdateEvent();
        }
        else if (target.alive == false && target.gameObject.activeSelf == true)
        {
            if (player != null)
                player.Respawn();
            InitData();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (status != EVENT_STATUS.STARTED && status != EVENT_STATUS.FINISHED)
            {
                player = other.gameObject.GetComponent<PlayerController>();
                EventStart();
            }
        }
    }

    virtual protected void EventStart()
    {
        status = EVENT_STATUS.STARTED;
        if (target != null)
        {
            target.hpSlider.gameObject.SetActive(true);
            target.alive = true;
        }
        //Debug.Log("Artillery event started");
        BlockExits();
    }

    virtual public void EventStart(PlayerController player)
    {
        this.player = player;
        EventStart();
    }

    protected void BlockExits()
    {
        foreach (GameObject eventWall in eventWalls)
        {
            eventWall.SetActive(true);
        }
    }

    protected void UnblockExits()
    {
        foreach (GameObject eventWall in eventWalls)
        {
            eventWall.SetActive(false);
        }
    }

    virtual protected void FinishedEvent()
    {
        currentWave = null;
        status = EVENT_STATUS.FINISHED;

        UnblockExits();
    }

    /*****************     Enemy Management ********************/

    protected Dictionary<EnemyType, uint> CleanUpEnemies()
    {
        Dictionary<EnemyType, uint> result = new Dictionary<EnemyType, uint>();
        int before = enemies.Count;
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i].IsDead() && enemies[i].Autokill == true)
            {
                //enemies[i].Autokill = true;
                if (result.ContainsKey(enemies[i].enemyType) == false)
                    result[enemies[i].enemyType] = 0;
                enemies[i].Dead();
                result[enemies[i].enemyType]++;
                //Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
                --i;
            }
        }

        return result;
    }

    protected void AddSpawnsToPendingEnemies(List<Spawn> spawns)
    {
        if (spawns.Count > 0)
        {
            foreach (Spawn spawn in spawns)
            {
                if (enemiesPendingToSpawn.ContainsKey(spawn.type))
                {
                    enemiesPendingToSpawn[spawn.type] += spawn.number;
                }
                else
                {
                    enemiesPendingToSpawn[spawn.type] = spawn.number;
                }
            }
        }
    }

    protected void SpawnEnemyType(EnemyType type)
    {
        int index = (int)UnityEngine.Random.Range(0, reusableSpawnPoints.Count - 1);
        manager.SpawnEnemy(reusableSpawnPoints[index], type, this);
    }
}

