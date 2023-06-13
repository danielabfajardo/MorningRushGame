using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public DBScript dbScript; // Reference to the DBScript component

    public Text timerText;
    public float maxTime = 300f; // Maximum time in seconds (5 minutes)

    private float currentTime;
    private bool isTimerRunning = false;
    private Color timerNormalColor = new Color(0f, 0f, 0f);
    private Color timerWarningColor = new Color(1f, 0.5f, 0f);
    private Color timerExpiredColor = new Color(1f, 0f, 0f);
    private bool gameFinished = false;

    private void Start()
    {
        ResetTimer();
        timerNormalColor = timerText.color; // Set the initial normal color to the current color of the timer text
        timerText.text = "05:00";
    }

    private void Update()
    {
        if (isTimerRunning && !gameFinished)
        {
            currentTime += Time.deltaTime;

            // Check if time has reached the warning threshold (04:00)
            if (currentTime >= 240f)
            {
                timerText.color = timerWarningColor;
            }

            // Check if time has reached the expired threshold (05:00 or more)
            if (currentTime >= 300f)
            {
                timerText.color = timerExpiredColor;
            }

            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        isTimerRunning = false;
        gameFinished = false;
        timerText.color = timerNormalColor;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }

    public float GetTimeElapsed()
    {
        return currentTime;
    }
}
