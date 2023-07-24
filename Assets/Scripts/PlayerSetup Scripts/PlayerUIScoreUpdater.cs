using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIScoreUpdater : MonoBehaviour
{
    [SerializeField] public TMP_Text nameText;
    [SerializeField] public TMP_Text scoreText;

    // M�todo para actualizar el nombre y puntaje en los Texts
    public void UpdatePlayerUI(string playerName, int score)
    {
        nameText.text = playerName;
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Updating UI for player: " + playerName + ", Score: " + score);

    }

}

