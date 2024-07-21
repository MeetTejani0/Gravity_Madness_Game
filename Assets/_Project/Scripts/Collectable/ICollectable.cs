using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    public int Point { get; }

    public int CollectPoint();
    public void CollectEffect();
}
