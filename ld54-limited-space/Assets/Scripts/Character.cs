using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public enum AttributeType
{
	Health,
	Damage,
	Defense,
	Magic,
	Resist,
	Speed
}

public class Character : MonoBehaviour
{
	public int Health;
	public int Damage;
	public int Defense;
	public int Magic;
	public int Resist;
	public int Speed;

	public float AttackGauge;

	public Character Target;

	public event Action<Character> OnAttackFinished;

	public void Attack()
	{
		StartCoroutine(AttackCoroutine());
	}

	public IEnumerator AttackCoroutine()
	{
		transform.DOMoveX(Target.transform.position.x, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
		yield return new WaitForSeconds(0.5f);

		Target.Health -= Mathf.Max(Damage - Target.Defense, 0);

		yield return new WaitForSeconds(0.5f);
		OnAttackFinished.Invoke(this);
	}
}
