$(function () {
    // load data to gridview
    $('#show_position').load('/Position/GetTable', {
        pageSize: $('#pageSize').val(),
        bid: $('#ddlBoss').val(),
        cid: 0,
        lid: 0,
        did: 0,
        textSearch: ""
    });

    // search
    $('#btnSearch').click(function () {
        $('#show_position').load('/Position/GetTable', {
            pageSize: $('#pageSize').val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            did: $('#ddlDepartment').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });

    $(document).on('click', '#show_position a', function (e) {
        try {
            var link = $(this).attr("href").split('=');
            $('#show_position').load('/Position/GetTable', {
                page: link[1],
                pageSize: $('#pageSize').val(),
                bid: $('#ddlBoss').val(),
                cid: $('#ddlCompany').val(),
                lid: $('#ddlLocation').val(),
                did: $('#ddlDepartment').val(),
                textSearch: $('#txbTextSearch').val()
            });
            e.preventDefault()
        } catch (ex) { }
    });

    $(document).on('change', '#show_position #pageSize', function () {
        $('#show_position').load('/Position/GetTable', {
            pageSize: $(this).val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            did: $('#ddlDepartment').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });
    // New Code

    $(document).on('click', '#show_position #btn_export_position', function () {
        $('#table_position').csvExport();
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
            url: "/Position/GetCompany",
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
            url: "/Position/GetLocation",
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
            }
        });
    });

    // get department by location id
    $('#ddlLocation').change(function () {
        let data = {
            dept_id: $(this).val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Position/GetDepartment",
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
            url: "/Position/GetCompany",
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
            url: "/Position/GetLocation",
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
            url: "/Position/GetDepartment",
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
            url: "/Position/GetCompany",
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
            url: "/Position/GetLocation",
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
            url: "/Position/GetDepartment",
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
            }
        });
    });
    // end

    // end

    // block add and edit record

    //open form save or edit
    $(document).on('click', '#show_position #btn_modal_position', function () {
        ClearContent("ddlAddBoss", "ddlAddCompany", "ddlAddLocation", "ddlAddDepartment", "txbPosition");
        $('#modal_position_create').modal("show");
    });

    validateControl('#txbPosition');
    validateControl('#txbEditPosition');
    protectdControl('#txbPosition');
    protectdControl('#txbEditPosition');
    clearValidation('ddlAddDepartment');
    clearValidation('ddlEditDepartment');
    clearValidation('txbEditPosition');
    clearValidation('txbEditPosition');

    $(document).delegate('#table_position tbody tr', 'dblclick', function () {

        ClearContent("ddlEditBoss", "ddlEditCompany", "ddlEditLocation", "ddlEditDepartment", "txbEditPosition");

        let position_id = $(this).attr("id");
        let boss_id = $(this).attr("class");

        $('#modal_position_edit').modal("show");

        let position_name = this.querySelectorAll("td")[5].textContent;
        let comp_id = this.querySelectorAll("td")[9].textContent.trim();
        let location_id = this.querySelectorAll("td")[10].textContent.trim();
        let dept_id = this.querySelectorAll("td")[11].textContent.trim();

        $('#ddlEditBoss').val(boss_id.trim());
        GetCompanyByBossId(boss_id, comp_id);
        GetLocationByCompanyId(comp_id, location_id);
        GetDepartmentByLocationId(location_id, dept_id);

        $('#txbEditPosition').val(position_name.trim());
        $('#hdPositionId').val(position_id.trim());
    });

    // save
    $('#btn_save_position').click(function () {
        var dept_id = $('#ddlAddDepartment').val();
        var positionName = $('#txbPosition').val();

        if ((dept_id == 0) & (positionName == '')) {

            $('#ddlAddDepartment').attr('class', 'form-control is-invalid');
            $('#msg_dept_add').text("Department is required");
            $('#txbAddPosition').attr('class', 'form-control is-invalid');
            $('#msg_position_Name').text("Position is required");
        } else if (dept_id == 0) {
            $('#ddlAddDepartment').attr('class', 'form-control is-invalid');
            $('#msg_dept_add').text("Location is required");
        } else if (positionName == '') {
            $('#txbAddPosition').attr('class', 'form-control is-invalid');
            $('#msg_position_Name').text("Position is required");
        } else {

            const data = {
                dept_id: dept_id,
                positionName: positionName
            };

            $.ajax({
                type: "POST",
                url: "/Position/AddPosition",
                data: data,
                success: function (response) {

                    if (response.msg_existing) {
                        $('#txbAddPosition').attr('class', 'form-control is-invalid');
                        $('#msg_position_Name').text(response.msg_existing);
                    }
                    else {
                        $('#modal_position_create').modal("hide");
                        swal(response.msg_success)

                        setTimeout(function () {
                            window.location.reload();
                        }, 1000)
                    }
                }
            });
        }
    });

    // update
    $('#btn_update_position').click(function () {
        var dept_id = $('#ddlEditDepartment').val();
        var positionName = $('#txbEditPosition').val();
        var position_id = $('#hdPositionId').val();

        if ((dept_id == 0) & (positionName == '')) {

            $('#ddlEditDepartment').attr('class', 'form-control is-invalid');
            $('#msg_dept_edit').text("Department is required");
            $('#txbEditPosition').attr('class', 'form-control is-invalid');
            $('#msg_position_name_edit').text("Position is required");
        } else if (dept_id == 0) {
            $('#ddlEditDepartment').attr('class', 'form-control is-invalid');
            $('#msg_dept_edit').text("Department is required");
        } else if (positionName == '') {
            $('#txbEditPosition').attr('class', 'form-control is-invalid');
            $('#msg_position_name_edit').text("Position is required");
        } else {

            const data = {
                dept_id: dept_id,
                positionName: positionName,
                position_id: position_id
            };

            $.ajax({
                type: "POST",
                url: "/Position/UpdatePosition",
                data: data,
                success: function (response) {

                    if (response.msg_existing) {
                        $('#txbEditPosition').attr('class', 'form-control is-invalid');
                        $('#msg_position_name_edit').text(response.msg_existing);
                    }
                    else {
                        $('#modal_position_create').modal("hide");
                        swal(response.msg_success)

                        setTimeout(function () {
                            window.location.reload();
                        }, 1000)
                    }
                }
            });
        }

    });

    // block another functions 
    function clearValidation(htmlTageName) {
        $('#' + htmlTageName).change(function () {
            $('#' + htmlTageName).attr('class', 'form-control');
        });
    }

    function ClearContent(ddlBoss, ddlCompany, ddlLocation, ddlDepartment, txbPosition) {
        $('#' + ddlBoss).val(0);
        $('#' + ddlCompany).val(0);
        $('#' + ddlLocation).val(0);
        $('#' + ddlDepartment).val(0);
        $('#' + txbPosition).val('');
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

    // end

    // block populate data to dropdownlist in form edit
    function GetCompanyByBossId(boos_id, comp_id) {
        let data = {
            boss_id: boos_id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            type: "POST",
            url: "/Department/GetCompany",
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
            url: "/Department/GetLocation",
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
            url: "/Position/GetDepartment",
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


});