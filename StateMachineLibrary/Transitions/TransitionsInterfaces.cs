using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
    public interface ITransitionModel : IName
    {
        IState? entryState { get; set; }
        IState? endState { get; set; }
    }
    public interface ITransitionCriteria
    {
        event Predicate<string> criteria;
        bool InvokePredicate(string input);
    }
    public interface ITransition
    {
        ITransitionModel transitionModel { get; set; }
        ITransitionCriteria transitionCriteria { get; set; }
        public bool CheckCriteria(string command);
    }
}
