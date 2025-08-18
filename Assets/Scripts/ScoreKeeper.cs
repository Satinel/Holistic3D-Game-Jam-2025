using UnityEngine;
using System;

public class ScoreKeeper : MonoBehaviour
{
    public static event Action<int> OnScoreDisplayed;

    public int Score { get; private set; }

    [SerializeField] int _escapeValue = 1500;
    [SerializeField] FloatingScore _floatingScorePrefab;

    int _levelScore;
    LemmingGoal _lemmingGoal;

    void Awake()
    {
        _lemmingGoal = FindFirstObjectByType<LemmingGoal>();
    }

    void OnEnable()
    {
        Collectable.OnGetCollectable += Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
        LevelManager.OnNextLevel += LevelManager_OnNextLevel;
    }

    void OnDisable()
    {
        Collectable.OnGetCollectable -= Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
        LevelManager.OnNextLevel -= LevelManager_OnNextLevel;
    }

    void Collectable_OnGetCollectable(Vector2 position, int value)
    {
        _levelScore += value;
        CreateFloatingText(position, value);
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth enemyHealth)
    {
        _levelScore += enemyHealth.ScoreValue;
        CreateFloatingText(enemyHealth.transform.position, enemyHealth.ScoreValue);
    }

    void Lemming_OnAnyLemmingEscaped()
    {
        _levelScore += _escapeValue;
        CreateFloatingText(_lemmingGoal.ScorePositon.position, _escapeValue);
    }
    void LevelManager_OnNextLevel()
    {
        AddLevelScoreToTotal();
        OnScoreDisplayed?.Invoke(Score);
    }

    void AddLevelScoreToTotal() // TODO When level successfully completed or Game Over
    {
        Score += _levelScore;
        _levelScore = 0;
    }

    void CreateFloatingText(Vector3 position, int value)
    {
        FloatingScore floatingScore = Instantiate(_floatingScorePrefab, position, Quaternion.identity);
        floatingScore.SetText(value.ToString());
    }
}
