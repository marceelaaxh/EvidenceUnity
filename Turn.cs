using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Turn : MonoBehaviour
{
    [SerializeField] 
    GameObject carPrefab;
    [SerializeField] 
    List <Vector3> path;
    GameObject car;
    List<Vector3> originals;
    int pathIndex;
    float rotCounter;
    Matrix4x4 m, mem, rot, tra, sca, rotStat, Tc, Tpivot, Tpivneg;
    Vector3 currPos, currPath, nextPath, afterNextPath, pivot;
    bool turning;
    
    void Start(){
        turning = false;
        pathIndex = 0;
        rotCounter = 0;
        car = Instantiate(carPrefab, Vector3.zero, Quaternion.identity);
        originals = new List<Vector3>(car.GetComponent<MeshFilter>().mesh.vertices);
        tra = VecOps.TranslateM(new Vector3(0, 0, -0.01f));
        rot = mem = m = Matrix4x4.identity;
        sca = VecOps.ScaleM(new Vector3(0.05f, 0.5f, 0.5f));
        rotStat = VecOps.RotateYM(90);
    }

    void Update()
    {
        if (!turning)
        {
            nextPath = path[pathIndex + 1];
            currPos = new Vector3(mem[0, 3], mem[1, 3], mem[2, 3]);
            float distance = VecOps.Magnitude(nextPath - currPos);
            if (distance < 0.001f)
            {
                currPath = path[pathIndex];
                afterNextPath = path[pathIndex + 2];
                if (currPath.x != afterNextPath.x && currPath.z != afterNextPath.z) // Turn!
                {
                    pivot = new Vector3(afterNextPath.x, 0, currPath.z);
                    Debug.Log("PIVOT: " + pivot.ToString());
                    Tc = VecOps.TranslateM(nextPath);
                    Tpivot = VecOps.TranslateM(pivot);
                    Tpivneg = VecOps.TranslateM(-pivot);
                    turning = true;
                }
                pathIndex++;
            }

            m = mem * tra * rot * rotStat * sca;
            mem = mem * tra * rot;
            car.GetComponent<MeshFilter>().mesh.vertices = VecOps.ApplyTransform(originals, m).ToArray();
        }
        else
        {
            if (rotCounter < 90) // keep turning
            {
                Matrix4x4 rotY = VecOps.RotateYM(-rotCounter);
                // Tc * Tpivneg * rotY * Tpivot
                rot = Tpivot * rotY * Tpivneg * Tc;
                rot = rot * rotY * rotStat * sca;
                car.GetComponent<MeshFilter>().mesh.vertices = VecOps.ApplyTransform(originals, rot).ToArray();
                rotCounter += 0.1f;
            }
            else // done turning
            {
                rot = Matrix4x4.identity;
                rotCounter = 0;
                turning = false;
                pathIndex++;
                // ToDo: update mem!!!
            }
        }
    }
}
