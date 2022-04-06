using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public class StateModel : IStateModel
    {
        public string name { get; set; }
        public ISet<ITransition>? transitions { get; set; }
        public StateModel(string name)
        {
            this.name = name;
        }
    }
    public class StateEvent : IStateEvent
    {
        public event FunctionHandler functionHandler;
        public object InvokeEvent(IStateData stateData)
        {
            return functionHandler.Invoke(stateData);
        }
        public StateEvent(FunctionHandler functionHandler)
        {
            this.functionHandler = functionHandler;
        }
    }
    public class StateData : IStateData
    {
        public EventDataBase eventData { get; set; }
        public InputDataBase inputData { get; set; }
        public StateData(EventDataBase eventData, InputDataBase inputData)
        {
            this.eventData = eventData;
            this.inputData = inputData;
        }
    }
    public class State : IState
    {
        public IStateModel stateModel { get; set; }
        public IStateEvent stateEvent { get; set; }
        public IStateData stateData { get; set; }
        public State(IStateModel model, IStateEvent stateEvent, IStateData stateData)
        {
            this.stateModel = model;
            this.stateEvent = stateEvent;
            this.stateData = stateData;
        }
        public object DoCommand(InputDataBase inputData)
        {
            stateData.inputData = inputData;
            return stateEvent.InvokeEvent(stateData);
        }
    }
}
