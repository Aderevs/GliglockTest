namespace GliglockTest
{
    public static class ModelConverter
    {
        public static DbLogic.Test TestToDbModel(Test test)
        {
            return new DbLogic.Test()
            {
                Id = test.Id,
                TestName = test.Name,
                MaxMark = test.MaxMark,
                TeacherId = test.TeacherId,
            };
        }
        /*public static DbLogic.TestQuestion QuestionToDbModel(TestQuestion question)
        {
            return new DbLogic.TestQuestion()
            {
                Id = Guid.NewGuid(),
                QuestionText = question.Question,
                TestId = question.TestId,
            };
        }*/
        public static IEnumerable<DbLogic.TestQuestion> AllQuestionsFromTestToDbModels(Test test)
        {
            foreach(var question in test.Questions)
            {
                yield return new DbLogic.TestQuestion()
                {
                    Id = question.Id,
                    QuestionText = question.Question,
                    TestId = test.Id
                };
            }
        }
        public static IEnumerable<DbLogic.AnswerOption>AllOptionsFromQuestionToDbModel(TestQuestion question)
        {
            foreach (var option in question.Options)
            {
                yield return new DbLogic.AnswerOption()
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Content = option.Content,
                    IsCorrect = option.IsCorrect,
                };
            }
        }
    }
}
