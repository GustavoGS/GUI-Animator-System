using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

public class GUIAnimatorItem : MonoBehaviour/*,*/ /*IGUIAnimatorItem*/ {

	#region Class members
	#endregion

	#region Class accesors
	//public bool state { get; set; }
	//public EasingCurves easingType { get; set; }
	//public float time { get; set; }
	//public float delay { get; set; }
	#endregion

	#region MonoBehaviour overrides
	#endregion

	#region Super class overrides
	#endregion

	#region Class implementation
	#endregion

	#region Interface implementation
	#endregion
}

public interface IGUIAnimatorItem {
	bool state { get; }
	EasingCurves easingType { get; }
	float time { get; }
	float delay { get; }
}