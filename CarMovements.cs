using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovements : MonoBehaviour{

    float percentageCompleted; //turning percentage
    bool sucess;
    bool done;
    Matrix4x4 mov; 
    int step;

    //declare car prefabs that will be used 
    [SerializeField] GameObject car_Mariana, car_Marce, car_Santiago, car_Emiliano;

    //create cars 
    GameObject car1, car2, car3, car4, car5, car6, car7;

    //we need to manipulate vertices?

    //accumulate transformations
    Matrix4x4 acum1, acum2, acum3, acum4, acum5, acum6, acum7;

    
    List<GameObject> cars;
    List<Matrix4x4> acumulators;
    List<GameObject> prefabs;
    //these will come from the json, may vary 
    List<List<int>> stepX;
    List<List<int>> stepZ;

    //endpoint
    private const string flaskUrl = "put here url";
    private const float requestInterval = 0.2f;

    //rotating transformations
    //do we still have to get the pivots or how?
    //pivots 

    //right turn 

    Matrix4x4 rightTurnHalf1(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(0f, 0f, 0f);
        Matrix4x4 rotTemp = VecOps.RotateY(45 * percentageCompleted);
        Matrix4x4 pivot2 = VecOps.TranslateM(0f, 0f, 10f);
        return pivot1 * rotTemp * pivot2;
    }

    Matrix4x4 rightTurnHalf2(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(0f, 0f, 0f);
        Matrix4x4 rotTemp = VecOps.RotateY(45 * percentageCompleted);
        Matrix4x4 pivot2 = VecOps.TranslateM(0f, 0f, 10f);
 
        return pivot1 * rotTemp * pivot2;
    }

    Matrix4x4 changeLaneRight(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(0f, 0f, 0f);
        Matrix4x4 rotTemp = VecOps.RotateY(-45 * percentageCompleted);
        Matrix4x4 pivot2 = VecOps.TranslateM(0f, 0f, 0f);
 
        return pivot1 * rotTemp * pivot2;
    }

    Matrix4x4 leftTurnHalf1(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(0f, 0f, 10f);
        Matrix4x4 rotTemp = VecOps.RotateY(-45 * percentageCompleted);
        Matrix4x4 pivot2 = VecOps.TranslateM(0f, 0f, -10);
 
        return pivot1 * rotTemp * pivot2;
    }

    Matrix4x4 leftTurnHalf2(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(0f, 0f, 10f);
        Matrix4x4 rotTemp = VecOps.RotateY( - (45 * percentageCompleted));
        Matrix4x4 pivot2 = VecOps.TranslateM(0f, 0f, 0);
 
        return pivot1 * rotTemp * pivot2;
    }

    Matrix4x4 changeLaneLeft(float percentageCompleted)
    {
        Matrix4x4 pivot1 = VecOps.TranslateM(7.5f, 0f, -7.5f);
        Matrix4x4 rotTemp = VecOps.RotateY(45 * percentageCompleted);
        Matrix4x4 pivot2 = VecOps.TranslateM(-7.5f, 0f, 7.5f);
 
        return pivot1 * rotTemp * pivot2;
    }

    //do we still want larger turns? 

    //request to get data 

    IEnumerator requestData(){
        while (true){
            yield return StartCoroutine(GetData());
            yield return new WaitForSeconds(requestInterval);
        }

    }

    IEnumerator getData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(flaskUrl))
        {
            yield return www.SendWebRequest();
 
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                
                string jsonResult = www.downloadHandler.text;
                JsonData jsonData = JsonUtility.FromJson<JsonData>(jsonResult);
            }
        }

    //we will get the data of the car agents here right? 
    //we can initialize here the coordinates of x and y and then transform them to the scale we are using (24)
    //we can add them to lists and the do the multiplication to scale them 

    List<int> positionx =new List<int>(){
        //here we can have like a for or something to apply the scale to the coordinates 
    }

    List<int> positiony =new List<int>(){
        //here we can have like a for or something to apply the scale to the coordinates 
    }

 
    stepX.Add(x);
    stepZ.Add(z);

    }
 

    void Start(){

        //get the coordinates and request data 
        stepX = new List<List<int>>();
        stepZ = new List<List<int>>();

        //initialize the cars, accumulators and prefabs 

        //instantiate cars 
        for (int i = 0; i < cars.Count; i++){
            cars[i] = Instantiate(prefabs[i % 4]); // we have four cars
        }


    }


    void Update(){

        //im not sure what to put here, maybe the rotating steps?

    }
}
