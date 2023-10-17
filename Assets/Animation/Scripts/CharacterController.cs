using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator _animator;

    private string _animPunchBlend = "PunchBlend";
    private string _animPunchTrigger = "Punch";
    private string _animFightReady = "FightReady";
    private bool _fightReady = false;
    private int _punchAmount = 0;
    private bool _disableAnimChange = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (_disableAnimChange) return;

        HandleCombatAnimations();
    }

    private void HandleCombatAnimations()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleFightState();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_fightReady)
            {
                _animator.SetFloat(_animPunchBlend, _punchAmount);
                _animator.SetTrigger(_animPunchTrigger);

                if (_punchAmount == 2)
                {
                    _punchAmount = 0;
                    DisableAnimationsForPeriod(2.3f);
                }
                else
                {
                    _punchAmount++;
                    DisableAnimationsForPeriod(1.1f);
                }
            }
            else
            {
                ToggleFightState();
            }
        }
    }

    private void ToggleFightState()
    {
        _fightReady = !_fightReady;
        _animator.SetBool(_animFightReady, _fightReady);
        DisableAnimationsForPeriod(0.6f);
    }

    private void DisableAnimationsForPeriod(float time)
    {
        StartCoroutine(WaitForTime(time));
    }

    private IEnumerator WaitForTime(float time)
    {
        _disableAnimChange = true;
        yield return new WaitForSeconds(time);
        _disableAnimChange = false;
    }
}