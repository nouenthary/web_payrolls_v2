$(function () {
  //
  SetValueByControl('#PK_Boss_Id', '/App/GetBoss');
  SetValueByControl('#CompanyId', '/App/GetCompany');
  SetValueByControl('#LocationId', '/App/GetLocation');

  // change boss
  $('#PK_Boss_Id').change(function () {

    $('#CompanyId').empty();
    $('#LocationId').empty();

    let value = {
      id: $(this).val()
    };
    SetValueToSelect('#CompanyId', value, '/Other/GetCompany')
  });

  // change company
  $('#CompanyId').change(function () {

    $('#LocationId').empty();

    let value = {
      id: $(this).val()
    };
    SetValueToSelect('#LocationId', value, '/Other/GetLocation')
  });

  // Type_Product
  $('#Type_Product').change(function () {
    let value = {
      id: $(this).val()
    };
    SetValueToSelect('#ProductId', value, '/Other/GetProductByProductType');

    $('#GradeId').empty();
  });

  // Product id
  $('#ProductId').change(function () {
    let value = {
      id: $(this).val()
    };
    SetValueToSelect('#GradeId', value, '/Other/GetProductGradesByProductId')
  });


  // remove
  $(document).on('click', '#btn-remove', function () {
    let id = $(this).closest('tr').attr('id');

    $.ajax({
      type: 'post',
      url: '/Accessory/Remove',
      data: {
        ...token,
        id
      },
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, 'warning');
          return false;
        }
        location.reload();
      }
    });
  });

  // update table row
  $(document).on('change', '#txtQty', function () {
    let id = $(this).closest('tr').attr('id');

    let qty = $(this).val();

    $.ajax({
      type: 'post',
      url: '/Accessory/Update',
      data: {
        ...token,
        id,
        qty
      },
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, 'warning');
        }
        location.reload();
      }
    });
  });

});
