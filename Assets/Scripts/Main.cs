using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// -280, 114 Oben Links
// 224, 134 Oben Rechts
// 66, 151 Mitte Mitte
// 250, 11 Mitte Rechts
// 220, -190 Unten Rechts
// 12, -168 Unten Mitte
// -220, -172 Unten Links
// -172, -90 Mitte Links


// -80 <> +80
// 50 <> -50


public class Main : MonoBehaviour
{
    // Start is called before the first frame update

    public float correct = 0;
    public float wrong_airport_score = 0;
    public float crashes = 0;


    public GameObject airport;
    public GameObject plane;

    private int numberOfPlanes = 8;
    private int lastSpawn = 0;

    Vector3 north_airport_position = new Vector3(-24.2f, 36.8f, -5);
    Quaternion north_airport_rotation = Quaternion.Euler(0,0,-80);

    Vector3 south_airport_position = new Vector3(27.8f, -30, -5);
    Quaternion south_airport_rotation = Quaternion.Euler(0,0,-95);

    Vector3 mid_airport_position = new Vector3(-57.2f, -23.8f, -5);


    private List<Vector3> spawnPoints = new List<Vector3>();

    public bool difficult = false;
    public bool prev_difficult = true;
    public float playerspeed = 3f;
    private IEnumerator coroutine;
    public GameObject clouds;
    public GameObject blitzLight;
    public GameObject mainLightGO;
    Light mainLight;

    private AudioManager audioManager;
    public AudioClip relax;
    public AudioClip stress;

    public AudioSource explosionSound;

    public bool paused = false;

    public Text pauseText;
    public Text infoText;
    public Text score;
    public Text scoreInfo;

    private bool shouldSpawnPlane = true;

    private bool can_play = true;


    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        mainLight = mainLightGO.GetComponent<Light>();


        scoreInfo.text = "Punktzahl: ";

        score.text = "0";

        CreateSpawn(-90, 60);
        CreateSpawn(90, 60);
        CreateSpawn(66, 60);
        CreateSpawn(90, 11);
        CreateSpawn(90, -60);
        CreateSpawn(12, -60);
        CreateSpawn(-90, -60);
        CreateSpawn(-90, -90);

