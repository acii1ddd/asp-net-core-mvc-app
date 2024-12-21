$(document).ready(function () {
    $("#searchButton").click(function () {
        var source = $("#source").val();
        var destination = $("#destination").val();

        if (source.trim() === "") {
            alert("Введите пункт отправления!");
            return;
        }

        if (destination.trim() === "") {
            alert("Введите пункт назначения!");
            return;
        }

        // AJAX-запрос для поиска
        $.ajax({
            url: '/Ticket/SearchMany',
            type: 'GET',
            data: {
                source: source,
                destination: destination
            },
            success: function (tableData) {
                // обновляем содержимое таблицы с результатами
                $("#ticketTable").html(tableData);
            },
            error: function () {
                alert("Произошла ошибка при поиске пассажиров.");
            }
        });
    });
});