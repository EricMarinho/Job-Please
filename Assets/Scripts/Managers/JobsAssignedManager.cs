using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobsAssignedManager : MonoBehaviour
{
    public static JobsAssignedManager instance;
    public List<Feedback> assignedJobs;

    private Dictionary<string, Dictionary<PossibleJobs, string>> feedbackDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        assignedJobs = new List<Feedback>();
        InitializeFeedbackDictionary();
    }

    private void InitializeFeedbackDictionary()
    {
        feedbackDictionary = new Dictionary<string, Dictionary<PossibleJobs, string>>();

        // Add initial feedback for each character and job combination
        // Vania
        AddFeedback("Vania", PossibleJobs.PodcastHost, "“The Vampire is doing way better than we expected. It’s impressive how they keep talking for hours without a single other soul in the room.”");
        AddFeedback("Vania", PossibleJobs.Nurse, "“BLOODY HELL! WHO LET A VAMPIRE WORK IN A HOSPITAL FULL OF INJURED PEOPLE??? THEY’RE HUNTING OUR PATIENTS!”");
        AddFeedback("Vania", PossibleJobs.FunFairDirector, "“I mean, they spent years isolated… The people at the park are not used to them talking to the walls relentlessly.”");
        // Miguel Mouse
        AddFeedback("Miguel", PossibleJobs.PodcastHost, "“He was just making strange onomatopoeic sounds on the microphone. Does he even have a voice?”");
        AddFeedback("Miguel", PossibleJobs.Nurse, "“Those Mouse hands didn't help a lot.”");
        AddFeedback("Miguel", PossibleJobs.FunFairDirector, "“OH MY GOD, YOU FOUND HIM, THANKS FOR SENDING HIM BACK! (I know he didn't want to, but he is great at this).”");
        // Kate
        AddFeedback("Kate", PossibleJobs.ElementaryTeacher, "“The kids loved her. She knows a lot of cool fire tricks to entertain the children and also tells interesting stories about her adventures all the time.”");
        AddFeedback("Kate", PossibleJobs.PetCare, "“For some reason, almost all of the cosmic creatures seems to hates fire :(”");
        AddFeedback("Kate", PossibleJobs.Astronaut, "“Space doesn't have that much oxygen. You know that fire literally needs oxygen to exist, right?”");
        // Cthulhu
        AddFeedback("Cthulhu", PossibleJobs.ElementaryTeacher, "“You know, he's undeniably intelligent, but talking in ancient and forbidden languages isn't a good idea when teaching kids.”");
        AddFeedback("Cthulhu", PossibleJobs.PetCare, "“Wow, we never met someone so patient and caring for little creatures, all of them liked that tentacle guy!”");
        AddFeedback("Cthulhu", PossibleJobs.Astronaut, "“Other civilizations trembled in fear upon encountering them... I mean, it could be useful if we ever intend to conquer other planets or something like this.”");
        // Add more feedback entries as needed
    }

    public void AssignJob(JobsPaperData job)
    {
        Feedback feedback = GetFeedbackDialogue(job);
        assignedJobs.Add(feedback);
    }

    public Feedback GetFeedbackDialogue(JobsPaperData job)
    {
        string characterName = GameManager.instance.GetCurrentInterview()?.characterName;

        if (!string.IsNullOrEmpty(characterName) && feedbackDictionary.ContainsKey(characterName))
        {
            PossibleJobs assignedJob = job.job;

            if (feedbackDictionary[characterName].ContainsKey(assignedJob))
            {
                string feedbackText = feedbackDictionary[characterName][assignedJob];
                return new Feedback(characterName, feedbackText, GameManager.instance.GetCurrentInterview().endCharacterSprite, job.job == GameManager.instance.GetCurrentInterview().correctJob);
            }
        }

        // Handle the case where no feedback is found
        return new Feedback(characterName, "Default feedback for " + characterName, GameManager.instance.GetCurrentInterview().endCharacterSprite, job.job == GameManager.instance.GetCurrentInterview().correctJob);
    }

    private void AddFeedback(string characterName, PossibleJobs job, string feedback)
    {
        if (!feedbackDictionary.ContainsKey(characterName))
        {
            feedbackDictionary[characterName] = new Dictionary<PossibleJobs, string>();
        }

        feedbackDictionary[characterName][job] = feedback;
    }

    public class Feedback
    {
        public string characterName;
        public string feedback;
        public Sprite characterSprite;
        public bool wasCorrect;

        public Feedback(string characterName, string feedback, Sprite characterSprite, bool wasCorrect)
        {
            this.characterName = characterName;
            this.feedback = feedback;
            this.characterSprite = characterSprite;
            this.wasCorrect = wasCorrect;
        }
    }
}