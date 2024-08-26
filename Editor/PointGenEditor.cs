/*
This script is used for the road generator.
It will iteratively create roads between the specified points.

This script is used in conjunction with the RoadGen script to ensure the maximum amount of connections applies to objects outside of the generator.




All code is made by Isaac van Beek unless specified otherwise.
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadGen), true)]
public class PointGenEditor : Editor
{
// _______________________________________________________________________Custom Inspector:___________________________________________________________________________________________________

    public override void OnInspectorGUI()
    {
        RoadGen targetScript = (RoadGen)target;

        GUILayout.Space(10);

        GUILayout.TextArea("This tool is used to generate road points in a network. The settings below determine the resulting road networks visuals. Hover over any of the settings for more information.");

        GUILayout.Space(10);

        // Displays the varius data to be used for the road generator and to debug:
        GUILayout.Label("Point data:");
        string iterationCount = "current iteration: " + targetScript.iterationCount.ToString();
        GUILayout.Label(iterationCount);
        string sideRoadCount = "Amount of side roads: " + targetScript.amountOfBranches.ToString() + "/" + targetScript.maxAmountOfBranches.ToString();
        GUILayout.Label(sideRoadCount);


        GUILayout.Space(10);

        // Buttons to manipulate the road generation:
        if(GUILayout.Button(new GUIContent("Generate Points", "Generate the road points with the current settings."))) {
            targetScript.Clear();
            targetScript.Execute();
            targetScript.ExecuteSideRoads();
            allCornerPositions.Clear();
            GetPositions(targetScript.allCorners.ToArray());
        }

        if(GUILayout.Button(new GUIContent("Clear Points", "Remove all the previously generated points."))) {
            targetScript.Clear();
            allCornerPositions.Clear();
        }


        GUILayout.Space(10);


        // Display all varius inputs:
        // The prefab references (the false ensures these are prefabs and not GameObjects from a scene)
        GUILayout.Label("Prefab reference:");
        targetScript.mainPointIndicator = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Main points", "Prefab used to indicate the main points that the road will always go through."), targetScript.mainPointIndicator, typeof(GameObject), false);
        targetScript.pointIndicator = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Road points", "Prefab used for the points in the road."), targetScript.pointIndicator, typeof(GameObject), false);

        GUILayout.Space(10);

        // Displays the main points:
        DrawDefaultInspector();

        GUILayout.Space(10);

        // Displays the variables for roads:
        GUILayout.Label("Road settings:");
        targetScript.distanceBetweenCorners = EditorGUILayout.FloatField(new GUIContent("Distance between corners", "The average distance between each road point."), targetScript.distanceBetweenCorners);
        if(targetScript.distanceBetweenCorners < 1) {
            targetScript.distanceBetweenCorners = 1;
        }
        targetScript.maxCornerMovement = EditorGUILayout.Slider(new GUIContent("Max corner movement", "The maximum distance each point can randomly move from its original spawn."), targetScript.maxCornerMovement, 0, targetScript.distanceBetweenCorners);
        targetScript.minimumAmountOfCornersBetweenConnections = EditorGUILayout.IntField(new GUIContent("Min amount of corners between connection", "The minimum amount of points needed between the start and end of each side road."), targetScript.minimumAmountOfCornersBetweenConnections);
        if(targetScript.minimumAmountOfCornersBetweenConnections < 1) {
            targetScript.minimumAmountOfCornersBetweenConnections = 1;
        }
        targetScript.minSpaceAroundEachConnectionPoint = EditorGUILayout.Slider(new GUIContent("Min space around each point", "The minimum amount of space that is required for an end point of a side road to spawn."), targetScript.minSpaceAroundEachConnectionPoint, 0, targetScript.distanceBetweenCorners);

        GUILayout.Space(10);

        // Displayes the variables for side roads:
        GUILayout.Label("Side road settings:");
        targetScript.maxIteration = EditorGUILayout.IntSlider(new GUIContent("Amount of iterations", "The amount of times side roads will be spawned by the newest points."), targetScript.maxIteration, 0, 15);
        targetScript.branchingChance = EditorGUILayout.IntSlider(new GUIContent("Chance of road branch", "The chance of a side road spawning for each point."), targetScript.branchingChance, 0, 100);
        targetScript.maxAmountOfBranches = EditorGUILayout.IntSlider(new GUIContent("Max branches per point", "The maximum amount of side roads each point can have. (Main road is not counted.)"), targetScript.maxAmountOfBranches, 0, 15);
        targetScript.maxBranchTravelDistance = EditorGUILayout.FloatField(new GUIContent("Max branch travel distance", "The maximum distance a side road can be."), targetScript.maxBranchTravelDistance);
        if(targetScript.maxBranchTravelDistance < targetScript.distanceBetweenCorners) {
            targetScript.maxBranchTravelDistance = targetScript.distanceBetweenCorners;
        }
    }

// ___________________________________________________________________________Road Lines:_____________________________________________________________________________________________________

    bool firstTimeOnSceneGUI = true;
    List<Vector3[]> allCornerPositions = new List<Vector3[]>();

    public void OnSceneGUI() {
        // Makes sure that the GetAllPositions is only called when needed.
        // This is done to ensure roads are correctly displayed even when the roads were edited while the RoadGen GUI was not open.
        if(firstTimeOnSceneGUI) {
            RoadGen targetScript = (RoadGen)target;
            allCornerPositions.Clear();
            GetPositions(targetScript.allCorners.ToArray());
            firstTimeOnSceneGUI = false;
        }

        // Draws a line connecting each Vector3 in the array, foreach array in the list:
        foreach(Vector3[] cornerPositions in allCornerPositions) {
            Handles.DrawPolyLine(cornerPositions);
        }
    }

    // Transforms the given array of GameObjects into an array of Vector3
    void GetPositions(GameObject[] corners) {
        // Create a new array to store every corners position in:
        Vector3[] positionsOfCorners = new Vector3[corners.Length];
        for(int i=0; i<positionsOfCorners.Length; i++) {
            // Set the corners position in the array:
            positionsOfCorners[i] = corners[i].transform.position;

            // Iterate when the child has its own roads and display these to the parents lines:
            if(corners[i].TryGetComponent<RoadGen>(out RoadGen pointGenOfCorner) && corners[i].transform.childCount > 0) {
                GetPositions(pointGenOfCorner.allCorners.ToArray());
            }
        }

        // Add this array of positions as a new Vector3[] in the allCornerPositions list:
        allCornerPositions.Add(positionsOfCorners);
    }
}

// _____________________________________________________________________________Old Code:___________________________________________________________________________________________________

    // void makeLines(GameObject[] objects) {
    //     for(int objectIndex=0; objectIndex<objects.Length; objectIndex++) {
    //         GameObject child = objects[objectIndex];
    //         if(child.TryGetComponent<RoadGen>(out RoadGen pointGenOfChild)) {
    //             allPoints.AddRange(pointGenOfChild.allCorners);
    //             // Vector3[] childCorners = pointGenOfChild.roadPoints.ToArray();
    //             // Handles.DrawLines(childCorners);
    //         }
    //         // if(child.transform.childCount > 0) {
    //         //     makeLines(child.transform);
    //         // }

    //     }
    // }
        
        // makeLines(corners);
        
        

        // for(int i=1; i<corners.Length; i++) {
            // Vector3 previousPointPosition = corners[i-1].transform.position;
            // Vector3 currentPointPosition = corners[i].transform.position;
            // Handles.DrawLine(previousPointPosition, currentPointPosition);

            // if(corners[i].TryGetComponent<RoadGen>(out RoadGen childScript)) {
            //     List<GameObject> childCorners = childScript.allCorners;

            //     for(int e=1; e<childCorners.Count; e++) {
            //         Vector3 point1 = corners[e-1].transform.position;
            //         Vector3 point2 = corners[e].transform.position;
            //         Handles.DrawLine(point1, point2);
            //     }
            // }
        // }

        // foreach(Transform child in targetScript.transform) {
        //     if(child.TryGetComponent<RoadGen>(out RoadGen pointGen)) {
        //         if(pointGen.attachmentPoints.Count > 0) {
        //             Debug.Log(pointGen.attachmentPoints.Count);
        //             for(int i=1; i<pointGen.attachmentPoints.Count; i++) {
        //                 Vector3 previousAttachmentPoint = pointGen.attachmentPoints[i-1];
        //                 Vector3 currentAttachmentPoint = pointGen.attachmentPoints[i];
        //                 Handles.DrawLine(previousAttachmentPoint, currentAttachmentPoint);
        //             }
        //         }
        //     }
        // }