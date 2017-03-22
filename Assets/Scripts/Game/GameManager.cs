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
        foreach (PlayerManager player in Players) {
            player.CarInstance.GetComponent<CarController>().enabled = false;
        }

        MessageText.text = $@"<size=7>post rock</size>
<size=7>demolition derby</size>
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
        foreach (PlayerManager player in Players) {
            player.CarInstance.GetComponent<CarController>().enabled = true;
        }

        MessageText.text = string.Empty;

        while (MoreThanOneCar()) {
            yield return null;
        }
    }

    IEnumerator End()
    {
        Winner = null;
        Winner = GetWinner();
        Winner.CarInstance.GetComponent<CarParts>().ExplodeSource.Play();

        if (Winner != null) {
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
                Destroy(bullet);
            }

            string playerColor = ColorUtility.ToHtmlStringRGB(Winner.PlayerColor);
            MessageText.text = $@"
<size=8><color=#{playerColor}>player {Winner.PlayerNumber}</color> wins</size>";
        } else {
            MessageText.text = $@"
<size=8>it was a tie</size>";
        }

        yield return new WaitForSeconds(5f);
    }

    private bool MoreThanOneCar() {
        int numActiveCars = 0;

        foreach (PlayerManager player in Players) {
            if (!player.CarInstance.activeSelf) {
                continue;
            }

            numActiveCars++;
        }

        return numActiveCars > 1;
    }

    private PlayerManager GetWinner()
    {
        foreach (PlayerManager player in Players) {
            if (!player.CarInstance.activeSelf) {
                continue;
            }

            return player;
        }

        return null;
    }

}
