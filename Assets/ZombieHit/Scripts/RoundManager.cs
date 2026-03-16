using UnityEngine;
using UnityEngine.Events;

// Owns game state — timer, score, round lifecycle
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [SerializeField] private float roundDuration = 60f;

    public UnityEvent<int> ScoreUpdated;
    public UnityEvent<float> TimerUpdated;
    public UnityEvent RoundEnded;

    public int Score { get; private set; }
    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        Application.targetFrameRate = 60;
    }

    private void Start() => StartRound();

    private void Update()
    {
        if (!IsRunning) return;

        TimeLeft -= Time.deltaTime;
        TimerUpdated?.Invoke(TimeLeft);

        if (TimeLeft <= 0f) EndRound();
    }

    private void StartRound()
    {
        Score = 0;
        TimeLeft = roundDuration;
        IsRunning = true;
        ScoreUpdated?.Invoke(Score);
        TimerUpdated?.Invoke(TimeLeft);
    }

    private void EndRound()
    {
        TimeLeft = 0f;
        IsRunning = false;
        RoundEnded?.Invoke();
    }

    public void AddScore(int amount = 1)
    {
        if (!IsRunning) return;
        Score += amount;
        ScoreUpdated?.Invoke(Score);
    }
}
