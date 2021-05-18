$(function (){
    
    $.fn.setClassError = function (id){
        $(id).attr('class', 'form-control is-invalid');
    }
    
    $.fn.setRemoveError = function (id){
        $(id).attr('class', 'form-control');
    }
    
    $.fn.onChangeRemoveError = function (id) {
        function setRemoveError(id) {
            $(id).attr('class','form-control');
        }

        this.change(function () {
            setRemoveError(id);
        });
    }
});
 

