using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public delegate object FunctionHandler (IStateData stateData);
    public interface IName
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
        public Dictionary<string, IState> stateDictionary { get; private protected set; }
        public Dictionary<string, ITransition> transitionDictionary { get; private protected set; }
        public string currentState { get; protected set; }
        public StateMachineBase(IStateMashineFactory stateMashineFactory)
        {
            this.stateDictionary = stateMashineFactory.BuildStateDictionary();
            this.transitionDictionary = stateMashineFactory.BuildTransitionDictionary();
            this.currentState = stateMashineFactory.BuildStartState();
        }
        public virtual object Execute(InputDataBase InputData) // InputDataBase
        {
            string? nextState = CheckTransitions(currentState, InputData.command);
            if (nextState is null)
            {
                return stateDictionary[currentState].DoCommand(InputData);
            }
            else
            {
                this.currentState = nextState;
                return stateDictionary[nextState].DoCommand(InputData);
            }
        }
        public string? CheckTransitions(string state, string command)
        {
            try
            {
                return (from t in stateDictionary[state].stateModel.transitions
                        where t.transitionCriteria.InvokePredicate(command)
                        select t.transitionModel!.endState!.stateModel.name)
                       .FirstOrDefault();
            }
            catch (Exception)
            {
                throw new NullReferenceException("Not added Criteria to transition");
            }
        }

    }
    public interface IStateMashineFactory
    {
        Dictionary<string, IState> BuildStateDictionary();
        Dictionary<string, ITransition> BuildTransitionDictionary();
        string BuildStartState();
    }
}