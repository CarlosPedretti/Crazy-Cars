using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddElements : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;

    private GameObject[] players;

    private GameObject[] elements;


    void Awake()
    {

        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            cinemachineTargetGroup.AddMember(player.transform, 1, 20);
        }

        elements = GameObject.FindGameObjectsWithTag("Element");

        foreach (GameObject element in elements)
        {
            cinemachineTargetGroup.AddMember(element.transform, 1, 20);
        }

    }

}
