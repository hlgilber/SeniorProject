function checkFormAndSubmit2() {
    var table = document.getElementById("chaptersTable");

    var rowCount = table.rows.length;
    var valid = true;

    for (var i = 1; i < rowCount; i++) {
        var inputRow = table.rows[i];
        var cellValue = inputRow.lastChild.firstChild.value;
        var num = +(cellValue);
        if (isNaN(num) || num < 1) {
            valid = false;
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