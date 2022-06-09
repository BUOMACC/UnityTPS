using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 데미지를 입힐수 있는 인터페이스
 * Entity에 데미지 처리를 하면 타격가능한 오브젝트가
 * Entity의 쓸모없는 기능까지 사용하게되므로 인터페이스 사용
*/
public interface IDamageObject
{
	void TakeDamage(int amount, Vector3 hitPosition);
}
