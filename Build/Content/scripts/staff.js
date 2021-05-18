$(function () {
    // load data to gridview
    $('#show_staff').load('/Staff/GetTable', {
        pageSize: $('#pageSize').val(),
        bid: $('#ddlBoss').val(),
        cid: 0,
        lid: 0,
        did: 0,
        pid: 0,
        salType: "",
        status: "",
        textSearch: ""
    });

    // search
    $('#btnSearch').click(function () {
        $('#show_staff').load('/Staff/GetTable', {
            pageSize: $('#pageSize').val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            did: $('#ddlDepartment').val(),
            pid: $('#ddlPosition').val(),
            salType: $('#ddlSalaryType').val(),
            status: $('#ddlStatus').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });

    $(document).on('click', '#show_staff a', function (e) {
        try {
            var link = $(this).attr("href").split('=');
            $('#show_staff').load('/Staff/GetTable', {
                page: link[1],
                pageSize: $('#pageSize').val(),
                bid: $('#ddlBoss').val(),
                cid: $('#ddlCompany').val(),
                lid: $('#ddlLocation').val(),
                did: $('#ddlDepartment').val(),
                pid: $('#ddlPosition').val(),
                salType: $('#ddlSalaryType').val(),
                status: $('#ddlStatus').val(),
                textSearch: $('#txbTextSearch').val()
            });
            e.preventDefault()
        } catch (ex) { }
    });

    $(document).on('change', '#show_staff #pageSize', function () {
        $('#show_staff').load('/Staff/GetTable', {
            pageSize: $(this).val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            did: $('#ddlDepartment').val(),
            pid: $('#ddlPosition').val(),
            salType: $('#ddlSalaryType').val(),
            status: $('#ddlStatus').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });
    // New Code

    $(document).on('click', '#show_staff #btn_export_staff', function () {
        $('#table_staff').csvExport();
    });

    // onchange page size
    $("#pageSize").change(function () {
        $("#form1").submit();
    });

    //filter dropdown list
    // get company by boss 
    $('#ddlBoss').change(function () {
        let data = {
            boss_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetCompany",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Companies -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Comp_Id + '">' + response[i].Comp_Name + '</option>';
                }
                $('#ddlCompany').html(option);
            },
            error: function () {
                $('#ddlCompany').html('<option value="0">-- All Companies --</option>');
                $('#ddlLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    // get location by boss
    $('#ddlCompany').change(function () {
        let data = {
            comp_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetLocation",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Locations -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Location_Id + '">' + response[i].Loc_Name + '</option>';
                }
                $('#ddlLocation').html(option);
            },
            error: function () {
                $('#ddlLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    // get department by location id
    $('#ddlLocation').change(function () {
        let data = {
            loc_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetDepartment",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Departments -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Depart_Id + '">' + response[i].Deppart_Name + '</option>';
                }
                $('#ddlDepartment').html(option);
            },
            error: function () {
                $('#ddlDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlDepartment').change(function () {
        let data = {
            dept_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetPosition",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Positions -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Pos_Id + '">' + response[i].Pos_Name + '</option>';
                }
                $('#ddlPosition').html(option);
            },
            error: function () {
                $('#ddlPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    // filter at add form
    $('#ddlAddBoss').change(function () {
        let data = {
            boss_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetCompany",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Companies -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Comp_Id + '">' + response[i].Comp_Name + '</option>';
                }
                $('#ddlAddCompany').html(option);
            },
            error: function () {
                $('#ddlAddCompany').html('<option value="0">-- All Companies --</option>');
                $('#ddlAddLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlAddDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlAddPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlAddCompany').change(function () {
        let data = {
            comp_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetLocation",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Locations -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Location_Id + '">' + response[i].Loc_Name + '</option>';
                }
                $('#ddlAddLocation').html(option);
            },
            error: function () {
                $('#ddlAddLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlAddDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlAddPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlAddLocation').change(function () {
        let data = {
            loc_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetDepartment",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Departments -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Depart_Id + '">' + response[i].Deppart_Name + '</option>';
                }
                $('#ddlAddDepartment').html(option);
            },
            error: function () {
                $('#ddlAddDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlAddPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlAddDepartment').change(function () {
        let data = {
            dept_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetPosition",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Positions -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Pos_Id + '">' + response[i].Pos_Name + '</option>';
                }
                $('#ddlAddPosition').html(option);
            },
            error: function () {
                $('#ddlAddPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });
    // end

    // filter at edit form
    $('#ddlEditBoss').change(function () {
        let data = {
            boss_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetCompany",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Companies -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Comp_Id + '">' + response[i].Comp_Name + '</option>';
                }
                $('#ddlEditCompany').html(option);
            },
            error: function () {
                $('#ddlEditCompany').html('<option value="0">-- All Companies --</option>');
                $('#ddlEditLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlEditDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlEditPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlEditCompany').change(function () {
        let data = {
            comp_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetLocation",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Locations -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Location_Id + '">' + response[i].Loc_Name + '</option>';
                }
                $('#ddlEditLocation').html(option);
            },
            error: function () {
                $('#ddlEditLocation').html('<option value="0">-- All Locations --</option>');
                $('#ddlEditDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlEditPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlEditLocation').change(function () {
        let data = {
            loc_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetDepartment",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Departments -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Depart_Id + '">' + response[i].Deppart_Name + '</option>';
                }
                $('#ddlEditDepartment').html(option);
            },
            error: function () {
                $('#ddlEditDepartment').html('<option value="0">-- All Departments --</option>');
                $('#ddlEditPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });

    $('#ddlEditDepartment').change(function () {
        let data = {
            dept_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetPosition",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Positions -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Pos_Id + '">' + response[i].Pos_Name + '</option>';
                }
                $('#ddlEditPosition').html(option);
            },
            error: function () {
                $('#ddlEditPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    });
    // end

    // end

    // block add and edit record
    //open form save or edit
    $(document).on('click', '#show_staff #btn_modal_staff', function () {
        ClearContent("ddlAddBoss", "ddlAddCompany", "ddlAddLocation", "ddlAddDepartment", "ddlAddPosition", "txbStaffName",
            "ddlSex", "ddlMarryStatus", "ddlDOB", "txbNationality", "txbWife", "txbSon", "txbDaughter", "txbPhone", "txbBankName",
            "txbBankNumber", "txbAddress", "txbIdentificationCard", "txbStaffIDCard", "txbSerialCard", "ddlStatus", "ddlNoScan",
            "txbJoinDate", "txbUsername", "txbPassword", "ddlSalaryType", "txbSalary", "txbCardPhone", "txbGym",
            "txbAccommodation", "txbFood", "txbInsurance", "txbMoneyPrim", "txbTravel", "txbGasoline", "txbOTUnitPrice",
            "txbWorkHourTarget", "txbRelaxHour", "txbDayOff", "txbAnnaulLeave", "txbPublicHoliday", "txbSick", "txbUPL",
            "txbAB", "txbCM", "txbNumYear", "txbMonth", "photo-view-response", "photo-id-card-view-response");

        $('#modal_staff_create').modal("show");
    });

    //end

    // other functions
    //ClearAllValidation("ddlAddBoss", "ddlAddCompany", "ddlAddLocation", "ddlAddDepartment", "ddlAddPosition", "txbStaffName",
        //"ddlSex", "ddlMarryStatus", "ddlDOB", "txbNationality", "txbWife", "txbSon", "txbDaughter", "txbPhone",
        //"txbBankName", "txbBankNumber", "txbAddress", "txbIdentificationCard", "txbStaffIDCard", "txbSerialCard",
        //"ddlStatus", "ddlNoScan", "txbJoinDate", "txbUsername", "txbPassword", "ddlSalaryType", "txbSalary", "txbCardPhone",
        //"txbGym", "txbAccommodation", "txbFood", "txbInsurance", "txbMoneyPrim", "txbTravel", "txbGasoline", "txbOTUnitPrice",
        //"txbWorkHourTarget", "txbRelaxHour", "txbDayOff", "txbAnnaulLeave", "txbPublicHoliday", "txbSick", "txbUPL", "txbAB",
        //"txbCM");

    showPreviousPicture("photo", "photo-view-response", "name-photo");
    showPreviousPicture("photo-id-card", "photo-id-card-view-response", "name-photo-id-card");

    showPreviousPicture("photo-Edit", "photo-view-response-Edit", "name-photo-Edit");
    showPreviousPicture("photo-id-card-Edit", "photo-id-card-view-response-Edit", "name-photo-id-card-Edit");

    $('.datetimepicker').datetimepicker({
        format: 'YYYY-MM-DD'
    });

    function showPreviousPicture(photo, photo_view_response, photo_name) {
        $('#' + photo).change(function () {

            var fileUpload = $('#' + photo).get(0);
            var files = fileUpload.files;

            var MB = 1024 * 1024;
            var filesizes = fileUpload.files[0].size / MB;
            var sizeMB = parseInt(filesizes, 10);
            if (sizeMB > 5) {
                alert(' File size is too big : ' + sizeMB + ' MB. \n We allow file size : 5 MB Only.');
                // Clear file upload value
                $('#photo-view-response').attr('src', '/Content/images/no-image.jpg');
                document.getElementById("photo").value = '';
                Return;
            }

            var fileData = new FormData();

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $.ajax({
                type: "POST",
                url: "/Staff/Upload",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (response) {
                    if (response.message) {
                        $('#' + photo_view_response).attr('src', '/Content/Uploads/Staff_Photoes/' + response.message);
                        $('#' + photo_name).val(response.message);
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        });
    }

    function clearValidation(htmlTageName) {
        $('#' + htmlTageName).change(function () {
            $('#' + htmlTageName).attr('class', 'form-control');
        });
    }

    function protectdControl(control) {
        $(control).bind("cut copy paste drag drop", function (e) {
            e.preventDefault();
        });
    }

    function validateControl(control_id) {
        $(control_id).keypress(function (event) {
            var ew = event.which;
            if (ew == 32)
                return true;
            if (48 <= ew && ew <= 57)
                return true;
            if (65 <= ew && ew <= 90)
                return true;
            if (97 <= ew && ew <= 122)
                return true;
            return false;
        });
    }

    function ClearContent(ddlBoss, ddlCompany, ddlLocation, ddlDepartment, ddlPosition, txbStaffName, ddlSex, ddlFamilyStatus, txbDOB, txbNationality, txbWife, txbSon, txbDaughter,
        txbPhone, txbBankName, txbBankNumber, txbAddress, txbIDCard, txbStaffIDCard, txbSerialCard, ddlStatus, ddlNoScan, txbJoinDate,
        txbUsername, txbPassword, ddlSalaryType, txbSalary, txbCardPhone, txbGym, txbAccommodation, txbFood, txbInsurance, txbMoneyPrim,
        txbTravel, txbGasoline, txbOTUnitPrice, txbWorkHourTarget, txbRelaxHour, txbDayOff, txbAnnaulLeave, txbPublicHoliday, txbSick,
        txbUPL, txbAB, txbCM, imgStaffPhoto, imgStaffIDCard) {

        $('#' + ddlBoss).val(0); $('#' + ddlCompany).val(0); $('#' + ddlLocation).val(0); $('#' + ddlDepartment).val(0);
        $('#' + ddlPosition).val(0); $('#' + txbStaffName).val(''); $('#' + ddlSex).val(0); $('#' + ddlFamilyStatus).val(0);
        $('#' + txbDOB).val(''); $('#' + txbNationality).val(''); $('#' + txbWife).val('0'); $('#' + txbSon).val('0');
        $('#' + txbDaughter).val('0'); $('#' + txbPhone).val(''); $('#' + txbBankName).val(''); $('#' + txbBankNumber).val('');
        $('#' + txbAddress).val(''); $('#' + txbIDCard).val(''); $('#' + txbStaffIDCard).val(''); $('#' + txbSerialCard).val('');
        $('#' + ddlStatus).val('Enable'); $('#' + ddlNoScan).val('Scan'); $('#' + txbJoinDate).val(''); $('#' + txbUsername).val('');
        $('#' + txbPassword).val(''); $('#' + ddlSalaryType).val('Daily'); $('#' + txbSalary).val('0'); $('#' + txbCardPhone).val('0');
        $('#' + txbGym).val('0'); $('#' + txbAccommodation).val('0'); $('#' + txbFood).val('0'); $('#' + txbInsurance).val('0');
        $('#' + txbMoneyPrim).val('0'); $('#' + txbTravel).val('0'); $('#' + txbGasoline).val('0'); $('#' + txbOTUnitPrice).val('0');
        $('#' + txbWorkHourTarget).val('0'); $('#' + txbRelaxHour).val('0'); $('#' + txbDayOff).val('0'); $('#' + txbAnnaulLeave).val('0');
        $('#' + txbPublicHoliday).val('0'); $('#' + txbSick).val('0'); $('#' + txbUPL).val('0'); $('#' + txbAB).val('0');
        $('#' + txbCM).val('0'); $('#' + imgStaffPhoto).attr('src', '/Content/images/no-image.jpg');
        $('#' + imgStaffIDCard).attr('src', '/Content/images/no-image.jpg');
    }

    function ClearAllValidation(ddlPosition, txbStaffName, ddlSex, ddlFamilyStatus, txbDOB, txbNationality, txbWife, txbSon, txbDaughter,
        txbPhone, txbBankName, txbBankNumber, txbAddress, txbIDCard, txbStaffIDCard, txbSerialCard, ddlStatus, ddlNoScan, txbJoinDate,
        txbUsername, txbPassword, ddlSalaryType, txbSalary, txbCardPhone, txbGym, txbAccommodation, txbFood, txbInsurance, txbMoneyPrim,
        txbTravel, txbGasoline, txbOTUnitPrice, txbWorkHourTarget, txbRelaxHour, txbDayOff, txbAnnaulLeave, txbPublicHoliday, txbSick,
        txbUPL, txbAB, txbCM) {

        clearValidation(ddlPosition); clearValidation(txbStaffName); clearValidation(ddlSex); clearValidation(ddlFamilyStatus);
        clearValidation(txbDOB); clearValidation(txbNationality); clearValidation(txbWife); clearValidation(txbSon);
        clearValidation(txbDaughter); clearValidation(txbPhone); clearValidation(txbBankName); clearValidation(txbBankNumber);
        clearValidation(txbAddress); clearValidation(txbIDCard); clearValidation(txbStaffIDCard); clearValidation(txbSerialCard);
        clearValidation(ddlStatus); clearValidation(ddlNoScan); clearValidation(txbJoinDate); clearValidation(txbUsername);
        clearValidation(txbPassword); clearValidation(ddlSalaryType); clearValidation(txbSalary); clearValidation(txbCardPhone);
        clearValidation(txbGym); clearValidation(txbAccommodation); clearValidation(txbFood); clearValidation(txbInsurance);
        clearValidation(txbMoneyPrim); clearValidation(txbTravel); clearValidation(txbGasoline); clearValidation(txbOTUnitPrice);
        clearValidation(txbWorkHourTarget); clearValidation(txbRelaxHour); clearValidation(txbDayOff); clearValidation(txbAnnaulLeave);
        clearValidation(txbPublicHoliday); clearValidation(txbSick); clearValidation(txbUPL); clearValidation(txbAB);
        clearValidation(txbCM); 

        protectdControl(ddlPosition); protectdControl(txbStaffName); protectdControl(ddlSex); protectdControl(ddlFamilyStatus);
        protectdControl(txbDOB); protectdControl(txbNationality); protectdControl(txbWife); protectdControl(txbSon);
        protectdControl(txbDaughter); protectdControl(txbPhone); protectdControl(txbBankName); protectdControl(txbBankNumber);
        protectdControl(txbAddress); protectdControl(txbIDCard); protectdControl(txbStaffIDCard); protectdControl(txbSerialCard);
        protectdControl(ddlStatus); protectdControl(ddlNoScan); protectdControl(txbJoinDate); protectdControl(txbUsername);
        protectdControl(txbPassword); protectdControl(ddlSalaryType); protectdControl(txbSalary); protectdControl(txbCardPhone);
        protectdControl(txbGym); protectdControl(txbAccommodation); protectdControl(txbFood); protectdControl(txbInsurance);
        protectdControl(txbMoneyPrim); protectdControl(txbTravel); protectdControl(txbGasoline); protectdControl(txbOTUnitPrice);
        protectdControl(txbWorkHourTarget); protectdControl(txbRelaxHour); protectdControl(txbDayOff); protectdControl(txbAnnaulLeave);
        protectdControl(txbPublicHoliday); protectdControl(txbSick); protectdControl(txbUPL); protectdControl(txbAB);
        protectdControl(txbCM); 

        validateControl(ddlPosition); validateControl(txbStaffName); validateControl(ddlSex); validateControl(ddlFamilyStatus);
        validateControl(txbDOB); validateControl(txbNationality); validateControl(txbWife); validateControl(txbSon);
        validateControl(txbDaughter); validateControl(txbPhone); validateControl(txbBankName); validateControl(txbBankNumber);
        validateControl(txbAddress); validateControl(txbIDCard); validateControl(txbStaffIDCard); validateControl(txbSerialCard);
        validateControl(ddlStatus); validateControl(ddlNoScan); validateControl(txbJoinDate); validateControl(txbUsername);
        validateControl(txbPassword); validateControl(ddlSalaryType); validateControl(txbSalary); validateControl(txbCardPhone);
        validateControl(txbGym); validateControl(txbAccommodation); validateControl(txbFood); validateControl(txbInsurance);
        validateControl(txbMoneyPrim); validateControl(txbTravel); validateControl(txbGasoline); validateControl(txbOTUnitPrice);
        validateControl(txbWorkHourTarget); validateControl(txbRelaxHour); validateControl(txbDayOff); validateControl(txbAnnaulLeave);
        validateControl(txbPublicHoliday); validateControl(txbSick); validateControl(txbUPL); validateControl(txbAB);
        validateControl(txbCM); 

    }

    // block populate data to dropdownlist in form edit
    function GetCompanyByBossId(boos_id, comp_id) {
        let data = {
            boss_id: boos_id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetCompany",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Companies -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Comp_Id + '">' + response[i].Comp_Name + '</option>';
                }
                $('#ddlEditCompany').html(option);
                $('#ddlEditCompany').val(comp_id);
            },
            error: function () {
                $('#ddlEditCompany').html('<option value="0">-- All Companies --</option>');
                //$('#ddlLocation').html('<option value="0">-- All Locations --</option>');
            }
        });
    }

    function GetLocationByCompanyId(comp_id, location_id) {
        let data = {
            comp_id: comp_id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetLocation",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Locations -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Location_Id + '">' + response[i].Loc_Name + '</option>';
                }
                $('#ddlEditLocation').html(option);
                $('#ddlEditLocation').val(location_id);
            },
            error: function () {
                $('#ddlEditLocation').html('<option value="0">-- All Locations 111 --</option>');
            }
        });
    }

    function GetDepartmentByLocationId(location_id, department_id) {
        let data = {
            loc_id: location_id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetDepartment",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Departments -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Depart_Id + '">' + response[i].Deppart_Name + '</option>';
                }
                $('#ddlEditDepartment').html(option);
                $('#ddlEditDepartment').val(department_id);
            },
            error: function () {
                $('#ddlEditDepartment').html('<option value="0">-- All Departments --</option>');
            }
        });
    }

    function GetPositionByDepartmentId(department_id, pos_id) {
        let data = {
            dept_id: department_id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Staff/GetPosition",
            data: data,
            success: function (response) {
                let option = '<option value="0">-- All Positions -- </option>';
                for (let i = 0; i < response.length; i++) {
                    option += '<option value="' + response[i].PK_Pos_Id + '">' + response[i].Pos_Name + '</option>';
                }
                $('#ddlEditPosition').html(option);
                $('#ddlEditPosition').val(pos_id);
            },
            error: function () {
                $('#ddlEditPosition').html('<option value="0">-- All Positions --</option>');
            }
        });
    }

    // end

    // Save Staff
    $('#btn_save_staff').click(function () {
        
        var date = new Date();
        // varialable for tblStaff
        var FK_Pos_Id = $('#ddlAddPosition').val();
        var Staff_Name = $('#txbStaffName').val();
        var Sex = $('#ddlSex').val();
        var Staff_DOB = $('#ddlDOB').val();
        var Marry_Status = $('#ddlMarryStatus').val();
        var Nationality = $('#txbNationality').val();
        var Wife = $('#txbWife').val();
        var Son = $('#txbSon').val();
        var Daughter = $('#txbDaughter').val();
        var Tell = $('#txbPhone').val();
        var Account_Name = $('#txbBankName').val();
        var Account_Number = $('#txbBankNumber').val();
        var Address = $('#txbAddress').val();
        var Identity_Card = $('#txbIdentificationCard').val();
        var Identify_Picture = $('#name-photo-id-card').val();
        var Staff_Id_Card = $('#txbStaffIDCard').val();
        var Serail_Card = $('#txbSerialCard').val();
        var Staff_Status = $('#ddlStatus').val();
        var Join_date = $('#txbJoinDate').val();
        var Photo = $('#name-photo').val();
        var UserName = $('#txbUsername').val();
        var Password = $('#txbPassword').val();
        var No_Scan = $('#ddlNoScan').val();

        //variable for tblRoster
        var Num_Year = date.getFullYear();
        var Num_Month = date.getMonth() + 1;

        //variable for tblSalaryType
        var Sal_Type = $('#ddlSalaryType').val();
        var Salary = $('#txbSalary').val();
        var Card_Phone = $('#txbCardPhone').val();
        var Gym = $('#txbGym').val();
        var Accommodation = $('#txbAccommodation').val();
        var Food = $('#txbFood').val();
        var insurance = $('#txbInsurance').val();
        var Money_Prim = $('#txbMoneyPrim').val();
        var Travel = $('#txbTravel').val();
        var Gasoline = $('#txbGasoline').val();
        var OT_Unit_Price = $('#txbOTUnitPrice').val();
        var Work_Hour_Taget = $('#txbWorkHourTarget').val();
        var Relax_Hour = $('#txbRelaxHour').val();
        var Off_Day = $('#txbDayOff').val();
        var AL_Day = $('#txbAnnaulLeave').val();
        var PH_Day = $('#txbPublicHoliday').val();
        var Sick = $('#txbSick').val();
        var UPL = $('#txbUPL').val();
        var AB = $('#txbAB').val();
        var CM = $('#txbCM').val();

        if (FK_Pos_Id == 0 | Staff_Name == "" | Sex == 0 | Staff_DOB == "" | Marry_Status == 0 | Nationality == "" |
            Wife == "" | Son == "" | Daughter == "" | Tell == "" | Account_Name == "" | Account_Number == "" | Address == "" |
            Identity_Card == "" | Staff_Id_Card == "" | Serail_Card == "" | Join_date == "" | UserName == "" | Password == "") {

            swal("Please fill your entry data to all the red fields.");
            //$('#ms_required').text("Please fill your entry data to all the red fields.");
        } else if (Password.length < 8) {

            swal("Password should be at least 8 characters.");
            //$('#ms_required').text("Password should be at least 8 characters.");
        } else {

            const data = {
                FK_Pos_Id: FK_Pos_Id,
                Staff_Name: Staff_Name,
                Sex: Sex,
                Staff_DOB: Staff_DOB,
                Marry_Status: Marry_Status,
                Nationality: Nationality,
                Wife: Wife,
                Son: Son,
                Daughter: Daughter,
                Tell: Tell,
                Account_Name: Account_Name,
                Account_Number: Account_Number,
                Address: Address,
                Identity_Card: Identity_Card,
                Identify_Picture: Identify_Picture,
                Staff_Id_Card: Staff_Id_Card,
                Serail_Card: Serail_Card,
                Staff_Status: Staff_Status,
                Join_date: Join_date,
                Photo: Photo,
                UserName: UserName,
                Password: Password,
                No_Scan: No_Scan,
                Num_Year: Num_Year,
                Num_Month: Num_Month,
                Sal_Type: Sal_Type,
                Salary: Salary,
                Card_Phone: Card_Phone,
                Gym: Gym,
                Accommodation: Accommodation,
                Food: Food,
                insurance: insurance,
                Money_Prim: Money_Prim,
                Travel: Travel,
                Gasoline: Gasoline,
                OT_Unit_Price: OT_Unit_Price,
                Work_Hour_Taget: Work_Hour_Taget,
                Relax_Hour: Relax_Hour,
                Off_Day: Off_Day,
                AL_Day: AL_Day,
                PH_Day: PH_Day,
                Sick: Sick,
                UPL: UPL,
                AB: AB,
                CM: CM
            };

            var url = "/Staff/AddStaff";
            $.post(url, data, function (respone) {
                swal(respone);
                setTimeout(function () {
                    window.location.reload();
                }, 1000)
            });
        }
    });

    $('#btn_update_staff').click(function () {
        // varialable for tblStaff
        var FK_Pos_Id = $('#ddlEditPosition').val();
        var Staff_Name = $('#txbEditStaffName').val();
        var Sex = $('#ddlEditSex').val();
        var Staff_DOB = $('#ddlEditDOB').val();
        var Marry_Status = $('#ddlEditMarryStatus').val();
        var Nationality = $('#txbEditNationality').val();
        var Wife = $('#txbEditWife').val();
        var Son = $('#txbEditSon').val();
        var Daughter = $('#txbEditDaughter').val();
        var Tell = $('#txbEditPhone').val();
        var Account_Name = $('#txbEditBankName').val();
        var Account_Number = $('#txbEditBankNumber').val();
        var Address = $('#txbEditAddress').val();
        var Identity_Card = $('#txbEditIdentificationCard').val();
        var Identify_Picture = $('#name-photo-id-card-Edit').val();
        var Staff_Id_Card = $('#txbEditStaffIDCard').val();
        var Serail_Card = $('#txbEditSerialCard').val();
        var Staff_Status = $('#ddlEditStatus').val();
        var Join_date = $('#txbEditJoinDate').val();
        var End_Date = $('#txbEditEndDate').val();
        var Photo = $('#name-photo-Edit').val();
        var UserName = $('#txbEditUsername').val();
        //var Password = $('#txbEditPassword').val();
        var No_Scan = $('#ddlEditNoScan').val();

        //variable for tblSalaryType
        var Sal_Type = $('#ddlEditSalaryType').val();
        var Salary = $('#txbEditSalary').val();
        var Card_Phone = $('#txbEditCardPhone').val();
        var Gym = $('#txbEditGym').val();
        var Accommodation = $('#txbEditAccommodation').val();
        var Food = $('#txbEditFood').val();
        var insurance = $('#txbEditInsurance').val();
        var Money_Prim = $('#txbEditMoneyPrim').val();
        var Travel = $('#txbEditTravel').val();
        var Gasoline = $('#txbEditGasoline').val();
        var OT_Unit_Price = $('#txbEditOTUnitPrice').val();
        var Work_Hour_Taget = $('#txbEditWorkHourTarget').val();
        var Relax_Hour = $('#txbEditRelaxHour').val();
        var Off_Day = $('#txbEditDayOff').val();
        var AL_Day = $('#txbEditAnnaulLeave').val();
        var PH_Day = $('#txbEditPublicHoliday').val();
        var Sick = $('#txbEditSick').val();
        var UPL = $('#txbEditUPL').val();
        var AB = $('#txbEditAB').val();
        var CM = $('#txbEditCM').val();
        var staff_id = $('#hdStaffId').val();
        var sal_type_id = $('#hdSalTypeId').val();
        var previousStaffPhoto = $('#hdPreviewPhoto').val();
        var previousStaffIDCardPhoto = $('#hdPreviewIDCardPhoto').val();

        if (FK_Pos_Id == 0 | Staff_Name == "" | Sex == 0 | Staff_DOB == "" | Marry_Status == 0 | Nationality == "" |
            Wife == "" | Son == "" | Daughter == "" | Tell == "" | Account_Name == "" | Account_Number == "" | Address == "" |
            Identity_Card == "" | Staff_Id_Card == "" | Serail_Card == "" | Join_date == "" | UserName == "") {

            swal("Please fill your entry data to all the red fields.");
            //$('#ms_required').text("Please fill your entry data to all the red fields.");
        } else {

            const data = {
                FK_Pos_Id: FK_Pos_Id,
                Staff_Name: Staff_Name,
                Sex: Sex,
                Staff_DOB: Staff_DOB,
                Marry_Status: Marry_Status,
                Nationality: Nationality,
                Wife: Wife,
                Son: Son,
                Daughter: Daughter,
                Tell: Tell,
                Account_Name: Account_Name,
                Account_Number: Account_Number,
                Address: Address,
                Identity_Card: Identity_Card,
                Identify_Picture: Identify_Picture,
                Staff_Id_Card: Staff_Id_Card,
                Serail_Card: Serail_Card,
                Staff_Status: Staff_Status,
                Join_date: Join_date,
                End_Date: End_Date,
                Photo: Photo,
                UserName: UserName,
                No_Scan: No_Scan,
                Sal_Type: Sal_Type,
                Salary: Salary,
                Card_Phone: Card_Phone,
                Gym: Gym,
                Accommodation: Accommodation,
                Food: Food,
                insurance: insurance,
                Money_Prim: Money_Prim,
                Travel: Travel,
                Gasoline: Gasoline,
                OT_Unit_Price: OT_Unit_Price,
                Work_Hour_Taget: Work_Hour_Taget,
                Relax_Hour: Relax_Hour,
                Off_Day: Off_Day,
                AL_Day: AL_Day,
                PH_Day: PH_Day,
                Sick: Sick,
                UPL: UPL,
                AB: AB,
                CM: CM,
                staff_id: staff_id,
                sal_type_id: sal_type_id,
                previousStaffPhoto: previousStaffPhoto,
                previousStaffIDCardPhoto: previousStaffIDCardPhoto
            };
            alert(data.Photo);
            var url = "/Staff/UpdateStaff";
            $.post(url, data, function (respone) {
                swal(respone);
                setTimeout(function () {
                    window.location.reload();
                }, 1000)
            });
            
        }

    });

    // TABLE ROW IMAGE
    $(document).delegate('#table_staff tbody tr img', 'click', function () {
        $('#Modal-View-Photo').modal("show");
        $('#image-card-view-photo').attr('src', $(this)[0]['src']);
    });

    $(document).delegate('#table_staff tbody tr', 'dblclick', function () {

        //ClearContent("ddlEditBoss", "ddlEditCompany", "ddlEditLocation", "ddlEditDepartment", "txbEditPosition");

        var staff_id = $(this).attr("id").trim();
        var sal_type_id = $(this).attr("class").trim();

        $('#modal_staff_Edit').modal("show");

        var Staff_Name = this.querySelectorAll("td")[48].textContent.trim();
        var Sex = this.querySelectorAll("td")[10].textContent.trim();
        var Staff_DOB = this.querySelectorAll("td")[11].textContent.trim();
        var Marry_Status = this.querySelectorAll("td")[16].textContent.trim();
        var Nationality = this.querySelectorAll("td")[17].textContent.trim();
        var Wife = this.querySelectorAll("td")[18].textContent.trim();
        var Son = this.querySelectorAll("td")[19].textContent.trim();
        var Daughter = this.querySelectorAll("td")[20].textContent.trim();
        var Tell = this.querySelectorAll("td")[12].textContent.trim();
        var Account_Name = this.querySelectorAll("td")[44].textContent.trim();
        var Account_Number = this.querySelectorAll("td")[45].textContent.trim();
        var Address = this.querySelectorAll("td")[13].textContent.trim();
        var Identity_Card = this.querySelectorAll("td")[14].textContent.trim();
        var Identify_Picture = $(this).attr('data-identify-picture');
        var Staff_Id_Card = this.querySelectorAll("td")[6].textContent.trim();
        var Serail_Card = this.querySelectorAll("td")[21].textContent.trim();
        var Staff_Status = $(this).attr('data-staff-status');
        var Join_date = this.querySelectorAll("td")[46].textContent.trim();
        var Photo = $(this).attr('staff-photo');
        var UserName = this.querySelectorAll("td")[22].textContent.trim();
        var No_Scan = this.querySelectorAll("td")[23].textContent.trim();

        //variable for tblSalaryType
        var Sal_Type = this.querySelectorAll("td")[24].textContent.trim();
        var Salary = this.querySelectorAll("td")[25].textContent.trim();
        var Card_Phone = this.querySelectorAll("td")[26].textContent.trim();
        var Gym = this.querySelectorAll("td")[27].textContent.trim();
        var Accommodation = this.querySelectorAll("td")[28].textContent.trim();
        var Food = this.querySelectorAll("td")[29].textContent.trim();
        var insurance = this.querySelectorAll("td")[30].textContent.trim();
        var Money_Prim = this.querySelectorAll("td")[31].textContent.trim();
        var Travel = this.querySelectorAll("td")[32].textContent.trim();
        var Gasoline = this.querySelectorAll("td")[33].textContent.trim();
        var OT_Unit_Price = this.querySelectorAll("td")[34].textContent.trim();
        var Work_Hour_Taget = this.querySelectorAll("td")[35].textContent.trim();
        var Relax_Hour = this.querySelectorAll("td")[36].textContent.trim();
        var Off_Day = this.querySelectorAll("td")[37].textContent.trim();
        var AL_Day = this.querySelectorAll("td")[38].textContent.trim();
        var PH_Day = this.querySelectorAll("td")[39].textContent.trim();
        var Sick = this.querySelectorAll("td")[40].textContent.trim();
        var UPL = this.querySelectorAll("td")[41].textContent.trim();
        var AB = this.querySelectorAll("td")[42].textContent.trim();
        var CM = this.querySelectorAll("td")[43].textContent.trim();

        var boss_id = this.querySelectorAll("td")[51].textContent.trim();
        var comp_id = this.querySelectorAll("td")[52].textContent.trim();
        var location_id = this.querySelectorAll("td")[53].textContent.trim();
        var dept_id = this.querySelectorAll("td")[54].textContent.trim();
        var pos_id = this.querySelectorAll("td")[55].textContent.trim();

        $('#ddlEditBoss').val(boss_id);
        GetCompanyByBossId(boss_id, comp_id);
        GetLocationByCompanyId(comp_id, location_id);
        GetDepartmentByLocationId(location_id, dept_id);
        GetPositionByDepartmentId(dept_id, pos_id);
        
        // varialable for tblStaff
        $('#txbEditStaffName').val(Staff_Name);
        $('#ddlEditSex').val(Sex);
        $('#ddlEditDOB').val(Staff_DOB);
        $('#ddlEditMarryStatus').val(Marry_Status);
        $('#txbEditNationality').val(Nationality);
        $('#txbEditWife').val(Wife);
        $('#txbEditSon').val(Son);
        $('#txbEditDaughter').val(Daughter);
        $('#txbEditPhone').val(Tell);
        $('#txbEditBankName').val(Account_Name);
        $('#txbEditBankNumber').val(Account_Number);
        $('#txbEditAddress').val(Address);
        $('#txbEditIdentificationCard').val(Identity_Card);
        $('#photo-id-card-view-response-Edit').attr('src', '/Content/Uploads/Staff_Photoes/' + Identify_Picture);
        $('#name-photo-id-card-Edit').val(Identify_Picture);
        $('#hdPreviewIDCardPhoto').val(Identify_Picture);
        $('#txbEditStaffIDCard').val(Staff_Id_Card);
        $('#txbEditSerialCard').val(Serail_Card);
        $('#ddlEditStatus').val(Staff_Status);
        $('#txbEditJoinDate').val(Join_date);
        $('#photo-view-response-Edit').attr('src', '/Content/Uploads/Staff_Photoes/' + Photo);
        $('#name-photo-Edit').val(Photo);
        $('#hdPreviewPhoto').val(Photo);
        $('#txbEditUsername').val(UserName);
        $('#ddlEditNoScan').val(No_Scan);

        //variable for tblSalaryType
        $('#ddlEditSalaryType').val(Sal_Type);
        $('#txbEditSalary').val(Salary);
        $('#txbEditCardPhone').val(Card_Phone);
        $('#txbEditGym').val(Gym);
        $('#txbEditAccommodation').val(Accommodation);
        $('#txbEditFood').val(Food);
        $('#txbEditInsurance').val(insurance);
        $('#txbEditMoneyPrim').val(Money_Prim);
        $('#txbEditTravel').val(Travel);
        $('#txbEditGasoline').val(Gasoline);
        $('#txbEditOTUnitPrice').val(OT_Unit_Price);
        $('#txbEditWorkHourTarget').val(Work_Hour_Taget);
        $('#txbEditRelaxHour').val(Relax_Hour);
        $('#txbEditDayOff').val(Off_Day);
        $('#txbEditAnnaulLeave').val(AL_Day);
        $('#txbEditPublicHoliday').val(PH_Day);
        $('#txbEditSick').val(Sick);
        $('#txbEditUPL').val(UPL);
        $('#txbEditAB').val(AB);
        $('#txbEditCM').val(CM);

        $('#hdStaffId').val(staff_id);
        $('#hdSalTypeId').val(sal_type_id);
    });

    
});