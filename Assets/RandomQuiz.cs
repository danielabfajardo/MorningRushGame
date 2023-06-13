using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomQuiz : MonoBehaviour
{
    public TMPro.TMP_Text QuizText;
    public InputField answerInputField;
    public Button submitButton;
    public TMPro.TMP_Text resultText;

    public libraryPuzzle libraryMissionComplete;

    private Dictionary<int, string> mossAnswers = new Dictionary<int, string>()
    {
        {0, "- - - - -"},
        {1, "* - - - -"},
        {2, "* * - - -"},
        {3, "* * * - -"},
        {4, "* * * * -"},
        {5, "* * * * *"},
        {6, "- * * * *"},
        {7, "- - * * *"},
        {8, "- - - * *"},
        {9, "- - - - *"}
    };

    public bool attended = false;
    private int[] AttendanceCode = new int[4];
    private int answer;
    string AttendanceAnswer = "";
    public bool MissionComplete = false;

    void Start()
    {
        makeAttendanceCode();
        showQuiz();
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    void Update() {
        showQuiz();
    }

    void makeAttendanceCode()
    {
        for (int i = 0; i < 4; i++)
        {
            AttendanceCode[i] = Random.Range(0, 10);
            AttendanceAnswer += AttendanceCode[i].ToString();
        }
        Debug.Log(AttendanceAnswer);
    }

    public void showQuiz()
    {
        if (libraryMissionComplete.missionComplete)
        {
            if (!attended)
            {
                QuizText.text = string.Format("Please enter the attendance code referring the code below\n{0}\n{1}\n{2}\n{3}", mossAnswers[AttendanceCode[0]], mossAnswers[AttendanceCode[1]],
                    mossAnswers[AttendanceCode[2]], mossAnswers[AttendanceCode[3]]);
            }
            else
            {
                answerInputField.text = "";
                resultText.text = "Attendance Checked, Time: ";
            }
        }
        else
        {
            QuizText.text = "";
            answerInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            resultText.text = "Please complete the previous mission first!";
        }
    }

    public void SubmitAnswer()
    {	
        string userAnswer = answerInputField.text;
        Debug.Log(userAnswer+" "+AttendanceAnswer);
        if (userAnswer.Equals(AttendanceAnswer))
        {
            Debug.Log("Both are equal.....");
            resultText.text = "Attendance Checked, Time: ";
            attended = true;
    
            answerInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
        else
        {
            answerInputField.text = "";
            resultText.text = string.Format("Wrong Attendance Code. Try again\n{0}\n{1}\n{2}\n{3}", mossAnswers[AttendanceCode[0]], mossAnswers[AttendanceCode[1]],
            mossAnswers[AttendanceCode[2]], mossAnswers[AttendanceCode[3]]);
        }
    }
}
