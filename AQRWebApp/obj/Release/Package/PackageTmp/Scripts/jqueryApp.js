var imagesPath = "../Content/img/";

function GenerateOptionOfSelectTag(items, selectedValue) {
    var result = "";

    for (var i = 0; i <= items.length - 1; i++) {
        result = result + "<option value='" + items[i].Id + "' ";

        if (selectedValue != null)
            if (selectedValue == items[i].Id)
                result = result + "selected='selected'";
        
        result = result + ">" + items[i].Name + "</option>";
    }

    return result;
}

function validEmail(v) {
    var r = new RegExp("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
    return (v.match(r) == null) ? false : true;
}

function validateDecimal(value) {
    var decimal = /^[-+][0-9]+\,[0-9]+[eE][-+]?[0-9]+$/;

    if (value.match(decimal)) {
        return true;
    } else {
        return false;
    }
}


function ExportMessage(object, typeMessage, message) {
    switch (typeMessage) {
        case "error":
            if (object.hasClass("message-success"))
                object.removeClass("message-success");

            object.addclass("message - error");

            break;

        case "success":

            if ($("#lblAddError").hasClass("message - error"))
                $("#lblAddError").removeClass("message - error");

            $("#lblAddError").addclass("message-success");

            break;
    }

    $("#lblAddError").text(message);

}

function IsValidDate(day)
{
    var result = true;

    if (!day.match(/^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/)) {
        result = false;
    }

    return result;
}

//$.datepicker.setDefaults($.datepicker.regional["vi"]);

function IsInteger(n) {
    return /^[0-9]+$/.test(n);
}



