$(document).ready(function () {
  //
  SetValueByControl('#BossId', '/App/GetBoss');
  SetValueByControl('#CompanyId', '/App/GetCompany');
  SetValueByControl('#LocationId', '/App/GetLocation');

  function GetObject() {
    let $form = $("#form-search");
    let data = getFormData($form);
    return data;
  }

  // load table
  function Load() {
    let data = GetObject();
    $('#show').load('/Accessory/GetTable', data);
  }

  Load();

  // form submit
  $(document).on('submit', '#form-search', function (e) {
    e.preventDefault();
    Load();
  });

  // page Size
  $('#pageSize_').change(function (e) {
    e.preventDefault();
    $('#PageSize').val($(this).val());
    $('#form-search').submit();
  });

  $(document).on('click', '#show-page a', function (e) {
    try {
      let link = $(this).attr("href").split('=');
      let data = GetObject();
      $('#show').load('/Accessory/GetTable', {
        page: link[1],
        ...data
      });
      e.preventDefault()
    } catch (ex) {
    }
  });

  Select2('#BossId');
  Select2('#Grade');
  Select2('#Size');
  Select2('#Status');
  Select2('#Color');
  Select2('#ProductId');
  Select2('#TypeName');
  Select2('#type');
  Select2('#LocationId');
  Select2('#CompanyId');
  Select2('#ProductTypeId');
  Select2('#Type');
  Select2('#StockInType');

  // Export
  $(document).on('click', '#btn-export', function () {
    $('#table-complete').csvExport();
  });

  //  get No
  $(document).on('click', '#btn-add-new', function () {
    location.href = `/Accessory/Create`;
  });

  // edit
  $(document).on('click', '#btn-edit', function () {

    let id = $(this).closest('tr').attr('data-id');

    let tokenUrl = token.__RequestVerificationToken;

    location.href = `/Accessory/Update?token=${tokenUrl}&no=${id}&update=${tokenUrl}`;
  });


  // reject
  $(document).on('click', '#btn-reject', function () {
    let no = $(this).closest('tr').attr('data-id');
    //alert(no);
    swal({
        title: "Are you sure reject ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes, I am sure!',
        cancelButtonText: "No, cancel it!",
        closeOnConfirm: false,
        closeOnCancel: false
      },
      function (isConfirm) {
        if (isConfirm) {
          $.ajax({
            type: 'post',
            url: '/Accessory/UpdateStatus',
            data: {
              ...token,
              no: no,
              status: 'Reject'
            },
            success: function (r) {
              SweatAlert(r.message, "warning");
              UpdateStatus("Stock_In_Accessory_" + no, "Reject");
              Load();
            }
          })
        } else {
          swal({
            title: "",
            text: "Cancel",
            type: "error",
            timer: 1500
          });
        }

      }
    );

  });


  // confirm
  $(document).on('click', '#btn-confirm', function () {
    let no = $(this).closest('tr').attr('data-id');
    //alert(no);

    swal({
        title: "Are you sure confirm ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes, I am sure!',
        cancelButtonText: "No, cancel it!",
        closeOnConfirm: false,
        closeOnCancel: false
      },
      function (isConfirm) {
        if (isConfirm) {
          $.ajax({
            type: 'post',
            url: '/Accessory/UpdateStatus',
            data: {
              ...token,
              no: no,
              status: 'Done'
            },
            success: function (r) {
              SweatAlert(r.message, "success");
              UpdateStatus("Stock_In_Accessory_" + no, "Done");
              Load();
            }
          })
        } else {
          swal({
            title: "",
            text: "Cancel",
            type: "error",
            timer: 1500
          });
        }

      }
    );

  });

  // Filter

  // BossId
  $('#BossId').change(function () {
    $('select#CompanyId').empty();
    $('select#type').empty();
    $('select#TypeName').empty();
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#CompanyId', dataForSent, '/Other/GetCompany');

    Load();
  });

  // cid
  $('#CompanyId').change(function () {
    $('select#LocationId').empty();
    $('select#type').val(0).empty();
    $('select#TypeName').empty();
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    SetValueToSelect('#LocationId', dataForSent, '/Other/GetLocation');

    Load();

  });

  //location
  $('#LocationId').change(function () {
    Load();
  });

  // product type
  $('#TypeName').change(function () {
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      bid: $('#BossId').val(),
      productType: $(this).val(),
      ...token
    };

    //alert(JSON.stringify(dataForSent));
    SetValueToSelect('#ProductTypeId', dataForSent, '/Other/GetProductTypeNameByBoss');

    Load();
  });


  //type name
  $('#ProductTypeId').change(function () {


    $('select#ProductId').val(0).select2();
    $('select#Color').val(0).select2();
    $('select#Size').val(0).select2();
    $('select#Grade').val(0).select2();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    let {data} = GetDataApi('/Other/GetProductByProductType', dataForSent);

    $('#ProductId').html(setToSelect({data: data, label: "Product"}));

    Load();
  });

  // product name
  $('#ProductId').change(function () {

    $('select#Color').val(0).select2();
    $('select#Size').val(0).select2();
    $('select#Grade').val(0).select2();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    let {data} = GetDataApi('/Other/GetProductGradeByProductId', dataForSent);
    //
    let {colors, sizes, grades} = data;

    // set Color
    $('#Color').html(setToSelect({data: colors, label: "Color"}));
    // set Size
    $('#Size').html(setToSelect({data: sizes, label: "Size"}));
    // set Grade
    $('#Grade').html(setToSelect({data: grades, label: "Grade"}));

    Load();
  });

  // Color
  $('#Color').change(function () {
    $('select#Size').val(0).select2();
    $('select#Grade').val(0).select2();
    Load();
  });

  // Size
  $('#Size').change(function () {
    $('select#Grade').val(0).select2();
    Load();
  });

  // Size
  $('#Status').change(function () {

    Load();
  });


  // Start Date
  $('#StartDate').change(function () {
    Load();
  });

  // Start Date
  $('#EndDate').change(function () {
    Load();
  });

  // Type
  $('#Type').change(function () {

    Load();
  });

  // StockInType
  $('#StockInType').change(function () {

    Load();
  })

  // set price
  $(document).on('click', '#btn-set', function () {
    $('#form-set-price #PK_Grade_Id').val($(this).attr('data-id'));
    $('#modal-add-price').modal('show');
  });

  // form-set-price
  $('#form-set-price').submit(function (e) {
    e.preventDefault();
    let form = $(this).serializeArray();
    //console.log(form);

    $.ajax({
      type: 'post',
      url: '/StockInComplete/SetGetPriceAndOtherSpent',
      data: form,
      success:function (r) {
        if(r.message){
          $('#modal-add-price').modal('hide');
          Load();
          $('#form-set-price')[0].reset();
        }
      }
    });
  });

});
