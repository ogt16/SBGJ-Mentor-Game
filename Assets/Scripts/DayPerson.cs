using Unity.VisualScripting;
using UnityEngine;

public enum DayPersonState
{
    Oblivious,
    Listening,
    Recruited
}

public class DayPerson : MonoBehaviour
{
    DayPersonState state;
    float progress;
    float progresDissipationRate = 10;
    [SerializeField] SpriteRenderer progressBar;

    void Start()
    {
        state = DayPersonState.Oblivious;
        progress = 0;
    }

    void Update()
    {
        if (state == DayPersonState.Listening)
        {
            progress -= Time.deltaTime * progresDissipationRate;
            if (progress <= 0)
            {
                state = DayPersonState.Oblivious;
                progress = 0;
                return;
            }
        }
        if (progressBar != null)
        {
            progressBar.color = new Color(1.0f - (progress / 100), progress / 100, 0f);
        }
    }

    public void Influence(float power)
    {
        if (state == DayPersonState.Recruited) return;

        progress += power;
        Debug.Log(progress);
        if (progress >= 100)
        {
            progress = 100;
            state = DayPersonState.Recruited;
            GameData.Instance.cultMembers += 1;
        }
        else
        {
            state = DayPersonState.Listening;
        }
    }
}
