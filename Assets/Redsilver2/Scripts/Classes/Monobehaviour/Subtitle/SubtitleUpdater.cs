using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles {

    public partial class Subtitle : ScriptableObject
    {
        public abstract class SubtitleUpdater {

            private SubtitleDisplayer template;
            private readonly SubtitleData[] datas;
            private readonly Dictionary<SubtitleData, SubtitleDisplayer> displayers;

            private readonly SubtitleUpdaterType updaterType;
            private CancellationTokenSource updaterSource;

            private float timeElapsed;
            private float worldSpaceCheckDistance;

            private float minFadeDistance;
            private float maxFadeDistance;

            public float WorldSpaceCheckDistance => worldSpaceCheckDistance;
            public float MinFadeDistance => minFadeDistance;
            public float MaxFadeDistance => maxFadeDistance;
            public float TimeElapsed             => timeElapsed;

            public    SubtitleUpdaterType UpdaterType => updaterType;
            protected SubtitleDisplayer   Template    => template;

            public SubtitleUpdater(SubtitleData[] subtitleDatas, SubtitleUpdaterType updaterType) {
                this.datas = subtitleDatas != null ? subtitleDatas.Where(x => x != null)
                                                                  .Distinct()
                                                                  .OrderBy(x => x.StartTime)
                                                                  .ToArray() : new SubtitleData[0];

                this.displayers  = new Dictionary<SubtitleData, SubtitleDisplayer>();
                this.updaterType = updaterType;
            }

            public SubtitleUpdater(List<SubtitleData> subtitleDatas, SubtitleUpdaterType updaterType) {
                this.datas  = subtitleDatas != null ? subtitleDatas.Where(x => x != null)
                                                                   .Distinct()
                                                                   .OrderBy(x => x.StartTime)
                                                                   .ToArray() : new SubtitleData[0];

                this.displayers  = new Dictionary<SubtitleData, SubtitleDisplayer>();
                this.updaterType = updaterType;
            }

            public async void Play()                { await PlayAsync(); }
            public async void Play(float startTime) { await PlayAsync(startTime); }
            public async void Play(int startIndex)  { await PlayAsync(startIndex); }
            public async void Play(GameTime time)   { await PlayAsync(time); }

            public async Awaitable PlayAsync(float timeElapsed) {
                timeElapsed = Mathf.Clamp(timeElapsed, 0f, float.MaxValue);

                if (datas != null) {
                    await PlayAsync(timeElapsed, datas.Where(x => timeElapsed < x.StartTime).OrderBy(x => x.StartTime).ToArray());
                }
            }

            public async Awaitable PlayAsync(int startIndex)  {
                if (datas != null) {
                    if(startIndex >= 0 && startIndex < datas.Length) {
                        await PlayAsync(datas[startIndex].StartTime, datas.Where(x => Array.IndexOf(datas, x) >= startIndex).OrderBy(x => x.StartTime).ToArray());
                    }
                }
            }
            public async Awaitable PlayAsync(GameTime time) {
                if (time != null) await PlayAsync(time.GetFloatConversion());
            }
            public async Awaitable PlayAsync() { 
                await PlayAsync(0f); 
            }

            private async Awaitable PlayAsync(float timeElapsed, SubtitleData[] datas) {
                if(datas == null) return;
                updaterSource?.Cancel();
                updaterSource = new CancellationTokenSource();

                CancellationToken token = updaterSource.Token;

                if (this.datas != null) {
                    foreach (SubtitleData data in this.datas)
                        data?.Reset();
                }
               
                CoroutineManager.Start(UpdateDatas(datas, token));              
                await UpdateTimeElapsed(GetLastData(datas), token);
            }

            private async Awaitable UpdateTimeElapsed(SubtitleData lastData, CancellationToken token)  {
                while (lastData != null && !token.IsCancellationRequested) {
                    if (lastData == null || timeElapsed >= lastData.EndTime) break;
                    this.timeElapsed += Time.deltaTime;
                    await Awaitable.NextFrameAsync(token);
                }
            }

            protected virtual IEnumerator UpdateDatas(SubtitleData[] datas, CancellationToken token)
            {
                SubtitleData lastData = GetLastData(datas);
                
                while(lastData != null && !token.IsCancellationRequested) {
                    if(timeElapsed >= lastData.EndTime) break;
                    yield return null;
                }
            }

            public void AddDisplayer(SubtitleData data, SubtitleDisplayer displayer) {
                if (this.displayers != null && this.datas != null && data != null && displayer != null) {
                    if (datas.Contains(data) && !displayers.ContainsKey(data) && !displayers.ContainsValue(displayer)) {
                        displayers?.Add(data, displayer);
                    }
                }
            }

            public void RemoveDisplayer(SubtitleData data) {
                if (this.displayers != null && this.datas != null && data != null)
                {
                    if (datas.Contains(data) && displayers.ContainsKey(data)) {
                        displayers?.Remove(data);
                    }
                }
            }

            public SubtitleDisplayer GetDisplayer(SubtitleData data) {
                if (this.displayers != null && this.datas != null && data != null)
                {
                    if (datas.Contains(data) && displayers.ContainsKey(data))
                        return displayers[data];
                }

                return null;
            }

            public void SetDisplayerTemplate(SubtitleDisplayer template) { this.template = template; }

            public void Stop() {
                updaterSource?.Cancel();
                updaterSource = null;
            }

            private SubtitleData GetLastData(SubtitleData[] datas) {
                if (datas == null) return null;
                int lastIndex = GetLastDataIndex(datas);
                return lastIndex < 0 ? null : datas[lastIndex];
            }

            private int GetLastDataIndex(SubtitleData[] datas) {
                int result = -1;
                if (datas == null) return result;

                for (int i = 0; i < datas.Length; i++) {
                    if(datas[i] == null) continue;
                    result = i;
                }

                return result;
            }

            public enum SubtitleUpdaterType {
                Hybrid,
                ScreenSpace,
                WorldSpace
            }
        }

        [System.Serializable]
        public sealed class SingleSubtitleUpdater : SubtitleUpdater
        {
            public SingleSubtitleUpdater(SubtitleData[] subtitleDatas, SubtitleUpdaterType updaterType) : base(subtitleDatas, updaterType) { }
            public SingleSubtitleUpdater(List<SubtitleData> subtitleDatas, SubtitleUpdaterType updaterType) : base(subtitleDatas, updaterType) { }

            protected sealed override IEnumerator UpdateDatas(SubtitleData[] datas, CancellationToken token) {
                SubtitleData selectedData = null;
                SubtitleDisplayer displayer = Template == null ? null : Instantiate(Template);

                CancellationTokenSource subtitleTokenSource  = null;
                CancellationTokenSource displayerTokenSource = null;

                while (datas != null && displayer != null && !token.IsCancellationRequested) {
                    SubtitleData nextData = GetNextValidData(selectedData, datas, token);

                    while (nextData != null) {
                        if (TimeElapsed >= nextData.StartTime) break;
                        yield return null;
                    }

                    if (nextData != null) {
                        SetNextData(nextData, displayer, ref selectedData, ref displayerTokenSource, ref subtitleTokenSource);
                    }
                    else { break; }
                }

                displayerTokenSource?.Cancel();
                subtitleTokenSource?.Cancel();

                yield return CoroutineManager.Start(base.UpdateDatas(datas, token));
                Debug.Log("Loop Exited");
            }

            private void SetNextData(SubtitleData nextData, SubtitleDisplayer displayer, ref SubtitleData selectedData, ref CancellationTokenSource displayerTokenSource, ref CancellationTokenSource subtitleTokenSource)
            {
                if (nextData == null) return;

                subtitleTokenSource?.Cancel();
                subtitleTokenSource = new CancellationTokenSource();

                displayerTokenSource?.Cancel();
                displayerTokenSource = new CancellationTokenSource();

                RemoveDisplayer(selectedData);
                AddDisplayer(nextData, displayer);

                CoroutineManager.Start(displayer?.UpdateDisplayMode(nextData, this, displayerTokenSource.Token));
                CoroutineManager.Start(nextData?.Update(this, displayer, subtitleTokenSource.Token));

                selectedData = nextData;
            }
         
            private SubtitleData GetNextValidData(SubtitleData data, SubtitleData[] datas, CancellationToken token) {
                if (datas == null) return null;
                int selectedIndex = datas != null ? Array.IndexOf(datas, data) : -1;

                while (!token.IsCancellationRequested) {
                    selectedIndex++;

                    if (selectedIndex >= datas.Length) break;
                    else if(datas[selectedIndex] != null) return datas[selectedIndex];
                }

                return null;
            }
        }

        [System.Serializable]
        public sealed class StackableSubtitleUpdater : SubtitleUpdater
        {
            public StackableSubtitleUpdater(SubtitleData[] subtitleDatas,     SubtitleUpdaterType updaterType) : base(subtitleDatas, updaterType) { }
            public StackableSubtitleUpdater(List<SubtitleData> subtitleDatas, SubtitleUpdaterType updaterType) : base(subtitleDatas, updaterType) { }

            protected sealed override IEnumerator UpdateDatas(SubtitleData[] datas, CancellationToken token) {
               if(datas != null) {
                    foreach(SubtitleData data in datas) {
                        if (data == null) continue;
                        CoroutineManager.Start(UpdateData(data, token));
                    }
               }
                
               yield return CoroutineManager.Start(base.UpdateDatas(datas, token));
            }

            private IEnumerator UpdateData(SubtitleData data, CancellationToken token) {

                if(data != null) {
                    CancellationTokenSource subtitleTokenSource  = new CancellationTokenSource();
                    CancellationTokenSource displayerTokenSource = new CancellationTokenSource();
                    SubtitleDisplayer       displayer            = Template == null ? null : Instantiate(Template);

                    AddDisplayer(data, displayer);

                    while (data != null && displayer != null && !token.IsCancellationRequested) {
                        if (data.IsUpdateFinished && displayer.GetAlpha() <= 0f) {
                            break;
                        }
                        else if (!data.IsUpdateStarted && TimeElapsed >= data.StartTime) {
                            CoroutineManager.Start(displayer?.UpdateDisplayMode(data, this, displayerTokenSource.Token));
                            CoroutineManager.Start(data?.Update(this, displayer, subtitleTokenSource.Token));
                        }

                        yield return null;
                    }

                    RemoveDisplayer(data);

                    displayerTokenSource?.Cancel();
                    subtitleTokenSource?.Cancel();
                }
            }
        }
    }
}
