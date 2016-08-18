using System.Collections;

/// <summary>
/// Action quality.
/// Substitute for the missing Tuple-Class....
/// </summary>
public class ActionQuality
{
	public int m_ActionID { get; set; }

	public float m_Quality{ get; set; }

	public ActionQuality (int actionID, float quality)
	{
		m_ActionID = actionID;
		m_Quality = quality;
	}
}
