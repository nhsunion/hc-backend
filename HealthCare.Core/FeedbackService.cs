using System;
namespace HealthCare.Core
{
    public class FeedbackService
    {
        private List<string> feedbackList = new List<string>();

        public FeedbackService()
        {
            LoadDummyData();
        }

        private void LoadDummyData()
        {
            // mock data
            feedbackList.Add("Great service, thank you!");
            feedbackList.Add("Very satisfied with the care provided.");
        }

        public void SaveFeedback(string feedback)
        {
            feedbackList.Add(feedback);
        }

        public IEnumerable<string> GetAllFeedback()
        {
            return feedbackList;
        }
    }
}



