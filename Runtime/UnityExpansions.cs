// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionsPack {
    public class UnityExpansions : MonoBehaviour
    {
        /// <summary>Returns a random position inside the given bounds</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/> position.</returns>
        /// <param name="bounds">The bounds in which the position needs to be inside.</param>
        public static Vector3 RandomPos(Bounds bounds) {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        /// <summary>Randomely rotates the given Vector3 somewhere between the given max for each cardinal direction.</summary>
        /// <returns>The new <see cref="UnityEngine.Quaternion"/>.</returns>
        /// <param name="originalRot">The original rotation that needs to be rotated.</param>
        /// <param name="maxRandomisedRotation">The max amount of degrees the original can rotate by.(max 90)</param>
        /// <param name="rotationAxis">The axis where the new rotation needs to rotate around.</param>
        public static Quaternion RandomRot(Vector3 originalRot, float maxRandomisedRotation, Vector3 rotationAxis) {
            float[] cardinalDirections = {0, 90, 180, 270};
            float randomCardinalDirection = cardinalDirections[Random.Range(0, 4)] + originalRot.y;
            return Quaternion.AngleAxis(Random.Range(-maxRandomisedRotation, maxRandomisedRotation) + randomCardinalDirection, rotationAxis);
        }

        /// <summary>Returns the absolute version of the vector.</summary>
        /// <returns>The <see cref="UnityEngine.Vector4"/>.</returns>
        /// <param name="vectorToConvert">The Vector to convert into its absolute version.</param>
        public static Vector4 VectorAbs(Vector4 vectorToConvert) {
            return new Vector4(Mathf.Abs(vectorToConvert.w), Mathf.Abs(vectorToConvert.x), Mathf.Abs(vectorToConvert.y), Mathf.Abs(vectorToConvert.z));
        }
        /// <summary>Returns the absolute version of the vector.</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="vectorToConvert">The Vector to convert into its absolute version.</param>
        public static Vector3 VectorAbs(Vector3 vectorToConvert) {
            return new Vector3(Mathf.Abs(vectorToConvert.x), Mathf.Abs(vectorToConvert.y), Mathf.Abs(vectorToConvert.z));
        }
        /// <summary>Returns the absolute version of the vector.</summary>
        /// <returns>The <see cref="UnityEngine.Vector2"/>.</returns>
        /// <param name="vectorToConvert">The Vector to convert into its absolute version.</param>
        public static Vector2 VectorAbs(Vector2 vectorToConvert) {
            return new Vector2(Mathf.Abs(vectorToConvert.x), Mathf.Abs(vectorToConvert.y));
        }


        /// <summary>Divides the 2 given Vectors together axis wise. (w/w, x/x, y/y, z/z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector4"/> result.</returns>
        /// <param name="vectorToDivide">The vector to divide.</param>
        /// <param name="vectorToDivideBy">The vector to divide by.</param>
        public static Vector4 VectorDiv(Vector4 vectorToDivide, Vector4 vectorToDivideBy) {
            return new Vector4(vectorToDivide.w / vectorToDivideBy.w, vectorToDivide.x / vectorToDivideBy.x, vectorToDivide.y / vectorToDivideBy.y, vectorToDivide.z / vectorToDivideBy.z);
        }
        /// <summary>Divides the 2 given Vectors together axis wise. (x/x, y/y, z/z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/> result.</returns>
        /// <param name="vectorToDivide">The vector to divide.</param>
        /// <param name="vectorToDivideBy">The vector to divide by.</param>
        public static Vector3 VectorDiv(Vector3 vectorToDivide, Vector3 vectorToDivideBy) {
            return new Vector3(vectorToDivide.x / vectorToDivideBy.x, vectorToDivide.y / vectorToDivideBy.y, vectorToDivide.z / vectorToDivideBy.z);
        }
        /// <summary>Divides the 2 given Vectors together axis wise. (x/x, y/y)</summary>
        /// <returns>The <see cref="UnityEngine.Vector2"/> result.</returns>
        /// <param name="vectorToDivide">The vector to divide.</param>
        /// <param name="vectorToDivideBy">The vector to divide by.</param>
        public static Vector2 VectorDiv(Vector2 vectorToDivide, Vector2 vectorToDivideBy) {
            return new Vector2(vectorToDivide.x / vectorToDivideBy.x, vectorToDivide.y / vectorToDivideBy.y);
        }


        /// <summary>Multiplies each axis of the vector with the multiplier.</summary>
        /// <returns>The <see cref="UnityEngine.Vector4"/>.</returns>
        /// <param name="vectorToMultiply">The vector to multiply.</param>
        /// <param name="multiplier">The multiplier.</param>
        public static Vector4 VectorMultByFloat(Vector4 vectorToMultiply, float multiplier) {
            return new Vector4(vectorToMultiply.w * multiplier, vectorToMultiply.x * multiplier, vectorToMultiply.y * multiplier, vectorToMultiply.z * multiplier);
        }
        /// <summary>Multiplies each axis of the vector with the multiplier.</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="vectorToMultiply">The vector to multiply.</param>
        /// <param name="multiplier">The multiplier.</param>
        public static Vector3 VectorMultByFloat(Vector3 vectorToMultiply, float multiplier) {
            return new Vector3(vectorToMultiply.x * multiplier, vectorToMultiply.y * multiplier, vectorToMultiply.z * multiplier);
        }
        /// <summary>Multiplies each axis of the vector with the multiplier.</summary>
        /// <returns>The <see cref="UnityEngine.Vector2"/>.</returns>
        /// <param name="vectorToMultiply">The vector to multiply.</param>
        /// <param name="multiplier">The multiplier.</param>
        public static Vector2 VectorMultByFloat(Vector2 vectorToMultiply, float multiplier) {
            return new Vector2(vectorToMultiply.x * multiplier, vectorToMultiply.y * multiplier);
        }


        /// <summary>Multiplies the 2 given Vectors together axis wise. (w*w, x*x, y*y, z*z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector4"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector4 VectorMult(Vector4 vectorA, Vector4 vectorB) {
            return new Vector4(vectorA.w * vectorB.w, vectorA.x * vectorB.x, vectorA.y * vectorB.y, vectorA.z * vectorB.z);
        }
        /// <summary>Multiplies the 2 given Vectors together axis wise. (x*x, y*y, z*z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector3 VectorMult(Vector3 vectorA, Vector3 vectorB) {
            return new Vector3(vectorA.x * vectorB.x, vectorA.y * vectorB.y, vectorA.z * vectorB.z);
        }
        /// <summary>Multiplies the 2 given Vectors together axis wise. (x*x, y*y)</summary>
        /// <returns>The <see cref="UnityEngine.Vector2"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector2 VectorMult(Vector2 vectorA, Vector2 vectorB) {
            return new Vector2(vectorA.x * vectorB.x, vectorA.y * vectorB.y);
        }


        /// <summary>Adds the 2 given Vectors together axis wise. (w+w, x+x, y+y, z+z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector4"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector4 VectorAdd(Vector4 vectorA, Vector4 vectorB) {
            return new Vector4(vectorA.w + vectorB.w, vectorA.x + vectorB.x, vectorA.y + vectorB.y, vectorA.z + vectorB.z);
        }
        /// <summary>Adds the 2 given Vectors together axis wise. (x+x, y+y, z+z)</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector3 VectorAdd(Vector3 vectorA, Vector3 vectorB) {
            return new Vector3(vectorA.x + vectorB.x, vectorA.y + vectorB.y, vectorA.z + vectorB.z);
        }
        /// <summary>Adds the 2 given Vectors together axis wise. (x+x, y+y)</summary>
        /// <returns>The <see cref="UnityEngine.Vector2"/> result.</returns>
        /// <param name="vectorA">VectorA.</param>
        /// <param name="vectorB">VectorB.</param>
        public static Vector2 VectorAdd(Vector2 vectorA, Vector2 vectorB) {
            return new Vector2(vectorA.x + vectorB.x, vectorA.y + vectorB.y);
        }


        /// <summary>Destroyes a given object in any scenario.</summary>
        /// <param name="objectToDestroy">The object that needs to be destoyed.</param>
        public static void DestroySafely(Object objectToDestroy) {
            if(!objectToDestroy) {
                return;
            }
            if(Application.isEditor) {
                DestroyImmediate(objectToDestroy);
            } else if(Application.isPlaying) {
                Destroy(objectToDestroy);
            } else {
                Debug.LogWarning("Cannot destroy: " + objectToDestroy.name + " since application is in unknown state");
            }
        }

        /// <summary>Change a specific keys value in a given curve.</summary>
        /// <returns>The key with its new values.</returns>
        /// <param name="curve">The curve of the key to be changed.</param>
        /// <param name="time">The time where the key is located.</param>
        /// <param name="value">The value to be given to the new key.</param>
        public static int ChangeKey(AnimationCurve curve, float time, float value) {
            for (int i = 0; i < curve.keys.Length; i++) {
                if (curve.keys[i].time == time) {
                    curve.RemoveKey(i);
                    break;
                }
            }
            return curve.AddKey(time, value);
        }

        /// <summary>Checks if all keys in a curve are below the value of 0.1.</summary>
        /// <returns><see cref="true"/> if any of the keys are above 0.1.</returns>
        /// <param name="curve">The curve that will be checked.</param>
        public static bool CheckGraph(AnimationCurve curve) {
            for (int i = 0; i < curve.keys.Length; i++) {
                if (curve.keys[i].value >= .1f) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns a random vector3 between min and max. (Inclusive)</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// https://gist.github.com/Ashwinning/269f79bef5b1d6ee1f83
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max) {
            return min + Random.Range (0f, 1f) * (max - min);
        }

        /// <summary>Returns the vector3 where percentage determines the distance from the start to the end.</summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="start">Start.</param>
        /// <param name="end">End.</param>
        /// <param name="percentage">Percentage from min (0-1).</param>
        public static Vector3 GetVector3Between(Vector3 start, Vector3 end, float percentage) {
            return start + percentage * (end - start);
        }

        /// <summary>Returns all children inside the hierarchy that contain any of the given tags.</summary>
        /// <returns>The List<Transform> of children that contain any of the tags.</returns>
        /// <param name="parent">The transform to search through.</param>
        /// <param name="tags">The various tags to search.</param>
        /// https://discussions.unity.com/t/how-to-gameobject-findgameobjectswithtag-within-children-of-a-specific-gameobject/166649
        public static List<Transform> FindObjectsWithTag(Transform parent, string[] tags) {
            List<Transform> taggedGameObjects = new();

            for (int i = 0; i < parent.childCount; i++) {
                Transform child = parent.GetChild(i);
                foreach(string tag in tags) {
                    if(child.tag == tag) {
                        taggedGameObjects.Add(child);
                    }
                }
                if (child.childCount > 0) {
                    taggedGameObjects.AddRange(FindObjectsWithTag(child, tags));
                }
            }
            return taggedGameObjects;
        }

        /// <summary>Destroys all children of a parent.</summary>
        /// <param name="parent">parent to destroy all children from.</param>
        public static void DestroyAllChildren(Transform parent) {
            int amountOfChildrenToDestroy = parent.childCount;
            for(int child = amountOfChildrenToDestroy-1; child >= 0; child--) {
                GameObject childToDestroy = parent.GetChild(child).gameObject;
                DestroySafely(childToDestroy);
            }
        }

        // Addapted from code by: Bunny83
        // https://discussions.unity.com/t/physics-overlapsphere-with-a-min-radius/15689
        /// <summary>Computes and stores colliders that are between a minimum and a maximum sphere.</summary>
        /// <returns>The <see cref="UnityEngine.Collider"/>[]</returns>
        /// <param name="center">Center of hollow sphere.</param>
        /// <param name="innerRadius">Radius to start of sphere.</param>
        /// <param name="outerRadius">Radius to end of sphere.</param>
        /// <param name="layerMask">A defines which layers of colliders to include in the query.</param>
        public static Collider[] OverlapHollowSphere(Vector3 center, float innerRadius, float outerRadius, int layerMask) {
            List<Collider> outerSphere = new(Physics.OverlapSphere(center, outerRadius, layerMask));
            Collider[] innerSphere = Physics.OverlapSphere(center, innerRadius, layerMask);
            foreach (Collider collider in innerSphere) {
                outerSphere.Remove(collider);
            }
            return outerSphere.ToArray();
        }

        // /// <summary>Removes all duplicates from an array</summary>
        // /// <returns>The <see cref="UnityEngine.Object"/>[]</returns>
        // /// <param name="array">The array to be checked for duplicates.</param>
        // public static Object[] RemoveDuplicates(Object[] array) {
        //     List<Object> arrayList = new List<Object>(new HashSet<Object>(array));
        //     return arrayList.ToArray();
        // }

        /// <summary>Removes all duplicates from an array</summary>
        /// <returns>The <see cref="UnityEngine.Object"/>[]</returns>
        /// <param name="array">The array to be checked for duplicates.</param>
        public static T[] RemoveDuplicates<T>(T[] array) {
            List<T> arrayList = new(new HashSet<T>(array));
            return arrayList.ToArray();
        }

        /// <summary>Sorts the elements in an array</summary>
        /// <returns>The <see cref="UnityEngine.Object"/>[]</returns>
        /// <param name="array">The array to be sorted.</param>
        public static Object[] Sort(Object[] array) {
            List<Object> arrayList = new();
            arrayList.AddRange(array);
            arrayList.Sort();
            array = arrayList.ToArray();
            return array;
        }

        /// <summary>Removes a specific element in an array (Use a List<> for extended use)</summary>
        /// <returns>The <see cref="UnityEngine.Object"/>[]</returns>
        /// <param name="array">The array where the element needs to be removed from.</param>
        /// <param name="index">The index of the element.</param>
        public static Object[] RemoveAt(Object[] array, int index) {
            List<Object> arrayList = new();
            arrayList.AddRange(array);
            arrayList.RemoveAt(index);
            array = arrayList.ToArray();
            return array;
        }
    }
}