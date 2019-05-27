using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Metrics : MonoBehaviour
{
 
    float[] wave_times ;
    int KDRatio;
    private string currentMetric;
    private bool win;
    int[] apm_per_wave;
    int[] king_ability_per_wave ;
    int[] archer_ability_per_wave;
    int[] archer_kills_per_wave ;
    int[] king_kills_per_wave ;
    int[] knight_kills_per_wave ;
    int[] knight_killed_per_wave;
    int[] archer_killed_per_wave; 
    int lastActions;

    int currentWave;

    private int Actions;
    // Start is called before the first frame update
    void Start()
    {

        string path2 = "Assets/config.txt";
        StreamReader reader1 = new StreamReader(path2);
        string strWaves = reader1.ReadToEnd();
        int n_waves = int.Parse(strWaves);

        reader1.Close();

        wave_times = new float[n_waves];
        apm_per_wave = new int[n_waves];
         king_ability_per_wave = new int[n_waves];
         archer_ability_per_wave = new int[n_waves];
        archer_kills_per_wave = new int[n_waves];
        king_kills_per_wave = new int[n_waves];
         knight_kills_per_wave = new int[n_waves];
         knight_killed_per_wave = new int[n_waves];
         archer_killed_per_wave = new int[n_waves];

        string path = "Assets/currentMetrics.txt";
        StreamReader reader = new StreamReader(path);
         currentMetric = reader.ReadToEnd();

        lastActions = -1;
        reader.Close();
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
                file.WriteLine("Wave " + wave + " Archers' Kills: " + archer_kills_per_wave[wave-1]);
                file.WriteLine("Wave " + wave + " Archers Killed: " + archer_killed_per_wave[wave-1]);
                file.WriteLine("Wave " + wave + " Archers Abilities: " + archer_ability_per_wave[wave - 1]);
                file.WriteLine("Wave " + wave + " Knights Kills: " + knight_kills_per_wave[wave-1]);
                file.WriteLine("Wave " + wave + " Knights Killed: " + knight_killed_per_wave[wave - 1]);
                file.WriteLine("Wave " + wave + " King Kills: " + king_kills_per_wave[wave - 1]);
                file.WriteLine("Wave " + wave + " King Abilities: " + king_ability_per_wave[wave - 1]);
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
        Debug.Log(currentWave);
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
        if (currentWave <= 0)
        {
            king_ability_per_wave[0]++;
        }
        else
        {
            king_ability_per_wave[currentWave]++;
        }
    }

    public void countAbilityArcher()
    {
        if (currentWave <= 0)
        {
            archer_ability_per_wave[0]++;
        }
        else
        {
            archer_ability_per_wave[currentWave]++;
        }
    }
}
