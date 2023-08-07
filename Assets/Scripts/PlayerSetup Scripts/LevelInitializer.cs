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
    private GameObject playerUIPrefab; 

    [SerializeField]
    private Transform canvasTransform; 


    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            var playerUI = Instantiate(playerUIPrefab, canvasTransform); 
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            playerConfigs[i].PlayerUI = playerUI;

            GameManager.Instance.AddPlayer(playerConfigs[i]);

            var scoreUpdater = playerUI.GetComponent<PlayerUIScoreUpdater>();

            scoreUpdater.UpdatePlayerUI("Player " + (i + 1).ToString(), GameManager.Instance.GetScore(playerConfigs[i].PlayerIndex));

            var respawnPoint = player.AddComponent<RespawnPoint>();
            respawnPoint.respawnPosition = playerSpawns[i].position;
            respawnPoint.respawnRotation = playerSpawns[i].rotation;



        }
    }

}
