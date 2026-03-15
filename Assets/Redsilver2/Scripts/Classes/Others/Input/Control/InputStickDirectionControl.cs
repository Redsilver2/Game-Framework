using System;
using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputStickDirectionControl : InputControl 
    {
        private bool canResetKeyDown;
        private bool canKeyUpBeTriggered;

        protected InputStickDirectionControl(Sprite icon) : base(icon)
        { 
            this.canKeyUpBeTriggered = false;
            this.canResetKeyDown     = true;
        }

        protected abstract float GetAxisValue();
       
        public sealed override bool GetKey()
        {
            return Mathf.Abs(GetAxisValue()) > 0f;
        }

        public sealed override bool GetKeyDown()
        {
            float axisValue = Mathf.Abs(GetAxisValue());

            if (axisValue == 1f && canResetKeyDown)
            {
                canResetKeyDown = false;
                return true;
            }

            if (axisValue < 1f)
                canResetKeyDown = true;

            return false;
        }

        public sealed override bool GetKeyUp()
        {
            float axisValue = Mathf.Abs(GetAxisValue());

            if (axisValue == 1f && !canKeyUpBeTriggered){
                canKeyUpBeTriggered = true;
            }
            else if (axisValue < 1f && canKeyUpBeTriggered) {
                canKeyUpBeTriggered = false;
                return true;
            }

            return false;
        }
    }
}
