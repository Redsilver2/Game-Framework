using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{

    [CreateAssetMenu(fileName = "New Note Data", menuName = "Note/Data")]
    public class NoteData : ScriptableObject
    {
        [Space]
        [SerializeField] private string noteName;

        [Space]
        [SerializeField] private string[] pages;

        public string NoteName => noteName;
        public string[] Pages => pages;
    }
}
