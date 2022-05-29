# StateMachineLibrary 
**StateMachineLibrary** -  library implementing the concept of a finit state machine  

To use it, you need to implement the interface _IStateMashineFactory_  

 Interface _IStateMashineFactory_  uses the methods _BuildStateDictionary_ to build a state model (_IState_) and _BuildTransitionDictionary_ to build a model of transitions between states (_ITransition_), as well as the _BuildStartState_ method to specify the initial state of the automaton

 After creating a finit state machine model, it has one public method _Execute_ to transition to a new state

During the transition, it may fail with an error, in which case the transition will not be completed and the machine will remain in the previous state