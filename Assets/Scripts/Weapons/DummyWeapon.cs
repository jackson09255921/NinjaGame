using UnityEngine;

[CreateAssetMenu]
public class DummyWeapon : Weapon
{
    public override void PerformAttack(Player player)
    {
        player.animator.SetTrigger("Attack");
    }
}