using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;


    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject playerUIPrefab; // El prefab de la UI para cada jugador

    [SerializeField]
    private Transform canvasTransform; // Referencia al transform del Canvas que contiene las UI de los jugadores

    // Start is called before the first frame update


    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            var playerUI = Instantiate(playerUIPrefab, canvasTransform); // Instanciar la UI y establecer el Canvas como padre
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            playerConfigs[i].PlayerUI = playerUI;

            // Agregar el jugador al GameManager
            GameManager.Instance.AddPlayer(playerConfigs[i]);

            // Obtener referencia al script PlayerUIScoreUpdater
            var scoreUpdater = playerUI.GetComponent<PlayerUIScoreUpdater>();

            // Establecer el nombre y puntaje usando el PlayerUIScoreUpdater
            scoreUpdater.UpdatePlayerUI("Player " + (i + 1).ToString(), GameManager.Instance.GetScore(playerConfigs[i].PlayerIndex));

            var respawnPoint = player.AddComponent<RespawnPoint>();
            respawnPoint.respawnPosition = playerSpawns[i].position;
            respawnPoint.respawnRotation = playerSpawns[i].rotation;



        }
    }

}
