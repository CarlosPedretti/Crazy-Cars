using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }

    public void TwoPlayers()
    {
        SceneManager.LoadScene("TwoPlayersMode");
    }

    public void TrheePlayers()
    {
        SceneManager.LoadScene("ThreePlayersMode");
    }

    public void FourPlayers()
    {
        SceneManager.LoadScene("FourPlayersMode");
    }
}