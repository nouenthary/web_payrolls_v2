
//let token = { __RequestVerificationToken : $('input[name="__RequestVerificationToken"]').val()};
//console.log(token);
// get data from api
// function GetDataApi(url, data) {
//   let dataResponse = null;
//   let error = false;
//   $.ajax({
//     url: url,
//     async: false,
//     type: "POST",
//     global: false,
//     dataType: 'json',
//     data: data,
//     success: function(response) {
//       dataResponse = response
//     },
//     error: function() {
//       error =  true
//     }
//   });
//   return {data: dataResponse, error};
// }

// get select option
// function setToSelect({data : data, label : label}) {
//   let option = '<option value="">-- ' + 'Choose ' +  label + ' --</option>';
//   for(let item in data){
//     option += '<option value="'+data[item].key+'">' + data[item].value + '</option>';
//   }
//   return option;
// }

// $('#hod').change(function () {
//   alert()
//   $('select#company').val(0).select2();
//   $('select#type').val(0).select2();
//   $('select#type_name').val(0).select2();
//   $('select#product_name').val(0).select2();
//   $('select#color').val(0).select2();
//   $('select#size').val(0).select2();
//   $('select#grade').val(0).select2();
//
//   let dataForSent = {
//     id: $(this).val(),
//     ...token
//   };
//
//   let {data} = GetDataApi('/Other/GetCompany',dataForSent);
//
//   $('#company').html(setToSelect({data: data, label: '@Resources.Voucher.Company'}));
// });
