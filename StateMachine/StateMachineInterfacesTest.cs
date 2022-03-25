using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineTest
{
    internal interface StateMachineInterfacesTest
    {

    }
    public delegate object FunctionHandler (IStateData stateData);
    public abstract class CommandBase : EventArgs
    {
        public string command { get; set; }
        public CommandBase(string command)
        {
            this.command = command;
        }
    }

    public interface Iname
    {
        string name { get; set; }
    }
    public interface IStateModel : Iname
    {
        ISet<ITransitionModel> transitions { get; set; }
    }
    public interface IStateEvent
    {
        event FunctionHandler functionHandler;
        object InvokeEvent(IStateData stateData);
    }
    public abstract class EventDataBase
    { }
    public abstract class InputDataBase : CommandBase
    {
        protected InputDataBase(string command) : base(command)
        {

        }
    }
    public interface IStateData
    {
        EventDataBase eventData { get; set; }
        InputDataBase inputData { get; set; }
    }
    public interface IState
    {
        IStateModel stateModel { get; set; }
        IStateEvent stateEvent { get; set; }
        IStateData stateData { get; set; }
        public object DoCommand(InputDataBase inputData);
    }

    // Transition
    public interface ITransitionModel : Iname
    {
        IStateModel entryState { get; set; }
        IStateModel endState { get; set; }    
    }
    public interface ITransitionCriteria
    {
        event Predicate<string>? Criteria;
        bool InvokePredicate(string input);
    }
    public interface ITransition
    {
        ITransitionModel transitionModel { get; set; }
        ITransitionCriteria transitionCriteria { get; set; }
        public bool CheckCriteria(string command);
    }
    // TEST


    public interface IStateOld
    {
        IStateModel stateModel { get; set; }
        IStateEvent stateEvent { get; set; }
        CommandBase eventData { get; set; }
        public object DoCommand(string command);
    }
}