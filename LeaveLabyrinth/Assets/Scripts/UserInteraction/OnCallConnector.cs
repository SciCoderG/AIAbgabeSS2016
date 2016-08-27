using UnityEngine;
using System.Collections;

public class OnCallConnector : MonoBehaviour
{

	public void onSaveBoard ()
	{
		FieldManager.save ();
		FieldModifier.resetVisibleElements ();
	}

	public void onLoadBoard ()
	{
		FieldManager.load ();
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

	public void onApplyFieldEdit ()
	{
		FieldModifier.onApplyEditField ();
	}
}
