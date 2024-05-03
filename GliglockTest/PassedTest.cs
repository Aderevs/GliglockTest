using GliglockTest.DbLogic;

namespace GliglockTest
{
    public class PassedTest
    {
        private float _mark;
        public int MaxDefaultMark
        {
            get
            {
                return Test.Questions.Count(q => q.HasManyAnswers) * 2 + Test.Questions.Count(q => !q.HasManyAnswers);
            }
        }
        public float Mark
        {
            get
            {
                return _mark;
            }
            set
            {
                if (value <= Test.MaxMark && value < 0)
                {
                    _mark = (int)value;
                }
                else
                {
                    throw new InvalidOperationException("Attempt to set mark bigger than maximal value or less than 0");
                }
            }
        }
        public StudentTestTaker Student { get; set; }
        public Test Test { get; set; }
        public void CalculateMark()
        {
            float resultMark = 0;
            foreach (var question in Test.Questions)
            {
                resultMark += CalculateQuestion(question);
            }
            Mark = Test.MaxMark / MaxDefaultMark * resultMark;
        }
        private float CalculateQuestion(TestQuestion question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (question.Options == null)
            {
                throw new InvalidOperationException($"The field {nameof(question.Options)} of the argument {nameof(question)} is null");
            }
            if (question.HasStudentAnswer)
            {
                throw new InvalidOperationException($"Attempt to calculate number of points per question that doesn't executed");
            }
            float result;
            if (question.HasManyAnswers)
            {
                int numberOfCorrectAnswers = question.Options.Count(o => o.IsCorrect);
                var CorrectAnswers = question.Options.Where(o => o.IsCorrect).ToList();
                int numberOfRightAnswers = 0;
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
                        numberOfRightAnswers--;
                    }
                }
                result = numberOfRightAnswers < 0 ? 0 : numberOfRightAnswers;
                result = 2f / numberOfCorrectAnswers * result;
                return result;
            }
            else
            {
                if (question.Options[0] == question.StudentsAnswer[0])
                {
                    return 1f;
                }
                return 0f;


            }
        }

    }
}
