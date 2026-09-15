using UnityEditor.Rendering;
using UnityEngine;


namespace RedSilver2.Framework.Interactions
{
    [System.Serializable]
    public sealed partial class TransformInteractionHandler : InteractionHandler
    {
        [SerializeField, HideInInspector] private Transform transform;

        public TransformInteractionHandler() : base()
        {

        }

        protected override Collider GetCollider(float interactionRange)
        {
            if (transform == null) return null;
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactionRange, ~GameManager.PlayerLayer);
            return hitInfo.collider;
        }
    }

    public sealed partial class TransformInteractionHandler : InteractionHandler
    {
#if UNITY_EDITOR
        protected override void DrawBaseSettings(Color foldoutColor, Color backgroundColor, Color buttonColor)
        {
            base.DrawBaseSettings(foldoutColor, backgroundColor, buttonColor);

            EditorExtension.Space(10f);
            transform = EditorExtension.DisplayCustomField("Transform", true, transform);
        }
#endif
    }
}
