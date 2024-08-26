/*
This script is used for the road generator.
It will iteratively create roads between the specified points.

This script is used in conjunction with the PointGen script to ensure the maximum amount of connections applies to objects outside of the generator.




All code is made by Isaac van Beek unless specified otherwise.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityExpansionsPack;

[ExecuteInEditMode]
public class RoadAttachment : MonoBehaviour
{
    [ReadOnly] public List<GameObject> attachedObjects = new();

    void Update() {
        // Removes the attachedObjects:
        attachedObjects.RemoveAll(go => go == null);

        if(attachedObjects.Count == 0) {
            UnityExpansions.DestroySafely(this);
        }
    }
}
