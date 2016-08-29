using UnityEngine;
using System.Collections;

/// <summary>
/// Connects all onClick-Methods of the various UI-buttons to the corresponding static-classes.
/// This is needed, because in the Unity-Inspector-View, onClick-Calls can only be connected to 
/// Unity-Objects.
/// Why is this called "OnCallConnector" and not "OnClickConnector"? My guess, that's because
/// I think "onCall" is the function used by the Android-API. But honestly, I think it's because
/// I'm dumb :D
/// </summary>
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
