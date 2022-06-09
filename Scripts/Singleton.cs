using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// instance에서 instance를 사용하면 무한으로 반복되는 문제발생 가능성
	// - 따라서 _instance라는 변수를 따로 선언하여 사용
	static T _instance;
	public static T instance
	{
		get
		{
			// 인스턴스가 없으면 게임오브젝트에서 찾음
			if (_instance == null)
			{
				GameObject go = GameObject.Find(typeof(T).ToString());
				// - 그래도 없으면 새로만듬
				if (go == null)
				{
					go = new GameObject(typeof(T).ToString());
					go.AddComponent<T>();
					go.GetComponent<Singleton<T>>().Init();
				}
				_instance = go.GetComponent<T>();
				DontDestroyOnLoad(go);
			}
			return _instance;
		}
		private set { }
	}


	void Awake()
	{
		if (_instance)
		{
			Destroy(this.gameObject);
			return;
		}
	}
	protected virtual void Init() { }
}
