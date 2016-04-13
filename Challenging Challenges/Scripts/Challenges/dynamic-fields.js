﻿$(function () {
    $(document).on('focus', 'div.form-group-options div.input-group-option:last-child input', function () {
        if (fieldsCounter < 5) {
            var sInputGroupHtml = "<input type='text' name='Answers' autocomplete='off' class='form-control' placeholder='Answer'><span class='input-group-addon input-group-addon-remove'><span class='glyphicon glyphicon-remove'></span></span>";//$(this).parent().html();
            var sInputGroupClasses = $(this).parent().attr('class');
            $(this).parent().parent().append('<div class="' + sInputGroupClasses + '">' + sInputGroupHtml + '</div>');
        }
        fieldsCounter++;
    });

    $(document).on('click', 'div.form-group-options .input-group-addon-remove', function () {
        $(this).parent().remove();
        fieldsCounter--;
    });
});