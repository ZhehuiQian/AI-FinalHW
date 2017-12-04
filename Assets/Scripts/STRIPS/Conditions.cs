using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions{

    public bool atA, atB, atC, atGoal;
    public bool possesA, possesB, possesC;

    public Conditions(bool _atA, bool _atB, bool _atC,bool _possesA, bool _possesB, bool _possesC, bool _atGoal)
    {
        atA = _atA;             // if npc is at A
        atB = _atB;             // if npc is at B
        atC = _atC;             // if npc is at C
        possesA = _possesA;     // if the player posseses A
        possesB = _possesB;     // if the player posseses B
        possesC = _possesC;     // if the player posseses C
        atGoal = _atGoal;       // if the player is at Goal
    }

    public bool CompareConditions(Conditions CondB)
    {
        if (atA == CondB.atA && atB == CondB.atB && atC == CondB.atC
            && atGoal == CondB.atGoal && possesA == CondB.possesA &&
            possesB == CondB.possesB && possesC == CondB.possesC)
        {
            return true;
        }

        return false;
    }

}