        if (difficult) {
            audioManager.ChangeMusic(stress);
            // audioManager.HighVolume();
        } else {
            Debug.Log("changed music to relaxed");
            audioManager.ChangeMusic(relax);
            // audioManager.LowVolume();
        }
        SpawnAirports();
        InvokeRepeating("GetDifficulty", 0f, 1f);
        InvokeRepeating("PostScore", 10f, 10f);
        StartCoroutine(WaitAndSpawn());



    }

    void CreateSpawn(float x,float y) {
        Vector3 spawnPoint = new Vector3(x,y,-5f);
        spawnPoints.Add(spawnPoint);

    }


    void PostScore() {
        StartCoroutine(Upload());
    }

    void GetDifficulty() {
        StartCoroutine(GetText());
    }

    static Difficulty CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Difficulty>(jsonString);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("correct", correct.ToString());
        form.AddField("wrong_airport_score", wrong_airport_score.ToString());
        form.AddField("crashes", crashes.ToString());



        UnityWebRequest www = UnityWebRequest.Post("http://h2942775.stratoserver.net:5000/post_score", form);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)  {
            Debug.Log(www.error);
        }

    }

    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://h2942775.stratoserver.net:5000/info");
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)  {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            var diffy = www.downloadHandler.text;
            Difficulty difficulty = CreateFromJSON(diffy);
            playerspeed = difficulty.plane_speed;
            numberOfPlanes = difficulty.number_of_planes;
            difficult = difficulty.darkness;
            paused = difficulty.paused;
            infoText.text = difficulty.info_text;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (paused) {
            pauseText.enabled = true;
            infoText.enabled = true;
            score.enabled = false;
            scoreInfo.enabled = false;
        } else {
            pauseText.enabled = false;
            infoText.enabled = false;
            score.enabled = true;
            scoreInfo.enabled = true;

        }

        if (prev_difficult != difficult) {
            score.text = "0";

            if (difficult) {
                audioManager.ChangeMusic(stress);
                // audioManager.HighVolume();
            } else {
                audioManager.ChangeMusic(relax);
                // audioManager.LowVolume();

            }


            GameObject[] airplanes = GameObject.FindGameObjectsWithTag("plane");
            foreach (GameObject ap in airplanes) {
                Destroy(ap);
            }

            GameObject[] airports = GameObject.FindGameObjectsWithTag("airport");
            foreach (GameObject ap in airports) {
                Destroy(ap);
            }
            SpawnAirports();
            
            if (difficult) {
                mainLight.intensity = 0.2f;
                clouds.SetActive(true);
                blitzLight.GetComponent<FlashingBlitz>().active = true;
                
            } else {
                mainLight.intensity = 0.8f;
                clouds.SetActive(false);
                blitzLight.GetComponent<FlashingBlitz>().active = false;
            }
            prev_difficult = difficult;
        }

    }

      
    private IEnumerator WaitAndSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!paused){
            GameObject[] airplanes = GameObject.FindGameObjectsWithTag ("plane");
            int airplaneCount = airplanes.Length;

            

            if(numberOfPlanes > airplaneCount) {
                if (shouldSpawnPlane) {
                    shouldSpawnPlane = false;
                    SpawnPlane();
                    yield return new WaitForSeconds(3);
                    shouldSpawnPlane = true;

                }
            }}
        }
    }

    void SpawnPlane() {

        int spawnPoint = Random.Range(0,spawnPoints.Count);

        while (spawnPoint == lastSpawn) {
            spawnPoint = Random.Range(0,spawnPoints.Count);
        }

        GameObject p1 = Instantiate(plane, spawnPoints[spawnPoint], plane.transform.rotation);
        Color planeColor = Color.red;
        int color = Random.Range(1,3);
        switch (color) {
            case 1: planeColor = Color.red;
                    break;
            case 2: planeColor = Color.green;
                    break;
            default: 
                    break;
        }

        if (difficult) {
            color = Random.Range(1,4);
            switch (color) {
            case 1: planeColor = Color.red;
                    break;
            case 2: planeColor = Color.green;
                    break;
            case 3: planeColor = Color.yellow;
                    break;
            default: 
                    break;
        }
        }

        p1.GetComponent<Plane>().SetColor(planeColor);
        p1.GetComponent<PlaneMovement>().playerSpeed = playerspeed + Random.Range(-1,2);

        lastSpawn = spawnPoint;
    }

    void SpawnAirports() {
        GameObject air1 = Instantiate(airport, north_airport_position, north_airport_rotation);
        AirPort airscript = air1.GetComponent<AirPort>();
        airscript.SetColor(Color.green);
        GameObject air2 = Instantiate(airport, south_airport_position, south_airport_rotation);
        air2.GetComponent<AirPort>().SetColor(Color.red);

        if (difficult) {
            GameObject air3 = Instantiate(airport, mid_airport_position, north_airport_rotation);
            air3.GetComponent<AirPort>().SetColor(Color.yellow);

        }
    }


    public void WrongAirport() {
        wrong_airport_score++;
        float current_score = float.Parse(score.text);
        current_score -= 0.5f;
        score.text = current_score.ToString();
    }
     
     public void IncreaseScore() {
        correct++;
        float current_score = float.Parse(score.text);
        current_score += 1f;
        score.text = current_score.ToString();
    }
    public void Crash() {
        crashes++;
        float current_score = float.Parse(score.text);
        if (can_play) {
            explosionSound.time = 0.5f;
            explosionSound.Play(0);
        }
        can_play = !can_play;
        
        current_score -= 5f;
        score.text = current_score.ToString();
    }
}
