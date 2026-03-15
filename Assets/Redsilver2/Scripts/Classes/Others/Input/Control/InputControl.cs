using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputControl
    {
        private Sprite icon;
        public readonly string Path;

        public  Sprite Icon => icon;
       
        private InputControl() { }

        protected InputControl(string path)
        {
            this.icon = null;
            this.Path = path;
        }

        protected InputControl(Sprite icon) {
            this.icon = icon;
            this.Path = string.Empty;
        }

        protected InputControl(string path, Sprite icon)
        {
            this.icon = icon;
            this.Path = path;
        }


        public void OverrideIcon(Sprite icon) {
            this.icon = icon;
        }

        public bool ComparePath(string path) {
            if(string.IsNullOrEmpty(path) || string.IsNullOrEmpty(this.Path)) 
                return false;
            return this.Path.ToLower().Equals(path.ToLower());
        }

        public abstract bool GetKey();
        public abstract bool GetKeyDown();
        public abstract bool GetKeyUp();
    }
}
