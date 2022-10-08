// only decimal number can input into text box
function isDecimalNumber(key) {
  let keycode = (key.which) ? key.which : key.keycode;
  let parts = key.srcElement.value.split('.');

  if (parts.length > 1 && keycode == 46) {
    return false;
  } else {
    if (keycode == 46 || keycode >= 48 && keycode <= 57)
      return true;
    return false;
  }
}

// only int number can input into text box
function isDecimalInt(key) {
  let keycode = (key.which) ? key.which : key.keycode;
  let parts = key.srcElement.value.split('.');

  if (keycode == 46) {
    return false;
  } else {
    if (keycode == 46 || keycode >= 48 && keycode <= 57)
      return true;
    return false;
  }
}

$(document).ready(function () {
  console.log(token);
  //
  SetValueByControl('#FK_Com_Id', '/App/GetCompany');
  SetValueByControl('#FK_Loc_From_Id', '/App/GetLocation');

  // change company
  $('#FK_Com_Id').change(function () {
    let value = {
      id: $(this).val()
    }
    SetValueToSelect('#FK_Loc_From_Id', value, '/Other/GetLocation')
  });

  // Type_Product
  $('#Type_Product').change(function () {
    let value = {
      id: $(this).val()
    }
    SetValueToSelect('#ProductId', value, '/Other/GetProductByProductType');

    $('#FK_Grade_Id').empty();
  });

  // Product id
  $('#ProductId').change(function () {
    let value = {
      id: $(this).val()
    }
    SetValueToSelect('#FK_Grade_Id', value, '/Other/GetProductGradesByProductId')
  });


  // form create
  $('#form-add').submit(function (e) {
    e.preventDefault();
    let form = $(this).serializeArray();

    $.ajax({
      type: 'post',
      url: '/StockInComplete/Save',
      data: form,
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, "warning");
          return false;
        }
        // clear
        $('#ProductId').val('');
        $('#FK_Grade_Id').empty();
        $('#In_QTY').val('');
        $('#Cost').val('');
        $('#WH').val('');
        $('#WW').val('');
        $('#Discount').val('');
        $('#Descr').val('');

        Load();
      }
    });
  });


  // load create blog
  function Load() {
    $('#show').load('/StockInComplete/GetTableCreate', {});
  }

  Load();

  // remove
  $(document).on('click', '#btn-remove', function () {
    let id = $(this).closest('tr').attr('id');

    $.ajax({
      type: 'post',
      url: '/StockInComplete/Remove',
      data: {
        id,
        ...token
      },
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, "warning");
          return false;
        }
        Load();
      }
    });
  });

  // update
  $(document).on('change', '#table-item tbody tr input', function () {

    let PK_StockIn_Complete_Product_Id = $(this).closest('tr').attr('id');

    let In_QTY = $(this).closest('tr').find('#txtQty').val();
    let Cost = $(this).closest('tr').find('#txtCost').val();
    let WH = $(this).closest('tr').find('#txtWH').val();
    let WW = $(this).closest('tr').find('#txtWW').val();
    let Discount = $(this).closest('tr').find('#txtDiscount').val();

    $.ajax({
      type: 'post',
      url: '/StockInComplete/Update',
      data: {
        ...token,
        PK_StockIn_Complete_Product_Id,
        In_QTY,
        Cost,
        WH,
        WW,
        Discount
      },
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, "warning");
          return false;
        }
        Load();
      }
    })
  });

});
