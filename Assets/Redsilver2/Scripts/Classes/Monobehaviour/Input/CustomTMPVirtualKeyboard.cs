using RedSilver2.Framework.Inputs;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomTMPVirtualKeyboard : TMPVirtualKeyboard
{
    [Space]
    [SerializeField] private int maxWidthElements;

    [Space]
    [SerializeField] private int layoutWidthSpace;
    [SerializeField] private int layoutHeightSpace;

    private IEnumerator growth;

    protected override void Awake()
    {
        base.Awake();

        AddOnPhysicalKeysLayoutUpdatedListener(keys =>
        {
            if (keys == null || keys.Length == 0) return;
            int lineCount = 0, spaceCount = 0;

            foreach (var key in keys.Where(x => x != null))
            {
                UpdateLayout(key.Button, GetNextButtonPosition(lineCount, spaceCount));
                lineCount++;

                if (lineCount > maxWidthElements)
                {
                    lineCount = 0;
                    spaceCount++;
                }
            }
        });

        AddOnPhysicalKeyExecutedListener((key, value) => {
            if (growth != null) StopCoroutine(growth);
            growth = null;

            growth = Growth(key, PhysicalKeys);
            StartCoroutine(growth);
        });
    }

    private IEnumerator Growth(VirtualKeyboardPhysicalKey main, VirtualKeyboardPhysicalKey[] others) {
        float t = 0f;

        while (t < 0.5f)
        {
            Transform transform;
            transform = main.Button.transform;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.5f, t / 0.5f);

            foreach (var otherKey in others.Where(x => x != null).Where(x => !x.Equals(main))){
                transform = otherKey.Button.transform;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one / 2, t/0.5f);
            }

            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        yield return new WaitForSeconds(0.05f);


        while (t < 0.5f)
        {
            Transform transform;
            transform = main.Button.transform;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one / 2, t / 0.5f);
            t += Time.deltaTime; 
            yield return null;
        }
    }


    private void UpdateLayout(Button button, Vector3 position) {
        if (button == null) return;
        button.transform.localPosition = position;
    }

    private Vector2 GetNextButtonPosition(int lineCount, int spaceCount) {

        return (Vector2.right * (spaceCount * layoutWidthSpace)) +
               (Vector2.down  * (lineCount  * layoutHeightSpace));
    }
}
