using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for comprehensive data-exchange (scene to scene, etc.)
public class ApplicationModel : MonoBehaviour {

	public enum PlayerCharacter 
	{
		Dragon,
		Knight,
		None
	}

	public static PlayerCharacter character;
}
