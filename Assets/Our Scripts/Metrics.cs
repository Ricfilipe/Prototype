using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    float[] wave_times = new float[9];
    int KDRatio;
    private string currentMetric;
    private bool win;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/currentMetrics.txt";
        StreamReader reader = new StreamReader(path);
         currentMetric = reader.ReadToEnd();
        int number = int.Parse(currentMetric); 
        reader.Close();
        number++;
        System.IO.File.WriteAllText(path, ""+(number));

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


    public void setWin()
    {
        win = true;
    }

    public void toFile()
    {

        using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"metrics" + currentMetric + ".txt"))
        {
            file.WriteLine("Metrics for Game " + currentMetric);
            int wave = 1;
            foreach (float n in wave_times)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                file.WriteLine("Wave " + wave + " Duration: " + n);
                wave++;
            }

            if (win)
            {
                file.WriteLine("Won");
            }
            else
            {
                file.WriteLine("Lost");
            }
        }
    }
}
