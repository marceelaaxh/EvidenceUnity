using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] carPrefabs; // car prefabs
    private List<GameObject> spawnedCars = new List<GameObject>(); // list of cars
    private bool carsGenerated = false; 

    void Start()
    {
        StartCoroutine(RequestPositions());
    }

    IEnumerator RequestPositions()
    {
        string url = "http://127.0.0.1:8000/positionsCar";

        while (!carsGenerated) 
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error al acceder al endpoint: {www.error}");
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    Debug.Log($"Respuesta del JSON:\n{jsonResponse}");

                    Car[] carData = JsonHelper.FromJson<Car>(jsonResponse);

                    if (carData != null && carData.Length > 0)
                    {
                        SpawnCars(carData);
                        carsGenerated = true; 
                    }
                    else
                    {
                        Debug.LogError("Error al deserializar el JSON o lista vacía.");
                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator RequestStep()
    {
        string url = "http://127.0.0.1:8000/stepCall";

        while (!carsGenerated) 
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error al acceder al endpoint: {www.error}");
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    Debug.Log($"Respuesta del JSON:\n{jsonResponse}");

                    // Deserializar el JSON
                    Car[] carData = JsonHelper.FromJson<Car>(jsonResponse);

                    if (carData != null && carData.Length > 0)
                    {
                        SpawnCars(carData);
                        carsGenerated = true; // stop when all cars are generated
                    }
                    else
                    {
                        Debug.LogError("Error al deserializar el JSON o lista vacía.");
                    }
                }
            }

            yield return null; 
        }
    }

    void SpawnCars(Car[] cars)
    {
        foreach (GameObject car in spawnedCars)
        {
            Destroy(car);
        }
        spawnedCars.Clear();

        foreach (Car carData in cars)
        {
            GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            GameObject carInstance = Instantiate(carPrefab);

            Vector3 position = CalculatePosition(carData.x, carData.y);
            carInstance.transform.position = position;

            spawnedCars.Add(carInstance);

            carInstance.name = $"Car_{carData.x}_{carData.y}";
        }
    }

    private static float cellSize = 10f; 
    private static Vector3 originOffset = new Vector3(115f, 1f, -115f); 

    Vector3 CalculatePosition(int x, int y)
    {
        float unityX = originOffset.x - (y * cellSize);
        float unityZ = originOffset.z + (x * cellSize);

        Vector3 transformedPosition = new Vector3(unityX, originOffset.y, unityZ);

        Debug.Log($"Scaled positions: JSON: x = {x}, y = {y} | Unity: {transformedPosition}");

        return transformedPosition;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
