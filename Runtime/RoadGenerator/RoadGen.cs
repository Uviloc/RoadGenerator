/*
This script is used for the road generator.
It will iteratively create roads between the specified points.

This script has the main functionality of the generator.
The variables can be tweaked to get different results.




All code is made by Isaac van Beek unless specified otherwise.
*/



using System.Collections.Generic;
using UnityEngine;
using UnityExpansionsPack;

[ExecuteInEditMode]
public class RoadGen : MonoBehaviour
{
// ___________________________________________________________________________Variables:______________________________________________________________________________________________________

    // Prefab objects to use for the points:
    [HideInInspector] public GameObject pointIndicator;
    [HideInInspector] public GameObject mainPointIndicator;

    // The points that the generator will go through to generate the roads:
    [Tooltip("The points in the road that will always be hit.")]
    public List<GameObject> mainPoints = new();

    // Road variables:
    [HideInInspector] public float minSpaceAroundEachConnectionPoint = 7;
    [HideInInspector] public float distanceBetweenCorners = 7.5f;
    [HideInInspector] public float maxCornerMovement = 5.5f;
    [HideInInspector] [Min(1)] public int minimumAmountOfCornersBetweenConnections = 5;

    // Iteration counter and max iteratoin for the side roads creating new side roads:
    [HideInInspector] [Min(0)] public int maxIteration = 3;
    [HideInInspector] public int iterationCount = 0; // Not to be set by user.

    // Side road/Branches variables:
    [HideInInspector] [Min(0)] public int maxAmountOfBranches = 2;
    [HideInInspector] [Range(0,100)] public int branchingChance = 20;
    [HideInInspector] public int amountOfBranches = 0; // Not to be set by user.
    [HideInInspector] public float maxBranchTravelDistance = 30;


    // List of all generated corners in order. This is used for the road network lines:
    [HideInInspector] public List<GameObject> allCorners = new();

// __________________________________________________________________________Check Events:____________________________________________________________________________________________________
// These checks are done to have the inspector show the correct information and so that the generator has all the correct components:

    private bool executeChecks = false;

    void Awake() {executeChecks = true;}
    void OnValidate() {executeChecks = true;}

    void Update() {
        // Done if any MainPoints are deleted from the scene
        if(!mainPoints.TrueForAll(x => x != null)) {
            executeChecks = true;
        }

        // Check for the various components that are needed to have the script function
        if(executeChecks && iterationCount==0) {
            if(CheckParams()) { // Needs to be done before CheckMainPoints is executed.
                Debug.LogWarning("Abandoning Execution of checks.");
                executeChecks = false;
                return;
            }
            CheckMainPoints();
            CheckGenerationFolder();
            executeChecks = false;
        }
    }

// ________________________________________________________________________Check Functions:___________________________________________________________________________________________________
    // CheckGenerationFolder checks if there is a folder named generatedPoints in the road generator. In this folder the generated road points will be put.
    Transform generatedPoints;
    void CheckGenerationFolder() {
        generatedPoints = transform.Find("generatedPoints");
        if(generatedPoints == null) {
            generatedPoints = new GameObject().transform;
            generatedPoints.name = "generatedPoints";
            generatedPoints.SetParent(transform);
            generatedPoints.localScale = Vector3.one;
            generatedPoints.localRotation = Quaternion.identity;
            generatedPoints.localPosition = Vector3.zero;
        }
    }


