using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RedSilver2.Framework.Dialogs
{
    [System.Serializable]
    public sealed class Dialog {

        private float choiceDuration;
        private bool  canUpdateChoiceDuration;

        public  readonly string Name;
        private readonly List<Subtitle> subtitles;
        private readonly List<DialogChoice> choices;

        public bool  CanUpdateChoiceDuration => canUpdateChoiceDuration;
        public float ChoiceDuration => choiceDuration;

        private static readonly List<Dialog> instances = new List<Dialog>();
        private const string PATH = "Dialog/";

        public Dialog(string name) {
            this.Name      = name.ToUpper();
            this.subtitles = new List<Subtitle>();
          
            this.choices   = new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(List<Subtitle> subtitles, string name)
        {
            this.Name = name.ToUpper();
            this.subtitles = subtitles != null ? subtitles : new List<Subtitle>();
           
            this.choices = new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(Subtitle[] subtitles, string name) {
            this.Name = name.ToUpper();
            this.subtitles = subtitles != null ? subtitles.ToList() : new List<Subtitle>();
           
            this.choices   = new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(List<Subtitle> subtitles, List<DialogChoice> choices, string name)
        {
            this.Name = name.ToUpper();
            this.subtitles = subtitles != null ? subtitles : new List<Subtitle>();
           
            this.choices = choices != null ? choices : new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(List<Subtitle> subtitles, DialogChoice[] choices, string name)
        {
            this.Name = name.ToUpper();
            this.subtitles = subtitles != null ? subtitles : new List<Subtitle>();
           
            this.choices   = choices   != null ? choices.ToList() : new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(Subtitle[] subtitles, DialogChoice[] choices, string name)
        {
            this.Name = name.ToUpper();
            this.subtitles  = subtitles != null ? subtitles.ToList() : new List<Subtitle>();
           
            this.choices    = choices   != null ? choices.ToList()   : new List<DialogChoice>();
            AddInstance();
        }

        public Dialog(Subtitle[] subtitles, List<DialogChoice> choices, string name)
        {
            this.Name = name.ToUpper();
            this.subtitles  = subtitles != null ? subtitles.ToList() : new List<Subtitle>();
           
            this.choices    = choices != null ? choices : new List<DialogChoice>();
            AddInstance();
        }

        private void AddInstance() {
            if (string.IsNullOrEmpty(Name) || Get(Name) != null) return;
            instances?.Add(this);
        }

        public void SetChoiceDuration(float choiceDuration) {
            this.choiceDuration = Mathf.Clamp(choiceDuration, Mathf.Epsilon, float.MaxValue);
        }

        public void SetCanUpdateChoiceDuration(bool canUpdateChoiceDuration) {
            this.canUpdateChoiceDuration = canUpdateChoiceDuration;
        }

        public void AddChoice(DialogChoice choice) {
            if (choice == null || choices == null || choices.Contains(choice))
                return;

            if(GetChoice(choice.Name) == null) {
                choices?.Add(choice);
            }
        }

        public void RemoveChoice(DialogChoice choice) {
            if(choice == null || choices == null || !choices.Contains(choice))
                return;

            choices?.Remove(choice);
        }

        public void AddSubtitle(Subtitle subtitle) {
            if (subtitle == null || subtitles == null || subtitles.Contains(subtitle)) return;
            subtitles?.Add(subtitle);
        }

        public void RemoveSubtitle(Subtitle subtitle) {
            if (subtitle == null || subtitles == null || !subtitles.Contains(subtitle)) return;
            subtitles?.Remove(subtitle);
        }

        public bool IsSimilar(Dialog data) {
           foreach(Subtitle subtitle in data.subtitles) {
               var results = data.subtitles.Where(x => x != null).Where(X => X.IsSimilar(subtitle));
               if(results.Count() > 0) return true;
           }

           return data.Name.Equals(Name, System.StringComparison.OrdinalIgnoreCase);
        }

        public int GetChoicesCount() { 
            return choices != null ? choices.Count() : 0; 
        }

        public int GetSubtitlesCount() {
            return subtitles != null ? subtitles.Count() : 0;
        }

        public bool ContainsChoice(DialogChoice choice)
        {
            return choices != null ? choices.Contains(choice) : false;
        }

        public bool IsDone(float time) {
            
            return false;
        }

        public Subtitle GetSubtitle(int index) {
            if(subtitles == null || subtitles.Count <= 0 || index < 0 || index >= subtitles.Count) 
                return null;

            return subtitles[index];
        }

        public Subtitle[] GetSubtitles() {
            return subtitles != null ? subtitles.ToArray() : new Subtitle[0];
        }

        public DialogChoice GetChoice(string name)
        {
            if (choices == null || string.IsNullOrEmpty(name)) return null;
            var results = choices.Where(x => x.Name.ToLower().Equals(name.ToLower()));
            return results.Count() > 0 ? results.First() : null;             
        }

        public DialogChoice GetChoice(int index)
        {
            if (choices == null || choices.Count <= 0 || index < 0 || index >= choices.Count)
                return null;

            return choices[index];
        }

        public DialogChoice GetRandomChoice()
        {
            if (choices == null || choices.Count <= 0) return null;
            return GetChoice(Random.Range(0, choices.Count));
        }

        public DialogChoice[] GetChoices() {
            return choices != null ? choices.ToArray() : new DialogChoice[0];
        }

        private DialogChoice[] GetSimilarChoices(string name)
        {
            if (choices == null) return new DialogChoice[0];
            else if (string.IsNullOrEmpty(name)) return new DialogChoice[1];

            return choices.Where(x => x != null)
                          .Where(x => x.Name.ToLower().Equals(name.ToLower()))
                          .ToArray();
        }

        public static string GetPath()
        {
            return PATH;
        }


        public static void Create(string name) {
            if (Get(name) == null) new Dialog(name);
        }

        public static void Create(Subtitle[] subtitles, string name) {
            if(Get(name) == null) new Dialog(subtitles, name);
        }
        public static void Create(Subtitle[] subtitles, List<DialogChoice> choices, string name)
        {
            if (Get(name) == null) new Dialog(subtitles, choices, name);
        }

        public static void Create(Subtitle[] subtitles, DialogChoice[] choices, string name)
        {
            if (Get(name) == null) new Dialog(subtitles, choices, name);
        }


        public static void Create(List<Subtitle> subtitles, string name) {
            if (Get(name) == null) new Dialog(subtitles, name);
        }

        public static void Create(List<Subtitle> subtitles, List<DialogChoice> choices, string name) {
            if (Get(name) == null) new Dialog(subtitles, choices, name);
        }

        public static void Create(List<Subtitle> subtitles, DialogChoice[] choices, string name)  {
            if (Get(name) == null) new Dialog(subtitles, choices, name);
        }

        public static Dialog Get(string name) 
        {
            foreach(Dialog dialog in instances) {
                if(dialog == null) continue;
                else if(dialog.Name.ToLower().Equals(name.ToLower())) return dialog;
            }

            return null;
        }
    }
}
