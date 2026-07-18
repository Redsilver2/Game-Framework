using RedSilver2.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class UISelector : MonoBehaviour
{
    [SerializeField] private List<UISelectorData> datas;

    private uint veritcalIndex;
    private uint horizontalIndex;
    private bool isUpdating;

    private UISelection selectedSelection;
    private List<List<UISelection>> selections;

    public uint VerticalIndex    => veritcalIndex;
    public uint HorizontalIndex  => horizontalIndex;
    public uint MaxVerticalIndex => selections != null ? (uint)selections.Count : 1;
    public UISelection SelectedSelection => selectedSelection;

    protected virtual void Awake() {
        this.selections = new List<List<UISelection>>();

        foreach (UISelectorData data in datas)
            AddRow(data.selections);

    }

    public void UpdateSelector() {
        UpdateSelector(0f);
    }

    public void UpdateSelector(float waitTime) {
        GameUIController.UpdateSelector(this, waitTime);
    }

    public void StopSelectorUpdate() {
        GameUIController.StopActifSelector(this);
    }

    public void AddRow() {
        AddRow(new List<UISelection>());
    }

    public void AddRow(UISelection[] selections)
    {
        AddRow(selections != null ? selections.ToList() : null);
    }

    public void AddRow(List<UISelection> selections)
    {
        if (this.selections == null) return;
        this.selections.Add(selections != null ? selections : new List<UISelection>());

        foreach(UISelection selection in this.selections[this.selections.Count - 1])
            selection?.SetOwner(this);
    }

    public void AddColumn(int row, UISelection[] selections)
    {
        if(selections == null) return;
        foreach (UISelection selection in selections) AddColumn(row, selection);
    }

    public void AddColumn(int row, List<UISelection> selections)
    {
        if (selections == null) return;
        foreach (UISelection selection in selections) AddColumn(row, selection);
    }


    public void AddColumn(int row, UISelection selection)
    {
        if (selections == null || row >= selections.Count || selection == null) return;
        else if (selections[row] == null) selections[row] = new List<UISelection>();

        if (!selections[row].Contains(selection)) selections[row]?.Add(selection);
        selection?.SetOwner(this);
    }

    public void Select(UISelection selection)
    {
        if (selection == null) return;

        for (int i = 0; i < this.selections.Count; i++)
        {
            if (this.selections[i] == null) continue;
            else if (this.selections[i].Contains(selection))
            {
                veritcalIndex = (uint)i;
                horizontalIndex = (uint)selections[i].IndexOf(selection);
            }
        }
    }

    private void Select()
    {
        UISelection nextSelectedSelection = GetNextSelectedSelection();

        if (nextSelectedSelection != null)
        {
            if (!nextSelectedSelection.Equals(selectedSelection))
            {
                Debug.Log("Select " + nextSelectedSelection);

                selectedSelection?.Deselect();
                selectedSelection = nextSelectedSelection;
                selectedSelection?.Select();
            }
        }
    }

    private bool CanSelect()
    {
        if (selectedSelection is DropdownUISelection)
            return false;

        return true;
    }

    public void StopUpdate()
    {
        if (isUpdating) {
            selectedSelection?.Deselect();
            isUpdating = false;
        }
    }

    public void ResetIndexes()
    {
        veritcalIndex   = 0; 
        horizontalIndex = 0;
    }

    public IEnumerator UpdateSelections(float enableTime) {

        isUpdating = true;
        yield return new WaitForSeconds(enableTime);

        selectedSelection?.Select();

        while (selections != null && isUpdating) {
            UpdateSelections();
            yield return null;
        }

        selectedSelection?.Deselect();
    }

    protected virtual void UpdateSelections()
    {
        if (CanSelect()) {
            Select();
            UpdateVericalIndex();
            UpdateHorizontalIndex();
        }
    }

    private void UpdateVericalIndex()
    {
        if (selections == null) return;

        if (GameUIController.GetNavigateDownState(true)) veritcalIndex++;
        else if (GameUIController.GetNavigateUpState(true)) veritcalIndex--;

        veritcalIndex = (uint)Mathf.Clamp(veritcalIndex, uint.MinValue, selections.Count - 1);
    }


    private void UpdateHorizontalIndex()
    {
        int vertical = (int)veritcalIndex;
        if (selections == null || selections[vertical] == null) return;

        if (GameUIController.GetNavigateLeftState(true)) horizontalIndex--;
        else if (GameUIController.GetNavigateRightState(true)) horizontalIndex++;

        horizontalIndex = (uint)Mathf.Clamp(horizontalIndex, uint.MinValue, selections[vertical].Count - 1);
    }

    private UISelection GetNextSelectedSelection()
    {
        int veritcal = (int)veritcalIndex;
        int horizontal = (int)horizontalIndex;

        if (selections == null || veritcal >= selections.Count || selections[veritcal] == null) return null;
        else if (horizontal >= selections[veritcal].Count) return null;

        return selections[veritcal][horizontal];
    }

    private List<List<UISelection>> GetUISelections() {
        List<List<UISelection>> results = new List<List<UISelection>>();

        foreach(UISelectorData data in datas){
            UISelection[] selections = data.selections;
            if (selections == null) continue;

            selections = selections.Where(x => x is not null).ToArray();
            if(selections.Length <= 0) continue;

            results?.Add(selections.ToList());           
        }

        return results;
    }

    [System.Serializable]
    private struct UISelectorData {
        public UISelection[] selections;
    }
}
