using System.Collections;

/// <summary>
/// Action quality.
/// Substitute for the missing Tuple-Class....
/// </summary>
public class ActionQuality
{
	public int m_ActionID { get; set; }

	public int m_Quality{ get; set; }

	public ActionQuality (int actionID, int quality)
	{
		m_ActionID = actionID;
		m_Quality = quality;
	}
}