    // CheckMainPoints checks if the main points in the list are all populated with the correct corners. This check also deletes the enty when the gameobject no longer exists.
    // Furthermore the check will populate atleast 2 main points at all times to ensure a start and end of the road.
    void CheckMainPoints() {
        // Make sure there are atleast 1 startpoint and 1 endpoint:
        while(mainPoints.Count<2) {
            mainPoints.Add(null);
        }

        // Delete any entries where the gameObject does not exist, except if all are empty:
        // Leaves a minimum of 2 points, even if these are empty
        if(!mainPoints.TrueForAll(x => x == null) && mainPoints.Count > 2) {
            mainPoints.RemoveAll(x => x == null);
        }

        // This ensures that all entries are in the correct order and no duplicates are ever made. It will also rename the points if they are incorrect.
        for(int i=0; i<mainPoints.Count; i++) {
            GameObject mainPoint;
            
            int indexOfFirstOccurrence = mainPoints.IndexOf(mainPoints.Find(o => o == mainPoints[i]));
            // Since we already got rid of empty entries except start and end, this will ensure that these 2 are populated at all times.
            if(indexOfFirstOccurrence < i || // This is a duplicate point, this means a new entry got made (Unity duplicates the last entry when creating a new one)
            (indexOfFirstOccurrence == i && mainPoints[i] == null)) { // This is only true if all previous entries are null, meaning the list just got generated and there are no existing corners
                mainPoint = Instantiate(mainPointIndicator, Vector3.zero, Quaternion.identity, transform);
                mainPoints[i] = mainPoint;
            } else {
                mainPoint = mainPoints[i];
            }

            // Names the points accordingly:
            if(i==0) {
                mainPoint.name = "StartPoint";
            } else if(i==mainPoints.Count-1) {
                mainPoint.name = "EndPoint";
            } else {
                mainPoint.name = "Corner" + (i).ToString();
            }
        }

        // For any old points found in the transform that we have removed from the list in the code above, we now want to destroy.
        for(int i=transform.childCount-1; i>0; i--) {
            GameObject child = transform.GetChild(i).gameObject;
            if((child.name.StartsWith("Corner") || child.name == "StartPoint" || child.name == "EndPoint") && !mainPoints.Contains(child)) {
                UnityExpansions.DestroySafely(child);
            }
        }
    }

    /// <summary>
    /// Checks the various parameters of script to check if any incorrect values are found.
    /// </summary>
    /// <returns>The <see cref="bool"/> is true if any parameters are incorrect.</returns>
    public bool CheckParams() {
        if(!pointIndicator) {
            Debug.LogWarning("pointIndicator is not filled in.");
            return true;
        }
        if(!mainPointIndicator) {
            Debug.LogWarning("mainPointIndicator is not filled in.");
            return true;
        }
        return false;
    }

// _________________________________________________________________________Main Functions:___________________________________________________________________________________________________
// These functions are the ones used to generate the roads or clear the generator.

    /// <summary>
    /// Clears all the points that were generated in this transform and reset the list and branch count.
    /// </summary>
    public void Clear() {
        CheckGenerationFolder(); // Done to ensure the existence of the folder.
        UnityExpansions.DestroyAllChildren(generatedPoints);
        allCorners.Clear();
        amountOfBranches = 0;
    }

    /// <summary>
    /// This will create a road for each main point.
    /// </summary>
    public void Execute() {
        // Check prefabs to see if they exist before executing the code:
        if(CheckParams()) {
            Debug.LogWarning("Abandoning Execution of generation.");
            return;
        }

        for(int i = 0; i < mainPoints.Count; i++) {
            // We create a new point for every main point so that we can have this in the list of all corners and so we will not mess with the original main points.
            GameObject mainPoint = Instantiate(pointIndicator, mainPoints[i].transform.position, mainPoints[i].transform.rotation, generatedPoints);
            mainPoint.name = "MainPoint" + (i).ToString();
            allCorners.Add(mainPoint);

            if(i != mainPoints.Count-1) { // Check if not the last point
                Vector3 start = mainPoints[i].transform.position;
                Vector3 end = mainPoints[i+1].transform.position;
                GenerateRoadPoints(allCorners, pointIndicator, start, end, distanceBetweenCorners, maxCornerMovement, generatedPoints);
            }
        }
    }

