using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterSolved : MonoBehaviour
{
    public bool entrancePuzzleFinished = true;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(gameObject.GetComponent<ST_PuzzleDisplay>().Complete == true){
            transform.GetChild(0).gameObject.SetActive(true);
            for(int i=1; i<=10; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
