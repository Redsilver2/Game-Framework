using System.Collections;
using UnityEngine;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelParentOffsetter : InventoryUI
    {
        [Space]
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;
        [SerializeField] private float offsetZ;

        [Space]
        [SerializeField] private float offsetSpeed;

        private IEnumerator updateParent;

        protected override void Awake()
        {
            base.Awake();

            if(navigator != null) {
                navigator.AddOnEnableListener(OnNavigatorEnable);
                navigator.AddOnDisableListener(OnNavigatorDisable);
            }
        }

        private void OnDestroy() {
            OnNavigatorDisable();
        }

        private void OnDisable() {
            OnNavigatorDisable();
        }

        private void OnEnable()
        {
            if (InventoryUINavigator.IsCurrent(navigator)) { OnNavigatorEnable(); }
        }

        private void OnNavigatorEnable()
        {
            OnNavigatorDisable();
            updateParent = UpdateParent();
            StartCoroutine(updateParent);   
        }

        private void OnNavigatorDisable()
        {
            if(updateParent != null) StopCoroutine(updateParent);
            updateParent = null;
        }

        private IEnumerator UpdateParent() 
        {
            while (navigator != null) {
                UpdateParentPosition(navigator.ModelParentTransform);
                yield return null;
            }
        }

        private void UpdateParentPosition(Transform transform)
        {
            if (transform != null)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                       Vector3.right   * offsetX +
                                                       Vector3.up      * offsetY +
                                                       Vector3.forward * offsetZ,
                                                       Time.deltaTime  * offsetSpeed);
                                                                                
            }
        }
    }
}
