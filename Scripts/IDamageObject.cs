using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* �������� ������ �ִ� �������̽�
 * Entity�� ������ ó���� �ϸ� Ÿ�ݰ����� ������Ʈ��
 * Entity�� ������� ��ɱ��� ����ϰԵǹǷ� �������̽� ���
*/
public interface IDamageObject
{
	void TakeDamage(int amount, Vector3 hitPosition);
}
