using System;
using UnityEngine;

[Serializable]
public class PlayerManager {

    [HideInInspector] public Color PlayerColor = Color.black;
    [SerializeField] private Transform SpawnPoint;

    public void Setup()
    {

    }
}