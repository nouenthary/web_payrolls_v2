$(function () {
    // get Label
    function GetLabel(text) {
        return '<option value="">-- Choose ' + text + ' -- </option>';
    }

    // get company
    $.fn.GetDropListSelect = function (object) {
        $(this).change(function () {
            let data = {
                id: $(this).val(),
                __RequestVerificationToken: object.token
            };
            if (object.removeChild === true) {
                for (let i = 0; i < object.child.length; i++) {
                    $('#' + object.child[i].key).html(GetLabel(object.child[i].value));
                }
            }
            $.ajax({
                type: "POST",
                url: object.url,
                data: data,
                success: function (response) {
                    let option = GetLabel(object.label);
                    for (let i = 0; i < response.length; i++) {
                        option += '<option value="' + response[i].key + '">' + response[i].value + '</option>';
                    }
                    $(object.supChild).html(option);
                },
                error: function () {
                    $(object.supChild).html(GetLabel(object.label));
                }
            });

        });
    }

    // // get location
    // $.fn.GetLocationByCompany = function (object) {
    //     $(this).change(function () {
    //         let data = {
    //             id: $(this).val(),
    //             __RequestVerificationToken: object.token
    //         };
    //         if(object.removeChild === true){
    //             for(let i = 0; i < object.child.length ; i++){
    //                 $('#' + object.child[i].key).html(GetLabel(object.child[i].value));
    //             }
    //         }
    //         $.ajax({
    //             type: "POST",
    //             url: "/Other/GetLocation",
    //             data: data,
    //             success: function (response) {
    //                 let option = GetLabel('Location');
    //                 for (let i = 0; i < response.length; i++) {
    //                     option += '<option value="' + response[i].key + '">' + response[i].value + '</option>';
    //                 }
    //                 $(object.supChild).html(option);
    //             },
    //             error: function () {
    //                 $(object.supChild).html(GetLabel('Location'));
    //             }
    //         });
    //     });
    // }

});