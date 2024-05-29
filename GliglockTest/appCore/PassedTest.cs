using GliglockTest.appCore.Account;
using GliglockTest.DbLogic;

namespace GliglockTest.appCore
{
    public class PassedTest
    {
        private float _mark;
        private float MaxDefaultMark => (float)(Test.Questions.Count(q => q.HasManyAnswers) * 1.5 + Test.Questions.Count(q => !q.HasManyAnswers));
        public Test Test { get; set; }
        public float Mark
        {
            get
            {
                return _mark;
            }
            set
            {
                if (value <= Test.MaxMark && value >= 0)
                {
                    _mark = (int)value;
                }
                else
                {
                    throw new InvalidOperationException("Attempt to set mark bigger than maximal value or less than 0");
                }
            }
        }
        //public StudentTestTaker? Student { get; set; }
        public Models.StudentView? Student { get; set; }
        public void CalculateMark()
        {
            float resultMark = 0;
            foreach (var question in Test.Questions)
            {
                resultMark += CalculateQuestion(question);
            }
            var res = Test.MaxMark / (float)MaxDefaultMark * resultMark;
            Mark = res;
        }
        private float CalculateQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (question.AnswerOptions == null)
            {
                throw new InvalidOperationException($"The field {nameof(question.AnswerOptions)} of the argument {nameof(question)} is null");
            }
            if (!question.HasStudentAnswer)
            {
                //throw new InvalidOperationException($"Attempt to calculate number of points per question that doesn't executed");
                return 0f;
            }
            float result;
            if (question.HasManyAnswers)
            {
                int numberOfCorrectAnswers = question.AnswerOptions.Count(o => o.IsCorrect);
                var CorrectAnswers = question.AnswerOptions.Where(o => o.IsCorrect).ToList();
                int numberOfRightAnswers = 0;
                int numberOfFaults = 0;
                foreach (var studentAnswer in question.StudentsAnswer)
                {
                    bool ifAnswerIsRight = false;
                    foreach (var correctAnswer in CorrectAnswers)
                    {
                        if (studentAnswer == correctAnswer)
                        {
                            ifAnswerIsRight = true;
                            numberOfRightAnswers++;
                            break;
                        }
                    }
                    if (!ifAnswerIsRight)
                    {
                        numberOfFaults++;
                    }
                }
                result = numberOfRightAnswers - numberOfFaults * 0.5f;
                result = result < 0 ? 0 : result;
                result = 1.5f / numberOfCorrectAnswers * result;
                return result;
            }
            else
            {
                if (question.StudentsAnswer.Contains(question.AnswerOptions.First(q => q.IsCorrect)))
                {
                    return 1f;
                }
                return 0f;


            }
        }

    }
}
