using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManagement : MonoBehaviour
{
	public void DragonButton ()
	{
		ApplicationModel.character = ApplicationModel.PlayerCharacter.Dragon;
		SwitchToGameScene ();
	}

	public void KnightButton ()
	{
		ApplicationModel.character = ApplicationModel.PlayerCharacter.Knight;
		SwitchToGameScene ();
	}

	private void SwitchToGameScene ()
	{
		SceneManager.LoadScene ("Game");
	}

	public void SwitchToInfoScene ()
	{
		SceneManager.LoadScene ("Info");
	}

    public void EndGame()
    {
        Application.Quit();
    }
}
