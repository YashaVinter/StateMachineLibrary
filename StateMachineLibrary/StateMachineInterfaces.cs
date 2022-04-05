using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public delegate object FunctionHandler (IStateData stateData);
    public interface Iname
    {
        string name { get; set; }
    }
    public abstract class CommandBase : EventArgs
    {
        public string command { get; set; }
        public CommandBase(string command)
        {
            this.command = command;
        }
    }

    public abstract class StateMachineBase
    {
        public Dictionary<string, IState> stateDictionary { get; private protected set; } = null;
        public Dictionary<string, ITransition> transitionDictionary { get; private protected set; } = null; // TODO разобратьс япочему есть доступ несмотря на protected
        public string currentState { get; protected set; } = null;
        //protected abstract (Dictionary<string, IState> stateDictionary, Dictionary<string, ITransition> transitionDictionary)
        //    DictionaryBilder(ISet<string> states, ISet<string> transitions);
    }
    public interface IStateMashineFactory
    {
        Dictionary<string, IState> BuildStateDictionary();
        Dictionary<string, ITransition> BuildTransitionDictionary();
        string BuildStartState();

    }
}