    /// <summary>
    /// Creates side roads for each point in the allCorners list. This function then calls on each next iteration.
    /// </summary>
    public void ExecuteSideRoads() {
        // Exit the iteration loop when the max iteration has been hit:
        if(iterationCount+1 > maxIteration) {
            return;
        }

        // Generate side roads for each point.
        for(int i=1; i<allCorners.Count; i++) {
            // Create variables for GenerateAttachmentPoints function based on the current point index:
            Transform currentPoint = allCorners[i].transform;
            Transform previousPoint = allCorners[i-1].transform;
            RoadGen pointGenOfCurrent;
            if(!currentPoint.gameObject.TryGetComponent<RoadGen>(out pointGenOfCurrent)) {
                pointGenOfCurrent = AddPointGen(currentPoint.gameObject);
            }
            GenerateAttachmentPoints(currentPoint, previousPoint, pointGenOfCurrent);

            // If a sideroad got created, we now want to generate the road itself.
            if(pointGenOfCurrent.mainPoints.Count > 0) {
                // Currently we have the end point in the mainPoints, this means we did get a hit to generate a side road, but still need to insert the start point at index 0.
                pointGenOfCurrent.mainPoints.Insert(0, currentPoint.gameObject);

                // Go through the same steps as all roads.
                pointGenOfCurrent.CheckGenerationFolder();
                pointGenOfCurrent.Execute();
                pointGenOfCurrent.ExecuteSideRoads();
            }
        }
    }

// ___________________________________________________________________Add RoadGen Component Function:________________________________________________________________________________________

// Adds the RoadGen script to the specified object and returns the script reference.
// All variables are copied over to the new script, increasing the iteration by 1.

    /// <summary>
    /// Adds the RoadGen script to the specified object and sets all variable to that of the parent.
    /// </summary>
    /// <returns>The script reference of the <see cref="RoadGen"/></returns>
    /// <param name="obj">GameObject to add the RoadGen script to.</param>
    RoadGen AddPointGen(GameObject obj) {
        RoadGen pointGen = obj.AddComponent<RoadGen>();

        pointGen.pointIndicator = pointIndicator;
        pointGen.maxIteration = maxIteration;
        pointGen.distanceBetweenCorners = distanceBetweenCorners;
        pointGen.maxCornerMovement = maxCornerMovement;
        pointGen.maxAmountOfBranches = maxAmountOfBranches;
        pointGen.branchingChance = branchingChance;
        pointGen.mainPointIndicator = mainPointIndicator;
        pointGen.minimumAmountOfCornersBetweenConnections = minimumAmountOfCornersBetweenConnections;
        pointGen.maxBranchTravelDistance = maxBranchTravelDistance;
        pointGen.iterationCount = iterationCount + 1;

        return pointGen;
    }

// _________________________________________________________________Road Points Generation Function:_________________________________________________________________________________________


    /// <summary>
    /// Generates road points between the 2 specified locations.
    /// </summary>
    /// <param name="listOfCorners">The list of objects to add each generated point to. (IMPORTANT: atleast 1 point needs to already be present.)</param>
    /// <param name="pointIndicatorPrefab">The prefab to use for the road points.</param>
    /// <param name="startPoint">The location of the first point.</param>
    /// <param name="endPoint">The location of the last point.</param>
    /// <param name="distanceBetweenCorners">The initial distance the corners will be placed in.</param>
    /// <param name="maxCornerMovement">The maximum distance a corner can randomly translate.</param>
    /// <param name="parentFolder">The folder to put the generated corner object into. If left out, corners will be put in transform.</param>
    void GenerateRoadPoints(List<GameObject> listOfCorners, GameObject pointIndicatorPrefab, Vector3 startPoint, Vector3 endPoint, float distanceBetweenCorners, float maxCornerMovement, Transform parentFolder = null) {
        if(parentFolder) {
            parentFolder = transform;
        }
        
        // Initially calculate the distnace left between the start and end
        float distanceLeft = Vector3.Distance(startPoint, endPoint);
        // Continue to add a point while there is still space left between the current point and the end.
        for (int i = 1; distanceLeft >= distanceBetweenCorners + maxCornerMovement; i++) {
            // Get basic values to get new point:
            Transform previousPoint = listOfCorners[listOfCorners.Count-1].transform;
            float distanceLeftToEndPoint = Vector3.Distance(previousPoint.position, endPoint);
            Vector3 newPosition = UnityExpansions.GetVector3Between(previousPoint.position, endPoint, distanceBetweenCorners / distanceLeftToEndPoint);
            // Create a new point at the specified location:
            GameObject corner = Instantiate(pointIndicatorPrefab, newPosition, Quaternion.identity, generatedPoints);
            corner.name = "point" + (i).ToString();

            // Have the point rotate to the endPoint (only done to have correct translation and for the last point to have correct rotation):
            corner.transform.LookAt(endPoint);
            // Add some random translation to the points:
            corner.transform.Translate(UnityExpansions.GetRandomVector3Between(new Vector3(-maxCornerMovement, 0, -maxCornerMovement), new Vector3(maxCornerMovement, 0, maxCornerMovement)), Space.Self);
            // Have the previous point rotate towards the new point:
            previousPoint.LookAt(corner.transform);

            // Add object to list of all corners to be made into the line network:
            listOfCorners.Add(corner);

            // Add a RoadGen script to the new points
            AddPointGen(corner);

            // Calculate new distance left for next iteration of the loop:
            distanceLeft = Vector3.Distance(corner.transform.position, endPoint);
        }
    }

// _______________________________________________________________________Side Road Generation Function:______________________________________________________________________________________

