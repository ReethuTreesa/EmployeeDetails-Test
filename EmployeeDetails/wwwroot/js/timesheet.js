
//Bind Project dropdownlist
$(".clientDropdown").change(function () {
    var clientId = $(this).val();
    $.getJSON("/Home/GetProjectList", { ClientId: clientId }, function (data) {
        var item = "";
        $(".projectDropdown").find('option').not(':first').remove();
        item += '<option value="">Select Project</option>'
        $.each(data, function (i, project) {
            item += '<option value="' + project.value + '">' + project.text + '</option>'
        });
        $(".projectDropdown").html(item);
    });
});

//$('.delete').click(function () {
//    var TimeSheetDet_Id = $(this).attr("value");
//    var selected = $(this).parents("tr").addClass("selected");

//    if (confirm("Do you really want to delete item?")) {
//        axios({
//            method: 'post',
//            url: '/Timesheet/Delete',
//            data: {
//                "TimeSheetDet_Id": TimeSheetDet_Id,
//            }
//        }).then(function (res) {
//            var message = res.data.objResultSet.errorMsgVal;

//            if (res.data.objResultSet.errorCodeVal === '0') {
//                table.row(".selected").remove().draw(false); // Delete row from view
//                Swal.fire(message);
//            } else {
//                alert("Something went wrong: ".concat(message));
//            }
//        }).catch(function (err) {
//            return alert("Something went wrong: ".concat(err));
//        });
//    }
//});

$(function () {
    $('#lnkDelete').click(function () {
        alert('click');
        var $letter = $(this);
        $.post(this.href, function (result) {
            alert('post');
            $letter.toggleClass('selected');
        });
    });
});

//$(".delete").unbind().click(function () {
//    var TimeSheetHeadId = $(this).val().TimeSheetHead_Id;
//    alert(TimeSheetHeadId);
//    if (confirm("Are you sure you want to delete?")) {
//        var selected = $(this).parents("tr").addClass("selected");
//        $.ajax(
//            {
//                url: '/Timesheet/Delete',
//                method: "POST",
//                data: { "TimeSheetHead_Id": TimeSheetHeadId }
//            })
//            .done(function (data) {
//                if (data == 1) {
//                    table.row(".selected").remove().draw(false); // Delete row from view
//                }
//                else {
//                    Alert("Something went wrong...")
//                }
//            });
//    }
//});