$(document).ready(function () {
    // обработчик клика на кнопку поиска
    $("#searchButton").click(function () {
        var name = $("#searchName").val(); // имя для поиска

        if (name.trim() === "") {
            alert("Введите имя для поиска!");
            return;
        }

        // AJAX-запрос для поиска
        $.ajax({
            // идем в контроллер Passenger и метод searchByName
            url: '/Passenger/SearchByName',
            type: 'GET',
            data: { name: name },
            success: function (tableData) {
                // обновляем содержимое таблицы с результатами
                $("#passengerTable").html(tableData);
            },
            error: function () {
                alert("Произошла ошибка при поиске пассажиров.");
            }
        });
    });
});