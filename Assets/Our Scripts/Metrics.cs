using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    float[] wave_times = new float[5];
    int KDRatio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addWaveTimer(float time, int wave_nr)
    {
        Debug.Log(wave_nr);
        wave_times[wave_nr-1] = time;
    }

    public void toFile()
    {   
        float r= Random.Range(0, 100);
        using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"metrics"+r+".txt"))
        {
            foreach (float n in wave_times)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                file.WriteLine(n);
                
            }
        }
    }
}
