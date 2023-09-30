using UnityEngine;

public class CombatManager : MonoBehaviour
{
	public float BaseCombatTickTime;

	public Character Character1;
	public Character Character2;

	private bool _waitingAttack;

	private void Start()
	{
		Character1.AttackGauge = BaseCombatTickTime;
		Character2.AttackGauge = BaseCombatTickTime;
		_waitingAttack = false;
	}

	private void Update()
	{
		if (_waitingAttack)
		{
			return;
		}

		Character1.AttackGauge -= Time.deltaTime * (100 + Character1.Speed) / 100f;
		Character2.AttackGauge -= Time.deltaTime * (100 + Character2.Speed) / 100f;

		if (Character1.AttackGauge <= 0)
		{
			_waitingAttack = true;
			Character1.OnAttackFinished += Character_OnAttackFinished;
			Character1.Attack();
			return;
		}

		if (Character2.AttackGauge <= 0)
		{
			_waitingAttack = true;
			Character2.OnAttackFinished += Character_OnAttackFinished;
			Character2.Attack();
		}
	}

	private void Character_OnAttackFinished(Character character)
	{
		character.OnAttackFinished += Character_OnAttackFinished;
		character.AttackGauge = BaseCombatTickTime;
		_waitingAttack = false;
	}
}
