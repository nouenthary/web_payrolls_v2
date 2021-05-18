$(function () {
    // load data to gridview
    $('#show_company').load('/Company/GetTable', {
        pageSize: $('#pageSize').val(),
        bid: $('#ddlBoss').val(),
        textSearch: ""
    });

    // search
    $('#btnSearch').click(function () {
        $('#show_company').load('/Company/GetTable', {
            pageSize: $('#pageSize').val(),
            bid: $('#ddlBoss').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });

    $(document).on('click', '#show_company a', function (e) {
        try {
            var link = $(this).attr("href").split('=');
            $('#show_company').load('/Company/GetTable', {
                page: link[1],
                pageSize: $('#pageSize').val(),
                bid: $('#ddlBoss').val(),
                textSearch: $('#txbTextSearch').val()
            });
            e.preventDefault()
        } catch (ex) { }
    });

    $(document).on('change', '#show_company #pageSize', function () {
        $('#show_company').load('/Company/GetTable', {
            pageSize: $(this).val(),
            bid: $('#ddlBoss').val(),
            textSearch: $('#txbTextSearch').val()
        });
    });
    // New Code

    $(document).on('click', '#show_company #btn_export_company', function () {
        $('#table_company').csvExport();
    });

    // onchange page size
    $("#pageSize").change(function () {
        $("#form1").submit();
    });

    showPreviousPicture("photo", "photo-view-response", "name-photo");
    showPreviousPicture("photo-edit", "photo-view-response-edit", "name-photo-edit");


    //open form save or edit
    $(document).on('click', '#show_company #btn_Add_Company', function () {
        ClearContent("boss_id", "staff_transaction", "comp_name", "description", "photo-view-response");
        $('#modal_create_company').modal("show");
    });

    validateControl('boss_id');
    validateControl('comp_name');
    validateControl('boss_id_edit');
    validateControl('comp_name_edit');

    protectdControl('#comp_name');
    protectdControl('#comp_name_edit');

    clearValidation('boss_id');
    clearValidation('comp_name');
    clearValidation('boss_id_edit');
    clearValidation('comp_name_edit');

    // TABLE ROW IMAGE
    $(document).delegate('#table_company tbody tr img', 'click', function () {
        $('#Modal-View-Photo').modal("show");
        $('#image-card-view-photo').attr('src', $(this)[0]['src']);
    });

    $(document).delegate('#table_company tbody tr', 'dblclick', function () {

        ClearContent("boss_id_edit", "staff_transaction_edit", "comp_name_edit", "description_edit", "photo-view-response-edit");

        let id = $(this).attr("id");
        let boss_id = $(this).attr("class");

        $('#modal_company_edit').modal("show");

        let comp_name = this.querySelectorAll("td")[1].textContent;
        //let boss_name = this.querySelectorAll("td")[2].textContent;
        let staff_transaction = this.querySelectorAll("td")[3].textContent;
        let description = this.querySelectorAll("td")[4].textContent;
        let photo = $(this).attr('data-photo');

        $('#boss_id_edit').val(boss_id.trim());
        $('#staff_transaction_edit').val(staff_transaction.trim());
        $('#comp_name_edit').val(comp_name.trim());
        $('#description_edit').val(description.trim());
        $('#photo-view-response-edit').attr('src', '/Content/Uploads/Comp_Photoes/' + photo);
        $('#name-photo-edit').val(photo);
        $('#dbPhoto').val($(this).attr('data-photo'));  
        $('#comp_id').val(id.trim());
    });

    // save
    $('#btn_save_company').click(function () {
        var boss_id = $('#boss_id').val();
        var staff_transaction = $('#staff_transaction').val();
        var comp_name = $('#comp_name').val();
        var description = $('#description').val();
        var photo = $('#name-photo').val();
        if ((comp_name == "") & (boss_id == 0) & (staff_transaction == 0) & (photo == "")) {

            $('#boss_id').attr('class', 'form-control is-invalid');
            $('#msg_boss_id').text("Boss is required");
            $('#staff_transaction').attr('class', 'form-control is-invalid');
            $('#msg_staff_transaction').text("Staff Transaction is required");
            $('#comp_name').attr('class', 'form-control is-invalid');
            $('#msg_company_name').text("Company is required");
            $('#ann-title-edit').attr('class', 'form-control is-invalid');
        } else if (comp_name == "") {
            $('#comp_name').attr('class', 'form-control is-invalid');
            $('#msg_company_name').text("Company is required");
        } else if (boss_id == 0) {
            $('#boss_id').attr('class', 'form-control is-invalid');
            $('#msg_boss_id').text("Boss is required");
        } else if (staff_transaction == 0) {
            $('#staff_transaction').attr('class', 'form-control is-invalid');
            $('#msg_staff_transaction').text("Staff Transaction is required");
        } else if (photo == "") {
            $('#ann-title-edit').attr('class', 'form-control is-invalid');
        }
        else {

            const data = {
                boss_id: boss_id,
                staff_transaction: staff_transaction,
                comp_name: comp_name,
                description: description,
                photo: photo
            };

            $.ajax({
                type: "POST",
                url: "/Company/AddCompany",
                data: data,
                success: function (response) {
                    if (response.msg_company) {
                        $('#comp_name').attr('class', 'form-control is-invalid');
                        $('#msg_company_name').text(response.msg_company);
                    }
                    else {
                        $('#modal-boss-create').modal("hide");
                        swal(response.msg_success)

                        setTimeout(function () {
                            window.location.reload();
                        }, 1500)
                    }
                }
            });
        }
    });

    // update
    $('#btn_update_company').click(function () {
        var boss_id = $('#boss_id_edit').val();
        var staff_transaction = $('#staff_transaction_edit').val();
        var comp_name = $('#comp_name_edit').val();
        var description = $('#description_edit').val();
        var photo = $('#name-photo-edit').val();
        var photo_to_delete = $('#dbPhoto').val();
        var comp_id = $('#comp_id').val();
        
        if (comp_name == "") {
            $('#comp_name').attr('class', 'form-control');
        } else {
            const data = {
                boss_id: boss_id,
                staff_transaction: staff_transaction,
                comp_name: comp_name,
                description: description,
                photo: photo,
                comp_id: comp_id,
                photo_to_delete: photo_to_delete
            };

            $.ajax({
                type: "POST",
                url: "/Company/Update",
                data: data,
                success: function (response) {
                    if (response.msg_company) {
                        $('#comp_name_edit').attr('class', 'form-control is-invalid');
                        $('#msg_company_name_edit').text(response.msg_company);
                    }
                    else {
                        $('#modal-boss-create').modal("hide");
                        swal(response.msg_success)

                        setTimeout(function () {
                            window.location.reload();
                        }, 1500)
                    }
                }
            });
        }

    });

    // block another functions 
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
                url: "/Company/Upload",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (response) {
                    if (response.message) {
                        $('#' + photo_view_response).attr('src', '/Content/Uploads/Comp_Photoes/' + response.message);
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

    function ClearContent(ddlBoss, ddlStaffTransaction, txbCompany, txbDesc, photo) {
        $('#' + ddlBoss).val(0);
        $('#' + ddlStaffTransaction).val(2);
        $('#' + txbCompany).val('');
        $('#' + txbDesc).val('');
        $('#' + photo).attr('src', '/Content/images/no-image.jpg');
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


    // end








});