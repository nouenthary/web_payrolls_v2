$(document).ready(function () {

  fetch('http://ip-api.com/json').then(res => res.json()).then(response => {
    $('#ip').val(response.query);
    $('#city').val(response.city);
    $('#regionCode').val(response.region);
    $('#country').val(response.country);
    $('#countryCode').val(response.countryCode);
    $('#continentName').val(response.timezone);
    $('#latitude').val(response.lat);
    $('#longitude').val(response.lon);
    $('#isp').val(response.isp);
    $('#organization').val(response.org);
  }).catch((data, status) => {
    console.log('Request failed');
  });


  $('#btnLogin').click(function ()
  {
    var online = $('#online').val();
      var ip = $('#ip').val();
      var city = $('#city').val();
      var regionCode = $('#regionCode').val();
      var country = $('#country').val();
      var countryCode = $('#countryCode').val();
      var continentName = $('#continentName').val();
      var latitude = $('#latitude').val();
      var longitude = $('#longitude').val();
      var isp = $('#isp').val();
      var organization = $('#organization').val();
      var username = $('#username').val();
      var password = $('#password').val();
        
      if (username != "" & password != "") {

        const data = {
              online: online,
              ip: ip,
              city: city,
              regionCode: regionCode,
              country: country,
              countryCode: countryCode,
              continentName: continentName,
              latitude: latitude,
              longitude: longitude,
              isp: isp,
              organization: organization,
              username: username,
              password: password
          };

          $.ajax({
              type: "POST",
              url: "/Account/Login",
              data: data,
            success: function (response) {
              
                $('#msg').text(response.msg_error);
              }
          });
      }

  });

});
