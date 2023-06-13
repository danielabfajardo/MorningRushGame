using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class libraryEntranceHint : MonoBehaviour
{
    public ST_PuzzleDisplay stPuzzle;
    public Text hintTextFront;
    public Text hintTextBack;
    public Text libraryHintTitle;
    public RawImage booksImage;

    void Start () 
    {
        if (stPuzzle.Complete)
        {
            hintTextFront.text = "Hint: Go to where endless shelves hold the keys to academic enlightenment.";
            hintTextBack.text = "Hint: Go to where endless shelves hold the keys to academic enlightenment.";
            libraryHintTitle.text = "Ready for your next adventure?";
            booksImage.enabled = true;
        }
        else
        {
            hintTextFront.text = "Please complete the previous mission first!";
            hintTextBack.text = "";
            libraryHintTitle.text = "";
            booksImage.enabled = false;
        }
    }
}
