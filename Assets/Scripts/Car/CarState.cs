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
	public bool IsFiring { get; set; }
	public float RateOfFire { get; set; }

	public float CarHealth { get; set; }
	[HideInInspector] public Color CarColor;

	// [SerializeField] private float MinRateOfFire = 1f; // per second
	// [SerializeField] private float MaxRateOfFire = 10f; // per second
	[SerializeField] private float MaxCarHealth = 100f;

	private Color ZeroHealthColor;
	private Color FullHealthColor;
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
			FullHealthColor,
			CarHealth / MaxCarHealth
		);

		if (IsFiring) {
			Rigidbody2D bullet = Instantiate(
				carParts.BulletGameObject,
				carParts.BulletSpawnTransform.position,
				carParts.BulletSpawnTransform.rotation
			).GetComponent<Rigidbody2D>();

			bullet.velocity = carParts.BulletSpawnTransform.up * 100f;
		}
	}

	private void InitializeSingleValues()
	{
		TiresSteeringAngle = 0f;
		TiresPoweredForce = 0f;

		IsAiming = false;
		CannonAngle = 0f;

		float hue = Random.value;
		CarColor = Color.HSVToRGB(hue, 0.7f, 0.7f);

		Color tempFull = Color.HSVToRGB(hue, 0.5f, 0.5f);
		FullHealthColor = new Color(tempFull.r, tempFull.g, tempFull.b, 0.5f);

		Color tempZero = Color.HSVToRGB(hue, 0.3f, 0.3f);
		ZeroHealthColor = new Color(tempZero.r, tempZero.g, tempZero.b, 0.5f);

		CarHealth = MaxCarHealth;
	}

}
