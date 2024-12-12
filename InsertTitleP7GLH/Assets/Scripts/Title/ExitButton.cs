using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(clicked);
    }
    private void clicked()
    {
        //note, this only works in actual builds
        //this does absolutely nothing in the editor
        Application.Quit();
    }
}
