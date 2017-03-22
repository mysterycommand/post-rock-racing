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
	public CarParts carParts { get; private set; }

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		carParts = GetComponent<CarParts>();
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		InitializeSingleValues();
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
			GameObject bullet = Instantiate(
				carParts.BulletGameObject,
				carParts.BulletSpawnTransform.position,
				carParts.BulletSpawnTransform.rotation
			);
			Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
			PolygonCollider2D bulletCollider = bullet.GetComponentInChildren<PolygonCollider2D>();

			bullet.layer =
			bulletBody.gameObject.layer =
			bulletCollider.gameObject.layer =
			carParts.BoxColliders2D[0].gameObject.layer;

			bulletBody.velocity = carParts.BulletSpawnTransform.up * 100f;
		}
	}

	private void InitializeSingleValues()
	{
		TiresSteeringAngle = 0f;
		TiresPoweredForce = 0f;

		IsAiming = false;
		CannonAngle = 0f;

		float h, s, v;
		Color.RGBToHSV(CarColor, out h, out s, out v);

		Color tempFull = Color.HSVToRGB(h, 0.7f, 0.5f);
		FullHealthColor = new Color(tempFull.r, tempFull.g, tempFull.b, 0.5f);

		Color tempZero = Color.HSVToRGB(h, 0.7f, 0.3f);
		ZeroHealthColor = new Color(tempZero.r, tempZero.g, tempZero.b, 0.5f);

		CarHealth = MaxCarHealth;
	}

}
