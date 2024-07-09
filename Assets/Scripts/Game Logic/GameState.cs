// Onur Ereren - June 2024
// Popcore case

// Abstract class that is the basis of all game state classes.

using System;

namespace PopsBubble
{

    public abstract class GameState
    {
        public Action OnStateComplete;
        
        public virtual void OnEnter()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
        
    }
}