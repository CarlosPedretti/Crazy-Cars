using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float totalTime = 60f;
    private float currentTime;
    private bool isCounting = false;

    public TMP_Text timerText;

    // Variables para llevar el registro de los puntajes de los jugadores
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();
    private List<PlayerConfiguration> playerConfigurations = new List<PlayerConfiguration>(); // Agrega esta l�nea

    public int maxScorePoints;

    public int pointPerPlayer;


    PlayerConfiguration playerConfiguration;

    // Variable est�tica para mantener la referencia a la �nica instancia del GameManager
    private static GameManager instance;

    // Propiedad p�blica para acceder a la �nica instancia del GameManager desde otros scripts
    public static GameManager Instance
    {
        get
        {
            // Si la instancia no existe, intentamos encontrarla en la escena
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            // Si a�n no se encuentra la instancia, lanzamos una advertencia
            if (instance == null)
            {
                Debug.LogWarning("No se encontr� el GameManager en la escena. Aseg�rate de tener un objeto GameManager en la escena.");
            }

            return instance;
        }
    }

    //TIMER

    private void Start()
    {
        currentTime = totalTime;
        isCounting = true;
        UpdateTimerText();
    }

    private void Update()
    {
        if (isCounting)
        {
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                currentTime = 0f;
                UpdateTimerText();
                StopCounting();
                // Aqu� puedes realizar acciones cuando el tiempo se haya agotado
            }
        }
    }


    public void StartCounting()
    {
        isCounting = true;
    }

    public void StopCounting()
    {
        isCounting = false;
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    // M�todo para aumentar el puntaje de un jugador cuando mate a otro
    public void IncreaseScore(int playerIndex)
    {
        // Verificar si el jugador ya existe en el diccionario
        if (playerScores.ContainsKey(playerIndex))
        {
            // Si existe, aumentar su puntaje en 10
            playerScores[playerIndex] += 10;
            Debug.Log("IncreaseScore desde GameManager, aumentado en 10");
            UpdatePlayerUIs();
        }
        else
        {
            // Si no existe, agregar al jugador al diccionario con puntaje inicial de 10
            playerScores[playerIndex] = 10;
            Debug.Log("IncreaseScore desde GameManager, PUNTAJE INICIAL");
            UpdatePlayerUIs();
        }

        UpdatePlayerUIs();
        // L�gica adicional que desees realizar cuando un jugador mate a otro
        // Por ejemplo, mostrar un mensaje en pantalla, actualizar la UI, etc.
    }

    public void UpdatePlayerUIs()
    {
        foreach (var playerConfig in playerConfigurations)
        {
            if (playerConfig.PlayerUI != null)
            {
                var scoreUpdater = playerConfig.PlayerUI.GetComponent<PlayerUIScoreUpdater>();
                if (scoreUpdater != null)
                {
                    scoreUpdater.UpdatePlayerUI("Player " + (playerConfig.PlayerIndex + 1).ToString(), GetScore(playerConfig.PlayerIndex));
                }
            }
        }
    }

    public void AddPlayer(PlayerConfiguration playerConfig)
    {
        playerConfigurations.Add(playerConfig);
    }

    // M�todo para obtener el PlayerConfiguration del jugador seg�n su �ndice
    public PlayerConfiguration GetPlayerConfiguration(int playerIndex)
    {
        // Buscamos el PlayerConfiguration en la lista por su �ndice
        return playerConfigurations.Find(playerConfig => playerConfig.PlayerIndex == playerIndex);

    }

    public Weapon GetPlayerWeapon(int playerIndex)
    {
        // Aqu� obtenemos el PlayerConfiguration del jugador seg�n su �ndice
        PlayerConfiguration playerConfiguration = GetPlayerConfiguration(playerIndex);

        if (playerConfiguration != null)
        {
            // Devolvemos el componente Weapon del jugador desde su PlayerConfiguration
            return playerConfiguration.PlayerWeapon;
        }

        return null;
    }

    // M�todo para obtener el puntaje de un jugador espec�fico
    public int GetScore(int playerIndex)
    {
        // Verificar si el jugador existe en el diccionario
        if (playerScores.ContainsKey(playerIndex))
        {
            // Devolver el puntaje del jugador
            return playerScores[playerIndex];
        }
        else
        {
            // Si el jugador no existe en el diccionario, devolver un puntaje predeterminado (como 0)
            return 0;
        }
    }
}
