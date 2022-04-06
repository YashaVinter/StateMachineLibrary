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
        public Dictionary<string, IState> stateDictionary { get; private protected set; } //= null;
        public Dictionary<string, ITransition> transitionDictionary { get; private protected set; } // = null; // TODO разобратьс япочему есть доступ несмотря на protected
        public string currentState { get; protected set; }
        //protected abstract (Dictionary<string, IState> stateDictionary, Dictionary<string, ITransition> transitionDictionary)
        //    DictionaryBilder(ISet<string> states, ISet<string> transitions);
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
                //var v1 = stateDictionary[state].stateModel.transitions.Select(t => t.name);
                //var v2 = (from tn in (from t in stateDictionary[state].stateModel.transitions select t.name) // v1
                //          where transitionDictionary[tn].transitionCriteria.InvokePredicate(command)
                //          select tn.Split(':')[1])
                //         .FirstOrDefault();

                //var v3 = (from t in transitionDictionary.Values
                //         where t.transitionModel.entryState.name == state
                //         where t.transitionCriteria.InvokePredicate(command)
                //         select t.transitionModel.endState.name)
                //         .FirstOrDefault();
                //// worked
                //return (from t1 in stateDictionary[state].stateModel.transitions //TODO mb remove warning
                //        join t2 in transitionDictionary.Values on t1 equals t2.transitionModel
                //        where t2.transitionCriteria.InvokePredicate(command)
                //        select t2.transitionModel.endState.name)
                //        .FirstOrDefault();

                //// worked
                //return transitionDictionary.Values
                //    .Where(t => t.transitionModel.entryState.name == state && t.transitionCriteria.InvokePredicate(command))
                //    .Select(t =>t.transitionModel.endState.name).FirstOrDefault();

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