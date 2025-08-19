using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public static event Action<int, int> OnScoreDisplayed;

    int _totalScore, _totalSavedLemmings;

    [SerializeField] int _escapeValue = 1500;
    [SerializeField] FloatingScore _floatingScorePrefab;

    int _levelScore;
    int _currentSavedLemmings;
    LemmingGoal _lemmingGoal;
    GameUI _gameUI;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        Collectable.OnGetCollectable += Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
        LevelManager.OnNextLevel += LevelManager_OnNextLevel;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        Collectable.OnGetCollectable -= Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
        LevelManager.OnNextLevel -= LevelManager_OnNextLevel;
    }

    void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _levelScore = 0;
        _currentSavedLemmings = 0;
        _lemmingGoal = FindFirstObjectByType<LemmingGoal>();
        _gameUI = FindFirstObjectByType<GameUI>();
        UpdateCurrentScore(_totalScore + _levelScore);
    }

    void Collectable_OnGetCollectable(Vector2 position, int value)
    {
        _levelScore += value;
        CreateFloatingText(position, value);
        UpdateCurrentScore(_totalScore + _levelScore);
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth enemyHealth)
    {
        _levelScore += enemyHealth.ScoreValue;
        CreateFloatingText(enemyHealth.transform.position, enemyHealth.ScoreValue);
        UpdateCurrentScore(_totalScore + _levelScore);
    }

    void Lemming_OnAnyLemmingEscaped()
    {
        _currentSavedLemmings++;
        _levelScore += _escapeValue;
        if(!_lemmingGoal)
        {
            _lemmingGoal = FindFirstObjectByType<LemmingGoal>();
        }
        CreateFloatingText(_lemmingGoal.ScorePositon.position, _escapeValue);
        UpdateCurrentScore(_totalScore + _levelScore);
    }

    void LevelManager_OnNextLevel()
    {
        AddLevelScoreToTotal();
        OnScoreDisplayed?.Invoke(_totalScore, _totalSavedLemmings);
    }

    void AddLevelScoreToTotal() // TODO When level successfully completed or Game Over
    {
        _totalScore += _levelScore;
        _totalSavedLemmings += _currentSavedLemmings;
        _levelScore = 0;
        _currentSavedLemmings = 0;
    }

    void CreateFloatingText(Vector3 position, int value)
    {
        FloatingScore floatingScore = Instantiate(_floatingScorePrefab, position, Quaternion.identity);
        floatingScore.SetText(value.ToString());
    }

    void UpdateCurrentScore(int amount)
    {
        if(_gameUI)
        {
            _gameUI.SetTotalScore(amount);
        }
    }
}
