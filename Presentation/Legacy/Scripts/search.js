$(function () {
    $('#search-btn').on('click', function (event) {
        event.preventDefault();
        $('#search-box').addClass('open');
        $('#search-box > form > input[type="search"]').focus();
    });

    $('#search-box, #search-box button.close').on('click keyup', function (event) {
        if (event.target == this || event.target.className == 'close' || event.keyCode == 27) {
            $(this).removeClass('open');
        }
    });
});

$(function () {
    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
    }

    $("#tags").autocomplete({
        minLength: 0,
        source: function (request, response) {
            response($.ui.autocomplete.filter(
                '@Url.Action("TagSearch", "Challenges")', extractLast(request.term)));
        },
        focus: function () {
            return false;
        },
        select: function (event, ui) {
            var terms = split(this.value);
            terms.pop();
            terms.push(ui.item.value);
            terms.push("");
            this.value = terms.join(", ");
            return false;
        }
    });
});