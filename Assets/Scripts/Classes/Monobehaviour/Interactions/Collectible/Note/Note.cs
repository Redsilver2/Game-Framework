using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{ 
    public class Note : Collectible
    {
        [SerializeField] private NoteData noteData;

        public override CollectibleData GetData()
        {
            return noteData;
        }
    }
}