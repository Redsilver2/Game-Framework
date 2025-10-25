using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class SeperatedItemModelsPositionUpdater : ItemModelsPositionUpdater
    {
        [Space]
        [SerializeField] private float modelFowardSpacing = 5f;

        [Space]
        [SerializeField] private float modelHorziontalSpacing = 5f;
        
        [Space]
        [SerializeField] private float modelVerticalSpacing   = 5f;

        [Space]
        [SerializeField] private float modelPositionLerpSpeed = 5f;

        protected sealed override void UpdateModelsPosition(int horizontalIndex, GameObject[] models) 
        {
            if(models == null || models.Length == 0) return;
            base.UpdateModelsPosition(horizontalIndex, models);
            UpdateModelsPosition(GetLeftArray(models, horizontalIndex), GetRightArray(models, horizontalIndex), models[horizontalIndex]);
        }

        protected sealed override void UpdateModelsPosition(int horizontalIndex, int verticalIndex, ComplexInventory inventory, GameObject[] models) 
        {
            if (models == null || inventory == null || models.Length == 0) return;
            base.UpdateModelsPosition(horizontalIndex, verticalIndex, inventory, models);
            UpdateModelsPosition(horizontalIndex, verticalIndex, GetModels(inventory, models));
        }

        private void UpdateModelsPosition(int horizontalIndex, int verticalIndex, GameObject[][] models)
        {
            if (models == null || models.GetLength(0) == 0) return;

            for (int i = 0; i < models.GetLength(0); i++)
                UpdateModelsPosition(i, horizontalIndex, verticalIndex, models[i]);

        }

        private void UpdateModelsPosition(int currentVerticalIndex, int horizontalIndex, int verticalIndex, GameObject[] models)
        {
            if(models == null || models.Length == 0) return; 

            if (currentVerticalIndex == verticalIndex) 
                UpdateModelsPosition(GetLeftArray(models, horizontalIndex), GetRightArray(models, horizontalIndex), models[horizontalIndex]);
            else
                UpdateModelsPosition(currentVerticalIndex, verticalIndex, models);
        }

        private void UpdateModelsPosition(int currentVerticalIndex, int verticalIndex, GameObject[] models)
        {
            if (models == null || models.Length == 0) return;

            bool isIndexBelowTarget = false;

            if (currentVerticalIndex < verticalIndex)
                isIndexBelowTarget = true;

            UpdateModelsPosition(verticalIndex, isIndexBelowTarget, models);
        }

        protected abstract void UpdateModelsPosition(int spacingIndex, bool isIndexBelowTarget, GameObject[] models);

        private void UpdateModelsPosition(GameObject[] left, GameObject[] right, GameObject current)
        {
            if (right == null || left == null || current == null) return;
            UpdateModelPosition   (current);
            UpdateModelsPositions (left , current, true);
            UpdateModelsPositions (right, current, false);
        }

        private void UpdateModelsPositions(GameObject[] models, GameObject current, bool isLeftArray)
        {
            if (models == null || current == null || models.Length == 0) return;

            for (int i = 0; i < models.Length; i++) {
                Transform transform = models[i].transform;
                UpdateModelPosition(transform, current.transform, i, isLeftArray);
            }
        }

        private void UpdateModelPosition(Transform model, Transform current, int index, bool isleftArray) { 
             if(model != null && current != null) {
                 Vector3 nextPosition = current.localPosition + (GetNextModelPosition(index, isleftArray) * modelHorziontalSpacing);
                  model.localPosition = Vector3.Lerp(model.localPosition, nextPosition, Time.deltaTime * modelPositionLerpSpeed);
             }                 
        }

        private void UpdateModelPosition(GameObject current)
        {
            if(current != null) {
                Transform transform     = current.transform;
                transform.localPosition = Vector3.Lerp(transform.localPosition,Vector3.forward * modelFowardSpacing, Time.deltaTime * modelPositionLerpSpeed);
            }
        }

        protected abstract Vector3 GetNextModelPosition(int index, bool isLeftArray);


        private void SetModelsArray(ComplexInventory inventory, GameObject[] models, ref List<GameObject[]> results)
        {
            int minIndex = 0; ;
            if (inventory == null || models == null) return;

            for (int i = 0; i < inventory.GetMaxVerticalIndex(); i++) {
                if (i > 0) minIndex += inventory.GetMaxHorizontalIndex(i) + 1;
                int maxIndex = minIndex + inventory.GetMaxHorizontalIndex(i) - 1;
                results.Add(GetModels(minIndex, maxIndex, models));
            }
        }

        private GameObject[][] GetModels(ComplexInventory inventory, GameObject[] models)
        {

            List<GameObject[]> results;
            if(inventory == null) return new GameObject[0][];

            results = new List<GameObject[]>();

            SetModelsArray(inventory, models, ref results);
            return results.ToArray();
        }


        private GameObject[] GetModels(int minIndex, int maxIndex, GameObject[] models) 
        {
            List<GameObject> results;
            if(models == null) return new GameObject[0];
            
            results = new List<GameObject>();

            for(int i = minIndex; i < maxIndex; i++)
                 results.Add(models[i]);

            return results.ToArray();
        }

        private GameObject[] GetLeftArray(GameObject[] models, int currentIndex)
        {
            List<GameObject> results;
            if(models == null || models.Length == 0) return new GameObject[0];

            results = new List<GameObject>();

            for(int i = currentIndex - 1;  i >= 0; i--)
            {
                if (i < 0) break;
                results.Add(models[i]);
            }

            return results.ToArray();
        }

        private GameObject[] GetRightArray(GameObject[] models, int currentIndex)
        {
            List<GameObject> results;
            if (models == null || models.Length == 0) return new GameObject[0];

            results = new List<GameObject>();

            for (int i = currentIndex + 1; i < models.Length; i++)
                results.Add(models[i]);

            return results.ToArray();
        }
    }
}
