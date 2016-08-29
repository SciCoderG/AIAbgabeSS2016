using UnityEngine;
using System.Collections;


/// <summary>
/// Simple representation of a possible location, a new field can be created in 
/// </summary>
public class PossibleField : MonoBehaviour
{
	void OnMouseUpAsButton ()
	{
		FieldModifier.onMakeNewField (this);
	}
}
