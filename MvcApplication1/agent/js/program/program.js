$("#Shree").focus();

$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    $("#jodiProgramButton").click(function () {
        $("#programOption").modal("hide");
        var yantraNumber = $("#jodiProgramButton").attr("name");
        var agent = $(".agent_type").val();
        if (agent != "agent_3") {
            getJodiYantraStock(yantraNumber, "jodi");
        }
        getJodiWinningYantra("jodi");
        $("#jodiModal-" + yantraNumber).modal("show");
        jQuery(".blur").keyup(function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#jodiModal-" + yantraNumber).data("class");

            $("." + className).each(function () {
                formData.push($(this).val());
                total += parseFloat($(this).val()) || 0;
            });

            var jodi_cut_amount = $("#jodiCutAmount").val();
            var jodi_final_total = total * jodi_cut_amount;

            $("#jodi_total_" + series).val(total);
            $("#jodi_final_total_" + series).val(jodi_final_total);
        });

        // jodi ALL input box

        jQuery(".jAll_" + yantraNumber).keyup(function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#jodiModal-" + yantraNumber).data("class");

            $("." + className).val($(this).val());
            $("." + className).each(function () {
                formData.push($(this).val());
                total += parseFloat($(this).val()) || 0;
            });

            var jodi_cut_amount = $("#jodiCutAmount").val();
            var jodi_final_total = total * jodi_cut_amount;

            $("#jodi_total_" + series).val(total);
            $("#jodi_final_total_" + series).val(jodi_final_total);
        });
    });
    $("#tripleProgramButton").click(function () {
        $("#programOption").modal("hide");
        var yantraNumber = $("#tripleProgramButton").attr("name");
        var agent = $(".agent_type").val();
        if (agent != "agent_3") {
            getJodiYantraStock(yantraNumber, "triple");
        }
        getJodiWinningYantra("triple");
        $("#tripleModal-" + yantraNumber).modal("show");

        jQuery(".blur").keyup(function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#tripleModal-" + yantraNumber).data("class");

            $("." + className).each(function () {
                formData.push($(this).val());
                total += parseFloat($(this).val()) || 0;
            });

            var triple_cut_amount = $("#tripleCutAmount").val();
            var triple_final_total = total * triple_cut_amount;
            $("#triple_total_" + series).val(total);
            $("#triple_final_total_" + series).val(triple_final_total);
        });
        //triple ALL input box
        $(".tAll_" + yantraNumber).on("input", function (event) {
            var qua = $(this).val();
            $(".temptTP").val("");
            $(".temptDP").val("");
            $(".temptSP").val("");

            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#tripleModal-" + yantraNumber).data("class");
            $("." + className).val($(this).val());
            $("." + className).each(function () {
                formData.push($(this).val());
                total += parseFloat($(this).val()) || 0;
            });

            var triple_cut_amount = $("#tripleCutAmount").val();
            var triple_final_total = total * triple_cut_amount;
            $("#triple_total_" + series).val(total);
            $("#triple_final_total_" + series).val(triple_final_total);
        });
        //Triple TP input box
        $(".tTP_" + yantraNumber).on("input", function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#tripleModal-" + yantraNumber).data("class");

            var qua = $(this).val();
            $("." + className).each(function () {
                if ($(this).hasClass("inputTP_" + series) == true) {
                    $(this).val(qua);
                }
                total += parseFloat($(this).val()) || 0;
            });

            var triple_cut_amount = $("#tripleCutAmount").val();
            var triple_final_total = total * triple_cut_amount;
            $("#triple_total_" + series).val(total);
            $("#triple_final_total_" + series).val(triple_final_total);
        });
        //Triple DP input box
        $(".tDP_" + yantraNumber).on("input", function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#tripleModal-" + yantraNumber).data("class");

            var qua = $(this).val();
            $("." + className).each(function () {
                if ($(this).hasClass("inputDP_" + series) == true) {
                    $(this).val(qua);
                }
                total += parseFloat($(this).val()) || 0;
            });

            var triple_cut_amount = $("#tripleCutAmount").val();
            var triple_final_total = total * triple_cut_amount;
            $("#triple_total_" + series).val(total);
            $("#triple_final_total_" + series).val(triple_final_total);
        });
        //Triple SP input box
        $(".tSP_" + yantraNumber).on("input", function (event) {
            $(this).val(
                $(this)
                    .val()
                    .replace(/[^0-9]/g, "")
            );
            var formData = new Array();
            var series = $(this).attr("series");
            var total = 0;
            var className = $("#tripleModal-" + yantraNumber).data("class");

            var qua = $(this).val();
            $("." + className).each(function () {
                if ($(this).hasClass("inputSP_" + series) == true) {
                    $(this).val(qua);
                }
                total += parseFloat($(this).val()) || 0;
            });

            var triple_cut_amount = $("#tripleCutAmount").val();
            var triple_final_total = total * triple_cut_amount;
            $("#triple_total_" + series).val(total);
            $("#triple_final_total_" + series).val(triple_final_total);
        });
    });

    $(".card-one").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 1);
        $("#tripleProgramButton").attr("name", 1);
    });
    $(".card-two").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 2);
        $("#tripleProgramButton").attr("name", 2);
    });
    $(".card-three").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 3);
        $("#tripleProgramButton").attr("name", 3);
    });
    $(".card-four").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 4);
        $("#tripleProgramButton").attr("name", 4);
    });
    $(".card-five").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 5);
        $("#tripleProgramButton").attr("name", 5);
    });
    $(".card-six").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 6);
        $("#tripleProgramButton").attr("name", 6);
    });
    $(".card-seven").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 7);
        $("#tripleProgramButton").attr("name", 7);
    });
    $(".card-eight").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 8);
        $("#tripleProgramButton").attr("name", 8);
    });
    $(".card-nine").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 9);
        $("#tripleProgramButton").attr("name", 9);
    });
    $(".card-ten").click(function () {
        $("#programOption").modal("show");
        $("#jodiProgramButton").attr("name", 0);
        $("#tripleProgramButton").attr("name", 0);
    });
});