    /// <summary>
    /// Will attempt to create create attachment points for the given road point. If successfull these will be added into the Main Points of the given road point.
    /// </summary>
    /// <param name="currentPoint">The point that should be checked for connections.</param>
    /// <param name="previousPoint">The point that is previous in the list of allCorners.</param>
    /// <param name="pointGenOfCurrent">The script reference of the RoadGen on the current point.</param>
    void GenerateAttachmentPoints(Transform currentPoint, Transform previousPoint, RoadGen pointGenOfCurrent) {
        // Check the distance to the previousPoint to not include this point in the possible attachment points:
        float distanceToPreviousPoint = Vector3.Distance(currentPoint.position, previousPoint.position);
        
        // Find all possible attachment points.
        Collider[] hits = UnityExpansions.OverlapHollowSphere(currentPoint.position, distanceToPreviousPoint+1, maxBranchTravelDistance, 1 << 9);
        hits = UnityExpansions.RemoveDuplicates(hits);

        // Check multiple factors for every point.
        foreach(Collider hit in hits) {
            // Checks if the attachment limit has been reached:
            if(pointGenOfCurrent.amountOfBranches >= pointGenOfCurrent.maxAmountOfBranches) {
                break;
            }

            // Check if the branch should be made (IterationCount is included here to have the chances deminish so it doesnt grow out of control)
            if(Random.Range(0, 101) > branchingChance/(1.5*iterationCount+1)) {
                continue;
            }

            // Checks if there are no other points.() This is done since some points can overlap creating a connection cap of the sum of these points.)
            Collider[] nearbyObjects = Physics.OverlapSphere(hit.ClosestPoint(currentPoint.position), minSpaceAroundEachConnectionPoint, 1 << 9);
            if(nearbyObjects.Length > 1) {
                continue;
            }


            // Get indeces of the start and end point:
            int ownIndex = allCorners.IndexOf(currentPoint.gameObject);
            int hitIndex = allCorners.IndexOf(allCorners.Find(go => go == hit.gameObject)); // Will be -1 if the hit is not in the allcorners

            GameObject hitPoint;
            // Check if the object that was hit is part of the road generator (and therefore a road point):
            if(hitIndex == -1 && hit.transform.root != currentPoint.root) {
                // The hit is not in the generator.
                // Add a RoadAttachment component (if it did not exist yet) to the hit object. This is done to ensure not too many other roads can attach to the object.
                RoadAttachment roadAttachment;
                if(!hit.transform.TryGetComponent<RoadAttachment>(out roadAttachment)) {
                    roadAttachment = hit.transform.gameObject.AddComponent<RoadAttachment>();
                }
                // Check if this object did not already have too many attached roads.
                if(roadAttachment.attachedObjects.Count >= maxAmountOfBranches) {
                    continue;
                }

                // Create an attachment point
                hitPoint = Instantiate(pointIndicator, hit.ClosestPoint(currentPoint.position), Quaternion.identity, currentPoint);
                hitPoint.name = "EndPoint";
                // Set the layer to 0 (defealt) as we do not want this point to show up as a possible attachment point.
                hitPoint.layer = 0;

                // Add this point to the object that was hit. This will make sure it gets reset when the point is deleted.
                roadAttachment.attachedObjects.Add(hitPoint);

            // Gets the RoadGen script reference (also ensures objects that are in the generator need a RoadGen script to be seen as an attachment point):
            } else if(hit.transform.TryGetComponent<RoadGen>(out RoadGen pointGenOfHit) &&
            // Checks if this point is not at the limit of branches:
            pointGenOfHit.amountOfBranches < pointGenOfHit.maxAmountOfBranches &&
            // Checks if the distance between the 2 is enough to connect:
            Mathf.Abs(ownIndex - hitIndex) > minimumAmountOfCornersBetweenConnections) {
                hitPoint = hit.gameObject;
                pointGenOfHit.amountOfBranches++;
            } else {
                continue;
            }
            pointGenOfCurrent.mainPoints.Add(hitPoint);
            pointGenOfCurrent.amountOfBranches++;
        }
    }
}

