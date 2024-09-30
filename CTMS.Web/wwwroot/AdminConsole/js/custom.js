
//window.addEventListener('DOMContentLoaded', event => {

//    // Toggle the side navigation
//    const sidebarToggle = document.body.querySelector('#sidebarToggle');
//    if (sidebarToggle) {
//        // Uncomment Below to persist sidebar toggle between refreshes
//        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
//        //     document.body.classList.toggle('sb-sidenav-toggled');
//        // }
//        sidebarToggle.addEventListener('click', event => {
//            event.preventDefault();
//            document.body.classList.toggle('sb-sidenav-toggled');
//            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
//        });
//    }

//});

//for toggle-rotate
$(document).ready(function () {


    const sidebarToggle = $('#sidebarToggle');
    if (sidebarToggle.length) {        
        sidebarToggle.click(function (event) {
            event.preventDefault();
            $('body').toggleClass('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', $('body').hasClass('sb-sidenav-toggled'));
        });
    }
    $('#btn_drop').click(function () {
        $('#rotate_up').toggleClass('rotate_to_up');
    });
    
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
});



