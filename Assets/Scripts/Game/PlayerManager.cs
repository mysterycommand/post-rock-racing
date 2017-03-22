using System;
using UnityEngine;

[Serializable]
public class PlayerManager {

    public Transform SpawnPoint;
    [HideInInspector] public int PlayerNumber;
    [HideInInspector] public Color PlayerColor;
    [HideInInspector] public GameObject CarInstance;

    public string PlayerText { get; private set; }
    public int PlayerScore { get; private set; }

    private CarController PlayerCarController;
    private CarParts PlayerCarParts;
    private CarState PlayerCarState;

    public void Setup(int number, Color color, GameObject instance)
    {
        PlayerNumber = number;
        PlayerColor = color;
        CarInstance = instance;

        instance.GetComponent<CarController>().PlayerNumber = PlayerNumber;
        instance.GetComponent<CarState>().CarColor = PlayerColor;
    }
}