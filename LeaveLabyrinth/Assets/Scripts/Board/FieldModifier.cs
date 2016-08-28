using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FieldModifier
{
	private static List<GameObject> possibleFields = new List<GameObject> ();

	private static int currentMode = FieldModifier.MODE_ADDING;

	public static EditFieldUI editFieldUI{ get; set; }

	public const int MODE_ADDING = 0;
	public const int MODE_DELETING = 1;

	private static Field markedForDelete = null;

	public static Field currentlySelectedField{ get; set; }

	public static void updateQualityToState (QualityChangeBuffer qChangeBuffer)
	{
		foreach (uint state in qChangeBuffer.qualityChangeBuffer.Keys) {
			Field field = FieldManager.getFieldFromState (state);
			float quality = 0f;
			qChangeBuffer.qualityChangeBuffer.TryGetValue (state, out quality);
			field.setCurrentQuality (quality);
		}
		qChangeBuffer.clearBuffer ();
	}


	public static void resetVisibleElements ()
	{
		if (null != markedForDelete) {
			markedForDelete.OnUnmarkForDelete ();
		}
		hidePossibleFields ();
		unselectCurrentField ();
	}

	public static void onDeleteField (Field field)
	{
		FieldManager.existingFields.Remove (field);
		GameObject.Destroy (field.gameObject);
	}

	/// <summary>
	/// Ons the change mode.
	/// </summary>
	/// <param name="fieldModifyingMode">Field modifying mode.</param>
	public static void onChangeMode (int fieldModifyingMode)
	{
		resetVisibleElements ();
		currentMode = fieldModifyingMode;
	}

	private static void updateSelectedField (Field field)
	{
		// change color to selected
		if (null != currentlySelectedField) {
			if (field.Equals (currentlySelectedField)) {
				unselectCurrentField ();
				return;
			} else {
				// unselect old field
				currentlySelectedField.OnSwitchSelect ();
			}
		} 
		currentlySelectedField = field;
		field.OnSwitchSelect ();
		if (editFieldUI.gameObject.activeInHierarchy) {
			uint fieldState = FieldManager.getStateFromField (field);
			short posX, posY;
			StateConversion.convertFromState (fieldState, out posX, out posY);
			string stateString = "(" + posX + "," + posY + ")";
			editFieldUI.onNewFieldClicked (field, stateString);
		}
	}

	private static void unselectCurrentField ()
	{
		// unselect current field
		if (null != currentlySelectedField) {
			currentlySelectedField.OnSwitchSelect ();
			currentlySelectedField = null;
			if (editFieldUI.gameObject.activeInHierarchy) {
				editFieldUI.onUnselect ();
			}
		}
	}

	public static void onClickField (Field field)
	{
		updateSelectedField (field);

		switch (currentMode) {
		case MODE_ADDING:
			{
				onShowPossibleFields (field);
				break;
			}
		case MODE_DELETING:
			{
				onStartDeletingField (field);
				break;
			}
		default:
			{
				Debug.Log ("FieldModifier: Current Mode is not implemented");
				break;
			}
		}
	}

	public static void onApplyEditField ()
	{
		editFieldUI.onApplyEditField ();
	}

	public static void onStartDeletingField (Field field)
	{
		if (null == markedForDelete) {
			field.OnMarkForDelete ();
			markedForDelete = field;
		} else if (null != markedForDelete && field.Equals (markedForDelete)) {
			onDeleteField (field);
			markedForDelete = null;
		} else if (null != markedForDelete && !field.Equals (markedForDelete)) {
			markedForDelete.OnUnmarkForDelete ();
			field.OnMarkForDelete ();
			markedForDelete = field;
		}
	}

	public static Field createAndAddNewField (float posX, float posZ, bool accessible, float reward)
	{
		Field toAdd = createField (posX, posZ, accessible, reward);
		FieldManager.existingFields.Add (toAdd);
		return toAdd;
	}

	public static Field createField (float posX, float posZ, bool accessible, float reward)
	{
		GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/FieldPrefab", typeof(GameObject))) as GameObject;
		fieldObject.transform.position = new Vector3 (posX, 0f, posZ);
		fieldObject.name = "Field";

		Field field = fieldObject.GetComponent<Field> ();
		field.M_IsAccessible = accessible;
		field.M_Reward = reward;
		field.m_Neighbours = FieldManager.findNeighbours (field);

		for (int i = 0; i < field.m_Neighbours.Length; i++) {
			if (null != field.m_Neighbours [i]) {
				field.m_Neighbours [i].m_Neighbours = FieldManager.findNeighbours (field.m_Neighbours [i]);
			}
		}
		return field;
	}

	/** 	>> Create new Field from a PossibleField	<< */

	public static void onMakeNewField (PossibleField possibleField)
	{
		Field newField = createAndAddNewField (possibleField.transform.position.x, possibleField.transform.position.z, true, 0f);
		hidePossibleFields ();
		showPossibleFields (newField);
		updateSelectedField (newField);
	}

	/** 	>> Show Possible Fields << 	*/

	public static void onShowPossibleFields (Field field)
	{
		if (possibleFields.Count > 0 && !field.Equals (currentlySelectedField)) {
			hidePossibleFields ();
		} else if (field.Equals (currentlySelectedField)) {
			hidePossibleFields ();
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
