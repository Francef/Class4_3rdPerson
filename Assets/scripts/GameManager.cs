using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector3 spawnPoint;
    [SerializeField]
    PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = player.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < 0)
        {
            player.Respawn(spawnPoint);
        }
        
    }
}
