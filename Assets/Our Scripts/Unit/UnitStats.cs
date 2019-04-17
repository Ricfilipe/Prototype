using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [HideInInspector]
    public float speed,range,size;
    [HideInInspector]
    public int MaxHP, HP,AD;
   	
   	private static int InfantryMaxHP, InfantryAD, ArcherMaxHP, ArcherAD, InfantrySpeed, ArcherSpeed;

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

    

    // Start is called before the first frame update
    void Start()
    {
    	switch(team)
    	{
    		case Team.England:
    			switch(troop)
    			{
    				case Troops.Infantry:
    					InfantrySpeed=4;
	    				this.range=1.5f;
	    				this.size=5;
	    				InfantryMaxHP=6;
	    				this.HP=6;
	    				InfantryAD=3;
	    				break;

	    			case Troops.Archer:
	    				ArcherSpeed=3;
	    				this.range=10;
	    				this.size=4;
	    				ArcherMaxHP=4;
	    				this.HP=4;
	    				ArcherAD=2;
	    				break;

	    			case Troops.King:
	    				this.speed=6;
	    				this.range=2;
	    				this.size=9;
	    				this.MaxHP=13;
	    				this.HP=13;
	    				this.AD=3;
	    				break;
    			}
    			break;	   		

    		case Team.France:
    			switch(troop)
    			{
    				case Troops.Archer:
	    				this.speed=3;
	    				this.range=9;
	    				this.size=4;
	    				this.MaxHP=3;
	    				this.HP=3;
	    				this.AD=2;
	    				break;
    				
    				case Troops.Infantry:
    					this.speed=4;
	    				this.range=1.5f;
	    				this.size=5;
	    				this.MaxHP=5;
	    				this.HP=5;
	    				this.AD=2;
	    				break;

	    			case Troops.Cavalry:
	    				this.speed=5;
	    				this.range=2;
	    				this.size=8;
	    				this.MaxHP=5;
	    				this.HP=5;
	    				this.AD=2;
	    				break;

	    			case Troops.CrossBowMan:
	    				this.speed=4;
	    				this.range=7;
	    				this.size=4;
	    				this.MaxHP=3;
	    				this.HP=3;
	    				this.AD=2;
	    				break;

	    			case Troops.King:
	    				this.speed=6;
	    				this.range=2;
	    				this.size=9;
	    				this.MaxHP=9;
	    				this.HP=9;
	    				this.AD=2;
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
					return ArcherAD;

				case Troops.Infantry:
					return InfantryAD;

				default:
					return AD;

			}

		}
		else{
			return AD;
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
