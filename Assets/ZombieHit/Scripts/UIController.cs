using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Listens to RoundManager events and updates the UI
public class UIController : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private TextMeshProUGUI fpsLabel;

    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScoreLabel;

    private int _frames;
    private float _fpsTimer;

    private void Start()
    {
        endScreen.SetActive(false);

        RoundManager.Instance.ScoreUpdated.AddListener(OnScoreUpdated);
        RoundManager.Instance.TimerUpdated.AddListener(OnTimerUpdated);
        RoundManager.Instance.RoundEnded.AddListener(OnRoundEnded);
    }

    private void Update() => TrackFPS();

    private void OnScoreUpdated(int s) => scoreLabel.text = $"SCORE: {s}";

    private void OnTimerUpdated(float t)
    {
        int s = Mathf.CeilToInt(t);
        timerLabel.text = $"TIME: {s}";
        timerLabel.color = s <= 10 ? Color.red : Color.white;
    }

    private void TrackFPS()
    {
        _frames++;
        _fpsTimer += Time.deltaTime;
        if (_fpsTimer < 0.5f) return;
        fpsLabel.text = $"FPS: {Mathf.RoundToInt(_frames / _fpsTimer)}";
        _frames = 0; _fpsTimer = 0f;
    }

    private void OnRoundEnded()
    {
        endScreen.SetActive(true);
        endScoreLabel.text = $"{RoundManager.Instance.Score}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
