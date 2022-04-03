﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
    public delegate object FunctionHandler(object sender, CommandBase e);
    public abstract class CommandBase : EventArgs
    {
        public abstract string command { get; set; }
    }
    public interface Iname
    {
        string name { get; set; }
    }
    public interface IState : Iname
    {
        internal ISet<ITransition> transitions { get; set; }

        event Action<string>? action;
        event FunctionHandler? functionHandler;
        void DoCommand(string cmd);
        object DoCommand(object sender, CommandBase e);
    }

    public interface ITransition : Iname
    {
        IState entryState { get; set; }
        IState endState { get; set; }
        Predicate<string>? Criteria { get; set; } //
    }
    public abstract class StateMachineBase
    {
        public Dictionary<string, IState> stateDictionary { get; private protected set; } = null;
        public Dictionary<string, ITransition> transitionDictionary { get;  private protected set; } = null; // TODO разобратьс япочему есть доступ несмотря на protected
        public string currentState { get; protected set; } = null;
        protected abstract void DictionaryBilder(ISet<string> states, ISet<string> transitions);

        public abstract void AddAction(string state, Action<string> action);
        public abstract void RemoveAction(string state, Action<string> action);
        public abstract void AddCritera(string transition, Predicate<string> critera);
        public abstract void RemoveCritera(string transition);
        public abstract string? CheckTransitions(string state, string command);// ret name transition?

        public abstract object Execute(object sender,CommandBase e);
        public abstract void Execute(string command);


    }
}