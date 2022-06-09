using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
	[SerializeField] protected EndScreen endScreen;
	[SerializeField] protected string menuSceneName;
	protected bool gameEnd = false;

	protected virtual void Start()
	{
		// 마우스 설정
		GameData.instance.SetShowMouseCursor(false);
	}


	protected virtual void GameEnd(bool isClear)
	{
		gameEnd = isClear;
		StartCoroutine(LoadSceneCoroutine(menuSceneName));
		Time.timeScale = 0.1f;
		endScreen.gameObject.SetActive(true);
		endScreen.PlayEndAnimation(isClear);
	}


	IEnumerator LoadSceneCoroutine(string sceneName)
	{
		yield return new WaitForSecondsRealtime(6.0f);
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(sceneName);
	}
}
