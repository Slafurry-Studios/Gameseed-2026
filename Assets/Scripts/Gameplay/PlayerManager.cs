using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Transform PlayerTransform;
    private void Start() { PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform; }
}
