using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DBScript : MonoBehaviour
{
    // File imports
    private DatabaseReference databaseReference;
    public RandomQuiz attendancePuzzle;
    private Timer timerScript;

    // General variables
    private const int maxTimeLeftEntries = 6;
    public bool startGame;
    private bool isTimerStopped = false;
    public int currentUserID;
    public string currentUsername;
    float currentUserRecordTime = 0f;
    public Text showPosRankingText;
    public Text showNameRankingText;
    public Text showTimeRankingText;

    // Marker 1
    public InputField usernameInputField;
    public Text validationText;
    public Button startButton;
    public Canvas canvas;
    public Canvas rankingCanvas;
    public Canvas timerCanvas;

    private void Start()
    {
        rankingCanvas.enabled = false;
        databaseReference = FirebaseDatabase.GetInstance("https://morningrush-830ce-default-rtdb.firebaseio.com/").RootReference;
        timerScript = GetComponent<Timer>();
        startButton.onClick.AddListener(SaveUsername);
    }

    private IEnumerator GetNextUserID(Action<int> callback)
    {
        int nextUserID = 0;
        DatabaseReference usersReference = databaseReference.Child("users");
        var task = usersReference.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Failed to retrieve user data from the database.");
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        if (snapshot != null && snapshot.Exists)
        {
            nextUserID = (int)snapshot.ChildrenCount;
        }

        callback(nextUserID);
    }

    public void SaveUsername()
    {
        if (usernameInputField.text != "")
        {
            string username = usernameInputField.text;

            StartCoroutine(GetNextUserID((nextUserID) =>
            {
                DatabaseReference userReference = databaseReference.Child("users").Child(nextUserID.ToString());
                userReference.Child("username").SetValueAsync(username);

                canvas.enabled = false;
                startGame = true;
                currentUserID = nextUserID;

                if (timerScript != null)
                {
                    timerScript.StartTimer();
                }
            }));
        }
        else
        {
            validationText.text = "Please enter your initials.";
        }
    }

    private void Update()
    {
        if (attendancePuzzle.attended && !isTimerStopped)
        {
            StopTimerAndStoreTime();
            isTimerStopped = true;
        }
    }

    private void StopTimerAndStoreTime()
    {
        if (timerScript != null)
        {
            timerScript.StopTimer();
            float recordTime = timerScript.GetTimeElapsed();
            StoreTimeInDatabase(recordTime);
        }
    }

    private void StoreTimeInDatabase(float recordTime)
    {
        DatabaseReference userReference = databaseReference.Child("users").Child(currentUserID.ToString());
        userReference.Child("recordTime").SetValueAsync(recordTime.ToString()).ContinueWith(storeTask =>
        {
            if (storeTask.IsFaulted)
            {
                Debug.Log("Failed to store recordTime data in the database.");
                return;
            }

            Debug.Log("RecordTime data stored successfully.");
        });

        StartCoroutine(RetrieveRankingList(currentUserID));
    }

    private IEnumerator RetrieveRankingList(int currentUserID)
    {
        DatabaseReference usersReference = databaseReference.Child("users");
        var task = usersReference.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Failed to retrieve user data from the database.");
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        if (snapshot != null && snapshot.Exists)
        {
            List<KeyValuePair<string, float>> userRecordTimes = new List<KeyValuePair<string, float>>();

            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                int userID = Convert.ToInt32(userSnapshot.Key);
                string username = userSnapshot.Child("username").Value as string;
                float recordTime = 0f;

                if (userSnapshot.Child("recordTime").Value != null)
                {
                    recordTime = Convert.ToSingle(userSnapshot.Child("recordTime").Value);
                }

                if(currentUserID == userID) {
                    currentUsername = username;
                    currentUserRecordTime = recordTime;
                }

                userRecordTimes.Add(new KeyValuePair<string, float>(username, recordTime));
            }

            userRecordTimes.Sort((x, y) => x.Value.CompareTo(y.Value));
            List<KeyValuePair<string, float>> top5RecordTimes = userRecordTimes.Take(5).ToList();

            for (int i = 0; i < top5RecordTimes.Count; i++)
            {
                string users = top5RecordTimes[i].Key;
                float recordTimes = top5RecordTimes[i].Value;
            }

            int currentUserRank = userRecordTimes.FindIndex(x => x.Key == currentUsername) + 1; 
            DisplayRankingList(top5RecordTimes, currentUserRank);
        }
    }

    private void DisplayRankingList(List<KeyValuePair<string, float>> recordTimes, int currentUserRank)
    {
        rankingCanvas.enabled = true;
        timerCanvas.enabled = false;
        string rankingPosListText = "", rankingNameListText = "", rankingTimeListText = "";

        for (int i = 0; i < recordTimes.Count; i++)
        {
            string username = recordTimes[i].Key;
            float recordTime = recordTimes[i].Value;

            int minutes = Mathf.FloorToInt(recordTime / 60f);
            int seconds = Mathf.FloorToInt(recordTime % 60f);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            rankingPosListText += $"#{i + 1}\n\n";
            rankingNameListText += $"{username}\n\n";

            if (recordTime > 300f) 
            {
                int lateMinutes = minutes - 5;
                int lateSeconds = seconds;
                string formattedLateTime = string.Format("{0:00}:{1:00}", lateMinutes, lateSeconds);
                rankingTimeListText += $"<color=red>{formattedLateTime}</color>\n\n";
            }
            else
            {
                rankingTimeListText += $"<color=#00FF00>{formattedTime}</color>\n\n";
            }
        }

        int currentUserMinutes = Mathf.FloorToInt(currentUserRecordTime / 60f);
        int currentUserSeconds = Mathf.FloorToInt(currentUserRecordTime % 60f);
        string formattedCurrentUserTime = string.Format("{0:00}:{1:00}", currentUserMinutes, currentUserSeconds);

        if (currentUserRecordTime > 300f) 
        {
            int lateMinutes = currentUserMinutes - 5;
            int lateSeconds = currentUserSeconds;
            string formattedLateTime = string.Format("{0:00}:{1:00}", lateMinutes, lateSeconds);
            rankingTimeListText += $"\n\n\n<color=red>{formattedLateTime}</color>";
        }
        else
        {
            rankingTimeListText += $"\n\n\n<color=#00FF00>{formattedCurrentUserTime}</color>";
        }

        rankingPosListText += $"\n\n\n#{currentUserRank}";
        rankingNameListText += $"\n\n\n{currentUsername}";

        showPosRankingText.text = rankingPosListText;
        showNameRankingText.text = rankingNameListText;
        showTimeRankingText.text = rankingTimeListText;
    }
}
