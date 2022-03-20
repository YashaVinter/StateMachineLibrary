using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkApp.StateMachine
{
    internal interface StateMachineInterfaces
    {
    }
    public delegate object FunctionHandler(object sender, CommandBase e);
    public abstract class CommandBase : EventArgs
    {
        public abstract string command { get; set; }
    }
    public interface Iname
    {
        string name { get; set; }
    }
    public interface IState : Iname
    {
        List<ITransition> transitions { get; set; }

        event Action<string>? action;
        event FunctionHandler? functionHandler;
        void DoCommand(string cmd);
        public object DoCommand(object sender, CommandBase e);
    }

    public interface ITransition : Iname
    {
        IState entryState { get; set; }
        IState endState { get; set; }
        Func<string, bool>? Criteria { get; set; } //
    }
    public abstract class StateMachineBase
    {
        public Dictionary<string, IState> stateDictionary { get; protected set; } = null;
        public Dictionary<string, ITransition> transitionDictionary { get; protected set; } = null; // TODO разобратьс япочему есть доступ несмотря на protected
        public string currentState { get; protected set; } = null;

        public StateMachineBase(List<string> states, List<string> transitions, string startState)
        {
            DictionaryBilder(states, transitions);
            this.currentState = startState;
        }
        protected abstract void DictionaryBilder(List<string> states, List<string> transitions);

        public abstract void AddAction(string state, Action<string> action);
        public abstract void RemoveAction(string state, Action<string> action);
        public abstract void AddCritera(string transition, Func<string, bool> critera);
        public abstract void RemoveCritera(string transition);
        public abstract string? CheckTransitions(string state, string command);// ret name transition?

        public abstract object Execute(object sender,CommandBase e);
        public abstract void Execute(string command);


        public abstract void test();


    }
}
