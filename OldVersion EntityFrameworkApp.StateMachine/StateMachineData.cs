using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFrameworkApp.DataBase;
using System.Text.RegularExpressions;

namespace EntityFrameworkApp.StateMachine
{
    public class StateMachineData
    {
        public List<string> states;
        public List<string> transitions;
        public List<Action<string>> actions;
        public List<Func<string, bool>> criteria;
        public StateMachineData() 
        {
            States st = new States();
            states = new List<string>()
            {
                st.home,
                st.find,
                st.edit,
                st.help,
                st.findPerson
            };

            var act = new StateConsoleActions();
            actions = new List<Action<string>>()
            {
                act.homeAction,
                act.findAction,
                act.editAction,
                act.helpAction,
                act.findPersonAction
            };
            Func<string,string,string> tr = (a, b) => { return a +":"+ b; };

            transitions = new List<string>()
            { 
                tr(st.home,st.find),
                tr(st.home,st.edit),
                tr(st.home,st.help),
                tr(st.find,st.home),
                tr(st.find,st.findPerson),
                tr(st.findPerson,st.home),
                tr(st.edit,st.home),
                tr(st.help,st.home)
            };
            criteria = new List<Func<string, bool>>()
            { 
                new Criteria().toFind,
                new Criteria().toEdit,
                new Criteria().toHelp,
                new Criteria().toHome,
                new Criteria().toFindPerson,
                new Criteria().toHome,
                new Criteria().toHome,
                new Criteria().toHome,
            };
        }
        public class States
        {
            public readonly string home = "home";
            public readonly string find = "find";
            public readonly string edit = "edit";
            public readonly string help = "help";
            public readonly string findPerson = "findPerson";

        }
        public struct Transitions 
        { 
            
        }
        public class StateConsoleActions
        {
            private States states = new States();
            public void homeAction(string cmd) {
                Console.WriteLine($"Choose mode: {states.home} {states.find} {states.edit} {states.help}");   
            }
            public void findAction(string cmd)
            {
                Console.WriteLine("Write person name. If you want see all persons write \"ALL\"");
            }
            public void editAction(string cmd)
            {
                Console.WriteLine("Write person name. If you want add or edit person");
            }
            public void helpAction(string cmd)
            {
                Console.WriteLine("Its a friendBot/ Here you can add informations about your friends");
            }
            public void findPersonAction(string cmd)
            {
                Person person = new Person().Find(cmd);
                if (person is null)
                {
                    Console.WriteLine("Person not found, try again or return home");
                    return;
                }
                Console.WriteLine($"Person found! Its a : {person.name}, notes: {person.notes}");
                Console.WriteLine($"Write peron name");
            }
        }

        public class Criteria
        {
            private States states = new States();
            private bool isMatch(string pattern, string input) { 
                return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(input); 
            }

            public bool toHome(string input) {
                return isMatch(states.home, input);
            }
            public bool toFind(string input)
            {
                return isMatch(states.find, input);
            }
            public bool toEdit(string input)
            {
                return isMatch(states.edit, input);
            }
            public bool toHelp(string input)
            {
                return isMatch(states.help, input);
            }
            public bool toFindPerson(string input)
            {
                return isMatch(states.findPerson, input);
            }

        }
        public class Criteria1 
        {
            private FriendsBot.FrontendData.ButtonData button
                = new FriendsBot.FrontendData.ButtonData();
            public bool toHome(string input) {
                return input == button.home;
            }
        }
    }
}