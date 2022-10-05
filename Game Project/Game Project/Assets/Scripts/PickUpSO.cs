using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bozo Objects / Create Pick Up")]
public class PickUpSO : ScriptableObject
{
    public int ScoreWorth;
    public int PickUpTime;
    public Color PickUpColor;
}
