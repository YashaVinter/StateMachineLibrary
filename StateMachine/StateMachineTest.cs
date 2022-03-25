using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineTest
{
    internal class StateMachineTest
    {
    }
    // State
    public class StateModel : IStateModel
    {
        public string name { get; set; }
        public ISet<ITransitionModel> transitions { get; set; }
        public StateModel(string name)
        {
            this.name = name;
        }
    }
    public class StateEvent : IStateEvent
    {
        public event FunctionHandler functionHandler;
        public object InvokeEvent(IStateModel model, CommandBase args)
        {
            return functionHandler?.Invoke(model, args);
        }
        public StateEvent(FunctionHandler functionHandler)
        {
            this.functionHandler = functionHandler;
        }
    }
    public class State : IState
    {
        public IStateModel stateModel { get; set; }
        public IStateEvent stateEvent { get; set; }
        public CommandBase eventData { get; set; }
        public State(IStateModel model, IStateEvent stateEvent, CommandBase eventData)
        {
            this.stateModel = model;
            this.stateEvent = stateEvent;
            this.eventData = eventData;
        }
        public object DoCommand(string command)
        {
            eventData.command = command;
            return stateEvent.InvokeEvent(stateModel, eventData);
        }
    }
    // Transition
    public class TransitionModel : ITransitionModel
    {
        public IStateModel entryState { get; set; }
        public IStateModel endState { get; set; }
        public string name { get; set; }
        public TransitionModel(string name)
        {
            this.name = name;
        }
    }
    public class TransitionCriteria : ITransitionCriteria
    {
        public event Predicate<string> Criteria;
        public bool InvokePredicate(string input)
        {
            return Criteria.Invoke(input);
        }
        public TransitionCriteria(Predicate<string> Criteria)
        {
            this.Criteria = Criteria;
        }
    }
    public class Transition : ITransition
    {
        public ITransitionModel transitionModel { get; set; }
        public ITransitionCriteria transitionCriteria { get; set; }
        public bool CheckCriteria(string command)
        {
            return transitionCriteria.InvokePredicate(command);
        }
        public Transition(ITransitionModel transitionModel, ITransitionCriteria transitionCriteria)
        {
            this.transitionModel = transitionModel;
            this.transitionCriteria = transitionCriteria;
        }
    }

    public class State2 : IState2
    {
        public IStateModel stateModel { get; set; }
        public IStateEvent stateEvent { get; set; }
        public IStateData stateData { get; set; }
        public State2(IStateModel model, IStateEvent stateEvent, IStateData stateData)
        {
            this.stateModel = model;
            this.stateEvent = stateEvent;
            this.stateData = stateData;
        }
        public object DoCommand(InputDataBase inputData)
        {
            stateData.inputData = inputData;
            //return stateEvent.InvokeEvent(stateData);
            return null;
        }
    }
}
