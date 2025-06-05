using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    [SerializeField] public GameObject background1;
    [SerializeField] public GameObject background2;
    [SerializeField] public GameObject ground;
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public AudioSource music1;
    [SerializeField] public AudioSource music2;
    [SerializeField] public GameObject spawner1;
    [SerializeField] public GameObject spawner2;
    public float spawnTimeFactor = Spawner.instance.obstacleSpawnTimeFactor;
    public float speedFactor = Spawner.instance.obstacleSpeedFactor;
    public bool level1 = true;
    public static LevelChange instance;
    void Awake()
    {
        instance = this;
    }
    void Update()
    {   
        if(Spawner.instance.timeAlive > 20){
            ChangeLevelToTwo();
        }
    }
    public void ResetLevel(){
        SpawnerLevel2.instance.ResetFactors();
        SpawnerLevel2.instance.ClearObstacles();
        spawner2.SetActive(false);
        spawner1.SetActive(true);
        Spawner.instance.ResetFactors();
        background2.SetActive(false);
        background1.SetActive(true);
        player2.SetActive(false);
        player1.SetActive(true);
        ground.SetActive(true);
        music2.Stop();
        music1.Play();
        level1 = true;
    }
    public void ChangeLevelToTwo(){
        if(level1 == true){
            FadeCamera.instance.fadeTrigger = true;
            Spawner.instance.ClearObstacles();
            background1.SetActive(false);
            background2.SetActive(true);
            ground.SetActive(false);
            player1.SetActive(false);
            player2.SetActive(true);
            spawner1.SetActive(false);
            spawner2.SetActive(true);
            music1.Stop();
            music2.Play();
            level1 = false;
            FadeCamera.instance.fadeTrigger = true;
        }
    }
}
