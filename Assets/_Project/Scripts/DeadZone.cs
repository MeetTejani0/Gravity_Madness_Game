using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }
}
