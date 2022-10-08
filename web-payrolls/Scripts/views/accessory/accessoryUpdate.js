$(function () {
  let no = getParamsMap()['no'];
  // form Update
  $('#form-add').submit(function (e) {

    e.preventDefault();

    let form = $(this).serializeArray();

    form.push({name: "No", value: no});

    //console.log(form);
    //return

    $.ajax({
      type: 'post',
      url: '/Accessory/SaveAndInvoice',
      data: form,
      success: function (r) {
        if (r.error) {
          SweatAlert(r.error, "warning");
          return false;
        }
        // clear
        location.reload();
      }
    });
  });

  // btn save all
  $(document).on('click', '#btn-save-all', function () {
    location.href = '/Accessory/Index';
  });

});
