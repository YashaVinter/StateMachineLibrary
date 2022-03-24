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
    public delegate object FunctionHandler (IStateModel stateModel, CommandBase commandBase);
    public abstract class CommandBase : EventArgs
    {
        public abstract string command { get; set; }
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
        object InvokeEvent(IStateModel model, CommandBase args);
    }
    public interface IState
    {
        IStateModel stateModel { get; set; }
        IStateEvent stateEvent { get; set; }
        CommandBase eventData { get; set; }
        public object DoCommand(string command);
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

}