using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected virtual void Move()
    {

    }

    protected abstract void Attack();
}
