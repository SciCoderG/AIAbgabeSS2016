﻿using UnityEngine;
using System.Collections;

public class PossibleField : MonoBehaviour
{

	void OnMouseUpAsButton ()
	{
		FieldCreator.onMakeNewField (this);
	}


}
