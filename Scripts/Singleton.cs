using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// instance���� instance�� ����ϸ� �������� �ݺ��Ǵ� �����߻� ���ɼ�
	// - ���� _instance��� ������ ���� �����Ͽ� ���
	static T _instance;
	public static T instance
	{
		get
		{
			// �ν��Ͻ��� ������ ���ӿ�����Ʈ���� ã��
			if (_instance == null)
			{
				GameObject go = GameObject.Find(typeof(T).ToString());
				// - �׷��� ������ ���θ���
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
