using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BoardSave
{
	public int m_CurrentFieldIndex{ get; set; }

	public List<FieldSave> m_FieldSaves{ get; set; }

	public BoardSave ()
	{
		m_FieldSaves = new List<FieldSave> ();
	}
}