// _____________________________________________________________________________Old Code:___________________________________________________________________________________________________


            // // Go to next road point if the chance was not hit. (IterationCount is included here to have the chances deminish so it doesnt grow out of control)
            // // This is also here to not do physics calculations if none of the brances were a hit
            // if(Random.Range(0, 101) > branchingChance*maxAmountOfBranches/(1.5*iterationCount+1)) {
            //     continue;
            // }


                    // int ownIndex = allCorners.IndexOf(corner);
                    // int hitIndex = allCorners.IndexOf(allCorners.Find(go => go == hit.gameObject));
                    // if(Random.Range(0, 101) < branchingChance/hits.Length && 
                    // (hitIndex == -1 || Mathf.Abs(ownIndex - hitIndex) > minimumAmountOfCornersBetweenConnections)) {
                    //         if(hit.transform.TryGetComponent<RoadGen>(out RoadGen pointGenOfHit)) {
                    //             if(pointGenOfHit.iterationCount == pointGen.iterationCount) {
                    //                 GameObject hitPoint;
                    //                 // if(!hit.transform.IsChildOf(generatedPoints)) {
                    //                 if(hit.transform.root != corner.transform.root) {
                    //                     hitPoint = Instantiate(pointIndicator, hit.ClosestPoint(corner.transform.position), Quaternion.identity, corner.transform);
                    //                     hitPoint.name = "EndPoint";
                    //                 } else {
                    //                     hitPoint = hit.gameObject;
                    //                 }
                    //                 pointGen.mainPoints.Add(corner);
                    //                 pointGen.mainPoints.Add(hitPoint);
                    //                 pointGen.CheckGenerationFolder();
                    //                 pointGen.Execute();
                    //                 // pointGen.allCorners.Add(hitPoint);
                    //             }
                    //         }
                    //     }
                    // }
                    // chance is correct {
                    //     calculate indexes
                    //     if hitindex = -1 {
                    //         generate new end point (instantiate)
                    //     } else if (index is far enough appart &&
                    //     iterationcount == same) {
                    //         hitpoint = hit.gameobject
                    //     } else {
                    //         continue;
                    //     }
                    // }

                    // if(Random.Range(0, 101) < branchingChance/hits.Length && hit.transform != previousPoint) {
                    //     GameObject newEndPoint = Instantiate(pointIndicator, hit.ClosestPoint(corner.transform.localPosition), Quaternion.identity, corner.transform);
                    //     newEndPoint.name = "EndPoint";
                    //     pointGen.mainPoints.Add(newEndPoint);
                    //     pointGen.CheckGenerationFolder();
                    //     pointGen.Execute();
                    // }
                // Have each point be able to be a startpoint/endpoint. Then have these connect so they go across



