﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
    public class State : IState
    {
        public string name { get; set; }
        public ISet<ITransition> transitions { get; set; } = null!;

        public event Action<string>? action = null;

        //public event EventHandler<EventArgs> eventHandler;
        public event FunctionHandler? functionHandler = null;
        //public object DoFunctionHandler(object sender, EventArgs e) { // dont using
        //    return functionHandler?.Invoke(sender, e);
        //}

        public State(string name)
        {
            this.name = name;
        }
        public void DoCommand(string cmd)
        {
            if (action is null)
            {
                throw new Exception("Action not added");
            }
            action?.Invoke(cmd);
        }
        public object DoCommand(object sender, CommandBase e)
        {
            if (functionHandler is null)
            {
                throw new Exception("FunctionHandler not added");
            }
            return functionHandler?.Invoke(sender, e);
        }
    }

    public class Transition : ITransition
    {
        public string name { get; set ; }
        public IState entryState { get; set; } = null;
        public IState endState { get; set; } = null;
        public Transition(string name) {
            this.name = name;
        }
        public Predicate<string>? Criteria { get; set; } = null;

    }

    public class StateMachineCommand : CommandBase
    {
        public override string command { get ; set ; }
    }
    public class StateMachine : StateMachineBase
    {
        public StateMachine(ISet<string> states, ISet<string> transitions, string startState)
        {
            //creates stateDictionary and transitionDictionary
            DictionaryBilder(states, transitions);
            this.currentState = startState;
        }
        /// <summary>
        /// initialaze stateDictionary and transitionDictionary
        /// </summary>
        /// <param name="stateNames"></param>
        /// <param name="transitionNames"></param>
        protected override void DictionaryBilder(ISet<string> stateNames, ISet<string> transitionNames)
        {
            stateDictionary = new Dictionary<string, IState>();
            transitionDictionary = new Dictionary<string, ITransition>();
            /// create stateDictionary
            foreach (var stateName in stateNames)
            {
                State state = new State(stateName);
                stateDictionary.Add(stateName, state);
            }
            /// create transitionDictionary
            foreach (var transitionName in transitionNames)
            {
                var names = transitionName.Split(new char[] { ':'});
                string entryStateName = names[0];
                string endStateName = names[1];
                State entryState = stateDictionary[entryStateName] as State;
                State endState = stateDictionary[endStateName] as State;

                Transition transition = new Transition(transitionName) { entryState = entryState, endState = endState };
                transitionDictionary.Add(transitionName, transition);
            }
            //add transitions to every state
            foreach (var state in stateDictionary.Values)
            {
                var foundTransitions = transitionDictionary.Values.Select(x=>x).Where(x => x.entryState.name == state.name).ToHashSet();
                state.transitions = foundTransitions;
            }
        }
        public override void AddAction(string state, Action<string> action)
        {
            stateDictionary[state].action += action;
        }
        public void AddActionRange(IEnumerable<string> states, IEnumerable<Action<string>> actions) {
            if (states.Count() != actions.Count())
            {
                throw new IndexOutOfRangeException();
            }
            var enumerator = actions.GetEnumerator();
            enumerator.MoveNext();
            foreach (var state in states)
            {
                stateDictionary[state].action += enumerator.Current;
                enumerator.MoveNext();
            }
        }

        public override void RemoveAction(string state, Action<string> action)
        {
            stateDictionary[state].action -= action;
        }

        public void AddFunctionHandler(string state, FunctionHandler functionHandler) {
            stateDictionary[state].functionHandler += functionHandler;
        }
        /// <summary>
        /// key - state, value - FunctionHandler
        /// </summary>
        /// <param name="actionsDictionary"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddFunctionHandler(Dictionary<string, FunctionHandler> actionsDictionary)
        {
            if(actionsDictionary is null)
                throw new ArgumentNullException();
            foreach (var actionsPair in actionsDictionary)
            {
                string state = actionsPair.Key;
                FunctionHandler functionHandler = actionsPair.Value;
                stateDictionary[state].functionHandler += functionHandler;
            }
        }

        public override void AddCritera(string transition, Predicate<string> critera)
        {
            transitionDictionary[transition].Criteria += critera;
        }
        public void AddCriteraRange(IEnumerable<string> transitions, IEnumerable<Predicate<string>> critera)
        {
            if (transitions.Count() != critera.Count())
            {
                throw new IndexOutOfRangeException();
            }
            var enumerator = critera.GetEnumerator();
            enumerator.MoveNext();
            foreach (var transition in transitions)
            {
                transitionDictionary[transition].Criteria += enumerator.Current;
                enumerator.MoveNext();
            }
        }
        /// <summary>
        /// key - transition, value - criteria
        /// </summary>
        /// <param name="criteriaDictionary"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddCriteraRange(Dictionary<string, Predicate<string>> criteriaDictionary)
        {
            if (criteriaDictionary is null)
                throw new ArgumentNullException();
            foreach (var criteriaPair in criteriaDictionary)
            {
                string transition = criteriaPair.Key;
                Predicate<string> criteria = criteriaPair.Value;
                transitionDictionary[transition].Criteria += criteria;
            }
        }
        public override void RemoveCritera(string transition)
        {
            transitionDictionary[transition].Criteria = null;
        }
        public override string? CheckTransitions(string state, string command)
        {
            try
            {
                return (from t in stateDictionary[state].transitions //TODO mb remove warning
                        where t.Criteria.Invoke(command)
                        select t.endState.name
                       ).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new NullReferenceException("Not added Criteria to transition");
            }
        }

        public override void Execute(string command)
        {
            string? transitionTo = CheckTransitions(currentState, command);
            if (transitionTo is null)
            {
                stateDictionary[currentState].DoCommand(command);
            }
            else
            {
                this.currentState = transitionTo;
                stateDictionary[currentState].DoCommand(command);
            }
        }
        public override object Execute(object sender, CommandBase e)
        {
            object ret = null;
            if (sender is null)
                return ret;
            if (e is CommandBase commandBase)
            {
                string command = commandBase.command;
                string? transitionTo = CheckTransitions(currentState, command);
                if (transitionTo is null)
                {
                    ret = stateDictionary[currentState].DoCommand(sender, commandBase);
                }
                else
                {
                    this.currentState = transitionTo;
                    ret = stateDictionary[currentState].DoCommand(sender, commandBase);
                }

            }
            return ret;
        }

        public void test() {
            int a = 1;
        }
    }
}
// before merging test