using System.Collections.Generic;

namespace RedSilver2.Framework.Inputs.Configurations.Datas
{
    public abstract class InputConfigurationData 
    {
        public  readonly string inputName;
    

        protected InputConfigurationData(string inputName) {
            this.inputName = inputName;
        }

        protected InputHandler GetInput() { 
            return InputManager.GetInputHandler(inputName);
        }

        public abstract bool IsOverridable();
    }
}
