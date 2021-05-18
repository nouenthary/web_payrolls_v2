$(document).ready(function () {
    $('#btnLogin').submit(function (e) {
        alert();
        var username = $('#username').val();
        var password = $('#password').val();
        
        if (username != "" & password != "") {

            const data = {
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