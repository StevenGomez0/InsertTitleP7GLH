using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        //button.onClick.AddListener(clicked);
    }
    private void clicked()
    {
        //MAKE SURE THIS ACTUALLY HAS THE SCENE NAME BEFORE SUBMITTING
        //SceneManager.LoadScene("basictest");
    }
}
