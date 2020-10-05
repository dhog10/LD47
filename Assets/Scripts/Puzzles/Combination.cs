using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Combination : MonoBehaviour
{
    public bool m_OverrideButtonMaterial = false;

    public CombinationItem[] m_CombinationItems;
    public CombinationScreen[] m_Screens;
    public UnityEvent m_OnCorrectEvent;
    public UnityEvent m_OnIncorrectEvent;

    private CombinationItem[] m_CombinationItemsOrdered;
    private int m_CombinationIndex = 0;
    private bool m_Correct;

    void Start()
    {
        // Sort the items into a random order.

        m_CombinationItemsOrdered = new CombinationItem[m_CombinationItems.Length];
        var itemsList = new ArrayList();
        var screensList = new ArrayList();

        foreach (var item in m_CombinationItems)
        {
            itemsList.Add(item);

            if (m_OverrideButtonMaterial)
            {
                var mesh = item.m_Button.m_ButtonPressObject.GetComponent<MeshRenderer>();

                if (mesh != null)
                {
                    mesh.sharedMaterial = item.m_Material;
                }
            }
        }

        foreach (var screen in m_Screens)
        {
            screensList.Add(screen);
        }

        var c = 0;

        while (itemsList.Count > 0)
        {
            var index = Random.Range(0, itemsList.Count);
            var item = (CombinationItem)itemsList[index];
            itemsList.RemoveAt(index);

            m_CombinationItemsOrdered[c++] = item;

            var combinationIndex = c - 1;

            item.m_Button.m_ActivateEvent.AddListener(() =>
            {
                this.EnterCombination(combinationIndex);
            });

            if (screensList.Count > 0)
            {
                var screenIndex = Random.Range(0, screensList.Count);
                var screen = (CombinationScreen)screensList[screenIndex];
                screensList.RemoveAt(screenIndex);

                screen.SetMaterial(item.m_Material);
                screen.SetIndex(c);
            }
        }

        m_CombinationIndex = 0;
        m_Correct = true;
    }

    void Update()
    {
        
    }

    public void Evaluate()
    {
        if (m_Correct)
        {
            m_OnCorrectEvent?.Invoke();
        }
        else
        { 
            m_Correct = true;

            StartCoroutine(this.ResetButtonsRoutine());
        }

        m_CombinationIndex = 0;
    }

    public void EnterCombination(int index)
    {
        if (index >= m_CombinationItemsOrdered.Length)
        {
            return;
        }

        if (m_CombinationIndex >= m_CombinationItemsOrdered.Length)
        {
            return;
        }

        var currentItem = m_CombinationItemsOrdered[m_CombinationIndex++];
        var item = m_CombinationItemsOrdered[index];
        var correct = currentItem == item;

        if (!correct)
        {
            m_Correct = false;
        }

        if (m_CombinationIndex >= m_CombinationItemsOrdered.Length)
        {
            this.Evaluate();
        }
    }

    private IEnumerator ResetButtonsRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var item in m_CombinationItems)
        {
            item.m_Button.Deactivate();
        }

        m_OnIncorrectEvent?.Invoke();
    }
}

[System.Serializable]
public class CombinationItem
{
    public Button m_Button;
    public Material m_Material;
}