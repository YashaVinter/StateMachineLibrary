using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
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
