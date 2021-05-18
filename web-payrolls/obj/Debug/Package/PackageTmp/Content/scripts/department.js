$(function () {
    // load data to gridview
    $('#show_department').load('/Department/GetTable', {
        pageSize: $('#pageSize').val(),
        bid: $('#ddlBoss').val(),
        cid: 0,
        lid: 0,
        textSearch: ""
    });

    // search
    $('#btnSearch').click(function () {
        $('#show_department').load('/Department/GetTable', {
            pageSize: $('#pageSize').val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });

    $(document).on('click', '#show_department a', function (e) {
        try {
            var link = $(this).attr("href").split('=');
            $('#show_department').load('/Department/GetTable', {
                page: link[1],
                pageSize: $('#pageSize').val(),
                bid: $('#ddlBoss').val(),
                cid: $('#ddlCompany').val(),
                lid: $('#ddlLocation').val(),
                textSearch: $('#txbTextSearch').val()
            });
            e.preventDefault()
        } catch (ex) { }
    });

    $(document).on('change', '#show_department #pageSize', function () {
        $('#show_department').load('/Department/GetTable', {
            pageSize: $(this).val(),
            bid: $('#ddlBoss').val(),
            cid: $('#ddlCompany').val(),
            lid: $('#ddlLocation').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });
    // New Code

    $(document).on('click', '#show_department #btn-export-department', function () {
        $('#table_department').csvExport();
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
            url: "/Department/GetCompany",
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
            url: "/Department/GetLocation",
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
            url: "/Department/GetCompany",
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
            url: "/Department/GetLocation",
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
            url: "/Department/GetCompany",
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
            url: "/Department/GetLocation",
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
            }
        });
    });
    // end

    // end

    // block add and edit record

    //open form save or edit
    $(document).on('click', '#show_department #btn-create-boss', function () {
        ClearContent("ddlAddBoss", "ddlAddCompany", "ddlAddLocation", "txbDeptName", "ddlStaffTransaction", "txbDescription");
        $('#modal_dept_create').modal("show");
    });

    validateControl('#txbDeptName');
    validateControl('#txbEditDeptName');
    protectdControl('#txbDeptName');
    protectdControl('#txbEditDeptName');
    clearValidation('ddlAddLocation');
    clearValidation('txbDeptName');
    clearValidation('ddlEditLocation');
    clearValidation('txbEditDeptName');

    $(document).delegate('#table_department tbody tr', 'dblclick', function () {

        ClearContent("ddlEditBoss", "ddlEditCompany", "ddlEditLocation", "txbEditDeptName", "ddlEditStaffTransaction", "txbEditDescription");

        let dept_id = $(this).attr("id");
        let boss_id = $(this).attr("class");

        $('#modal_department_edit').modal("show");

        let dept_name = this.querySelectorAll("td")[4].textContent;
        let num_staff_transaction = this.querySelectorAll("td")[5].textContent.trim();
        let description = this.querySelectorAll("td")[6].textContent;
        let comp_id = this.querySelectorAll("td")[10].textContent.trim();
        let location_id = this.querySelectorAll("td")[11].textContent.trim();

        $('#ddlEditBoss').val(boss_id.trim());
        GetCompanyByBossId(boss_id, comp_id);
        GetLocationByCompanyId(comp_id, location_id);

        $('#txbEditDeptName').val(dept_name.trim());
        $('#ddlEditStaffTransaction').val(num_staff_transaction);
        $('#txbEditDescription').val(description.trim());
        $('#hdDeptId').val(dept_id.trim());
    });

    // save
    $('#btn_save_department').click(function () {
        var location_id = $('#ddlAddLocation').val();
        var deptName = $('#txbDeptName').val();
        var staff_transaction = $('#ddlStaffTransaction').val();
        var description = $('#txbDescription').val();

        if ((location_id == 0) & (deptName == '')) {

            $('#ddlAddLocation').attr('class', 'form-control is-invalid');
            $('#msg_location').text("Location is required");
            $('#txbDeptName').attr('class', 'form-control is-invalid');
            $('#msg_Dept_Name').text("Department is required");
        } else if (location_id == 0) {
            $('#ddlAddLocation').attr('class', 'form-control is-invalid');
            $('#msg_location').text("Location is required");
        } else if (deptName == '') {
            $('#txbDeptName').attr('class', 'form-control is-invalid');
            $('#msg_Dept_Name').text("Department is required");
        } else {

            const data = {
                location_id: location_id,
                deptName: deptName,
                staff_transaction: staff_transaction,
                description: description
            };

            $.ajax({
                type: "POST",
                url: "/Department/AddDepartment",
                data: data,
                success: function (response) {
                    
                    if (response.msg_existing) {
                        $('#txbDeptName').attr('class', 'form-control is-invalid');
                        $('#msg_Dept_Name').text(response.msg_existing);
                    }
                    else {
                        $('#modal_dept_create').modal("hide");
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
    $('#btn_update_department').click(function () {
        var location_id = $('#ddlEditLocation').val();
        var staff_transaction = $('#ddlEditStaffTransaction').val();
        var deptName = $('#txbEditDeptName').val();
        var description = $('#txbEditDescription').val();
        var dept_id = $('#hdDeptId').val();

        if ((location_id == 0) & (deptName == '')) {

            $('#ddlEditLocation').attr('class', 'form-control is-invalid');
            $('#msg_location_edit').text("Location is required");
            $('#txbEditDeptName').attr('class', 'form-control is-invalid');
            $('#msg_dept_name_edit').text("Department is required");
        } else if (location_id == 0) {
            $('#ddlEditLocation').attr('class', 'form-control is-invalid');
            $('#msg_location_edit').text("Location is required");
        } else if (deptName == "") {
            $('#txbEditDeptName').attr('class', 'form-control is-invalid');
            $('#msg_dept_name_edit').text("Department is required");
        } else {

            const data = {
                location_id: location_id,
                deptName: deptName,
                staff_transaction: staff_transaction,
                description: description,
                dept_id: dept_id
            };

            $.ajax({
                type: "POST",
                url: "/Department/Update",
                data: data,
                success: function (response) {

                    if (response.msg_existing) {
                        $('#txbEditDeptName').attr('class', 'form-control is-invalid');
                        $('#msg_dept_name_edit').text(response.msg_existing);
                    }
                    else {
                        $('#modal_dept_create').modal("hide");
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

    function ClearContent(ddlBoss, ddlCompany, ddlLocation, txbDepartment, ddlStaffTransaction, txbDesc) {
        $('#' + ddlBoss).val(0);
        $('#' + ddlCompany).val(0);
        $('#' + ddlLocation).val(0);
        $('#' + ddlStaffTransaction).val(2);
        $('#' + txbDepartment).val('');
        $('#' + txbDepartment).val('');
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
    // end


    // end




    

    
});