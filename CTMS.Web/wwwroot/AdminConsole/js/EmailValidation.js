$(document).ready(function () {
    $('.Email').change(function () {
        var em = $('.Email').val();
        var emailReg = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
        if (emailReg.test(em)) {
            return true;
        } else {
            swal("Invalid Email Address !");
            $('.Email').val('');
            return false;
        }
    });
});



