using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Serializable Class, used to compress the fieldsaves into one file
/// </summary>
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
