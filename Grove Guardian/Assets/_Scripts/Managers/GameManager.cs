using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    [field:Header("Difficulty")]
    [field:SerializeField] public int PlayerHealthBonus { get; private set; }
    [field:SerializeField] public int PlayerDamageBonus { get; private set; }
    [field:SerializeField] public int EnemyHealthBonus { get; private set; }
    [field:SerializeField] public int EnemyDamageBonus { get; private set; }

    
    [field:Header("Setup")]
    [field:SerializeField] public GameObject Player { get; private set; }
    [field:SerializeField] public GameObject Tree { get; private set; }
    [field:SerializeField] public AbilityHolder PlayerAbilities { get; private set; }
    [field:SerializeField] public Point Point { get; private set; }
    [field:SerializeField] public GameObject MoldUIthingi { get; private set; }
    [field:SerializeField] public GameObject WaveCountGo { get; private set; }
    [field: SerializeField] public GameObject SkillSelectionHint;
    private bool _overlayOpen = false;


    [HideInInspector] public int CurrentWave { get; private set; } = 1;

    public Action<GameState> OnBeforeStateChanged;
    public Action<GameState> OnAfterStateChanged;
    public Action OnNewWave;
    public GameState State { get; private set; }

    private void Start() {
        Screen.SetResolution(1920, 1080, fullscreen: true);
        ChangeState(GameState.Starting); 
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);
        State = newState;
        switch(newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawnEnemies:
                HandleSpawn();
                break;
            case GameState.Combat:
                HandleCombat();
                break;
            case GameState.DownTime:
                HandleDownTime();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
        }
        OnAfterStateChanged?.Invoke(newState);
    }
    private void HandleStarting() {
        ChangeState(GameState.SpawnEnemies);
    }

    private void HandleSpawn() {
        Debug.Log("new wave");
        OnNewWave?.Invoke();
        WaveManager.Instance.StartWaveSpawning();
    }

    private void HandleCombat() {
       
    }

    private void HandleDownTime() {
        Debug.Log("HandleDownTime: 25 sec to next wave");
        Invoke(nameof(ChangeStateToStart), 10f);
        CurrentWave++;
    }


    private void HandleWin() {

    }

    private void HandleLose() {

    }

    private void ChangeStateToStart() {
        ChangeState(GameState.Starting);
    }

    [Serializable]
    public enum GameState {
        Starting = 0,
        SpawnEnemies = 1,
        Combat= 2,
        DownTime = 3,
        Win = 4,
        Lose = 5,
    }
}
