using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    public GameObject LoginCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void CloseLoginWindow()
    {
        LoginCanvas.SetActive(false); // Desactivar el canvas de Login
    }

    public void OpenLoginWindow()
    {
        LoginCanvas.SetActive(true); // Activar el canvas de Login
    }

    //public void LogInButtonPressA()
    //{


    //}

}
