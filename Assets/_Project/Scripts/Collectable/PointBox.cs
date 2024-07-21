using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBox : MonoBehaviour, ICollectable
{
    [SerializeField] private int points = 1;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }
    public int Point => points;

    public void CollectEffect()
    {
        Destroy(gameObject);
    }

    public int CollectPoint()
    {
        return Point;
    }
}
