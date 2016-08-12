using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeMazeMagnetic : MonoBehaviour 
{
	public enum Charge
	{
		None,
		Positive,
		Negative
	}

	public enum MovementType
	{
		Static,
		Dynamic
	}

	public Rigidbody rigid;
	public Collider rigidCollider;
	public Vector3 staticLocalMagneticDir = Vector3.zero;
	public PipeMazePlayerMachine player;

	private float lerpSpeed = 0.5f; // smoothing speed

	public bool IsSurfaceMagnetic() { return (this.staticLocalMagneticDir != Vector3.zero); }

	public Charge charge
	{
		get
		{
			return ((this.magneticCollider != null) && this.magneticCollider.enabled) ? this.currentCharge : Charge.None;
		}
	}
	public Charge currentCharge;
	public float chargeStrength;

	public MovementType movementType
	{
		get
		{
			return (this.rigid != null) ? MovementType.Dynamic : MovementType.Static;
		}
	}

	public Collider magneticCollider;

	public List<PipeMazeMagnetic> nearbyMagnetics;

	public void Awake() 
	{
		this.nearbyMagnetics = new List<PipeMazeMagnetic>();
		if (this.magneticCollider == null)
		{
			this.magneticCollider = this.gameObject.GetComponent<Collider>();
		}
	}

	public void Start() 
	{
		this.SetCharge(this.currentCharge);
	}

	public void OnTriggerEnter(Collider other)
	{
		PipeMazeMagnetic magnetic = other.gameObject.GetComponent<PipeMazeMagnetic>();
		if (magnetic != null)
		{
			if (!nearbyMagnetics.Contains(magnetic))
			{
				nearbyMagnetics.Add(magnetic);
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		PipeMazeMagnetic magnetic = other.gameObject.GetComponent<PipeMazeMagnetic>();
		if (magnetic != null)
		{
			if (nearbyMagnetics.Contains(magnetic))
			{
				nearbyMagnetics.Remove(magnetic);
			}
		}
	}

	public void Update() 
	{
		if (this.player != null)
		{
			Vector3 force = CalculateLocalChargeForce();
			if (force != Vector3.zero)
			{
				Vector3 desiredUp = (-1*force).normalized;
				Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(this.player.transform.forward, desiredUp);
				this.player.transform.rotation = Quaternion.Lerp(this.player.transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
			}
		}
	}

	public void FixedUpdate()
	{
		if (this.rigid != null)
		{
			Vector3 force = CalculateLocalChargeForce();
			if (force != Vector3.zero)
			{
				this.rigid.AddForce(force);
			}
		}
 	}

	public Vector3 GetChargeForce(PipeMazeMagnetic other)
	{
		Vector3 forceDirection = (other.transform.position - this.transform.position).normalized;
		if (this.IsSurfaceMagnetic())
		{
			forceDirection = other.transform.TransformDirection(other.staticLocalMagneticDir);
		}

		//TODO: Make the force ramp up as the distance shortens

		float forceMultiplier = this.GetChargePolarityMultiplier(other) * this.chargeStrength;
		return forceMultiplier*forceDirection;
	}

	public float GetChargePolarityMultiplier(PipeMazeMagnetic other)
	{
		if (this.charge == Charge.None)
		{
			return (other.charge == Charge.None) ? 0.0f : 0.5f;
		}
		else if (this.charge == Charge.Positive)
		{
			if (other.charge == Charge.None)
			{
				return 0.5f;
			}

			return (other.charge == Charge.Positive) ? -1.0f : 1.0f;
		}
		else if (this.charge == Charge.Negative)
		{
			if (other.charge == Charge.None)
			{
				return 0.5f;
			}

			return (other.charge == Charge.Negative) ? -1.0f : 1.0f;
		}

		return 0.0f;
	}

	public Vector3 CalculateLocalChargeForce()
	{
		if (this.movementType == MovementType.Static)
		{
			return Vector3.zero;
		}

		if ((this.player != null) && this.player.MaintainingGround())
		{
			return -1*this.transform.up;
			//return Vector3.zero;
		}

		Vector3 force = Vector3.zero;
		bool hasSurfaceMagnetic = false;
		for (int i=0; i<this.nearbyMagnetics.Count; i++)
		{
			if (this.nearbyMagnetics[i].IsSurfaceMagnetic())
			{
				if (hasSurfaceMagnetic)
				{
					//continue;
				}
				hasSurfaceMagnetic = true;
			}

			force += this.GetChargeForce(this.nearbyMagnetics[i]);
		}

		return force;
	}

	public void SetCharge(Charge newCharge)
	{
		this.currentCharge = newCharge;
		if (this.magneticCollider != null)
		{
			this.magneticCollider.enabled = (this.currentCharge != Charge.None);
		}
	}
}
