using UnityEngine;
using UnityEngine.UI;

public class Level_Stars : MonoBehaviour
{
    [SerializeField] Image[] stars;
    [SerializeField] string levelName;
    int value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = PlayerPrefs.GetInt(levelName, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (value == 0)
            return;
        for (int i = stars.Length - 1; i >= 0; i--)
        {
            if (i <= value)
                stars[i].color = Color.yellow;
        }
    }

    //for (int i = 0; i<starIcons.Length; i++)
    //    {
    //        starIcons[i].sprite = i<stars? filledStar : emptyStar;
    //starIcons[i].gameObject.SetActive(true);
    //    }
}
