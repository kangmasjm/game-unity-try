using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private List<QuizDataSO> quizDataSO;
    [SerializeField] private float timeLimit = 60f;

    private List<Question> questList;
    private Question selectedQuest;
    private int scoreCount = 0;
    private float currentTime;
    private int lifePoint = 3;
    
    //current question data
    private Question selectedQuetion = new Question();

    private GameStatus gameStatus = GameStatus.Next;
    public GameStatus @GameStatus { get { return gameStatus; } }

    public void StartGame(int index) 
    {
        scoreCount = 0;
        currentTime = timeLimit;
        lifePoint = 3;

        questList = new List<Question>();

        for (int i = 0; i< quizDataSO[index].questions.Count; i++) {
            questList.Add(quizDataSO[index].questions[i]);
        }

        SelectQuestion(); 
        gameStatus = GameStatus.Playing;
    }

    void Update()
    {
        if (gameStatus == GameStatus.Playing) {
            currentTime -= Time.deltaTime;
            SetTimer(currentTime);
        }
    }

    void SetTimer(float value) {
        TimeSpan time = TimeSpan.FromSeconds(value);
        quizUI.TimerText.text = "" + time.ToString("mm':'ss");

        if (currentTime <= 0) {
            gameStatus = GameStatus.Next;
            quizUI.GameOverPanel.SetActive(true);
        }
    }

    private void SelectQuestion()
    {
        //get the random number
        int val = UnityEngine.Random.Range(0, questList.Count);
        //set the selectedQuetion
        selectedQuetion = questList[val];
        //send the question to quizGameUI
        quizUI.SetQuestion(selectedQuetion);

        questList.RemoveAt(val);
        
    }

    public bool Answer(string selectedOption) 
    {
        //set default to false
        bool correct = false;
        //if selected answer is similar to the correctAns
        if (selectedQuetion.correctAns == selectedOption)
        {
            //Yes, Ans is correct
            correct = true;
            scoreCount += 50;
            quizUI.ScoreText.text = ""+scoreCount;
        }
        else
        {
            //No, Ans is wrong
            lifePoint--;
            //scoreCount -=50;
            //quizUI.ScoreText.text = ""+scoreCount;
            quizUI.ReduceLife(lifePoint);
        }

        if (gameStatus == GameStatus.Playing) {
            if (questList.Count > 0) {
                Invoke("SelectQuestion",1f);
            }
            else {
                gameStatus = GameStatus.Next;
                quizUI.GameOverPanel.SetActive(true);
            }
        }

        //return the value of correct bool
        return correct;
    }

}

//Data  structure for storing the quetions data
[System.Serializable]
public class Question
{
    public string questionInfo;         //question text
    public QuestionType questionType;   //type
    public Sprite questionImage;        //image for Image Type
    public AudioClip audioClip;         //audio for audio type
    public List<string> options;        //options to select
    public string correctAns;           //correct option
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO
}

[System.Serializable]
public enum GameStatus {
    Next,
    Playing
}