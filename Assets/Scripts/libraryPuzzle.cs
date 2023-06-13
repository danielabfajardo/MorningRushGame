using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class libraryPuzzle : MonoBehaviour
{
    public Text questionText;
    public InputField answerInputField;
    public Button submitButton;
    public Text resultText;

    private Dictionary<string, string> bookAnswers = new Dictionary<string, string>()
    {
        { "Which year was Ewha founded?", "1886" },
        { "Who founded Ewha?", "Mary Scranton" },
        { "When was the book 형식언어와 오토마타론 / 유은진, 방혜자 published?", "2004" },
        { "What is the book serial number for 가상현실 : 미래는 바로 우리 눈 앞에 있다 / 오컴 지음?", "006.8가61현" },
        { "When was the book 가상현실 : 증강 현실과 VRML / 박경배 저  published?", "2012" },
        { "What is the book serial number for 데이터베이스 응용 / 정정수 저?", "005.74정813ㄷ" },
        { "What is the name of the book for 621.3815 디819ㅌ?", "디지털 논리회로" },
        { "Write the first name of the authors for 621.381 H24d한", "David Sarah" },
        { "Which professor teaches the Virtual Reality class?", "오유란" },
        { "Which floor is the convenience store in this building?", "1" },
    };

    public bool missionComplete = false;
    public ST_PuzzleDisplay stPuzzle;

    private List<string> unansweredBooks;
    private string selectedBook;
    private string designatedAnswer;
    private int correctAnswersCount;
    private int questionsToAsk = 3;

    void Start()
    {
        unansweredBooks = new List<string>(bookAnswers.Keys);
        AskNewQuestion();
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    void Update() {
        AskNewQuestion();
    }

    void AskNewQuestion()
    {
        if (stPuzzle.Complete) {
            if (unansweredBooks.Count > 0)
            {
                int randomIndex = Random.Range(0, unansweredBooks.Count);
                selectedBook = unansweredBooks[randomIndex];
                designatedAnswer = bookAnswers[selectedBook];
                questionText.text = string.Format("{0}", selectedBook);
                unansweredBooks.RemoveAt(randomIndex);
            }
        } else {
            questionText.text = "Complete the previous puzzle to see the quiz!";
            answerInputField.interactable = false;
            submitButton.gameObject.SetActive(false);
        }
    }

    void SubmitAnswer()
    {
        string userAnswer = answerInputField.text;
        if (userAnswer.ToLower().Replace(" ", "") == designatedAnswer.ToLower().Replace(" ", ""))
        {
            answerInputField.text = "";
            resultText.text = "<color=#00FF00>Correct!</color>";
            correctAnswersCount++;

            if (correctAnswersCount == questionsToAsk)
            {
                missionComplete = true;
                resultText.text = "Yeah, you did that! Your next mission is waiting for you at Asan Eng. Building Room#122.";
                Debug.Log("missionComplete: ");
                Debug.Log(missionComplete);

                questionText.gameObject.SetActive(false);
                answerInputField.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);
            }
            else
            {
                AskNewQuestion();
            }
        }
        else
        {
            answerInputField.text = "";
            resultText.text = "<color=red>Incorrect!</color>";
        }
    }
}


