using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public class TransitionModel : ITransitionModel
    {
        public IState? entryState { get; set; }
        public IState? endState { get; set; }
        public string name { get; set; }
        public TransitionModel(string name)
        {
            this.name = name;
        }
    }
    public class TransitionCriteria : ITransitionCriteria
    {
        public event Predicate<string> criteria;
        public bool InvokePredicate(string input)
        {
            return criteria.Invoke(input);
        }
        public TransitionCriteria(Predicate<string> criteria)
        {
            this.criteria = criteria;
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
