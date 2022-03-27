using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public interface IStateModel : Iname
    {
        ISet<ITransitionModel> transitions { get; set; }
    }
    public interface IStateEvent
    {
        event FunctionHandler functionHandler;
        object InvokeEvent(IStateData stateData);
    }
    public abstract class EventDataBase
    { }
    public abstract class InputDataBase : CommandBase
    {
        protected InputDataBase(string command) : base(command)
        {

        }
    }
    public interface IStateData
    {
        EventDataBase eventData { get; set; }
        InputDataBase inputData { get; set; }
    }
    public interface IState
    {
        IStateModel stateModel { get; set; }
        IStateEvent stateEvent { get; set; }
        IStateData stateData { get; set; }
        public object DoCommand(InputDataBase inputData);
    }
}
