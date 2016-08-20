using System.Collections;

/// <summary>
/// Action quality.
/// Substitute for the missing Tuple-Class....
/// </summary>
public class AQTuple
{
	public int m_ActionID { get; set; }

	public float m_Quality{ get; set; }

	public AQTuple (int actionID, float quality)
	{
		m_ActionID = actionID;
		m_Quality = quality;
	}
}
