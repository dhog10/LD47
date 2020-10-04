using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SimonSays : MonoBehaviour
{
    private const int m_NumberOfScreens = 4;
    [SerializeField] private MeshRenderer m_MainScreen;
    [SerializeField] private MeshRenderer[] m_Screens = new MeshRenderer[m_NumberOfScreens];
    [SerializeField] private Material[] m_Symbols = new Material[m_NumberOfScreens];
    [Space(5)]
    [SerializeField] private uint m_RequiredSuccessiveCorrect = 4;
    [SerializeField] private float m_InitialDelay = 1f;
    [SerializeField] private float m_SymbolRevealTime = 3f;
    [SerializeField] private float m_MainScreenOnTime = 3f;
    [SerializeField] private float m_AnswerTime = 3f;
    [SerializeField] private float m_ResultsDisplayedTime = 2f;
    [SerializeField] private float m_RoundDelayTime = 2f;
    [Space(5)]
    [SerializeField] private UnityEvent m_SuccessEvent;
    private int m_SuccessiveCorrectCount;
    private bool m_InProgress;
    private bool m_Finished;
    private bool m_UserSelecting;
    private int m_SelectedScreenIndex = -1;

    private void OnValidate()
    {
        if (m_Screens.Length != m_NumberOfScreens)
        {
            System.Array.Resize(ref m_Screens, m_NumberOfScreens);
        }
        if (m_Symbols.Length != m_NumberOfScreens)
        {
            System.Array.Resize(ref m_Symbols, m_NumberOfScreens);
        }
    }

    public void StartGame()
    {
        if (m_InProgress || m_Finished)
        {
            return;
        }

        StartCoroutine(this.Puzzle());
    }

    private IEnumerator Puzzle()
    {
        m_InProgress = true;

        // Saving the materials that were originally on the screens
        var originalMainScreenMaterial = m_MainScreen.material;
        var originalScreenMaterials = new Material[m_NumberOfScreens];
        for (var i = 0; i < m_NumberOfScreens; i++)
        {
            originalScreenMaterials[i] = m_Screens[i].material;
        }

        // Generating an array of random indices for the order that the symbols will display on the screens
        var r = new System.Random();
        var symbolIndices = Enumerable.Range(0, m_NumberOfScreens).OrderBy(item => r.Next()).ToArray();

        yield return new WaitForSeconds(m_InitialDelay);

        for (var i = 0; i < m_NumberOfScreens; i++)
        {
            m_Screens[i].material = m_Symbols[symbolIndices[i]];
        }

        yield return new WaitForSeconds(m_SymbolRevealTime);

        for (var i = 0; i < m_NumberOfScreens; i++)
        {
            m_Screens[i].material = originalScreenMaterials[i];
        }

        for (var i = 0; i < m_RequiredSuccessiveCorrect; i++)
        {
            yield return new WaitForSeconds(m_RoundDelayTime);

            var currentSymbol = Random.Range(0, m_Symbols.Length);

            m_MainScreen.material = m_Symbols[symbolIndices[currentSymbol]];

            var userSelectingRoutine = StartCoroutine(this.UserSelecting());
            yield return new WaitUntil(() => m_UserSelecting == false);
            StopCoroutine(userSelectingRoutine);

            m_Screens[m_SelectedScreenIndex].material = m_Symbols[symbolIndices[m_SelectedScreenIndex]];
            yield return new WaitForSeconds(m_ResultsDisplayedTime);
            m_Screens[m_SelectedScreenIndex].material = originalScreenMaterials[m_SelectedScreenIndex];

            m_MainScreen.material = originalMainScreenMaterial;

            if (m_SelectedScreenIndex == -1 || m_SelectedScreenIndex != currentSymbol)
            {
                m_InProgress = false;
                Debug.Log("Fail");
                yield break;
            }
        }

        Debug.Log("Win");
        m_InProgress = false;
        m_Finished = true;
        m_SuccessEvent?.Invoke();
    }

    private IEnumerator UserSelecting()
    {
        m_UserSelecting = true;
        yield return new WaitForSeconds(m_AnswerTime);
        m_SelectedScreenIndex = -1;
        m_UserSelecting = false;
    }

    public void SelectScreen(int screenIndex)
    {
        if (!m_UserSelecting)
        {
            return;
        }

        m_SelectedScreenIndex = screenIndex;
        m_UserSelecting = false;
    }
}
