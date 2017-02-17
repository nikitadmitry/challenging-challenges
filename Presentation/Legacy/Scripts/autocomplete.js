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
                "/Challenges/TagSearch", extractLast(request.term)));
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