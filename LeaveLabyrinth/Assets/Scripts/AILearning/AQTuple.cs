using System.Collections;
using System;

/// <summary>
/// Action quality.
/// Substitute for the missing Tuple-Class....
/// </summary>
public class AQTuple : IComparable
{
	public int m_ActionID { get; set; }

	public float m_Quality{ get; set; }

	public AQTuple (int actionID, float quality)
	{
		m_ActionID = actionID;
		m_Quality = quality;
	}

	public override string ToString ()
	{
		return "A:" + m_ActionID + ", Q:" + m_Quality.ToString ("0.000");
	}

	public int CompareTo (object obj)
	{
		if (null == obj)
			return 1;
		AQTuple otherAQTuple = obj as AQTuple;
		if (null != otherAQTuple)
			return m_ActionID.CompareTo (otherAQTuple.m_ActionID);
		else
			throw new ArgumentException ("Object is not an AQTuple.");
	}
}
