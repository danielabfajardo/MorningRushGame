using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRClassroomHint : MonoBehaviour
{
    public libraryPuzzle libraryPuzzleComplete;
    public GameObject hintMessageObject;
    private TextMeshPro hintMessage;

    void Start()
    {
        hintMessage = hintMessageObject.GetComponent<TextMeshPro>();
        if (libraryPuzzleComplete.missionComplete)
        {
            hintMessage.text = "Please take out your \nstudent ID for the attendance";
        }
        else
        {
            hintMessage.text = "Complete the previous \nmission to see this hint!";
        }
    } 

    void Update() {
        if (libraryPuzzleComplete.missionComplete)
        {
            hintMessage.text = "Please take out your \nstudent ID for the attendance";
        }
        else
        {
            hintMessage.text = "Complete the previous \nmission to see this hint!";
        }
    }
}
