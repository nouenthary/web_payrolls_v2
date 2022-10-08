$(document).ready(function () {
  // form create
  $('#form-add').submit(function (e) {
    e.preventDefault();
    let form = $(this).serializeArray();
    //console.log(form);
    //return

    $.ajax({
      type: 'post',
      url: '/Accessory/Save',
      data: form,
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, "warning");
          return false;
        }
        // clear
        $('#ProductId').val('');
        $('#GradeId').empty();
        $('#Qty').val('');
        $('#Desc').val('');
        location.reload();
      }
    });
  });

  // btn save all
  $(document).on('click', '#btn-save-all', function () {
    $.ajax({
      type: 'post',
      url: '/Accessory/SaveAll',
      data: {
        ...token
      },
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, 'warning');
          return false;
        }
        location.href = '/Accessory/Index';
      }
    });
  });

});
