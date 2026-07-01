using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private GameObject playerPrefab;
    public static Transform PlayerTransform;

    private void Start()
    {
        Instance = this;
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = playerPrefab.transform;
    }

    public void Pause()
    {
        playerPrefab.GetComponent<PlayerShoot>().enabled = false;
        playerPrefab.GetComponent<PlayerAim>().enabled = false;

    }

    public void Resume()
    {
        playerPrefab.GetComponent<PlayerShoot>().enabled = true;
        playerPrefab.GetComponent<PlayerAim>().enabled = true;

    }
}
