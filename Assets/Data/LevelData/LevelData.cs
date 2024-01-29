using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level Data", order = 0)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public List<InterviewData> interviewDataList = new List<InterviewData>();
    public List<JobsPaperData> possibleJobs = new List<JobsPaperData>();
}
