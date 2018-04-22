using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomProperties : MonoBehaviour
{
    // Doors
    public bool IsLaser;
    public bool IsVertical;
    public bool IsOpen;

    // Enemy Triggers
    public int OnDeathToggle = -1;
    public bool OnDeathToggleAll;

    public string PatrolBehavior;
}
