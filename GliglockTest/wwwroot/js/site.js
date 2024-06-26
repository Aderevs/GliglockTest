﻿//// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.


function generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}


$(document).ready(function () {

    const testUuid = generateGUID();
    const questionUuid = generateGUID();

    const answerUuids = [generateGUID(), generateGUID(), generateGUID(), generateGUID()];
    const testIdInput = document.getElementById('TestId');
    testIdInput.setAttribute('value', testUuid);

    const questionIdInput = document.getElementById('QuestionId');
    questionIdInput.setAttribute('value', questionUuid);

    const answerIdInputs = document.getElementsByClassName('AnswerId');
    for (let i = 0; i < 4; i++) {
        answerIdInputs[i].setAttribute('value', answerUuids[i]);
    }

    let questionIndex = 0;
    let optionIndexes = [3]

    $('#addQuestion').click(function () {
        questionIndex++;
        optionIndexes.push(3);
        $('#questions').append(`
                    <div class="question">
                    <input type="hidden" id="${questionIndex}" class="question-index" />
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="Questions[${questionIndex}].Text"><b>Question ${questionIndex + 1}</b></label>
                        <input type="text" name="Questions[${questionIndex}].Text" class="form-control" />
                        <input type="hidden" name="Questions[${questionIndex}].Id" value="${generateGUID()}" />
                        <!-- <button type="button" class="btn btn-danger removeQuestion">Delete question</button> -->
                        <br />
                        <br />
                        <label for="Questions[0].Image">Add image (.jpg only)</label>
                        <input type="file" id="Questions[0].Image" name="Questions[${questionIndex}].Image" class="form-control" accept=".jpg" />
                    </div>

                        <div class="answers">
                            <div class="answer">
                                <div class="form-group">
                                    <label for="Questions[${questionIndex}].AnswerOptions[0].Content">Option</label>
                                    <input type="text" name="Questions[${questionIndex}].AnswerOptions[0].Content" class="form-control" />
                                    <label for="Questions[${questionIndex}].AnswerOptions[0].IsCorrect">
                                        <input type="checkbox" id="Questions[${questionIndex}].AnswerOptions[0].IsCorrect" name="Questions[${questionIndex}].AnswerOptions[0].IsCorrect" value="true"/> Correct
                                    </label>
                                    <input type="hidden" name="Questions[${questionIndex}].AnswerOptions[0].Id" value="${generateGUID()}" />
                                </div>
                            </div>
                            <br />

                            <div class="answer">
                                <div class="form-group">
                                    <label for="Questions[${questionIndex}].AnswerOptions[1].Content">Option</label>
                                    <input type="text" name="Questions[${questionIndex}].AnswerOptions[1].Content" class="form-control" />
                                    <label for="Questions[${questionIndex}].AnswerOptions[1].IsCorrect">
                                        <input type="checkbox" id="Questions[${questionIndex}].AnswerOptions[1].IsCorrect" name="Questions[${questionIndex}].AnswerOptions[1].IsCorrect" value="true"/> Correct
                                    </label>
                                    <input type="hidden" name="Questions[${questionIndex}].AnswerOptions[1].Id" value="${generateGUID()}" />
                                </div>
                            </div>
                                <br />
                            
                            <div class="answer">
                                <div class="form-group">
                                    <label for="Questions[${questionIndex}].AnswerOptions[2].Content">Option</label>
                                    <input type="text" name="Questions[${questionIndex}].AnswerOptions[2].Content" class="form-control" />
                                    <label for="Questions[${questionIndex}].AnswerOptions[2].IsCorrect">
                                        <input type="checkbox" id="Questions[${questionIndex}].AnswerOptions[2].IsCorrect" name="Questions[${questionIndex}].AnswerOptions[2].IsCorrect" value="true"/> Correct
                                    </label>
                                    <input type="hidden" name="Questions[${questionIndex}].AnswerOptions[2].Id" value="${generateGUID()}" />
                                </div>
                                </div>
                                <br />
                            
                            <div class="answer">
                                <div class="form-group">
                                    <label for="Questions[${questionIndex}].AnswerOptions[3].Content">Option</label>
                                    <input type="text" name="Questions[${questionIndex}].AnswerOptions[3].Content" class="form-control" />
                                    <label for="Questions[${questionIndex}].AnswerOptions[3].IsCorrect">
                                        <input type="checkbox" id="Questions[${questionIndex}].AnswerOptions[3].IsCorrect" name="Questions[${questionIndex}].AnswerOptions[3].IsCorrect" value="true"/> Correct
                                    <input type="hidden" name="Questions[${questionIndex}].AnswerOptions[3].Id" value="${generateGUID()}" />
                                    </label>
                                </div>
                            </div>
                                <br />


                            <!-- Повторити для інших відповідей -->
                        </div>
                        <button type="button" class="btn btn-secondary addAnswer">Add option</button>
                        <button type="button" class="btn btn-danger removeAnswer">Delete option</button>
                    </div>
        `);
    });

    $('#questions').on('click', '.addAnswer', function () {
        let parent = $(this).closest('.question');
        let currentQuestionIndex = parent.find('.question-index').attr('id');
        let answerIndex = ++optionIndexes[currentQuestionIndex];
        parent.find('.answers').append(`
                    <div class="answer">
                        <div class="form-group">
                            <label for="Questions[${questionIndex}].AnswerOptions[${answerIndex}].Content">Option</label>
                            <input type="text" name="Questions[${questionIndex}].AnswerOptions[${answerIndex}].Content" class="form-control" />
                            <label for="Questions[${questionIndex}].AnswerOptions[${answerIndex}].IsCorrect">
                                <input type="checkbox" id="Questions[${questionIndex}].AnswerOptions[${answerIndex}].IsCorrect" name="Questions[${questionIndex}].AnswerOptions[${answerIndex}].IsCorrect" value="true"/> Correct
                            </label>
                            
                            <input type="hidden" name="Questions[${questionIndex}].AnswerOptions[${answerIndex}].Id" value="${generateGUID()}" />
                        </div>
                    </div>
                `);
    });
    $('#questions').on('click', '.removeAnswer', function () {
        let $parentElements = $('.question');
        let currentQuestionIndex = $parentElements.find('.question-index').attr('id');
        optionIndexes[currentQuestionIndex]--;
        $parentElements.each(function () {
            let $innerElement = $(this).find('.answers');

            if ($innerElement.length > 0) {
                let $elements = $innerElement.find('.answer');
                if ($elements.length > 4) {
                    $elements.last().remove();
                }
            }
        });
    });
    $('#whole-form').on('click', '#removeQuestion', function () {
        let lastQuestion = $('#questions').children('.question').last();
        if ($('#questions').children('.question').length > 1) {
            lastQuestion.remove();
            questionIndex--;
        }
    });
})

