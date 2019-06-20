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


        int n_waves = 9;


        wave_times = new float[n_waves];
        apm_per_wave = new int[n_waves];
         king_ability_per_wave = new int[n_waves];
         archer_ability_per_wave = new int[n_waves];
        archer_kills_per_wave = new int[n_waves];
        king_kills_per_wave = new int[n_waves];
         knight_kills_per_wave = new int[n_waves];
         knight_killed_per_wave = new int[n_waves];
         archer_killed_per_wave = new int[n_waves];


        lastActions = -1;

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
            apm_per_wave[wave_nr-1] = Actions-lastActions;
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
        knight_killed_per_wave[currentWave]++;
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
