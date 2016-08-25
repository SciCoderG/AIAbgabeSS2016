using UnityEngine;
using System.Collections;

public class PossibleField : MonoBehaviour
{

	void OnMouseUpAsButton ()
	{
		FieldModifier.onMakeNewField (this);
	}


}
