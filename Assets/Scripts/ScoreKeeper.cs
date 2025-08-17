using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public int Score { get; private set; }

    [SerializeField] int _escapeValue = 1500;

    int _levelScore;

    void OnEnable()
    {
        Collectable.OnGetCollectable += Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
    }

    void OnDisable()
    {
        Collectable.OnGetCollectable -= Collectable_OnGetCollectable;
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
    }

    void Collectable_OnGetCollectable(int value)
    {
        _levelScore += value;
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth enemyHealth)
    {
        _levelScore += enemyHealth.ScoreValue;
    }

    void Lemming_OnAnyLemmingEscaped()
    {
        _levelScore += _escapeValue;
    }

    void AddLevelScoreToTotal() // TODO When level successfully completed or Game Over
    {
        Score += _levelScore;
        _levelScore = 0;
    }
}
