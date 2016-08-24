using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FieldCreator
{
	private static List<GameObject> possibleFields = new List<GameObject> ();

	public static Field createField (float posX, float posZ, bool accessible, float reward)
	{
		GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/FieldPrefab", typeof(GameObject))) as GameObject;
		fieldObject.transform.position = new Vector3 (posX, 0f, posZ);
		fieldObject.name = "Field";

		Field field = fieldObject.GetComponent<Field> ();
		field.m_Accessible = accessible;
		field.m_Reward = reward;
		field.m_Neighbours = FieldManager.findNeighbours (field);

		for (int i = 0; i < field.m_Neighbours.Length; i++) {
			if (null != field.m_Neighbours [i]) {
				field.m_Neighbours [i].m_Neighbours = FieldManager.findNeighbours (field.m_Neighbours [i]);
			}
		}
		FieldManager.existingFields.Add (field);
		return field;
	}

	/** 	>> Create new Field from a PossibleField	<< */

	public static void onMakeNewField (PossibleField possibleField)
	{
		Field newField = createField (possibleField.transform.position.x, possibleField.transform.position.z, true, 0f);
		hidePossibleFields ();
		showPossibleFields (newField);
	}

	/** 	>> Show Possible Fields << 	*/

	public static void onShowPossibleFields (Field field)
	{
		if (possibleFields.Count > 0) {
			hidePossibleFields ();
		} else {
			showPossibleFields (field);
		}
	}

	private static void showPossibleFields (Field field)
	{
		Field[] neighbours = field.m_Neighbours;

		for (int i = 0; i < neighbours.Length; i++) {
			Field possibleNeighbour = neighbours [i];
			if (null == possibleNeighbour) {
				GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/PossibleField", typeof(GameObject))) as GameObject;
				fieldObject.transform.position = field.transform.position + FieldManager.AVAILABLE_ACTION_DIRECTIONS [i];

				possibleFields.Add (fieldObject);
			}
		}
	}

	private static void hidePossibleFields ()
	{
		foreach (GameObject pf in possibleFields) {
			GameObject.Destroy (pf);
		}
		possibleFields.Clear ();
	}
}
