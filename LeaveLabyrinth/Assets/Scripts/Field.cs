using System.Collections;

/// <summary>
/// Data-class for a Field on a Board.
/// </summary>
public class Field
{
	public int m_Reward{ get; set; }

	public bool m_Accessible{ get; set; }

	public Field (int reward, bool accessible)
	{
		m_Reward = reward;
		m_Accessible = accessible;
	}
}
