using UnityEngine;

namespace RedSilver2.Framework.Player.Inventory
{
    public abstract class Inventory : MonoBehaviour {

        private uint horizontalIndex;
        public  uint HorizontalIndex => horizontalIndex;


        public abstract bool Contains();
        public abstract int GetMaxHorizontalIndex();

    }
}
