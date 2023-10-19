using UnityEngine;

public class CarrotBehavior : ProjectileWeaponBase
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += direction * (currentSpeed * Time.deltaTime);
    }
}
