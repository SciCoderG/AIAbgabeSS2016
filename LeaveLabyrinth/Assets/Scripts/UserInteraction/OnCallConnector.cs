using UnityEngine;
using System.Collections;

public class OnCallConnector : MonoBehaviour
{

	public void onSaveBoard ()
	{
		FieldManager.save ("test1");
		FieldModifier.resetVisibleElements ();
	}

	public void onLoadBoard ()
	{
		FieldManager.load ("test1");
		FieldModifier.resetVisibleElements ();
	}

	public void onSetAddingMode ()
	{
		FieldModifier.onChangeMode (FieldModifier.MODE_ADDING);
	}

	public void onSetDeletingMode ()
	{
		FieldModifier.onChangeMode (FieldModifier.MODE_DELETING);
	}

	public void onSwitchEditMode ()
	{
		FieldModifier.onChangeMode (FieldModifier.MODE_EDITING);
	}

	public void onApplyFieldEdit ()
	{
		FieldModifier.onApplyEditField ();
	}
}
