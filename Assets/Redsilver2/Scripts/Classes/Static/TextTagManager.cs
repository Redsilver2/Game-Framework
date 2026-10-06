using RedSilver2.Framework.Subtitles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using Color        = UnityEngine.Color;
using ColorUtility = UnityEngine.ColorUtility;

namespace RedSilver2.Framework
{
    [System.Serializable]
    public sealed partial class TextTagManager : MonoBehaviour {
        private static readonly List<TextTag> textTags = new List<TextTag>() {
            new ColorTag(), new SizeTag(), new WaveTag(), new RainbowTag(), new ShakeTag(),
            new RandomUnitCircleTag()
        };

        private static TextTagManager instance;

        public static TextTagManager Instance
        {
            get {
                if (instance != null) return instance;
                instance = new GameObject("TEXT TAG MANAGER").GetOrAddComponent<TextTagManager>();
                DontDestroyOnLoad(instance);
                return instance;
            }
        }

        private void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this) { Destroy(gameObject); }
        }

        public IEnumerator UpdateDisplayer(TMP_Text displayer, string text, float maxDuration, CancellationToken token) {
            if(displayer != null) 
                yield return new TextTagUpdater(displayer, text).Update(maxDuration, token);
        }

        public IEnumerator UpdateDisplayer(TMP_Text displayer, Subtitle.SubtitleData data, float maxDuration, CancellationToken token)
        {
            if (displayer != null)
                yield return new TextTagUpdater(displayer, data).Update(maxDuration, token);
        }


        public IEnumerator UpdateDisplayer(TMP_Text displayer, Subtitle.CharacterSubtitleData data, float maxDuration, CancellationToken token)
        {
            if (displayer != null)
                yield return new TextTagUpdater(displayer, data).Update(maxDuration, token);
        }

        public string GetFormattedText(string value)
        {
            return GetFormattedText(null, value, out uint maxWordCount, out int[] charsToIgnore);
        }

        public string GetFormattedText(string value, float progress)
        {
            string result = string.Empty;
            string formattedText = GetFormattedText(null, value, out uint maxWordCount, out int[] charsToIgnore);

            int wordCount = 0;
            progress = Mathf.Clamp01(progress);

            for(int i = 0; i < formattedText.Length; i++) {
                if (wordCount + 1 >= maxWordCount * progress) break;
                result += formattedText[i];

                if (!charsToIgnore.Contains(i))  wordCount++;
            }

            return result;
        }

        public string GetFormattedText(string value, out uint maxWordCount)
        {
            return GetFormattedText(null, value,out maxWordCount, out int[] charsToIgnore) ;
        }

        public string GetFormattedText(string value, out int[] charsToIgnore)
        {
            return GetFormattedText(null, value, out uint maxWordCount, out charsToIgnore);
        }

        private string GetFormattedText(TextTagUpdater updater, string value, out uint maxWordCount, out int[] charsToIgnore) {
            string result = string.Empty;

            charsToIgnore = new int[0];
            maxWordCount  = 0;
            
            if(string.IsNullOrEmpty(value)) return result;

            List<int> indexesToIgnore = new List<int>();
            char[] chars = value.ToCharArray();

            for(int i = 0; i < chars.Length; i++) {
                if (!HasFormattedTag(updater, chars, ref i, ref result, ref indexesToIgnore)) {
                    result += chars[i];
                    maxWordCount++;
                }
            }
 

            charsToIgnore = indexesToIgnore.ToArray();
            return result;
        }

        private bool HasFormattedTag(TextTagUpdater updater, char[] chars, ref int index, ref string result, ref List<int> charsToIgnore) {
            if(chars == null || index >= chars.Length || chars[index] != '<') return false;
            string tag = string.Empty;

            for (int i = index; i < chars.Length; i++) {
                tag += chars[i];

                if (chars[i] == '>') {
                    if (IsOpenTagValid(tag, out TextTag textTag, out string[] tagArgs)) {
                        textTag?.Execute(updater, tagArgs, ref result, ref charsToIgnore);
                        index = i;
                        return true;
                    }
                    else if (IsCloseTagValid(tag)) {
                        result += string.Empty;
                        index = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsOpenTagValid(string tag, out TextTag textTag, out string[] args)
        {
            if(textTags != null) {
                foreach(TextTag t in textTags) {
                    if(t == null) continue;
                    else if (t.IsValidOpen(tag, out args)) {
                        textTag = t;
                        return true;
                    }
                }
            }

            args = null;
            textTag = null;

            return false;
        }

        private bool IsCloseTagValid(string tag)
        {
            if (textTags != null) {
                foreach (TextTag t in textTags) {
                    if (t == null) continue;
                    else if (t.IsValidClose(tag)) {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public sealed partial class TextTagManager : MonoBehaviour
    {
        private abstract class TextTagEvent {

            public readonly int StartIndex;
            public readonly int EndIndex;   

            protected TextTagEvent(int startIndex) {
                this.StartIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);
                this.EndIndex = int.MaxValue;
            }

            protected TextTagEvent(int startIndex, int endIndex) {
                this.StartIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);
                this.EndIndex = Mathf.Clamp(endIndex, this.StartIndex, int.MaxValue);
            }

            public abstract void Update(TextTagUpdater updater);
        }

        private abstract class MeshTextTagEvent : TextTagEvent {
            protected MeshTextTagEvent(int startIndex) : base(startIndex) { }
            protected MeshTextTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex) { }

            public override void Update(TextTagUpdater updater)
            {
                if (updater != null)
                {
                    TMP_Text displayer = updater.Displayer;
                    TMP_CharacterInfo[] characterInfos = displayer != null ? displayer.textInfo.characterInfo : null;
                    TMP_MeshInfo[]      meshInfos      = displayer != null ? displayer.textInfo.meshInfo : null; 

                    if (displayer != null && characterInfos != null && meshInfos != null && characterInfos != null)  {
                        for (int i = StartIndex; i < characterInfos.Length; i++)
                        {
                            if (i > EndIndex) break;
                            else if (!characterInfos[i].isVisible) continue;

                            int materialIndex = characterInfos[i].materialReferenceIndex;

                            Update(i, characterInfos[i].vertexIndex, updater.DefaultMeshInfos[materialIndex], ref meshInfos[materialIndex]);
                            displayer?.UpdateGeometry(meshInfos[materialIndex].mesh, materialIndex);
                        }
                    }
                }
            }

            protected abstract void Update(int index, int vertexIndex, TMP_MeshInfo original, ref TMP_MeshInfo meshInfo);
        }

        private abstract class MeshPositionTextTagEvent : MeshTextTagEvent {
            protected MeshPositionTextTagEvent(int startIndex) : base(startIndex) { }
            protected MeshPositionTextTagEvent(int startIndex, int  endIndex) : base(startIndex, endIndex) { }

            protected override void Update(int index, int vertexIndex, TMP_MeshInfo original, ref TMP_MeshInfo meshInfo)
            {
                Vector3[] verts         = meshInfo.vertices;
                Vector3[] originalVerts = original.vertices;
               
                for (int i = 0; i < 4; i++) 
                    Update(original, index, originalVerts[vertexIndex + i], ref verts[vertexIndex + i]);

                meshInfo.mesh.vertices = verts;
            }

            protected abstract void Update(TMP_MeshInfo original, int index, Vector3 originalPosition, ref Vector3 position);
        }
        private sealed class WaveTextTagEvent : MeshPositionTextTagEvent
        {
            private readonly float waveHeight;
            private readonly float waveSpeed;
            private readonly float waveSpacing;

            public WaveTextTagEvent(int startIndex) : base(startIndex)
            {
                waveHeight = 1f;
                waveSpacing = 1f;
                waveSpeed = 1f;
            }

            public WaveTextTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex)
            {
                waveHeight = 1f;
                waveSpacing = 1f;
                waveSpeed = 1f;
            }

            public WaveTextTagEvent(int startIndex, int endIndex, float waveHeight, float waveSpeed, float waveSpacing) : base(startIndex, endIndex)
            {
                this.waveHeight = Mathf.Clamp(waveHeight, 1f, float.MaxValue);
                this.waveSpeed = Mathf.Clamp(waveSpeed, 1f, float.MaxValue);
                this.waveSpacing = Mathf.Clamp(waveSpacing, 1f, float.MaxValue);
            }

            protected override void Update(TMP_MeshInfo original, int index, Vector3 originalPosition, ref Vector3 position)
            {
                float offset = Mathf.Sin(Time.time * waveSpeed + (index * waveSpacing)) * waveHeight;
                position.y = originalPosition.y + offset;
            }
        }
        private sealed class ShakeTextTagEvent : MeshPositionTextTagEvent {
            public ShakeTextTagEvent(int startIndex) : base(startIndex) { }
            public ShakeTextTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex) { }

            protected sealed override void Update(TMP_MeshInfo original, int index, Vector3 originalPosition, ref Vector3 position) {
                float positionX = Mathf.Sin(Time.time * 20f) * 1.5f;
                position.x = originalPosition.x + positionX;
            }
        }

        private sealed class RandomUnitCircleTagEvent : MeshPositionTextTagEvent
        {
            private readonly Dictionary<TMP_MeshInfo, Vector2> positionDatas;

            public RandomUnitCircleTagEvent(int startIndex) : base(startIndex)
            {
                positionDatas = new Dictionary<TMP_MeshInfo, Vector2>();
            }

            public RandomUnitCircleTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex)
            {
                positionDatas = new Dictionary<TMP_MeshInfo, Vector2>();
            }

            protected override void Update(int index, int vertexIndex, TMP_MeshInfo original, ref TMP_MeshInfo meshInfo)
            {

                if (!positionDatas.ContainsKey(original)) positionDatas.Add(original, Vector2.zero); 
                base.Update(index, vertexIndex, original, ref meshInfo);

                Vector3[] meshDatas     = meshInfo.vertices;
                Vector3[] originalDatas = original.vertices;

                bool canUpdatePosition = true;

                for(int i = 0; i < 4; i++) {
                    Vector3 desiredPosition = (Vector2)originalDatas[vertexIndex + i] + positionDatas[original];

                    if (Vector3.Distance(meshDatas[vertexIndex + i], desiredPosition) > Mathf.Epsilon) {
                        canUpdatePosition = false;
                    }
                   // Working In Progress
                }

                if(canUpdatePosition) positionDatas[original] = (UnityEngine.Random.insideUnitCircle * 5);
            }

            protected sealed override void Update(TMP_MeshInfo original, int index, Vector3 originalPosition, ref Vector3 position)
            {
                position = (Vector2)originalPosition + positionDatas[original];
            }
        }

        private abstract class MeshColorTextTagEvent : MeshTextTagEvent
        {
            protected MeshColorTextTagEvent(int startIndex) : base(startIndex)
            {
            }

            protected MeshColorTextTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex)
            {

            }

            protected sealed override void Update(int index, int vertexIndex, TMP_MeshInfo original, ref TMP_MeshInfo meshInfo)
            {
                Color32[] vertsColors    = meshInfo.colors32;
                Color32[] originalColors = original.colors32;

                for (int i = 0; i < 4; i++)
                    Update(index, originalColors[vertexIndex + i], ref vertsColors[vertexIndex + i]);

                meshInfo.mesh.colors32 = vertsColors;
            }

            protected abstract void Update(int index, Color32 original, ref Color32 color);
        }
        private class RainbowTextTagEvent : MeshColorTextTagEvent
        {
            public RainbowTextTagEvent(int startIndex) : base(startIndex) { }
            public RainbowTextTagEvent(int startIndex, int endIndex) : base(startIndex, endIndex) { }

            protected override void Update(int index, Color32 original, ref Color32 color) {
                float hue = (Time.time * 0.25f + index * 0.1f) % 1f;
                Color newColor = Color.HSVToRGB(hue, 1f, 1f);
                color = newColor;
            }
        }


    }
    public sealed partial class TextTagManager : MonoBehaviour
    {
        private sealed class TextTagUpdater
        {
            public readonly uint MaxWordCount;
            public readonly int[] CharsToIgnore;
            public readonly TMP_MeshInfo[] DefaultMeshInfos;

            private readonly string text;
            public  readonly TMP_Text Displayer;
            private readonly List<TextTagEvent> events;

            public TextTagUpdater(TMP_Text displayer, string text) {
                this.Displayer = displayer;
                this.events = new List<TextTagEvent>();

                this.text = Instance.GetFormattedText(this, text, out MaxWordCount, out CharsToIgnore);

                if(displayer != null) {
                    displayer.gameObject.SetActive(false);
                    displayer.text = text;

                    DefaultMeshInfos = displayer.textInfo.meshInfo;
                   
                    displayer.text = string.Empty;
                    displayer.gameObject.SetActive(true);
                }
            }

            public TextTagUpdater(TMP_Text displayer, Subtitle.SubtitleData data) {
                this.Displayer = displayer;
                this.events = new List<TextTagEvent>();

                this.text = Instance.GetFormattedText(this, data != null ? data.Context : string.Empty, out MaxWordCount, out CharsToIgnore);

                if (displayer != null) {
                    displayer.gameObject.SetActive(false);
                    displayer.text = text;

                    DefaultMeshInfos = displayer.textInfo.meshInfo;

                    displayer.text = string.Empty;
                    displayer.gameObject.SetActive(true);
                }
            }

            public TextTagUpdater(TMP_Text displayer, Subtitle.CharacterSubtitleData data) {
                string characterName = data != null ? data.CharacterName + ": " : string.Empty;
                this.Displayer = displayer;

                this.events = new List<TextTagEvent>();
                this.text = Instance.GetFormattedText(this, data != null ? data.Context : string.Empty, out MaxWordCount, out CharsToIgnore);

                List<int> indexesToIgnore = CharsToIgnore.ToList();

                for (int i = 0; i < indexesToIgnore.Count; i++) { indexesToIgnore[i] += characterName.Length; }
                for (int i = 0; i < characterName.Length; i++)  { indexesToIgnore?.Add(i); }

                CharsToIgnore = indexesToIgnore.ToArray();
            }

            public void AddTextTagEvent(TextTagEvent _event)
            {
                if(_event != null && events != null) {
                    events?.Add(_event);
                }
            }

            public IEnumerator Update(float maxDuration, CancellationToken token) 
            {
                int currentIndex = -1;
                float t = 0f;

                maxDuration     = Mathf.Clamp(maxDuration, 0f, float.MaxValue);
                Start();

                while (true) {
                    Displayer?.ForceMeshUpdate();
                    Displayer?.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

                    if      (token.IsCancellationRequested) break;
                    else if (t >= maxDuration) t = maxDuration;

                    float progress = Mathf.Clamp01(t / maxDuration);
                    int maxIndex = (int)(MaxWordCount * progress);

                    if (currentIndex != maxIndex) {
                        if (Displayer != null) {
                            Displayer.text = string.Empty;
                            Displayer.text += GetTextProgress(maxIndex);
                        }

                        currentIndex = maxIndex;
                    }

                    foreach (TextTagEvent _event in events)
                        _event?.Update(this);

                    Displayer?.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    t += Time.deltaTime;
                    yield return null;
                }
            }


            public void Start() {
                if (Displayer != null) {
                    Displayer.text = string.Empty;
                }
            }

            private string GetTextProgress(int maxIndex)
            {
                string result = string.Empty;
              

                if (Displayer != null) {
                    int currentIndex = 0;

                   for(int i = 0; i < text.Length; i++) {
                      if (currentIndex + 1 > maxIndex) break;
                      else if (CharsToIgnore.Contains(i)) continue;
                      
                      result += text[i];
                      currentIndex++;
                   }
                }

                return result;
            }
        }
    }
    public sealed partial class TextTagManager : MonoBehaviour
    {

        private abstract class TextTag
        {
            protected readonly string Tag;
            protected TextTag() { Initialize(ref Tag); }

            protected abstract void Initialize(ref string tag);

            public bool IsValid(string value) => IsValidOpen(value, out string[] args);

            public bool IsValidOpen(string value, out string[] args)
            {
                args = null;
                if (string.IsNullOrEmpty(value) || !value.StartsWith('<') || !value.EndsWith('>') || !value.Contains(Tag, System.StringComparison.OrdinalIgnoreCase)) return false;

                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);
                value = value.Remove(0, Tag.Length);


                if (!value.StartsWith('(') || !value.EndsWith(')')) return false;

                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);

                args = GetArguments(value);
                return true;
            }

            public virtual bool IsValidClose(string value)
            {
                if (string.IsNullOrEmpty(value) || !value.StartsWith('<') || !value.EndsWith('>') || !value.Contains(Tag, System.StringComparison.OrdinalIgnoreCase)) return false;
                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);
                return value.StartsWith('/');
            }


            private string[] GetArguments(string value)
            {
                string[] splitArgs = value.Split(string.Concat("','"));
                List<string> validArgs = new List<string>();

                if (splitArgs.Length >= 0)
                {


                    if (splitArgs[0].Length > 0) splitArgs[0] = splitArgs[0].Remove(0, 1);
                    if (splitArgs.Length - 1 >= 1) splitArgs[splitArgs.Length - 1] = splitArgs[splitArgs.Length - 1].Remove(splitArgs[splitArgs.Length - 1].Length - 1, 1);

                    for (int i = 0; i < splitArgs.Length; i++)
                    {
                        validArgs?.Add(GetArgument(splitArgs[i]));
                    }
                }

                return validArgs.ToArray();
            }

            private string GetArgument(string value)
            {
                string arg = string.Empty;

                for (int i = 0; i < value.Length - 1; i++)
                    arg += value[i];

                return arg;
            }

            public virtual void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                string startValue = GetStartTag(args);

                for (int i = 0; i < startValue.Length; i++) { charsToIgnore.Add(result.Length + i); }
                result += startValue;
            }

            protected abstract string GetStartTag(string[] args);

            protected int GetIntArgument(string[] args, int argIndex, int defaultValue){
                if (args == null || argIndex < 0 || argIndex >= args.Length) return defaultValue;
                else if (int.TryParse(args[argIndex], out int result)) return result;
                else { return defaultValue; }
            }

            protected float GetFloatArgument(string[] args, int argIndex, float defaultValue)
            {
                if (args == null || argIndex < 0 || argIndex >= args.Length) return defaultValue;
                else if (float.TryParse(args[argIndex], out float result)) return result;
                else { return defaultValue; }
            }
        }

        private sealed class ColorTag : TextTag
        {
            public ColorTag() : base() { }

            protected sealed override string GetStartTag(string[] args)
            {
                return $"<color=#{GetColorHexCode(args)}>";
            }

            protected override void Initialize(ref string tag) => tag = "color";

            private string GetColorHexCode(string[] args)
            {
                Color color = Color.white;

                if (args == null) return ColorUtility.ToHtmlStringRGBA(color);
                else if (args.Length == 1)
                {
                    if (!ColorUtility.TryParseHtmlString(args[0], out color)) color = Color.white;
                }
                else if (args.Length > 1)
                {
                    float[] colorValues = new float[4];

                    for (int i = 0; i < colorValues.Length; i++)
                    {
                        if (i >= args.Length) { colorValues[i] = 1f; continue; }
                        else
                        {
                            if (!float.TryParse(args[i], out float value)) value = 1f;
                            args[i] = Mathf.Clamp01(value).ToString();
                        }
                    }

                    color = new Color(colorValues[0], colorValues[1], colorValues[2], colorValues[3]);
                }

                return ColorUtility.ToHtmlStringRGBA(color);
            }
        }

        private sealed class SizeTag : TextTag
        {
            public SizeTag() : base() { }
            protected override void Initialize(ref string tag) => tag = "size";

            private string GetSizeValue(string[] args)
            {
                int size = 14;
                if (args == null || args.Length == 0 || !int.TryParse(args[0], out size)) return size.ToString();
                return Mathf.Clamp(size, 1, int.MaxValue).ToString();
            }

            protected sealed override string GetStartTag(string[] args)
            {
                return $"<size={GetSizeValue(args)}>";
            }
        }

        private sealed class WaveTag : TextTag
        {
            public WaveTag() : base() { }

            public sealed override void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                base.Execute(updater, args, ref result, ref charsToIgnore);

                if(args != null)
                {
                    int   endIndex    = GetIntArgument  (args, 0, 1);
                    float waveHeight  = GetFloatArgument(args, 1, 1f);
                   
                    float waveSpeed   = GetFloatArgument(args, 2, 1f);
                    float waveSpacing = GetFloatArgument(args, 3, 1f);

                    updater?.AddTextTagEvent(new WaveTextTagEvent(result.Length, result.Length + endIndex, waveHeight, waveSpeed, waveSpacing));
                }
            }

            protected override string GetStartTag(string[] args)
            {
                return string.Empty;
            }

            protected sealed override void Initialize(ref string tag)
            {
                tag = "wave";
            }
        }

        private sealed class DelayTag : TextTag
        {
            public DelayTag() : base() { }

            public sealed override void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                base.Execute(updater, args, ref result, ref charsToIgnore);
                
            }

            protected sealed override string GetStartTag(string[] args) {
                return string.Empty;
            }

            protected override void Initialize(ref string tag) {
                tag = "delay";
            }
        }

        private sealed class RainbowTag : TextTag
        {
            public RainbowTag() : base() { }

            public sealed override void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                base.Execute(updater, args, ref result, ref charsToIgnore);
                updater?.AddTextTagEvent(new RainbowTextTagEvent(0));
            }

            protected sealed override string GetStartTag(string[] args)
            {
                return string.Empty;
            }

            protected sealed override void Initialize(ref string tag)
            {
                tag = "rainbow";
            }
        }

        private sealed class ShakeTag : TextTag
        {
            public ShakeTag() : base() { }

            public sealed override void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                base.Execute(updater, args, ref result, ref charsToIgnore);
                updater?.AddTextTagEvent(new ShakeTextTagEvent(0, 5));
            }

            protected sealed override string GetStartTag(string[] args)
            {
                return string.Empty;
            }

            protected sealed override void Initialize(ref string tag)
            {
                tag = "shake";
            }
        }

        private sealed class RandomUnitCircleTag : TextTag
        {
            public RandomUnitCircleTag() : base() { }

            public sealed override void Execute(TextTagUpdater updater, string[] args, ref string result, ref List<int> charsToIgnore)
            {
                base.Execute(updater, args, ref result, ref charsToIgnore);
                updater?.AddTextTagEvent(new RandomUnitCircleTagEvent(0, 4));
            }

            protected override string GetStartTag(string[] args)
            {
                return string.Empty;
            }

            protected override void Initialize(ref string tag)
            {
                tag = "randomunitcircle";
            }
        }
    }
}