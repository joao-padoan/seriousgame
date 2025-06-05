using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;
    [SerializeField] private TextMeshProUGUI maxEMGUI;
    [SerializeField] private TextMeshProUGUI instructions1;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Slider sliderUI1;
    [SerializeField] private Slider sliderUI2;
    public float configuraEMG;
    GameManager gm;
    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gm = GameManager.Instance;
        gm.onGameOver.AddListener(ActivateGameOverUI);
        sliderUI1.value = Spawner.instance.obstacleSpawnTimeFactor;
        sliderUI2.value = Spawner.instance.obstacleSpeedFactor;
    }
    public void PlayButtonHandler()
    {
        gm.StartGame();
        startMenuUI.SetActive(false);
    }
    public void SettingsButtonHandler()
    {
        settingsUI.SetActive(true);
        instructions1.enabled = false;
        startMenuUI.SetActive(false);
    }
    public void SliderHandler1()
    {
        Spawner.instance.obstacleSpawnTimeFactor = sliderUI1.value;
    }

    public void SliderHandler2()
    {
        Spawner.instance.obstacleSpeedFactor = sliderUI2.value;
    }

    IEnumerator DelayAction()
    {
        maxEMGUI.enabled = false;
        instructions1.enabled = true;
        yield return new WaitForSeconds(4);
        instructions1.enabled = false;
        configuraEMG = 3; //PlayerMovement.instance.firebaseValue;
        maxEMGUI.SetText("Valor de máxima contração: " + configuraEMG.ToString() + " (0 - 3)");
        maxEMGUI.enabled = true;
        print("Configura EMG: " + configuraEMG);
    }
    public void ConfigureEMGButtonHandler()
    {
        StartCoroutine(DelayAction());
    }
    public void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);
        gameOverScoreUI.text = "Pontuação: " + gm.PrettyScore();
        gameOverHighscoreUI.text = "Recorde: " + gm.PrettyHighscore();
    }
    private void OnGUI()
    {
        scoreUI.text = gm.PrettyScore();
    }
}
