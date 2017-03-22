using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private CameraController CameraRig;
    [SerializeField] private Text MessageText;
    [SerializeField] private GameObject Car;

    [SerializeField] private PlayerManager[] Players = new PlayerManager[2];

    private PlayerManager Winner;
    public bool AnyButtonPressed = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpawnCars();
        SetCameraTargets();
        StartCoroutine(GameLoop());
    }

    void SpawnCars()
    {
        float colorSeed = Random.value;

        for (int i = 0; i < Players.Length; ++i) {
            PlayerManager player = Players[i];

            float hue = colorSeed + (i * (1f / Players.Length));
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

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(Start());
        yield return StartCoroutine(Play());
        yield return StartCoroutine(End());

        if (Winner != null) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            StartCoroutine(GameLoop());
        }
    }

    IEnumerator Start()
    {
        MessageText.text = $@"<size=7>post rock racing</size>
<size=5>press any button</size>";

        yield return new WaitUntil(() => (
            Input.GetAxis("LeftStickX1") != 0f ||
            Input.GetAxis("LeftTrigger1") != 0f ||
            Input.GetButton("LeftBumper1") ||
            Input.GetAxis("RightStickY1") != 0f ||
            Input.GetAxis("RightStickX1") != 0f ||
            Input.GetAxis("RightTrigger1") != 0f
        ) || (
            Input.GetAxis("LeftStickX2") != 0f ||
            Input.GetAxis("LeftTrigger2") != 0f ||
            Input.GetButton("LeftBumper2") ||
            Input.GetAxis("RightStickY2") != 0f ||
            Input.GetAxis("RightStickX2") != 0f ||
            Input.GetAxis("RightTrigger2") != 0f
        ));
    }

    IEnumerator Play()
    {
        MessageText.text = string.Empty;
        yield return new WaitForSeconds(3f);
    }

    IEnumerator End()
    {
        MessageText.text = $@"
<size=8>player {1} wins</size>";
        yield return new WaitForSeconds(3f);
    }

}
