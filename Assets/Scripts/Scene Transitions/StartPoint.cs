using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartPoint
{
    // Location the player is entering from
    public SceneTransitionManager.Location enteringFrom;

    // The tranform the player should start in
    public Transform playerStart;
}
