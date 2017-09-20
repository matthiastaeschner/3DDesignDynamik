using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManagement : MonoBehaviour {

	public void SwitchToStartScene ()
	{
		SceneManager.LoadScene ("Start");
	}
}
