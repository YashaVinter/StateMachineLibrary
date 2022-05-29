using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StateMachineLibrary
{
    public class StateMachine : StateMachineBase
    {
        public ExitCode exitCode { get; protected set; }
        public StateMachine(IStateMashineFactory stateMashineFactory) : base(stateMashineFactory)
        {
        }
        public override object Execute(InputDataBase InputData) // InputDataBase
        {
            string? nextState = CheckTransitions(currentState, InputData.command);
            if (nextState is null)
            {
                var returnCode = (Task<ExitCode>)stateDictionary[currentState].DoCommand(InputData);
                return exitCode = returnCode.Result;
            }
            else
            {
                var returnCode = (Task<ExitCode>)stateDictionary[nextState].DoCommand(InputData);
                if (returnCode.Result is ExitCode.OK)
                    this.currentState = nextState;
                return exitCode;
            }
        }

    }
    public enum ExitCodeBase
    {
        OK,
        TelegramBotAPIError,
        EventError
    }
    public enum ExitCode
    { 
        OK,
        TelegramBotAPIError,
        EventError
    }
}