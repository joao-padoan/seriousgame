using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLevel2 : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = LevelChange.instance.spawnTimeFactor;
    [Range(0, 1)] public float obstacleSpeedFactor = LevelChange.instance.speedFactor;
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeUntilObstacleSpawn;
    public float timeAlive;
    public static SpawnerLevel2 instance;

    void Awake()
    {
      instance = this;
    }
    private void Start(){
        GameManager.Instance.onPlay.AddListener(ResetFactors);
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
    }

    private void Update(){
        if(GameManager.Instance.isPlaying){
            timeAlive += Time.deltaTime;
            CalculateFactors();
            SpawnLoop();
        }
    }

    private void SpawnLoop(){
        timeUntilObstacleSpawn += Time.deltaTime;

        if(timeUntilObstacleSpawn >= _obstacleSpawnTime){
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    public void ClearObstacles(){
        foreach(Transform child in obstacleParent){
            Destroy(child.gameObject);
        }
    }
    
    private void CalculateFactors(){
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    public void ResetFactors(){
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn(){
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent;
        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * _obstacleSpeed;

    }
}
