using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

	public Camera m_Camera;

	public float m_CameraMass = 1f;

	public float m_CameraToTargetAcceptance = 0.01f;

	[SerializeField]
	private float m_SpringConstant = 20f;

	public float M_SpringConstant { 
		get { 
			return m_SpringConstant;
		} 
		set {
			m_SpringConstant = value;

			// Critical damping
			m_DampingValue = 2f * Mathf.Sqrt (m_SpringConstant);
		}
	}

	private float m_DampingValue;

	public float m_CameraDistance = 8.0f;

	private Vector3 m_CameraTarget;

	private Vector3 m_CameraSpeed = new Vector3 ();

	public CameraMovement ()
	{
		// Critical damping
		m_DampingValue = 2f * Mathf.Sqrt (m_SpringConstant);
	}

	void Start ()
	{
		m_CameraTarget = m_Camera.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (null != FieldModifier.currentlySelectedField) {
			m_CameraTarget = FieldModifier.currentlySelectedField.transform.position;
			m_CameraTarget = new Vector3 (m_CameraTarget.x, m_CameraDistance, m_CameraTarget.z);
		} else {
			return;
		}
		

		// if we are near enough to the target - skip the computation
		if (Vector3.Distance (m_CameraTarget, m_Camera.transform.position) < m_CameraToTargetAcceptance) {
			return;
		}

		// spring equation computation for smooth camera transitions. 
		Vector3 springForce = m_SpringConstant * (m_CameraTarget - m_Camera.transform.position) + m_DampingValue * m_CameraSpeed;
		if (m_CameraMass > 0f) {
			m_CameraSpeed = (Time.smoothDeltaTime * springForce) / m_CameraMass;
		} else {
			return;
		}
		m_Camera.transform.position = m_Camera.transform.position + m_CameraSpeed * Time.smoothDeltaTime;
	}
}
