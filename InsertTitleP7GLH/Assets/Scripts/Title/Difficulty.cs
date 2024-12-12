using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    //0 = easy, 1 = medium, 2 = hard
    public static int difficulty;
    [SerializeField] private int difficultyvalue;
    
    public void Click()
    {
        difficulty = difficultyvalue;
        //MAKE SURE THIS IS ON THE RIGHT SCENE BEFORE SUBMISSION
        SceneManager.LoadScene("basictest");
    }
}
