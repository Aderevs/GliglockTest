//// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.
//let questions = @Html.Raw(questionsJson);
//let currentQuestionIndex = 0;
//let answers = [];


//function displayQuestion(index) {
//    var questionDiv = document.getElementById('question');
//    questionDiv.innerHTML = '<h3>' + questions[index].QuestionText + '</h3>';
//    questions[index].Options.forEach(function (option) {
//        let checked = '';
//        if (questions[index].HasManyAnswers) {
//            if (answers[currentQuestionIndex] && answers[currentQuestionIndex].selectedOptions.includes(option.Content)) {
//                checked = 'checked';
//            }
//            questionDiv.innerHTML += '<br> <input type="checkbox" id="' + option.Id + '" value="' + option.Content + ' name="' + questions[index].QuestionText + '" ' + checked + '/>';
//        } else {
//            if (answers[currentQuestionIndex] && answers[currentQuestionIndex].selectedOptions.includes(option)) {
//                checked = 'checked';
//            }
//            questionDiv.innerHTML += '<br> <input type="radio" id="' + option.Id + '" value="' + option.Content + ' name="' + questions[index].QuestionText + '" ' + checked + '/>';
//        }
//        questionDiv.innerHTML += ' <label for="' + option.Id + '">' + option.Content + '</label> <br> '
//    })

//}

//displayQuestion(currentQuestionIndex);
//function saveAnswer() {
//    let selectedOptions = [];
//    let currentQuestion = questions[currentQuestionIndex];

//    if (currentQuestion.HasManyAnswers) {
//        let checkboxes = document.querySelectorAll('.question input[type="checkbox"]:checked');
//        checkboxes.forEach(function (checkbox) {
//            selectedOptions.push(checkbox.value);
//        });
//    } else {
//        let radio = document.querySelector('.question input[type="radio"]:checked');
//        if (radio) {
//            selectedOptions.push(radio.value);
//        }
//    }

//    answers[currentQuestionIndex] = { questionId: currentQuestion.Id, selectedOptions: selectedOptions };
//}

//function nextQuestion() {
//    saveAnswer();
//    if (currentQuestionIndex < questions.length - 1) {
//        currentQuestionIndex++;
//        displayQuestion(currentQuestionIndex);
//    }
//}

//function previousQuestion() {
//    saveAnswer();
//    if (currentQuestionIndex > 0) {
//        currentQuestionIndex--;
//        displayQuestion(currentQuestionIndex);
//    }
//}