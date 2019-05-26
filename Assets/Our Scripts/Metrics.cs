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
    int[] apm_per_wave = new int[9];
    int[] king_ability_per_wave = new int[9];
    int[] archer_ability_per_wave = new int[9];
    int[] archer_kills_per_wave = new int[9];
    int[] king_kills_per_wave = new int[9];
    int[] knight_kills_per_wave = new int[9];
    int[] knight_killed_per_wave = new int[9];
    int[] archer_killed_per_wave = new int[9];
    int lastActions;

    int currentWave=-1;

    private int Actions;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/currentMetrics.txt";
        StreamReader reader = new StreamReader(path);
         currentMetric = reader.ReadToEnd();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addWaveTimer(float time, int wave_nr)
    {
        Debug.Log(wave_nr);
        wave_times[wave_nr-1] = time;
        if (wave_nr == 1)
        {
            apm_per_wave[0] = Actions;
            lastActions = Actions;
        }
        else
        {
            apm_per_wave[0] = Actions-lastActions;
            lastActions = Actions;
        }
        currentWave++;
    }


    public void setWin()
    {
        win = true;
    }

    public void toFile()
    {
        string path = "Assets/currentMetrics.txt";
        using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"metrics" + currentMetric + ".txt"))
        {
            file.WriteLine("Metrics for Game " + currentMetric);
            int wave = 1;
            foreach (float n in wave_times)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                file.WriteLine("Wave " + wave + " Duration: " + n);
                if ((n / 60) == 0)
                {
                    file.WriteLine("Wave " + wave + " APM: " + (apm_per_wave[wave - 1]));
                }
                else
                {
                    file.WriteLine("Wave " + wave + " APM: " + (apm_per_wave[wave - 1] / (n / 60)));
                }
                //TODO Contar K/D por classe
                //TODO Contar usos de habilidades
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
       
        int number = int.Parse(currentMetric);
        number++;
        System.IO.File.WriteAllText(path, "" + (number));
    }

    public void IncActions()
    {
        Actions++;
       
    }

    public void countKingKill()
    {
        king_kills_per_wave[currentWave]++;
    }

    public void countKnightKill()
    {
        knight_kills_per_wave[currentWave]++;
    }

    public void countArcherKill()
    {
        archer_kills_per_wave[currentWave]++;
    }

    public void countArcherDeath()
    {
        archer_killed_per_wave[currentWave]++;
    }

    public void countKnightDeath()
    {
        archer_killed_per_wave[currentWave]++;
    }

    public void countAbilityKing()
    {
        king_ability_per_wave[currentWave]++;
    }

    public void countAbilityArcher()
    {
        archer_ability_per_wave[currentWave]++;
    }
}
