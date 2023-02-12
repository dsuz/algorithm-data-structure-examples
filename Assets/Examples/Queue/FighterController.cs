using UnityEngine;

/// <summary>
/// プレイヤーを制御するコンポーネント
/// </summary>
public class FighterController : MonoBehaviour
{
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        _anim.SetInteger("AttackPattern", 0);
    }

    public void NormalAttack()
    {
        _anim.SetInteger("AttackPattern", 1);
    }

    public void BigAttack()
    {
        _anim.SetInteger("AttackPattern", 2);
    }

    public void QuickAttack()
    {
        _anim.SetInteger("AttackPattern", 3);
    }
}