//jodi program bet
$(".jodi-program-bet-form").on("submit", function (e) {
    var modalId = $(".modal.fade.in").attr("id");
    var formData = new Array();
    var total = 0;
    $.each($("input[type='text']"), function () {
        formData.push($(this).val());
        total += parseFloat($(this).val()) || 0;
    });
    var set_limit = $("#set_limit").val();
    var limit_reached = false;

    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each($(".input-blur").serializeArray(), function (index, value) {
            if (value.name != "_token" && value.value >= parseInt(set_limit)) {
                limit_reached = true;
            }
        });
    }
    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each(
            $(".jodi-program-bet-form").serializeArray(),
            function (index, value) {
                if (
                    value.name != "_token" &&
                    value.value >= parseInt(set_limit)
                ) {
                    limit_reached = true;
                }
            }
        );
    }
    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each(
            $(".triple-program-bet-form").serializeArray(),
            function (index, value) {
                if (
                    value.name != "_token" &&
                    value.value >= parseInt(set_limit)
                ) {
                    limit_reached = true;
                }
            }
        );
    }
    if (total == "") {
        alert("Enter Proper Jodi Yantra Quantity.");
        $("input:text:visible:first", this).focus();
        return false;
    }
    e.preventDefault();
    toastr.remove();
    toastr.options.closeButton = true;

    $.ajax({
        type: $(this).attr("method"),
        url: $(this).attr("action"),
        data: $(this).serialize(),
        headers: {
            "X-CSRF-TOKEN": "{{ csrf_token() }}",
            limitReached: limit_reached,
        },
        success: function (result) {
            if (typeof result.is_empty != "undefined") {
                alert("Ticket field is required.");
                return false;
            }
            $("[data-dismiss=modal]").trigger({ type: "click" });

            if (result.message == "low") {
                alertData("Low Balance.");
                resetAllBet();
                return false;
            }
            if (result.message == "barcode_number_empty") {
                alertData("Stock is less, Please add the stock.");
                resetAllBet();
                return false;
            }
            if (result.message == "barcode_not_set") {
                // $("form").trigger("reset");
                $("." + modalId).trigger("reset");
                alertData("No stock available, Please add the stock.");
                resetAllBet();
                return false;
            }

            if (result == 1) {
                $("#isJodiBet").val("true");
                toastr.success("Bet Saved Successfully.");
                getBalance();
                // $("form").trigger("reset");
            }
        },
        error: function (data) {},
    });
});

