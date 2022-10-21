using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private int defSpawnVal;
    private int _curEnemiesToSpawn;
    private int _enemiesThreshold;
    private int _curEnemies;
    
    public int maxWaves;
    private int _curWave;
    [SerializeField] private float _curTimer = 0f;
    [SerializeField] private float timerMax;
    
    #region  Singleton

    [SerializeField] private static EnemyManager _instance;
    
    public static EnemyManager Instance
    {
        get => _instance;
        set => _instance = value;
    }
    
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _curEnemiesToSpawn = defSpawnVal;
        SpawnEnemy(_curEnemiesToSpawn);
    }

    private void Update()
    {
        if(_curEnemies < _enemiesThreshold)
            _curTimer -= Time.deltaTime;
        
        if (_curTimer <= 0f)
        {
            _curWave++;
            _curEnemiesToSpawn = _curEnemiesToSpawn * _curWave;
            SpawnEnemy(_curEnemiesToSpawn);
            _curTimer = timerMax;
        }
    }

    void SpawnEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(enemyPrefab, RandomPointInCameraView(), Quaternion.identity) as GameObject;
            _curEnemies++;
        }
    }

    Vector3 RandomPointInCameraView()
    {
        return _camera.ViewportToWorldPoint(new Vector3(Random.Range(0, Screen.width),Random.Range(0,Screen.height),
            _camera.farClipPlane/2));
    }
}
