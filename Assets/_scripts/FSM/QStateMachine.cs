using System.Collections;
using System.Collections.Generic;


// Programmer: Quinton "Kiro" Baudoin
// Purpose: Modular FSM
// Use summary:
//     QState: Create and manage each State.
//          Each state is given a name of the state and a state it can fallow.
//    QStateMachine:
//          Premade State Machine.
//          You may make your own variant as your program needs.
//          Uses a list to store all the states.
//          Using Getter and Setters to change and run current state based on sname and sprev strings in the QStates


// Prefixes:
// Q     :     Quinton
// d    :     Delegate
// dv_   :     Void
// die_  :     IEnumerator
// s_    :     String
// s     :     State
// ls_   :     List of States
// 
// 
// Suffixes:

/// <summary>
/// State Object
/// </summary>
public class QStates
{
    public QStates(string name, string prev)
    {
        s_name = name;
        s_prev = prev;
    }
    /// <summary>
    /// This State Name
    /// </summary>
    public string s_name;
    /// <summary>
    /// State that this state can begin from
    /// </summary>
    public string s_prev;
}

/// <summary>
/// Premade State Manager
/// </summary>
public class QStateMachine
{
    /// <summary>
    /// List of all the QStates
    /// </summary>
    List<QStates> ls_States = new List<QStates>();

    /// <summary>
    /// Used to store values of the Getter/Setters
    /// </summary>
    private QStates cur;
    private QStates nex;
    //////////////////////////////////////

    /// <summary>
    /// State currently in.
    /// When Modified the state will play its delegates if delegates have functions stored
    /// NOTE: Void functions then IEnumerable functions
    /// </summary>
    QStates current
    {
        get
        { return cur; }  //Returns Qstate cur
        set
        {
            cur = value;
        }
    }

   public QStates currentState
    {
        get { return cur; } //Returns the value in cur
    }
    /// <summary>
    /// State intended to change to. If allowed the current state will be changed to the value the nextState was changed to.
    /// </summary>
     QStates nextState
    {
        get
        { return nex; }  //Returns the value in QState nex
        set
        {
            nex = value;           // sets nex to value
            if (current == null)  //checks to see if current is null
                current = value;   //Sets cukrrent to value
            else if (current.s_name == nex.s_prev || nextState.s_prev == null) // Checks to see if the s_prev matches the current's s_name
            {
                current = value;                                             //Sets current to value
            }

        }

    }

    /// <summary>
    /// Creates a QState based on the params passed in.
    /// </summary>
    /// <param name="name"> Name of QState</param>
    /// <param name="prevname">Name of QState to fallow</param>
    public void addState(string name, string prevname)
    {
        if (prevname != null)
        {
            ls_States.Add(new QStates(name.ToLower(), prevname.ToLower()));
        }
        else ls_States.Add(new QStates(name.ToLower(), null));
    }
    /// <summary>
    /// Removes a state from the list based on name variable
    /// </summary>
    /// <param name="name"> Name assigned to QState</param>
    public void removeState(string name)
    {
        foreach (QStates s in ls_States) // Loops threw ls_States
        {
            if (s.s_name.ToLower() == name.ToLower())      //checks the s_name to the string passed in
                ls_States.Remove(s);   // removes the QState
        }
    }
    /// <summary>
    /// Changes the nextState variable.
    /// To add further detail..
    /// If the state that is passed in to fallow the current state it is then added ass current state.
    /// </summary>
    /// <param name="name"></param>
    public bool changeState(string name)
    {
        if ( cur != null &&  cur.s_name.ToLower() == name.ToLower())
            return false;

        foreach (QStates s in ls_States) // Loops thru ls_States
            if (s.s_name.ToLower() == name.ToLower())        // checks s_name to the string passed in
            {
                nextState = s;
                if (currentState == s)//  sets nextState to the s QState
                    return true;        //  returns
                else
                    return false;
            }
        return false;
    }
    /// <summary>
    /// Clears the State list
    /// </summary>
    public void clearStateList()
    {
        ls_States.Clear();
    }
    /// <summary>
    /// Gtets the state using name variable.
    /// </summary>
    /// <param name="name">Would be the sname in QStates</param>
    /// <returns></returns>
    public QStates getState(string name)
    {
        foreach (QStates s in ls_States) //Loops threw ls_States
            if (s.s_name.ToLower() == name.ToLower())        //Checks the s_name to the string value passed in
                return s;                //Returns the QState

        return null;                    // returns null

    }
    //public addVoid()




}

