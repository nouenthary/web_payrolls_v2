$(document).ready(function () {
  let token = {__RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()};
  //
  SetValueByControl('#BossId', '/App/GetBoss');
  SetValueByControl('#CompanyId', '/App/GetCompany');
  SetValueByControl('#LocationId', '/App/GetLocation');

  function GetObject() {
    let $form = $("#form-search");
    let data = getFormData($form);
    console.log(data);
    return data;
  }

// load table
  function Load() {
    let data = GetObject();
    $('#show').load('/Accessory/GetTableBalance', data);
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
      $('#show').load('/Accessory/GetTableBalance', {
        page: link[1],
        ...data
      });
      e.preventDefault()
    } catch (ex) {
    }
  });


//
  function SetValueToSelect(control, value, url) {
    $.ajax({
      type: 'post',
      url: url,
      data: {
        ...value,
        ...token
      },
      success: function (r) {
        $(control).empty();

        $(control).append($('<option>', {
          value: "",
          text: "Select Option"
        }));

        $.each(r, function (i, item) {
          $(control).append($('<option>', {
            value: item.key,
            text: item.value
          }));
        });
      }
    });
  }

  Select2('#BossId');
  Select2('#Grade');
  Select2('#Size');
  Select2('#Status');
  Select2('#Color');
  Select2('#ProductId');
  Select2('#TypeName');
  Select2('#ProductTypeId');
  Select2('#LocationId');
  Select2('#CompanyId');
  Select2('#Barcode');
  Select2('#QrCode');

// Export
  $(document).on('click', '#btn-export', function () {
    $('#table-acc-balance').csvExport();
  });

// bid
  $('#BossId').change(function () {
    $('select#LocationId').empty();
    $('select#TypeName').val(0).select2();
    $('select#ProductTypeId').empty();
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    SetValueToSelect('#CompanyId', {id: $(this).val()}, '/Other/GetCompany');

    Load();
  });

// cid
  $('#company').change(function () {
    $('select#TypeName').val(0).select2();
    $('select#ProductTypeId').empty();
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    SetValueToSelect('#LocationId', {id: $(this).val()}, '/Other/GetLocation');

    Load();
  });

  $('#LocationId').change(function () {
    Load();
  });

// product type
  $('#TypeName').change(function () {
    $('select#ProductTypeId').empty();
    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      bossId: $('#BossId').val(),
      productType: $(this).val(),
    };

    SetValueToSelect('#ProductTypeId', dataForSent, '/StockInProductCut/GetProductTypeByBoss');

    Load();
  });


//type name
  $('#ProductTypeId').change(function () {

    $('select#ProductId').empty();
    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };
    //console.log(dataForSent);
    let {data} = GetDataApi('/Other/GetProductByProductType', dataForSent);

    $('#ProductId').html(setToSelect({data: data, label: "Product"}));

    Load();
  });

// product name
  $('#ProductId').change(function () {

    $('select#Color').empty();
    $('select#Size').empty();
    $('select#Grade').empty();

    let dataForSent = {
      id: $(this).val(),
      ...token
    };

    let {data} = GetDataApi('/Other/GetProductGradeByProductId', dataForSent);
    //
    let {colors, sizes, grades} = data;
    // set color
    $('#Color').html(setToSelect({data: colors, label: "Color"}));
    // set size
    $('#Size').html(setToSelect({data: sizes, label: "Size"}));
    // set grade
    $('#Grade').html(setToSelect({data: grades, label: "Grade"}));

    Load();

  });

// color
  $('#Color').change(function () {
  //  $('select#Size').empty();
   // $('select#Grade').empty();

    Load();
  });

// size
  $('#Size').change(function () {
   // $('select#Grade').empty();

    Load();
  });

  // Status
  $('#Status').change(function () {
    Load();
  });

});
