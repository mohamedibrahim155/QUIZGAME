using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnswerData : MonoBehaviour
{
    public bool isCorrect;
    public bool isIncorrect;
    public bool isClickedOnce;
    public QuizData data;
    
    private void Start()
    {
        data = FindObjectOfType<QuizData>();
        
    }
    public void ClickedButton()
    {      // displaying the Correct answer
        data.nextQuestionButton.gameObject.SetActive(true);
            if (isCorrect)
            {
           
                data.answerDisplay.text = "CORRECT ANSWER";
                data.answerDisplay.gameObject.SetActive(true);
                
            }
            else if (isIncorrect)
            {
                data.answerDisplay.text = "WRONG ANSWER";          
                data.answerDisplay.gameObject.SetActive(true);
               //deactivating the wrong answer and showing the correct answer
                for (int i = 0; i < data.IncorrectButtons.Count; i++)
                {
                    data.IncorrectButtons[i].SetActive(false);
                   
                }

            }
      
        
    }
   
}
