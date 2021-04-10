using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;







public class QuizData : MonoBehaviour
{

    // class to read the  data
    [System.Serializable]
    public class Results
    {
        public string category;
        public string correctAnswer;
        public string[] incorrectAnswers;
        public string question;
        public string type;
    }

   // Variables
    public string jsonUrl;
    public Text category,type;
    public Text questionToDisplay;
    public Text answerDisplay;

    public List<string> questions = new List<string>(); 
    public List<string> correctAns = new List<string>(); 
    public List<string> incorrectAns = new List<string>();
    public Button[] Buttons;
    public List<GameObject> IncorrectButtons;
    public Button nextQuestionButton;
    public int currentIndex = 0;
    public GameObject LoadPanel,questionPanel;
   
    public void Start()
    {
        LoadPanel.SetActive(true);
        questionPanel.SetActive(true);
        jsonUrl = "https://trivia.willfry.co.uk/api/questions?categories=general_knowledge&limit=6";
        //declaring the coroutine
        StartCoroutine(ReadData());
        answerDisplay.gameObject.SetActive(false);
    }
    public IEnumerator ReadData()
    {
        WWW web = new WWW(jsonUrl);

        yield return web;
        LoadPanel.SetActive(false);
        questionPanel.SetActive(true);
        // if the url is valid 
        if (web.error == null)
        {
            processData(web.text);

        }
        else
        {
            // url is not valid or no network connectivity
            Debug.Log("ERORR>>>>>>>>>>>>>"); 
        }
       
    }

    public void processData(string _url)
    {
        Results[] jsonData = JsonHelper.GetArray<Results>(_url);

        foreach (Results r in jsonData)
        {
            // adding every read data into Lists
            questions.Add(r.question);
            correctAns.Add(r.correctAnswer);
            incorrectAns.Add(r.incorrectAnswers[0]);
            incorrectAns.Add(r.incorrectAnswers[1]);
            incorrectAns.Add(r.incorrectAnswers[2]);
        }
        // displaying question , catergory and type from the red data
        category.text = "CATEGORY : " + jsonData[currentIndex].category;
        type.text = "TYPE : " + jsonData[currentIndex].type;
        questionToDisplay.text = questions[currentIndex];

        //clearing the list to be empty of incorrect words
        IncorrectButtons.Clear();
        //  adding random correct answer in a button and accesing their data
        int CorrectAnsRandomPlacement = Random.Range(0, Buttons.Length - 1);
        Buttons[CorrectAnsRandomPlacement].GetComponentInChildren<Text>().text = correctAns[currentIndex];
        Buttons[CorrectAnsRandomPlacement].GetComponent<AnswerData>().isCorrect = true;
        Buttons[CorrectAnsRandomPlacement].GetComponent<AnswerData>().isIncorrect = false;

        // loop runs for placing inccorect answers in button
        for (int i = 0; i <Buttons.Length-1; i++)
        {
            if (i == CorrectAnsRandomPlacement)
            {
                // if the iterations matches with random correct answer placement, 
                //  we are moving the inccorrect iteration to the last index
                Buttons[Buttons.Length-1].GetComponentInChildren<Text>().text = incorrectAns[CorrectAnsRandomPlacement];
                Buttons[Buttons.Length - 1].GetComponent<AnswerData>().isIncorrect = true;
                Buttons[Buttons.Length - 1].GetComponent<AnswerData>().isCorrect = false;
                IncorrectButtons.Add(Buttons[Buttons.Length - 1].gameObject);
               
            }
            else
            {
                if (i!=CorrectAnsRandomPlacement)
                {
                // Here we add the incorrect answer to the buttons 
                Buttons[i].GetComponentInChildren<Text>().text = incorrectAns[i];
                Buttons[i].GetComponent<AnswerData>().isIncorrect = true;
                Buttons[i].GetComponent<AnswerData>().isCorrect = false;
                IncorrectButtons.Add(Buttons[i].gameObject);
                }
            }
        }
    }

    //This function is placed for the Next question button
    public void nextQuestion()
    {
       
        currentIndex++;
        //Removing the previous incorrect answers
        incorrectAns.RemoveRange(0, 3);
        answerDisplay.gameObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
        // If the last questions is over , taking to Level Complete scene
        if (currentIndex  ==questions.Count)
        {
            Debug.Log("YOU HAVE SUCCESFULLY COMPLETED");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        else
        {
            // Re-actvating the  Incorrect buttons after displaying the correct answer from previous
            for (int i = 0; i < IncorrectButtons.Count; i++)
            {
                IncorrectButtons[i].SetActive(true);
            }
           // re doing the Line no 91 to 125 
            DisplayingOptions();
        }
       

    }

    void DisplayingOptions()
    {
        questionToDisplay.text = questions[currentIndex];
        IncorrectButtons.Clear();
        int CorrectAnsRandomPlacement = Random.Range(0, Buttons.Length - 1);
        Buttons[CorrectAnsRandomPlacement].GetComponentInChildren<Text>().text = correctAns[currentIndex];
        Buttons[CorrectAnsRandomPlacement].GetComponent<AnswerData>().isCorrect = true;
        Buttons[CorrectAnsRandomPlacement].GetComponent<AnswerData>().isIncorrect = false;

       

        for (int i = 0; i < Buttons.Length - 1; i++)
        {

            if (i == CorrectAnsRandomPlacement)
            {
                
                Buttons[Buttons.Length - 1].GetComponentInChildren<Text>().text = incorrectAns[CorrectAnsRandomPlacement];
               
                Buttons[Buttons.Length - 1].GetComponent<AnswerData>().isIncorrect = true;
                Buttons[Buttons.Length - 1].GetComponent<AnswerData>().isCorrect = false;
                IncorrectButtons.Add(Buttons[Buttons.Length - 1].gameObject);

            }
            else
            {
                if (i != CorrectAnsRandomPlacement)
                {

                    Buttons[i].GetComponentInChildren<Text>().text = incorrectAns[i];
                    Buttons[i].GetComponent<AnswerData>().isIncorrect = true;
                    Buttons[i].GetComponent<AnswerData>().isCorrect = false;
                    IncorrectButtons.Add(Buttons[i].gameObject);
                }
            }
        }

    }
}

