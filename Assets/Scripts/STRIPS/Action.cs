using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action:MonoBehaviour{

    public Conditions precondition;
    public Conditions postcondition;

    void Start()
    {
        precondition = new Conditions(false, false, false, false, false, false, false);
        //postcondition = new Conditions(false, false, false, false, false, false, false);
    }
    
    public Action(Conditions _precondition)
    {
        precondition = _precondition;
        postcondition = new Conditions(false, false, false, false, false, false, false);
        //Go to A
        if (!_precondition.atA && !_precondition.possesA && !_precondition.atB && !_precondition.atC && !_precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            GoToA();
            postcondition.atA = true;
            postcondition.atB = false;
            postcondition.atC = false;
            postcondition.atGoal = false;
            postcondition.possesA = false;
            postcondition.possesB = false;
            postcondition.possesC = false;
        }

        //Go to B
        if(_precondition.atA && _precondition.possesA && !_precondition.atB && !_precondition.atC && !_precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            GoToB();
            postcondition.atA = false;
            postcondition.atB = true;
            postcondition.atC = false;
            postcondition.atGoal = false;
            postcondition.possesA = true;
            postcondition.possesB = false;
            postcondition.possesC = false;
        }

        //Go to C
        if (!_precondition.atA && _precondition.possesA && _precondition.atB && !_precondition.atC && _precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            GoToC();
            postcondition.atA = false;
            postcondition.atB = false;
            postcondition.atC = true;
            postcondition.atGoal = false;
            postcondition.possesA = true;
            postcondition.possesB = true;
            postcondition.possesC = false;

        }

        //hint the player to pickup A
        if (_precondition.atA && !_precondition.possesA && !_precondition.atB && !_precondition.atC && !_precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            HintPlayer('A');
            postcondition.atA = true;
            postcondition.atB = false;
            postcondition.atC = false;
            postcondition.atGoal = false;
            postcondition.possesA = true;
            postcondition.possesB = false;
            postcondition.possesC = false;

        }

        //hint the player to pickup B
        if (!_precondition.atA && _precondition.possesA && _precondition.atB && !_precondition.atC && !_precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            HintPlayer('B');
            postcondition.atA = false;
            postcondition.atB = true;
            postcondition.atC = false;
            postcondition.atGoal = false;
            postcondition.possesA = true;
            postcondition.possesB = true;
            postcondition.possesC = false;

        }

        //hint the player to pickup C
        if (!_precondition.atA && _precondition.possesA && !_precondition.atB && _precondition.atC && _precondition.possesB && !_precondition.possesC && !_precondition.atGoal)
        {
            HintPlayer('C');
            postcondition.atA = false;
            postcondition.atB = false;
            postcondition.atC = true;
            postcondition.atGoal = false;
            postcondition.possesA = true;
            postcondition.possesB = true;
            postcondition.possesC = true;

        }

        //go to goal
        if (!_precondition.atA && _precondition.possesA && !_precondition.atB && !_precondition.atC && _precondition.possesB && _precondition.possesC && !_precondition.atGoal)
        {
            GoToGoal();
            postcondition.atA = false;
            postcondition.atB = false;
            postcondition.atC = false;
            postcondition.atGoal = true;
            postcondition.possesA = true;
            postcondition.possesB = true;
            postcondition.possesC = true;

        }
    }

    void GoToA()
    {
        // seeker move to A
        print("go to a!");
    }

    void GoToB()
    {
        print("go to b!");
    }

    void GoToC()
    {
        print("got to c!");
    }

    void HintPlayer(char c)
    {
        print("please click chest"+c);
    }

    void GoToGoal()
    {
        print("got to goal");
    }
}
