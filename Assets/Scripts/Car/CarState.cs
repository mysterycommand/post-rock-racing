﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof (CarParts))]
public class CarState : MonoBehaviour {

	/**
	 * Single Values:
	 */
	public float MaxTireAngle = 65f;
	public float MaxTireForce = 1000f;
	public float TiresSteeringAngle { get; set; }
	public float TiresPoweredForce { get; set; }
	public bool IsAiming { get; set; }
	public float CannonAngle { get; set; }
	public float RateOfFire { get; set; }
	private bool CanFire = true;

	public float CarHealth { get; set; }
	[HideInInspector] public Color CarColor;

	[SerializeField] public float MinRateOfFire { get; private set; }
	[SerializeField] public float MaxRateOfFire { get; private set; }
	[SerializeField] private float MaxCarHealth = 100f;

	private Color ZeroHealthColor;
	private Color FullHealthColor;
	private float EnginePitch;
	public CarParts carParts { get; private set; }

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		carParts = GetComponent<CarParts>();
		EnginePitch = 1f + Random.Range(-0.2f, 0.2f);
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
		carParts.EngineSource.pitch = Mathf.Lerp(EnginePitch, 3f, TiresPoweredForce / MaxTireForce);

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

		if (CanFire && RateOfFire != 0f) {
			StartCoroutine(Fire());
		}
	}

	IEnumerator Fire()
	{
		GameObject bullet = Instantiate(
			carParts.BulletGameObject,
			carParts.BulletSpawnTransform.position,
			carParts.BulletSpawnTransform.rotation
		);
		Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
		PolygonCollider2D bulletCollider = bullet.GetComponentInChildren<PolygonCollider2D>();
		SpriteRenderer bulletRenderer = bullet.GetComponentInChildren<SpriteRenderer>();

		bullet.layer =
		bulletBody.gameObject.layer =
		bulletCollider.gameObject.layer =
		carParts.BoxColliders2D[0].gameObject.layer;

		bulletRenderer.color = Color.black;

		bulletBody.velocity = carParts.BulletSpawnTransform.up * 40f;
		carParts.CannonBody.AddForce(carParts.CannonBody.transform.up * -2000f);

		carParts.FireSource.Play();

		CanFire = false;
		yield return new WaitForSeconds(1f / RateOfFire);
		CanFire = true;
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
		RateOfFire = 0f;
	}

}
