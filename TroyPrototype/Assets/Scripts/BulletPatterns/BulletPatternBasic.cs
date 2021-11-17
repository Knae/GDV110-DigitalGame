using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternBasic : BaseBulletPattern
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool fireProjectiles()
    {
        return base.fireProjectiles();
    }

    public override float GetBulletForce()
    {
        return base.GetBulletForce();
    }
}
