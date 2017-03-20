using UnityEngine;

public class DebugController : MonoBehaviour {

	private string[] Axes = {
		"LeftStickX",
		"LeftStickY",

		"RightStickX",
		"RightStickY",

		"LeftTrigger",
		"RightTrigger",
	};

	private string[] Buttons = {
		"LeftBumper",
		"RightBumper",
	};

	private string[] PlayerNumbers = { "1", "2" };

	/// <summary>
	/// LateUpdate is called every frame, if the Behaviour is enabled.
	/// It is called after all Update functions have been called.
	/// </summary>
	void LateUpdate()
	{
		string message = "";

		foreach (string player in PlayerNumbers) {
			foreach (string axis in Axes) {
				message += $"\n{axis}{player}: " + Input.GetAxis($"{axis}{player}");
			}

			foreach (string button in Buttons) {
				message += $"\n{button}{player}: " + Input.GetButton($"{button}{player}");
			}
		}

		Debug.Log(message);
	}

}
