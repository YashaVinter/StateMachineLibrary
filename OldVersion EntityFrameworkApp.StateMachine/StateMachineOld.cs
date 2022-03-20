using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityFrameworkApp.StateMachine.StateMachineOld
{
    public class StateMachineOld
    {
        public string currentState { get; set; } = null;
        public SMData smData { get; set; } = new SMData();
        public StateMachineOld() { 
            //treeNode.
        }

        public void ExecuteCurrentCommand() {
            smData.ExecuteStateCommand(currentState);
        }
        public void AddTransitionCriteria(string transitionName, Func<string, bool> FCriteria)
        {

            //stateTransitionDictionary[transitionName].FCriteria = FCriteria;
        }

        public void test() {
            // start
            StateMachineOld stateMachine = new StateMachineOld();

            stateMachine.smData.SetTransition("one", new List<string>() {
                "onetwo",
                "onethree",
            });
            stateMachine.smData.SetTransition("two", new List<string>() {
                "twofour"
            });
            stateMachine.smData.SetTransition("three", new List<string>() {
                "threefour"
            });

            stateMachine.smData.SetStateForStateTransition("one", "two");
            stateMachine.smData.SetStateForStateTransition("one", "three");
            stateMachine.smData.SetStateForStateTransition("two", "four");
            stateMachine.smData.SetStateForStateTransition("three", "four");
            // end creating
            //test
            Func<string, bool> f = str => { return str == ""; };
            StateTransition stateTransition = new StateTransition("1", "2");
            stateTransition.FCriteria = f;
            stateTransition.FCriteria = TransitionCriteria.onetwoCriteria;
            

            State state = new State("state");
            state.Notify += SMCommands.OneCommand;

            //state.DoCommands();
            string command = "two";

            stateMachine.smData.AddTransitionCriteria("onetwo", TransitionCriteria.onetwoCriteria);
            stateMachine.smData.AddTransitionCriteria("onethree", TransitionCriteria.onethreeCriteria);
            bool b;
            b = stateMachine.smData.CheckTransition("one", "two");
            b = stateMachine.smData.CheckTransition("one", "three");
            b = stateMachine.smData.CheckTransition("one", "four");

            b = stateMachine.smData.CheckTransition(SMStates.one, SMStates.two);

            stateMachine.smData.AddAction("one", SMCommands.OneCommand, "write one!");
            stateMachine.currentState = "one";
            stateMachine.ExecuteCurrentCommand();
            stateMachine.smData.ExecuteStateCommand("one");

            //stateMachine.SMData.SetTransition("one", new List<StateTransition>());
            //stateMachine.currentState = one;


        }

        public class State
        {
            public readonly string name;
            public string command { get; set; }
            //List<StateTransition> transitionTo { get; set; } = null!;
            public List<string> transitionNames { get; set; } = null!;//from this State
            public State(string name)
            {
                this.name = name;
            }

            public event Action<string>? Notify;
            public void DoCommands()
            {
                Notify?.Invoke(command);
            }
        }
        public class StateTransition
        {
            public readonly string name;
            public string initState { get; set; } = null!;
            public string finalState { get; set; } = null!;
            public StateTransition(string initState, string finalState)
            {
                this.name = initState + finalState;
                this.initState = initState;
                this.finalState = finalState;
            }

            public Func<string, bool>? FCriteria { get; set; } = null!;

        }
        public enum CurrentStateInfo
        {
            init,
            first
        }
        public class SMData
        {// TO DO make Dictionary<string, State> StateCreator (List<string> states, List<string> StateTransition) and as well for StateTransitionCreator
         //private List<State> states = new List<State> {
         //    new State("one"),
         //    new State("two"),
         //    new State("three"),
         //    new State("four"),
         //};
         //private List<StateTransition> stateTransitions = new List<StateTransition> {
         //    new StateTransition("one","two"),
         //    new StateTransition("one","three"),
         //    new StateTransition("two","four"),
         //    new StateTransition("three","four")
         //};

            private Dictionary<string, State> stateDictionary = new Dictionary<string, State>()
        {
            {"one",new State("one") },
            {"two",new State("two") },
            {"three",new State("three") },
            {"four",new State("four") },
        };
            private Dictionary<string, StateTransition> stateTransitionDictionary = new Dictionary<string, StateTransition>()
        {
            {"onetwo", new StateTransition("one","two") },
            {"onethree", new StateTransition("one","three") },
            {"twofour", new StateTransition("two","four") },
            {"threefour", new StateTransition("three","four") }
        };

            public SMData()
            {
                //states.First(st => st.name == "");
            }
            public void AddAction(string stateName, Action<string> action, string command)
            {
                stateDictionary[stateName].Notify += action;
                stateDictionary[stateName].command = command;
            }


            public void RemoveAction(string stateName, Action<string> action) =>
                stateDictionary[stateName].Notify += action;
            public void ExecuteStateCommand(string stateName)
            {
                stateDictionary[stateName].DoCommands();
            }
            public void SetTransition(string stateName, List<string> transitions)
            {//rename
                stateDictionary[stateName].transitionNames = transitions;
            }
            public void SetStateForStateTransition(string initState, string finalState)
            { //rename
                string stateTransitionName = initState + finalState;
                stateTransitionDictionary[stateTransitionName].initState = initState;
                stateTransitionDictionary[stateTransitionName].finalState = finalState;
            }

            public bool CheckTransition(string stateName, string command)
            {
                bool isTransit = false;
                var transitions = stateDictionary[stateName].transitionNames;
                foreach (var transition in transitions)
                {
                    isTransit = stateTransitionDictionary[transition].FCriteria(command);
                    if (isTransit == true) break;
                }
                return isTransit;
            }
            public void AddTransitionCriteria(string transitionName, Func<string, bool> FCriteria)
            {
                stateTransitionDictionary[transitionName].FCriteria = FCriteria;
            }
            public void test()
            {

            }
        }
        public static class SMCommands
        {
            public static void OneCommand(string st)
            {
                Console.WriteLine(st);
            }
            public static void TwoCommand(string st)
            {
                Console.WriteLine(st);
            }
        }
        public static class TransitionCriteria
        {
            public static bool onetwoCriteria(string str) { return str == "two"; }
            public static bool onethreeCriteria(string str) { return str == "three"; }
            public static bool twofourCriteria(string str) { return str == SMStates.four; }

        }
        public enum EState
        {
            one,
            two,
            three,
            four
        }
        public enum EStateTransition
        {
            onetwo,
            onethree,
            twofour,
            threefour
        }
    }

}
