using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{

    public class StateMachine : StateMachineBase
    {
        public StateMachine(ISet<string> states, ISet<string> transitions, string startState)
        {
            //creates stateDictionary and transitionDictionary
            (this.stateDictionary, this.transitionDictionary) = DictionaryBilder(states, transitions);
            this.currentState = startState;
        }
        /// <summary>
        /// initialaze stateDictionary and transitionDictionary
        /// </summary>
        protected override (Dictionary<string, IState> stateDictionary, Dictionary<string, ITransition> transitionDictionary)
            DictionaryBilder(ISet<string> stateNames, ISet<string> transitionNames)
        {
            var stateDictionary = new Dictionary<string, IState>();
            var transitionDictionary = new Dictionary<string, ITransition>();
            /// create stateDictionary
            foreach (var stateName in stateNames)
            {
                State state = new State(new StateModel(stateName), new StateEvent(null), new StateData(null,null) ); //new StateData()
                stateDictionary.Add(stateName, state);
            }
            /// create transitionDictionary
            foreach (var transitionName in transitionNames)
            {
                var names = transitionName.Split(new char[] { ':' });
                string entryStateName = names[0];
                string endStateName = names[1];
                State entryState = stateDictionary[entryStateName] as State;
                State endState = stateDictionary[endStateName] as State;

                TransitionModel transitionModel = new TransitionModel(transitionName) 
                { 
                    entryState = entryState.stateModel, endState = endState.stateModel 
                };
                Transition transition = new Transition(transitionModel, new TransitionCriteria(null));
                transitionDictionary.Add(transitionName, transition);
            }
            //add transitions to every state
            foreach (var state in stateDictionary.Values)
            {
                var foundTransitions = transitionDictionary.Values
                    .Where(t => t.transitionModel.entryState.name == state.stateModel.name)
                    .Select(t => t.transitionModel).ToHashSet();
                //var foundTransitions = transitionDictionary.Values.Select(x => x).Where(x => x.entryState.name == state.name).ToHashSet();
                state.stateModel.transitions = foundTransitions;
                //state.transitions = foundTransitions;
            }
            return (stateDictionary, transitionDictionary);
        }
        public void AddFunctionHandler(Dictionary<string, FunctionHandler> actionsDictionary)
        {
            if (actionsDictionary is null)
                throw new ArgumentNullException();
            foreach (var actionsPair in actionsDictionary)
            {
                string state = actionsPair.Key;
                FunctionHandler functionHandler = actionsPair.Value;
                stateDictionary[state].stateEvent.functionHandler += functionHandler;
            }
        }
        public void AddCriteraRange(Dictionary<string, Predicate<string>> criteriaDictionary)
        {
            if (criteriaDictionary is null)
                throw new ArgumentNullException();
            foreach (var criteriaPair in criteriaDictionary)
            {
                string transition = criteriaPair.Key;
                Predicate<string> criteria = criteriaPair.Value;
                transitionDictionary[transition].transitionCriteria.criteria += criteria;
            }
        }
        public void AddEventData(Dictionary<string, EventDataBase> eventDataDictionary) 
        {
            if (eventDataDictionary is null)
                throw new ArgumentNullException();
            foreach (var eventDataPair in eventDataDictionary)
            {
                string state = eventDataPair.Key;
                var eventData = eventDataPair.Value;
                stateDictionary[state].stateData.eventData = eventData;
            }
        }

        public object Execute(InputDataBase InputData) // InputDataBase
        {
            try
            {
                string? transitionTo = CheckTransitions(currentState, InputData.command);
                if (transitionTo is null)
                {
                    return stateDictionary[currentState].DoCommand(InputData);
                }
                else
                {
                    this.currentState = transitionTo;
                    return stateDictionary[currentState].DoCommand(InputData);
                }

            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }
        }
        public string? CheckTransitions(string state, string command)
        {
            try
            {
                //return (from t in stateDictionary[state].transitions //TODO mb remove warning
                //        where t.Criteria.Invoke(command)
                //        select t.endState.name
                //       ).FirstOrDefault();
                return transitionDictionary.Values
                    .Where(t => t.transitionModel.entryState.name == state && t.transitionCriteria.InvokePredicate(command))
                    .Select(t =>t.transitionModel.endState.name).FirstOrDefault();

            }
            catch (Exception)
            {
                throw new NullReferenceException("Not added Criteria to transition");
            }
        }
        public void test() 
        {
        }
    }
}
