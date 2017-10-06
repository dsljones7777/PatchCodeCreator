
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatchCodeCreator
{
    class WorkSynchronizer
    {
        //This value is the number of spins (No Operation Instrunctions) the thread should do before 
        //giving up it's scheduled processor time. Please note that a value too low or too high will decrease performance 
        private static readonly int SPIN_COUNT = 10;

        //The group number identifies which thread group should be operating on the data
        private volatile int currentGroupNumber;

        //The startGroupId is the initial thread group that should be doing work on the data
        WorkSynchronizer(int startGroupId)
        {
            this.currentGroupNumber = startGroupId;
        }
        
        //groupId is the group id of the thread that calls this function
        //The function will wait until the specified groupId is set to do work
        void Wait(int groupId)
        {
            //Loop until the specified id is present
            while(currentGroupNumber != groupId)
            {
                //Execute a spin wait
                Thread.SpinWait(SPIN_COUNT);

                //If the id is still not present give up the processor time slice and loop 
                if (currentGroupNumber != groupId)
                    Thread.Sleep(0);
            }
        }

        //Notifies a group Id that the current thread is done working and the groupIdToNotify needs to do work
        void Notify(int groupIdToNotify)
        {
            this.currentGroupNumber = groupIdToNotify;
        }
    }


    class ThreadControl
    {
        private static readonly int SPIN_COUNT = 10;
        private static readonly int STATE_DONE = 0;
        private static readonly int STATE_START = 1;
        private volatile int state;
        public  int threadId;
        public ThreadControl()
        {
            this.state = STATE_DONE;
            this.threadId = 0;
        }
        public void Wait()
        {
            while(this.state == STATE_DONE)
            {
                Thread.SpinWait(SPIN_COUNT);
                if (this.state == STATE_START)
                    break;
                Thread.Sleep(0);
            }
        }
        public void Suspend()
        {
            this.state = STATE_DONE;
        }
        public void Resume()
        {
            this.state = STATE_START;
        }
        public bool IsWaiting()
        {
            if (this.state == STATE_DONE)
                return true;
            return false;
        }
    }
    class MultiThreadControl
    {
        private ThreadControl[][] threadControllerGroups;

        MultiThreadControl(int numberOfGroups)
        {
            this.threadControllerGroups = new ThreadControl[numberOfGroups][];
        }
        ThreadControl[] InitializeGroup(int groupIndex, int totalThreads)
        {
            this.threadControllerGroups[groupIndex] = new ThreadControl[totalThreads];
            return this.threadControllerGroups[groupIndex];
        }

        bool AreAllThreadsDone(int groupIndex)
        {
            for (int i = 0; i < threadControllerGroups[groupIndex].Length; i++)
            {
                if (threadControllerGroups[groupIndex][i].IsWaiting() == false)
                    return false;
            }
            return true;
        }
        void StartAllThreads(int groupIndex)
        {
            for (int i = 0; i < threadControllerGroups[groupIndex].Length; i++)
            {
                threadControllerGroups[groupIndex][i].Resume();
            }
        }
    }

    /*
     *  This synchronized class allows multiple threads to execute different states that depend on a previous state being completed.
        The class is to be created before any threads are created. Global declaration is not necessary but the thread
        routine that executes state code must have access to an instance that is shared with other threads.

        Typical usage:

            Constructor called, object created.
            Threads are created and have access to the newly created object.
            Threads may enter a loop

                Thread calls WaitOnState to wait for State 0
                Thread execute State 0 code
                Thread calls TaskFinished

                Thread calls WaitOnState to wait for State 1
                Thread executes State 1 code
                Thread calls TaskFinished

                Thread calls WaitOnState to wait for State N
                Thread executes State N code
                Thread calls Task Finished

                Thread loops or exits
        

    */
    class TaskController
    {
        //Default spin lock count. Note a number too big or too small will decrease system performance. 
        //The best value will be determined by the thread contention for the resource.
        //Typically more threads accessing the resource the lesser you want the spin count and vice versa; within reasonable limitations
        //You can change the variable from static to an instance member and add an argument to the constructor so this variable
        //can be adjusted depending on circumstances
        private static readonly int SPIN_LOCK_COUNT = 50;

        //Indicates the current state that threads are executing. Declared volatile to eliminate compiler optimizations
        //Removing volatile declaration is not recommended
        private volatile int _state;

        //The index of the state array that corresponds to the current state. Declared volatile to eliminate compiler optimizations
        //The volatile declaration might be able to be removed with this variable, check with Dr.Thomas and Professor Nowlin
        private volatile int _currentStateIndex;

        //Array of states that are set. State 1 is set first (Index 0) until state N is reached (Index N-1). When
        //state N is reached it starts over at State 1
        private int[] _stateArray;
        
        //The total number of threads that have finished the current state.
        private int _finishedCount;

        //The total number of threads
        private int _threadsPerState;
        public static readonly int State0 = 7;
        public static readonly int State1 = 7;
        private static readonly int[] States = { State0, State1 };
        //Initializes all variables and sets the initial state
        public TaskController(int [] states,int totalthreads)
        {
            this._stateArray = states;
            this._state = states[0]; //Set the initial state to the first one in the array
            this._currentStateIndex = 0;
            this._finishedCount = 0;
            this._threadsPerState = totalthreads;
        }

        //Waits until the requested state is set
        public void WaitOnState(int requestedState)
        {
            //Loop until the current state is the requested state
            while(_state != requestedState)
            {
                //Spin in case the state changes soon
                Thread.SpinWait(SPIN_LOCK_COUNT);
                if (this._state == requestedState)
                    break;

                //Give up current thread time slice to allow the processor to do other work
                //Not doing so can increase thread contention and also make less time for other threads executing
                //a different state to finish
                Thread.Sleep(0);
            }
        }

        //Called when a thread has finished a task for the current state.
        public void TaskFinished()
        {
            //Increment the total number of threads that have finished the current state
            //Atomic operation
            int totalTasksCompleted = Interlocked.Increment(ref _finishedCount);

            //If all the threads have finished the current state then change the state
            if (totalTasksCompleted == _threadsPerState)
            {
                //Reset the thread finished count
                this._finishedCount = 0;

                //Increment the current state index
                this._currentStateIndex++;

                //If the current state index is greater than the total number of states then reset the current state index
                if (this._currentStateIndex >= this._stateArray.Length)
                    this._currentStateIndex = 0;

                //Make sure the processor or compiler does not reorder the set state before the current state index or finished count
                //is reset. If the processor or compiler reorders a thread could execute the nest task and increment finished count before
                //it is reset causing a deadlock
                Interlocked.MemoryBarrier();

                //Assign the new state to where the current state index points
                this._state = this._stateArray[this._currentStateIndex];
            }
        }
        
    }


}
