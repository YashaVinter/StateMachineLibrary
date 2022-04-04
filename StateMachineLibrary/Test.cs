using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StateMachineLibrary;

namespace StateMachineLibraryTest
{
    internal class Test
    {
        static void Main()
        {
            //// init state
            //StateModel model = new StateModel("one");
            //model.transitions = null;
            //StateEvent stateEventHome = new StateEvent(new BotImplementation().caseHome);
            //CommandBase caseHomeData = new CaseHomeData("");


            //State stateHome = new State(model, stateEventHome, caseHomeData);
            //// execute state event
            //stateHome.DoCommand("toTwo");
            //// Transitions
            //TransitionModel transitionModel = new TransitionModel("one:two");

            //string two = "two";
            //Predicate<string> crit = delegate (string s) { return s == two; };
            //TransitionCriteria transitionCriteria = new TransitionCriteria(crit);
            //Transition transition = new Transition(transitionModel, transitionCriteria);
            //transition.CheckCriteria(two);
            ////
            //State state1 = new State(new StateModel(""), new StateEvent(null), null);
            //Transition transition1 = new Transition(new TransitionModel(""), new TransitionCriteria(null));

            //ISet<string> states = new HashSet<string>() 
            //{
            //    "one","two","three"
            //};
            //ISet<string> transitions = new HashSet<string>()
            //{
            //    "one:two","two:three","three:one"
            //};
            //StateMachine stateMachine = new StateMachine(states, transitions, "one");

            
            //var actDict = new Dictionary<string, FunctionHandler>() 
            //{
            //    { "one",new BotImplementation().caseHome},
            //    { "two",new BotImplementation().caseHome}
            //};

            //var criteriaDict = new Dictionary<string, Predicate<string>>() 
            //{
            //    {"one:two",crit },
            //    {"two:three",crit }
            //};
            //var eventDataDict = new Dictionary<string, CommandBase>() 
            //{
            //    { "one",new CaseHomeData("")},
            //    { "two",new CaseHomeData("")},
            //};
            //stateMachine.AddFunctionHandler(actDict);
            //stateMachine.AddCriteraRange(criteriaDict);
            //stateMachine.AddEventData(eventDataDict);

            //stateMachine.stateDictionary["one"].eventData = new CaseHomeData("");
            //stateMachine.stateDictionary["one"].DoCommand("111");
            //stateMachine.transitionDictionary["one:two"].CheckCriteria("111");
        }
    }

    public class BotImplementation
    {
        public object caseHome(IStateModel model, CommandBase args)
        {
            var id = 111L;
            var text = "sending text";
            var buttons = new object();
            if (args is CaseHomeData data)
            {
                id = data.chatId;
                text = data.text;
                buttons = data.button;
                Console.WriteLine($"id: {id} text: {text}");
            }

            return null;
        }
    }
    public abstract class BotCommandBase : CommandBase
    {
        protected BotCommandBase(string command) : base(command)
        {
        }

        public abstract object bot { get; set; }
        public abstract long chatId { get; set; }
        public abstract string text { get; set; }
        public abstract object button { get; set; }
    }
    public class CaseHomeData : BotCommandBase
    {
        public override object bot { get; set; }
        public override long chatId { get; set; } = 15154L;
        public override string text { get; set; } = "some text";
        public override object button { get; set; }
        public CaseHomeData(string command) : base(command)
        {

        }
    }
}

// before changing to async