// SPIRAL:
// for(int i = 1; i<amountOfCorners; i++) {
//     Transform previousPoint = corners[corners.Count-1].transform;
//     Debug.Log("previous point localPosition " + previousPoint.localPosition);
//     GameObject corner = Instantiate(pointIndicator, UnityExpansions.GetVector3Between(startPoint, endPoint,  percentage * i), Quaternion.identity, transform);
//     // corner.transform.RotateAround(startPoint, startIndicator.transform.eulerAngles, (rotation / amountOfCorners) * i); // creates a spiral

//     Debug.Log("previous point " + (corners.Count-1));
//     corner.transform.RotateAround(previousPoint.localPosition, previousPoint.eulerAngles, maxCornerRotation);
//     Debug.Log("rotation " + maxCornerRotation);
//     // corner.transform.LookAt(previousPoint.localPosition);
//     corner.transform.rotation = Quaternion.LookRotation(corner.transform.localPosition - previousPoint.localPosition);
//     corners.Add(corner);
// }

    // public void Execute() {
    //     GameObject startIndicator = Instantiate(pointIndicator, startPoint, Quaternion.identity, generatedPoints);
    //     allCorners.Add(startIndicator);
    //     roadPoints.Add(startPoint);
    //     startIndicator.transform.LookAt(endPoint);

    //     for(int i = 0; i < mainPoints.Count + 1; i++) {
    //         Vector3 start = startPoint;
    //         if(i >= 1) {
    //             start = mainPoints[i-1].transform.localPosition;
    //         }
    //         Vector3 end = endPoint;
    //         if(i < mainPoints.Count) {
    //             end = mainPoints[i].transform.localPosition;
    //         }
    //         GenerateRoadPoints(allCorners, pointIndicator, start, end, distanceBetweenCorners, maxCornerMovement);
    //         GameObject inBetweenIndicator = Instantiate(pointIndicator, end, Quaternion.identity, generatedPoints);
    //         allCorners.Add(inBetweenIndicator);
    //         roadPoints.Add(inBetweenIndicator.transform.localPosition);
    //         roadPoints.Add(inBetweenIndicator.transform.localPosition);
    //     }
    //     roadPoints.Add(endPoint);
    // }


    // public void Shootcast(Vector3 direction) {
    //     Ray ray = new Ray(transform.localPosition, direction);
    //     if(Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, 1 << 9)) {
    //         endPoint = hitInfo.point;
    //         Execute();
    //     }
    // }

    
    // public void CreateSideRoads(List<GameObject> corners) {
    //     foreach(GameObject corner in corners) {
    //         RoadGen pointGen = corner.AddComponent<RoadGen>();
    //         pointGen.pointIndicator = pointIndicator;
    //         pointGen.startPoint = corner.transform.localPosition;
    //         pointGen.maxIteration = maxIteration;
    //         // pointGen.endPoint = ?????

    //         // smaller per iteration
    //         // vvvvvvvvvv
    //         pointGen.distanceBetweenCorners = distanceBetweenCorners;
    //         pointGen.maxCornerMovement = maxCornerMovement;
    //         pointGen.iterationCount = iterationCount + 1;
    //         pointGen.inBetweenPoints = new Vector3[0];


    //         if(pointGen.iterationCount <= maxIteration) {
    //             int indexOfCorner = corners.IndexOf(corner);
                
    //             Collider[] hits = UnityExpansions.OverlapHollowSphere(corner.transform.localPosition, minRoadLength, 1000, 1 << 9);
    //             foreach(Collider hit in hits) {
    //                 int indexOfHit = corners.IndexOf(hit.gameObject);
    //                 // if(indexOfCorner-1==indexOfHit || indexOfHit==indexOfCorner+1) {
    //                 //     return;
    //                 // }
    //                 if(Random.Range(0, 101) < branchingChance/hits.Length) {
    //                     pointGen.endPoint = hit.ClosestPoint(corner.transform.localPosition);
    //                     pointGen.Execute();
    //                 }
    //             }
    //         }
    //     }
    // }