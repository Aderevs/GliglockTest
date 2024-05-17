//// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.

$(document).ready(function () {
    let questionIndex = 0;
    let optionIndexes = [3]
    //let answerIndex = 0;

    $('#addQuestion').click(function () {
        questionIndex++;
        optionIndexes.push(3);
        $('#questions').append(`
                    <div class="question">
                    <input type="hidden" id="${questionIndex}" class="question-index" />
                    <br />
                    <br />
                        <label for="Questions[${questionIndex}].Text"><b>Question ${questionIndex + 1}</b></label>
                        <input type="text" name="Questions[${questionIndex}].Text" class="form-control" />
                        <button type="button" class="btn btn-danger removeQuestion">Delete question</button>

                        <div class="answers">
                            <label for="Questions[${questionIndex}].AnswerOptions[0].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[0].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[0].IsCorrect" /> Correct
                            <br />

                            <label for="Questions[${questionIndex}].AnswerOptions[1].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[1].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[1].IsCorrect" /> Correct
                            <br />
                            
                            <label for="Questions[${questionIndex}].AnswerOptions[2].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[2].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[2].IsCorrect" /> Correct
                            <br />
                            
                            <label for="Questions[${questionIndex}].AnswerOptions[3].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[3].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[3].IsCorrect" /> Correct
                            <br />


                            <!-- Повторити для інших відповідей -->
                        </div>
                        <button type="button" class="btn btn-secondary addAnswer">Add option</button>
                    </div>
                `);
    });

    $('#questions').on('click', '.addAnswer', function () {
        let parent = $(this).closest('.question');
        let currentQuestionIndex = parent.find('.question-index').attr('id');
        let answerIndex = ++optionIndexes[currentQuestionIndex];
        parent.find('.answers').append(`
                    <div class="answer">
                        <label for="Questions[${questionIndex}].AnswerOptions[${answerIndex}].Content">Option</label>
                        <input type="text" name="Questions[${questionIndex}].AnswerOptions[${answerIndex}].Content" class="form-control" />
                        <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[${answerIndex}].IsCorrect" /> Correct
                        <button type="button" class="btn btn-danger removeAnswer">Delete option</button>
                    </div>
                `);
    });
    $('#questions').on('click', '.removeAnswer', function () {
        $(this).closest('.answer').remove();
    });
    $('#questions').on('click', '.removeQuestion', function () {
        $(this).closest('.question').remove();
    });
});


/*$(document).ready(function () {
    let questionIndex = 0;
    let answerIndices = {};

    $('#addQuestion').click(function () {
        questionIndex++;
        answerIndices[questionIndex] = 0;  // Ініціалізуємо індекс для відповідей нового питання
        $('#questions').append(`
                 <div class="question">
                    <br />
                    <br />
                        <label for="Questions[${questionIndex}].Text"><b>Question ${questionIndex + 1}</b></label>
                        <input type="text" name="Questions[${questionIndex}].Text" class="form-control" />
                        <button type="button" class="btn btn-danger removeQuestion">Delete question</button>

                        <div class="answers">
                            <label for="Questions[${questionIndex}].AnswerOptions[0].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[0].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[0].IsCorrect" /> Correct
                            <br />

                            <label for="Questions[${questionIndex}].AnswerOptions[1].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[1].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[1].IsCorrect" /> Correct
                            <br />
                            
                            <label for="Questions[${questionIndex}].AnswerOptions[2].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[2].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[2].IsCorrect" /> Correct
                            <br />
                            
                            <label for="Questions[${questionIndex}].AnswerOptions[3].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[3].Content" class="form-control" />
                            <input type="checkbox" name="Questions[${questionIndex}].AnswerOptions[4].IsCorrect" /> Correct
                            <br />


                            <!-- Повторити для інших відповідей -->
                        </div>
                    <button type="button" class="btn btn-secondary addAnswer">Add option</button>
                </div>
            `);
    });

    $('#questions').on('click', '.addAnswer', function () {
        let parent = $(this).closest('.question');
        let qIndex = parent.index();  // Отримуємо індекс питання в списку
        answerIndices[qIndex]++;
        let aIndex = answerIndices[qIndex];

        parent.find('.answers').append(`
                <div class="answer">
                    <label for="Questions[${qIndex}].AnswerOptions[${aIndex}].Content">Option</label>
                    <input type="text" name="Questions[${qIndex}].AnswerOptions[${aIndex}].Content" class="form-control" />
                    <input type="checkbox" name="Questions[${qIndex}].AnswerOptions[${aIndex}].IsCorrect" /> Correct
                    <button type="button" class="btn btn-danger removeAnswer">Delete option</button>
                </div>
            `);
    });

    $('#questions').on('click', '.removeAnswer', function () {
        $(this).closest('.answer').remove();
    });

    $('#questions').on('click', '.removeQuestion', function () {
        let qIndex = $(this).closest('.question').index();
        delete answerIndices[qIndex];
        $(this).closest('.question').remove();
    });
});*/