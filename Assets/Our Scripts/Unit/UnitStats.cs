using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [HideInInspector]
    public float speed,range,size;
    [HideInInspector]
    public int MaxHP, HP,AD;
    [HideInInspector]
    public static int InfantryMaxHP = 15, InfantryAD=2, ArcherMaxHP=4, ArcherAD=1, InfantrySpeed= 6, ArcherSpeed=5;
    [HideInInspector]
    public static int[] knightLevel = { 0, 0, 0 };
    [HideInInspector]
    public static int[] archerLevel = { 0, 0, 0 };
    [HideInInspector]
    public float attackSpeed, adMultiplier=1f;
    [HideInInspector]
    public bool undead = false;

    public enum Team
    {
    	England,
    	France,
    };

    public enum Troops
    {
    	Archer,
    	Infantry,
    	Cavalry,
    	CrossBowMan,
    	King,
    };

 	public Team team;
    public Troops troop;
    [HideInInspector]
    public bool dead;



    // Start is called before the first frame update
    void Start()
    {
    	switch(team)
    	{
    		case Team.England:
    			switch(troop)
    			{
    				case Troops.Infantry:

	    				this.range=3f;
	    				this.size=0;
	    				this.HP= InfantryMaxHP;
                        this.attackSpeed = 1.5f;
	    				break;

	    			case Troops.Archer:
	    				this.range=30;
	    				this.size=0;	    				
	    				this.HP=ArcherMaxHP;
                        this.attackSpeed = 1.5f;
                        break;

	    			case Troops.King:
	    				this.speed=6;
	    				this.range=5f;
	    				this.size=1;
	    				this.MaxHP=100;
	    				this.HP= 100;
	    				this.AD=10;
                        this.attackSpeed = 1.5f;
                        break;
    			}
    			break;	   		

    		case Team.France:
    			switch(troop)
    			{
    				case Troops.Archer:
	    				this.speed=5;
	    				this.range=30;
	    				this.size=4;
	    				this.MaxHP=12;
	    				this.HP=12;
	    				this.AD=2;
                        this.attackSpeed = 1.5f;
                        break;
    				
    				case Troops.Infantry:
    					this.speed= 6;
	    				this.range=3f;
	    				this.size=5;
	    				this.MaxHP=18;
	    				this.HP=18;
	    				this.AD=3;
                        this.attackSpeed = 1.5f;
                        break;

	    			case Troops.Cavalry:
	    				this.speed= 10;
	    				this.range=3f;
	    				this.size=1;
	    				this.MaxHP=18;
	    				this.HP=18;
	    				this.AD=4;
                        this.attackSpeed = 1.5f;
                        break;

	    			case Troops.CrossBowMan:
	    				this.speed=6;
	    				this.range=20;
	    				this.size=4;
	    				this.MaxHP=12;
	    				this.HP=12;
	    				this.AD=2;
                        this.attackSpeed = 1.5f;
                        break;

	    			case Troops.King:
	    				this.speed=6;
	    				this.range=2.5f;
	    				this.size=9;
	    				this.MaxHP=9;
	    				this.HP=9;
	    				this.AD=2;
                        this.attackSpeed = 1.5f;
                        break;
    			}
    			break;

    	}

    }


	public int getMaxHP(){
		if (team==Team.England){
			switch(troop){
				case Troops.Archer:
					return ArcherMaxHP;

				case Troops.Infantry:
					return InfantryMaxHP;
					
				default:
					return MaxHP;
			}

		}
		else{
			return MaxHP;
		}
	}

	public int getAD(){
		if (team==Team.England){
			switch(troop){
				case Troops.Archer:
					return Convert.ToInt32(ArcherAD * adMultiplier);

				case Troops.Infantry:
					return Convert.ToInt32(InfantryAD*adMultiplier);

				default:
					return Convert.ToInt32(AD * adMultiplier);

			}

		}
		else{
			return Convert.ToInt32(AD * adMultiplier); 
		}
	}

	public float getSpeed(){
		if (team==Team.England){
			switch(troop){
				case Troops.Archer:
					return ArcherSpeed;

				case Troops.Infantry:
					return InfantrySpeed;

				default:
					return speed;

			}

		}
		else{
			return speed;
		}
	}
	


}
