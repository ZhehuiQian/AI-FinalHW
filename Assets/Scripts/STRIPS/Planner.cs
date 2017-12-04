using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour {

    Conditions initalState;
    Conditions goalState;
    Conditions currentState;
    GameObject player;
    Action action;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        initalState = new Conditions(false, false, false, false, false, false, false);
        goalState = new Conditions(false, false, false, true, true, true, true);
        currentState = initalState;
        while (currentState.CompareConditions(goalState) != true)
        {
            action = new Action(currentState);
            currentState = action.postcondition;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
