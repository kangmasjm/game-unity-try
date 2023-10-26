using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//guna dari CreateAssetMenu adalah 
[CreateAssetMenu(fileName = "QuestionsData", menuName = "QuestionsData", order = 1)]
public class QuizDataSO : ScriptableObject
{
    public List<Question> questions;
}