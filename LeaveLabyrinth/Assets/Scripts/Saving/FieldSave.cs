using System.Collections;

/// <summary>
/// Serializable class, used to store non-unity-information of the Field-class
/// </summary>
[System.Serializable]
public class FieldSave
{

	public float m_PosX{ get; set; }

	public float m_PosZ{ get; set; }

	public bool m_Accessible{ get; set; }

	public float m_Reward{ get; set; }

}
