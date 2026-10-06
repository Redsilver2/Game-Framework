using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace RedSilver2.Framework.Subtitles
{
    [CreateAssetMenu(fileName = "New Subtitle", menuName = "Subtitle")]
    public partial class Subtitle : ScriptableObject {

        [SerializeField] private string title;
        [SerializeField] private Dictionary<string, List<SubtitleDataReference>> dataReferences;

        private static readonly List<SubtitleUpdater> subtitleUpdaters = new List<SubtitleUpdater>();
       
        public string Title => title;



        public static void Play(List<SubtitleData> datas, string anchorName, bool resetAllDatas) {
          
        }

        public static void Play(List<SubtitleData> datas, bool resetAllDatas)
        {

        }

        public static void Play(SubtitleData[] datas, bool resetAllDatas) {

        }

        public static void Play(SubtitleData[] datas, string anchorName, bool resetAllDatas)
        {

        }

        public static async void Play() { }
        public static void Resume(Subtitle subtitle) { }



        public static void Pause(Subtitle subtitle) { }

        private void SortDatasByTime()
        {
            if (dataReferences != null)
            {
                foreach (var pair in dataReferences)
                {
                    dataReferences[pair.Key] = dataReferences[pair.Key].OrderBy(x => x.GetSortedTime()).ToList();
                }
            }
        }


        public SubtitleData[] GetSubtitleDatas()
        {
            List<SubtitleData> results = new List<SubtitleData>();
            if (dataReferences == null) return results.ToArray();

            foreach (List<SubtitleDataReference> references in dataReferences.Values) {
                foreach (SubtitleDataReference reference in references) {
                    SubtitleData data = reference != null ? reference.Data : null;
                    if (data == null || results.Contains(data)) continue;
                    results?.Add(data);
                }
            }

            return results.ToArray();
        }

        [System.Serializable]
        private class SubtitleDataReference
        {
            [SerializeField, SerializeReference] private SubtitleData data;
            public SubtitleData Data => data;

            public SubtitleDataReference(SubtitleData data)
            {
                this.data = data;
            }

            public float GetSortedTime() {
                return data != null ? data.StartTime : 0f;
            }
        }
    }

    public partial class Subtitle : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool addOrRemoveDatas;
        [SerializeField, HideInInspector] private bool showDatas;

        [SerializeField, HideInInspector] private string dataName;
        [SerializeField, HideInInspector] private bool[] showDataFoldouts;
        [SerializeField, HideInInspector] private bool[] showDataCreation;

        public void DrawInspector()
        {
           

            EditorExtension.DrawVerticalHelpBox(() =>
            {
                if (dataReferences != null &&  EditorExtension.DisplayFoldout("Add / Remove", ref addOrRemoveDatas, new Color32(0, 0, 0, 181))) {
                    EditorExtension.DrawHorizontal(() => {
                        EditorExtension.DisplayBoldLabel("Data Reference Name");
                        dataName = EditorGUILayout.TextField(dataName).ToLower();
                    }, true);

                    if (!dataReferences.ContainsKey(dataName)) EditorExtension.DisplayButton("Add"   , () => { 
                        dataReferences.Add(dataName, new List<SubtitleDataReference>());
                        showDataFoldouts = new bool[dataReferences.Keys.Count];
                        showDataCreation = new bool[dataReferences.Keys.Count];
                        Debug.Log(showDataCreation.Length);
                    });
                    else                                      
                    EditorExtension.DisplayButton("Remove", () => { 
                        dataReferences.Remove(dataName);
                        showDataFoldouts = new bool[dataReferences.Keys.Count];
                        showDataCreation = new bool[dataReferences.Keys.Count];
                    });

                    EditorExtension.Space(10f);
                }

                if (dataReferences != null && showDataFoldouts.Length > 0 && EditorExtension.DisplayFoldout("Show", ref showDatas, new Color32(0, 0, 0, 181)))
                {
                    var results = dataReferences;
                    int index = 0;

                    if (showDataCreation == null)
                    {
                        showDataCreation = new bool[results.Count];
                    }

                    foreach (var keyPair in results) {
                        bool isRemoved = false;
                        bool isRemovingData = false;

                        EditorExtension.DrawVerticalHelpBox(() => {
                            if (EditorExtension.DisplayFoldout(keyPair.Key, ref showDataFoldouts[index], new Color32(0, 0, 0, 181))) {

                                EditorExtension.DisplayButton("Remove", () => {
                                    results?.Remove(keyPair.Key);
                                    showDataFoldouts = new bool[results.Keys.Count];
                                    showDataCreation = new bool[results.Keys.Count]; 
                                    isRemoved = true;
                                });

                                EditorExtension.Space(10f);

                                EditorExtension.IncrementIndent();

                                Debug.Log(showDataCreation[index] + " | " + index);

                                if (EditorExtension.DisplayFoldout("Data Creation", ref showDataCreation[index], new Color32(0, 0, 0, 181))){
                                    EditorExtension.IncrementIndent();
                                    EditorExtension.Space(10f);

                                    EditorExtension.DisplayButton("Add Default Subtitle Data", () => {
                                        keyPair.Value.Add(new SubtitleDataReference(new SubtitleData()));
                                    });

                                    EditorExtension.Space(5f);

                                    EditorExtension.DisplayButton("Add Audible Subtitle Data", () => {
                                        keyPair.Value.Add(new SubtitleDataReference(new AudibleSubtitleData()));
                                    });

                                    EditorExtension.Space(5f);

                                    EditorExtension.DisplayButton("Add Character Subtitle Data", () => {
                                        keyPair.Value.Add(new SubtitleDataReference(new CharacterSubtitleData()));
                                    });

                                    EditorExtension.Space(5f);

                                    EditorExtension.DisplayButton("Add Audible Character Subtitle Data", () => {
                                        keyPair.Value.Add(new SubtitleDataReference(new AudibleCharacterSubtitleData()));
                                    });


                                    EditorExtension.Space(10f);
                                    EditorExtension.DecrementIndent();
                                }

                                var values = keyPair.Value;

                                foreach (var value in values) {
                                    if (value == null || value.Data == null) continue;

                                    value.Data?.DrawInspector($"Data {keyPair.Value.IndexOf(value) + 1}", ref isRemovingData);
                                    if (isRemovingData) { keyPair.Value.Remove(value); break; }
                                }

                                EditorExtension.DecrementIndent();
                            }

                        }, true);



                        index++;

                        if (isRemoved || isRemovingData) break;

                    }

                    dataReferences = results;
                }
            });
        }
#endif
    }
}
