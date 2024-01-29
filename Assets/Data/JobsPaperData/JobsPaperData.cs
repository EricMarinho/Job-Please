using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Jobs Paper Data", menuName = "Data/Jobs Paper Data", order = 0)]
public class JobsPaperData : ScriptableObject
{
    public PossibleJobs job;
    public Sprite jobSprite;
    public string jobName;
}
