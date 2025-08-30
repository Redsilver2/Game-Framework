using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputControl
    {
        private string path;
        private Sprite icon;

        public string Path => path;
        public Sprite Icon => icon;


        private InputControl() { }
       
        public InputControl(string path, Sprite icon)
        {
            this.path    = path;   
            this.icon    = icon;
        }

        public void OverrideIcon(Sprite icon)
        {
            this.icon = icon;
        }
    }
}
