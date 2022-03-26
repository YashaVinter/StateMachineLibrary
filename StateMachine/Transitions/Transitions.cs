using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
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
}
