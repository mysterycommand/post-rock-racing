using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof (CarParts))]
public class CarState : MonoBehaviour {

	/**
	 * Single Values:
	 */
	public float TiresSteeringAngle { get; set; }
	public float TiresPoweredForce { get; set; }
	public bool IsAiming { get; set; }
	public float CannonAngle { get; set; }
	public Color CarColor { get; set; }
	public float CarHealth { get; set; }

	[SerializeField] private float MaxCarHealth = 100f;

	[SerializeField] private Color ZeroHealthColor = new Color(0f, 0f, 0f, (1f / 3f));

	private CarParts carParts;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		carParts = GetComponent<CarParts>();
		InitializeSingleValues();
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		foreach (SpriteRenderer renderer in carParts.SpriteRenderers) {
			renderer.color = CarColor;
		}
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		foreach (Rigidbody2D Tire in carParts.TiresSteering) {
			Tire.rotation = Mathf.LerpAngle(Tire.rotation, carParts.ChassisBody.rotation + TiresSteeringAngle, 0.1f);
		}

		foreach (Rigidbody2D Tire in carParts.TiresPowered) {
			Tire.AddForce(Tire.transform.up * TiresPoweredForce);
		}

		carParts.CannonBody.rotation = Mathf.LerpAngle(
			carParts.CannonBody.rotation,
			(IsAiming) ?
				CannonAngle :
				carParts.ChassisBody.rotation,
			0.1f
		);

		carParts.HealthSlider.value = CarHealth;
		carParts.HealthSlider.GetComponentsInChildren<Image>()[1].color = Color.Lerp(
			ZeroHealthColor,
			CarColor,
			CarHealth / MaxCarHealth
		);
	}

	private void InitializeSingleValues()
	{
		TiresSteeringAngle = 0f;
		TiresPoweredForce = 0f;

		IsAiming = false;
		CannonAngle = 0f;

		CarColor = Color.HSVToRGB(Random.value, 0.7f, 0.7f);

		CarHealth = MaxCarHealth;
	}

}
