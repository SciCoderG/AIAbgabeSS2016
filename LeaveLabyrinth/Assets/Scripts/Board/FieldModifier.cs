using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Modifies the existing fields, including addition and deletion of fields,
/// changes of variables (like reward and accessability),
/// changes in color,
/// and display of current quality to corresponding states of the fields
/// </summary>
public static class FieldModifier
{
	private static List<GameObject> possibleFields = new List<GameObject> ();

	private static int currentMode = FieldModifier.MODE_ADDING;

	public static EditFieldUI editFieldUI{ get; set; }

	public const int MODE_ADDING = 0;
	public const int MODE_DELETING = 1;

	private static Field markedForDelete = null;

	public static Field currentlySelectedField{ get; set; }

	/// <summary>
	/// Updates the displayed quality on a field from the given quality-change buffer
	/// </summary>
	/// <param name="qChangeBuffer">Q change buffer.</param>
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

	/// <summary>
	/// Resets the visible elements.
	/// </summary>
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
	/// Called, if the construction-mode was called
	/// </summary>
	/// <param name="fieldModifyingMode">Field modifying mode.</param>
	public static void onChangeMode (int fieldModifyingMode)
	{
		resetVisibleElements ();
		currentMode = fieldModifyingMode;
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

	/// <summary>
	/// Updates changes of the field-variables on UI
	/// </summary>
	public static void onApplyEditField ()
	{
		editFieldUI.onApplyEditField ();
	}

	/// <summary>
	/// Handles clicks on fields in the deletion-mode
	/// </summary>
	/// <param name="field">Field clicked</param>
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

	/// <summary>
	/// Creates a new field and adds it to the List of existing fields in the FieldManager
	/// </summary>
	/// <returns>The new field</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posZ">Position z.</param>
	/// <param name="accessible">If set to <c>true</c>field is accessible.</param>
	/// <param name="reward">Reward.</param>
	public static Field createAndAddNewField (float posX, float posZ, bool accessible, float reward)
	{
		Field toAdd = createField (posX, posZ, accessible, reward);
		FieldManager.existingFields.Add (toAdd);
		return toAdd;
	}

	/// <summary>
	/// Creates a new field WITHOUT adding it to the FieldManager. Use with care, AI will not recognize
	/// fields, that are not added to the FieldManager. Those fields will also not be saved
	/// </summary>
	/// <returns>The new field.</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posZ">Position z.</param>
	/// <param name="accessible">If set to <c>true</c>field is accessible.</param>
	/// <param name="reward">Reward.</param>
	public static Field createField (float posX, float posZ, bool accessible, float reward)
	{
		GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/FieldPrefab")) as GameObject;
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

	/// <summary>
	/// Called, if a PossibleField was clicked. Creates and adds a new field on the position of the PossibleField
	/// </summary>
	/// <param name="possibleField">Possible field clicked.</param>
	public static void onMakeNewField (PossibleField possibleField)
	{
		Field newField = createAndAddNewField (possibleField.transform.position.x, possibleField.transform.position.z, true, 0f);
		resetVisibleElements ();
		showPossibleFields (newField);
		onClickField (newField);
	}

	/** 	>> Show Possible Fields << 	*/
	/// <summary>
	/// Called, if a field was clicked while in the addition-mode
	/// </summary>
	/// <param name="field">Clicked Field.</param>
	public static void onShowPossibleFields (Field field)
	{
		if (possibleFields.Count > 0 && !field.Equals (currentlySelectedField)) {
			hidePossibleFields ();
		} else if (field.Equals (currentlySelectedField)) {
			hidePossibleFields ();
			showPossibleFields (field);
		}
	}

	/// <summary>
	/// Shows possible new fields surrounding the specified field
	/// </summary>
	/// <param name="field">Field specified.</param>
	private static void showPossibleFields (Field field)
	{
		Field[] neighbours = field.m_Neighbours;

		for (int i = 0; i < neighbours.Length; i++) {
			Field possibleNeighbour = neighbours [i];
			if (null == possibleNeighbour) {
				GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/PossibleField")) as GameObject;
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

	/// <summary>
	/// Updates the currently selected field.
	/// </summary>
	/// <param name="field">Field.</param>
	private static void updateSelectedField (Field field)
	{
		// change color to selected
		if (null != currentlySelectedField) {
			if (field.Equals (currentlySelectedField)) {
				unselectCurrentField ();
				return;
			} else {
				currentlySelectedField.setSelected (false);
			}
		} 
		currentlySelectedField = field;
		field.setSelected (true);
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
			currentlySelectedField.setSelected (false);
			currentlySelectedField = null;
			if (editFieldUI.gameObject.activeInHierarchy) {
				editFieldUI.onUnselect ();
			}
		}
	}
}
