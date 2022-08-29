using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingBlitz : MonoBehaviour
{
    Light lightning;
    List<GameObject> lightnings = new List<GameObject>();
    public bool active = true;
    private bool notactive = false;

    public GameObject lightningBolt;
    // Start is called before the first frame update
    void Start()
    {
        lightning = gameObject.GetComponent<Light>();
        StartCoroutine(Lightning());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator Lightning()
    {
        while(true) {
            yield return new WaitForSeconds(Random.Range(2f,5f));
            float randX = Random.Range(-100,100);
            float randY = Random.Range(-60,60);
            Vector3 pos = new Vector3(randX,randY, -11f);
            for (int y = 0; y< Random.Range(1,7); y++) {
                if (active) {
                    Debug.Log(y);
                    lightning.intensity = Random.Range(3f,8f);
                }
                yield return new WaitForSeconds(0.2f);
                lightning.intensity = 0;
            }
            
        }
    }

}
