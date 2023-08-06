using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float totalTime = 60f;
    private float currentTime;
    private bool isCounting = false;


    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerScoreText;

    public TMP_Text timerText;

    // Variables para llevar el registro de los puntajes de los jugadores
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();
    private List<PlayerConfiguration> playerConfigurations = new List<PlayerConfiguration>();

    public int maxScorePoints;

    public int pointPerPlayer;


    PlayerConfiguration playerConfiguration;


    private static GameManager instance;


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
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
                GameOver();
                // Aquí puedes realizar acciones cuando el tiempo se haya agotado
            }
        }
    }



    private void GameOver()
    {
        // Detener la escena
        Time.timeScale = 0f;

        // Buscar el jugador con el mayor puntaje
        int highestScore = 0;
        List<int> playerIndicesWithHighestScore = new List<int>();

        foreach (var playerIndex in playerScores.Keys)
        {
            int playerScore = GetScore(playerIndex);

            if (playerScore > highestScore)
            {
                highestScore = playerScore;
                playerIndicesWithHighestScore.Clear();
                playerIndicesWithHighestScore.Add(playerIndex);
            }
            else if (playerScore == highestScore)
            {
                playerIndicesWithHighestScore.Add(playerIndex);
            }
        }

        // Mostrar la interfaz de usuario con el resultado del juego
        if (playerIndicesWithHighestScore.Count == 1)
        {
            // Solo un jugador tiene el puntaje más alto, muestra su nombre como ganador
            int playerIndex = playerIndicesWithHighestScore[0];
            PlayerConfiguration playerWithHighestScore = GetPlayerConfiguration(playerIndex);
            if (playerWithHighestScore != null)
            {
                string playerName = "Player " + (playerWithHighestScore.PlayerIndex + 1).ToString();
                int playerScore = GetScore(playerWithHighestScore.PlayerIndex);
                GameOverScreen.SetActive(true);
                playerNameText.text = playerName + " wins!";
                playerScoreText.text = "With a score of: " + playerScore;
            }
        }
        else
        {
            // Hay empate, muestra un mensaje de empate
            GameOverScreen.SetActive(true);
            playerNameText.text = "It's a draw.";
            playerScoreText.text = "You should try again!";
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

    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        GameObject playerConfigManager = GameObject.Find("PlayerConfigurationManager");
        if (playerConfigManager != null)
        {
            playerConfigManager.SetActive(false);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }
}
