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
    private List<PlayerConfiguration> playerConfigurations = new List<PlayerConfiguration>(); // Agrega esta línea

    public int maxScorePoints;

    public int pointPerPlayer;


    PlayerConfiguration playerConfiguration;

    // Variable estática para mantener la referencia a la única instancia del GameManager
    private static GameManager instance;

    // Propiedad pública para acceder a la única instancia del GameManager desde otros scripts
    public static GameManager Instance
    {
        get
        {
            // Si la instancia no existe, intentamos encontrarla en la escena
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            // Si aún no se encuentra la instancia, lanzamos una advertencia
            if (instance == null)
            {
                Debug.LogWarning("No se encontró el GameManager en la escena. Asegúrate de tener un objeto GameManager en la escena.");
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
                // Aquí puedes realizar acciones cuando el tiempo se haya agotado
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


    // Método para aumentar el puntaje de un jugador cuando mate a otro
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
        // Lógica adicional que desees realizar cuando un jugador mate a otro
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

    // Método para obtener el PlayerConfiguration del jugador según su índice
    public PlayerConfiguration GetPlayerConfiguration(int playerIndex)
    {
        // Buscamos el PlayerConfiguration en la lista por su índice
        return playerConfigurations.Find(playerConfig => playerConfig.PlayerIndex == playerIndex);

    }

    public Weapon GetPlayerWeapon(int playerIndex)
    {
        // Aquí obtenemos el PlayerConfiguration del jugador según su índice
        PlayerConfiguration playerConfiguration = GetPlayerConfiguration(playerIndex);

        if (playerConfiguration != null)
        {
            // Devolvemos el componente Weapon del jugador desde su PlayerConfiguration
            return playerConfiguration.PlayerWeapon;
        }

        return null;
    }

    // Método para obtener el puntaje de un jugador específico
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
