using UnityEngine;
using System.Collections;

public class OnCallConnector : MonoBehaviour
{

	public void onClose ()
	{
		Application.Quit ();
	}

	public void onSwitchScreenMode ()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void onSaveBoard ()
	{
		FieldModifier.resetVisibleElements ();
		FieldManager.save ();
	}

	public void onLoadBoard ()
	{
		FieldModifier.resetVisibleElements ();
		FieldManager.load ();
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
