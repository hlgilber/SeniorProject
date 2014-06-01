function checkFormAndSubmit() {
    var table = document.getElementById("chaptersTable");

    var rowCount = table.rows.length;
    var valid = true;

    var bookNameElement = document.getElementById("bookName");

    if (!bookNameElement || !bookNameElement.value)
    {
        alert("Book Title cannot be empty")
        return;
    }

    for (var i = 1; i < rowCount; i++) {
        var inputRow = table.rows[i];
        var cellValue = inputRow.lastChild.firstChild.value;
        var num = +(cellValue);
        if (isNaN(num) || num < 1) {
            valid = false;
            break;
        }
    }

    if (!valid)
    {
        alert("Invalid section number given.");
    }
    else
    {
        document.getElementById("myForm").submit();
    }
}