//triple program bet
$(".triple-program-bet-form").on("submit", function (e) {
    e.preventDefault();
    toastr.remove();
    toastr.options.closeButton = true;
    var modalId = $(".modal.fade.in").attr("id");
    var formData = new Array();
    var total = 0;
    $.each($("input[type='text']"), function () {
        formData.push($(this).val());
        total += parseFloat($(this).val()) || 0;
    });
    var set_limit = $("#set_limit").val();
    var limit_reached = false;

    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each($(".input-blur").serializeArray(), function (index, value) {
            if (value.name != "_token" && value.value >= parseInt(set_limit)) {
                limit_reached = true;
            }
        });
    }
    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each(
            $(".jodi-program-bet-form").serializeArray(),
            function (index, value) {
                if (
                    value.name != "_token" &&
                    value.value >= parseInt(set_limit)
                ) {
                    limit_reached = true;
                }
            }
        );
    }
    if (limit_reached == false && parseInt(set_limit) != 0) {
        $.each(
            $(".triple-program-bet-form").serializeArray(),
            function (index, value) {
                if (
                    value.name != "_token" &&
                    value.value >= parseInt(set_limit)
                ) {
                    limit_reached = true;
                }
            }
        );
    }
    if (total == "") {
        alert("Enter Proper Triple Yantra Quantity.");
        $("input:text:visible:first", this).focus();
        return false;
    }
    $.ajax({
        type: $(this).attr("method"),
        url: $(this).attr("action"),
        data: $(this).serialize(),
        headers: {
            "X-CSRF-TOKEN": "{{ csrf_token() }}",
            limitReached: limit_reached,
        },
        success: function (result) {
            if (typeof result.is_empty != "undefined") {
                alert("Enter Proper Triple Yantra Quantity.");
                return false;
            }
            $("[data-dismiss=modal]").trigger({ type: "click" });
            if (result.message == "low") {
                alertData("Low Balance.");
                resetAllBet();
                return false;
            }
            if (result.message == "barcode_number_empty") {
                alertData("Stock is less, Please add the stock.");
                resetAllBet();
                return false;
            }
            if (result.message == "barcode_not_set") {
                $("." + modalId).trigger("reset");
                alertData("No stock available, Please add the stock.");
                resetAllBet();
                return false;
            }

            if (result == 1) {
                $("#isTripleBet").val("true");
                toastr.success("Bet Saved Successfully.");
                getBalance();
                // $("form").trigger("reset");
            }
        },
        error: function (data) {},
    });
});
function getJodiYantraStock(yantra_number, program) {
    $.ajax({
        type: "GET",
        url: "stock-details",
        data: { yantraNumber: yantra_number, program: program },
        headers: {
            "X-CSRF-TOKEN": $('meta[name="csrf-token"]').attr("content"),
        },
        success: function (result) {
            if (result.jackpotStock != "") {
                $(".jackpot_stock").html("Stock- " + result.jackpotStock);
            }
            if (program == "program") {
                $(".program_stock").html(result.html);
            } else {
                $(".stock-item").html(result.html);
            }
        },
        error: function (response) {},
    });
}

function getJodiWinningYantra(program) {
    $.ajax({
        type: "GET",
        url: "winner-details",
        data: { program: program },
        headers: {
            "X-CSRF-TOKEN": $('meta[name="csrf-token"]').attr("content"),
        },
        beforeSend: function () {},
        success: function (result) {
            $(".jodi-winners").html(result);
        },
        error: function (response) {
            console.log("something went wrong");
        },
    });
}

$(".calculation_box").click(function (event) {
    var total = 0;
    var total_amount = 0;
    $.each($("input[type='text']"), function () {
        if (
            $(this).hasClass("tempjAll") == false &&
            $(this).hasClass("temptAll") == false &&
            $(this).hasClass("temptSP") == false &&
            $(this).hasClass("temptDP") == false &&
            $(this).hasClass("temptTP") == false
        ) {
            total += parseFloat($(this).val()) || 0;
        }
    });
    var final_total = total * 11;
    jQuery(".f_box_1").text(total);
    $(".multiply_amount").val(final_total);
    $(".total_yantra_amount").val(total);
    jQuery(".f_box_3").text(final_total);
});

$(".yantra-modal").on("shown.bs.modal", function () {
    $("input:text:visible:first", this).focus();
    $(".jodi-winners").scrollTop(830);
    $(".jodi-winners-one").scrollTop(830);
});

$(".close").click(function (event) {
    $("#Shree").focus();
});
$(".calculation_box").click(function (event) {
    $("#Shree").focus();
});

$(".blur").keyup(function (e) {
    if (e.which == 39) {
        $(this).closest("div").next().find("input").focus();
    } else if (e.which == 37) {
        $(this).closest("div").prev().find("input").focus();
    } else if (e.which == 40) {
        var name = parseInt($(this).attr("name")) + 5;
        $("input[name = '" + name + "']").focus();
    } else if (e.which == 38) {
        var name = parseInt($(this).attr("name")) - 5;
        $("input[name = '" + name + "']").focus();
    }
});

$(".func").keyup(function (event) {
    if (event.which == 39) {
        $(this).closest("div").next().find("input").focus();
    } else if (event.which == 37) {
        $(this).closest("div").prev().find("input").focus();
    }
});
