using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public GameObject howToPlayPanel;
 
    private void Start()
    {
        howToPlayPanel.SetActive(false);
    }
    public void OnStartGame()
    {
        GameManager.Instance.StartGame();
    }
    public void OnHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }
    public void OnCloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
    public void OnQuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
