using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private CameraController CameraRig;
    [SerializeField] private Text MessageText;
    [SerializeField] private GameObject Car;

    [SerializeField] private PlayerManager[] Players = new PlayerManager[2];

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpawnAllTanks();
        SetCameraTargets();
    }

    void SpawnAllTanks()
    {
        float colorSeed = Random.value;

        for (int i = 0; i < Players.Length; ++i) {
            PlayerManager player = Players[i];
            float hue = colorSeed + (i * (1f / Players.Length));
            Debug.Log(hue);
            Color color = Color.HSVToRGB(hue, 0.7f, 0.7f);
            GameObject car = Instantiate(Car, player.SpawnPoint.position, player.SpawnPoint.rotation);
            player.Setup(i + 1, color, car);
        }
    }

    void SetCameraTargets()
    {
        Transform[] targetTransforms = new Transform[Players.Length];

        for (int i = 0; i < Players.Length; ++i) {
            targetTransforms[i] = Players[i].CarInstance.GetComponent<Transform>();
        }

        CameraRig.TargetTransforms = targetTransforms;
    }

}
