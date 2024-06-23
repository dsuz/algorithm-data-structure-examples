using UnityEngine;

/// <summary>
/// プレイヤーを制御するコンポーネント
/// </summary>
public class FighterController : MonoBehaviour
{
    [SerializeField] Animator _animator;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        _animator.SetInteger("AttackPattern", 0);
    }

    public void NormalAttack()
    {
        _animator.SetInteger("AttackPattern", 1);
    }

    public void BigAttack()
    {
        _animator.SetInteger("AttackPattern", 2);
    }

    public void QuickAttack()
    {
        _animator.SetInteger("AttackPattern", 3);
    }
}
