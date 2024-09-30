
function getUrlVars() {
    let vars = [], hash;
    let hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (let i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
let host = window.location.host;
let pathInfo = window.location.pathname;
let FN1 = pathInfo.split('/')[1];
let FN2 = pathInfo.split('/')[2];
if (FN1 != '') {
    let appURL = "http://" + host + "/" + FN1;
} else {
    let appURL = "http://" + host;
}

let printMe
let backMe
let deleteMe
let downloadMe
let indicateMe
let excelMe
let pdfMe
let activetMe
let inactivetMe
let refreshMe
    // util function
function goBack() {
    window.history.back();
}

$("#printIcon").hide();
$("#backIcon").hide();
$("#deleteIcon").hide();
$("#indicate").hide();
$("#downloadIcon").hide();
$("#excelIcon").hide();
$("#pdfIcon").hide();
$("#activeIcon").hide();
$("#inactiveIcon").hide();
$("#refreshIcon").hide();

function checkStatus() {
    if (backMe == "yes") {
        $('#backIcon').show();
        $("#backIcon").tooltip();
    }
    if (printMe == "yes") {
        $('#printIcon').show();
        $("#printIcon").tooltip();
    }
    if (deleteMe == "yes") {
        $('#deleteIcon').show();
        $("#deleteIcon").tooltip();
    }

    if (indicateMe == "yes") {
        $('#indicate').show();
        $("#indicate").tooltip();
    }

    if (downloadMe == "yes") {
        $('#downloadIcon').show();
        $("#downloadIcon").tooltip();
    }
    if (excelMe == "yes") {
        $('#excelIcon').show();
        $("#excelIcon").tooltip();
    }
    if (pdfMe == "yes") {
        $('#pdfIcon').show();
        $("#pdfIcon").tooltip();
    }
    if (activetMe == "yes") {
        $('#activeIcon').show();
        $("#activeIcon").tooltip();
    }
    if (inactivetMe == "yes") {
        $('#inactiveIcon').show();
        $("#inactiveIcon").tooltip();
    }
    if (refreshMe == "yes") {
        $('#refreshIcon').show();
        $("#refreshIcon").tooltip();
    }
}



// Navigation button click
$('.nav-toggle-btn').on('click', function(event) {
    event.stopPropagation();
    $(this).toggleClass('on');
    $('.page-container').toggleClass('display-full');
    $('.sidemenu').toggleClass('active');
})
function bindeventForAccordion() {
    let acc = document.getElementsByClassName("accordion");
    let i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            let panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        });
    }
}
$(document).ready(function () {
    checkStatus();

    // accordion function
    let acc = document.getElementsByClassName("accordion");
    let i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function() {
            this.classList.toggle("active");
            let panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        });
    }
    // 

    let wndHght = $(window).height();
    if (wndHght > $('.page-controls-section > .card').height()) {
        $('.page-controls-section > .card').css("min-height", wndHght - 115);
    } else {
        //this part works, so it's hidden
    }


    // Tool Tip
    $('[data-toggle="tooltip"]').tooltip({
            trigger: 'hover'
        })
        // Tool Tip

    $('#information,#notifications,#changepassword,#faqdata,#userinfo').on('click', function() {

        let dataid = $(this).attr('data-target')
            //alert(dataid);
        $('.notification-sec').removeClass('active');
        $(dataid).toggleClass('active')
        $('[data-toggle="tooltip"]').tooltip("hide");
    })
    $('.div-close').on('click', function() {
        $(this).parents('.notification-sec').removeClass('active');

    })



    // Full width less than 800
    let windowWidth = $(window).width();
    if (windowWidth < 800) {
        $('.page-container').addClass('display-full');

    }
    // Full width less than 800


    // Btn web effect

    $(".btn").click(function(e) {


        $(".ripple").remove();

        let posX = $(this).offset().left,
            posY = $(this).offset().top,
            buttonWidth = $(this).width(),
            buttonHeight = $(this).height();


        $(this).prepend("<span class='ripple'></span>");



        if (buttonWidth >= buttonHeight) {
            buttonHeight = buttonWidth;
        } else {
            buttonWidth = buttonHeight;
        }


        let x = e.pageX - posX - buttonWidth / 2;
        let y = e.pageY - posY - buttonHeight / 2;



        $(".ripple").css({
            width: buttonWidth,
            height: buttonHeight,
            top: y + 'px',
            left: x + 'px'
        }).addClass("rippleEffect");
    });

    // Btn web effect

    // view password btn
    $('.passwordshowbtn').on('click', function() {
        $(this).find('i').toggleClass("icon-eye1 icon-key");
        let inputpass = $('.password').attr("type");

        if (inputpass == "password") {
            $('.password').attr("type", "text");
        } else {
            $('.password').attr("type", "password");
        }
    });

    // view password btn


    // Search panel
    // $('.searchopen').on('click', function() {
    //         $('.search-sec').toggleClass('open');
    //         $(this).find('i').toggleClass('icon-angle-down icon-angle-up')
    //     })
    // Search panel

    // $('#search-click').on('click',function(){
    // //alert(0);
    //   $('.searchopen').trigger( "click" );
    // })

    $('.searchbtn').on('click', function() {
        $('.search-sec').slideToggle();
        $('.searchbtn .ico').toggleClass('icon-angle-down icon-angle-up');
        if ($('.searchbtn span').text() == "Hide")
            $('.searchbtn span').text("Search")
        else
            $('.searchbtn span').text("Hide");
    });
    // Input focus


    $(".form-control")
        .on('focus', function() {

            $(this).parents('.control-group').addClass('focused');
        })
        .on('focusout', function() {
            if ($(this).val() != "") {
                //alert(1)
                $(this).parents('.control-group').addClass('focused');
            } else {
                //alert(2)
                $(this).parents('.control-group').removeClass('focused');
            }
        });

    $(".control-group label").click(function() {
        $(this).parents('.control-group').addClass('focused');
        $(this).parents('.control-group').find('.form-control').focus();

    });

    // Input focus




    // Logout madal
    $('.logout').click(function() {

        $('.logout-modal').addClass('show');

        setTimeout(function() {
            $('.logout-modal').removeClass('show');

        }, 5000);
    })


    $('.nologout').click(function() {
            $('.logout-modal').removeClass('show')
        })
        // Logout madal




    // Theme


    let lstorageThemeval = localStorage.getItem("webtheme");

    // alert(lstorageThemeval);
    if (lstorageThemeval !== "") {

        $('.theme-seeting ul li').removeClass('active');
        $('head').append('<link id="' + lstorageThemeval + 'css"  href="../css/' + lstorageThemeval + '.css" rel="stylesheet"  />');
        $('#' + lstorageThemeval).addClass('active');
    }

    if (lstorageThemeval == null) {
        localStorage.removeItem('webtheme');
        $('#custom').addClass('active')
    }


    $('.theme-seeting ul li').on('click', function() {
        $('.theme-seeting ul li').removeClass('active');
        $(this).addClass('active');

        $('head').find("#" + lstorageThemeval + 'css').remove();
        let themeid = $(this).attr("id");
        //alert(themeid);

        if (typeof(Storage) !== "undefined") {

            localStorage.setItem("webtheme", themeid);
            let lstorageThemeval = localStorage.getItem("webtheme");
            //alert(lstorageThemeval);
            $('head').append('<link class="othercss"  href="../css/' + lstorageThemeval + '.css" rel="stylesheet"  />');
        }
    });

    $('#custom').on('click', function() {
        localStorage.removeItem('webtheme');
        //alert(lstorageThemeval);
        $('head').find("#thm-blackcss").remove();
    })


});

// text area counter
function TextCounter(ctlTxtName, lblCouter, numTextSize) {
    let txtName = document.getElementById(ctlTxtName).value;
    let txtNameLength = txtName.length;
    if (parseInt(txtNameLength) > parseInt(numTextSize)) {
        let txtMaxTextSize = txtName.substr(0, numTextSize)
        document.getElementById(ctlTxtName).innerHTML = txtMaxTextSize;
        alert("Entered Text Exceeds '" + numTextSize + "' Characters.");
        document.getElementById(lblCouter).innerHTML = 0;
        return false;
    } else {
        document.getElementById(lblCouter).innerHTML = parseInt(numTextSize) - parseInt(txtNameLength);
        return true;
    }
}

function blockspecialchar_first(e) {
    let str;
    str = e.value;
}
//  end
function loadNavigation(pgName, gLink, pLink, fLink, lLink, title) {
    let totLink = '';
    let pathName = window.location.pathname;
    let fileName = pathName.split('/').reverse()[0].split('.').reverse()[1];
    if (pgName == fileName)
        if (pLink != '') {
            $('.' + gLink).addClass('active show');
            $('.' + gLink).children('.dropdown-menu').addClass('show');
        } else {
            $('.' + gLink).addClass('active show');
            $('.' + gLink).children('.dropdown-menu').addClass('show');
        }
    $('.' + pLink).addClass('active ');
    if (lLink != '')
        totLink = "<li class='breadcrumb-item'>" + fLink + " </li><li class='breadcrumb-item font-weight-bold'>" + lLink + "</li>";
    else
        totLink = " <li class='breadcrumb-item active'> " + fLink + "</li>";
    $('#navigation').html('<li class="breadcrumb-item"><a href="#" title="Home" ><i class="icon-home"></i></a></li>' + totLink);
    $('#title').append(title);
    printHeader = title;
}
// print function
function PrintPage( tabname) {
   
  
    if (tabname == undefined) {
        let windowName = "PrintPage";
        let wOption = "width=1000,height=600,menubar=yes,scrollbars=yes,location=no,left=100,top=100";
        let cloneTable = $("#viewtable").clone();
        cloneTable.find(".buttons-excel").remove();
        cloneTable.find(".noprint").closest('td').remove();
        let head = $('#viewtable thead tr');
        let printHeader = "";
        console.log($('#printheader').clone()[0])
        if ($('#printheader').clone()[0] != undefined) {
            printHeader = $('#printheader').clone()[0].innerHTML
        };


        cloneTable.find('input[type=text],select,textarea').each(function () {
            let elementType = $(this).prop('tagName');
            if (elementType == 'SELECT') {

                if ($(this).val() > 0)
                    let textVal = $(this).find("option:selected").text();
                else
                    textVal = '';
            } else
                let textVal = $(this).val();
            $(this).replaceWith('<label>' + textVal + '</label>');
        });
        cloneTable.find('a').each(function () {
            let anchorVal = $(this).html();
            $(this).replaceWith('<label>' + anchorVal + '</label>');
        });

        //let pageTitle = $("#title").text();
        let wWinPrint = window.open("", windowName, wOption);
        wWinPrint.document.write("<html><head><script type='text/javascript' src='/js/jquery-3.3.1.min.js'></script><link href='/css/custom.css' rel='stylesheet'><link href='/css/print.css' rel='stylesheet'><title>Enumeration Portal </title></head><body>");
        wWinPrint.document.write("<div id='header' class='mb-2 d-flex align-items-center justify-content-between'><h4 class='d-flex align-items-center'><p class='mb-0'><span >SURVEY PORTAL</span> <small class='d-block'>Odisha State Commission for Backward Classes</small> <small class='d-block'> " + printHeader + "</small></p> </h4><div class='pull-left text_logo'><h4 class='logo'><a href='javascript:void(0)' class='btn btn-success btn-sm pull-right' style='float:right;' title='Print' onclick='$(this).hide();window.print();$(this).show();'>Print</a></h4></div></div>")
        //wWinPrint.document.write("<div id='printHeader'>" + pageTitle + "</div>");
        //<img src='/images/print-logo.png' alt='Enumeration Portal ' width='70' class='mr-2'> 
        wWinPrint.document.write("<div id='printContent'>" + cloneTable.html() + "</div>");
        // wWinPrint.document.write("<div id='printFooter' class='text-center'>&copy; 2019 - GO SKILL</div>");
        wWinPrint.document.write("</body></html>");
        wWinPrint.document.close();
        wWinPrint.focus();
        return wWinPrint;
        $('#title').append(title);
        printHeader = title;
    }
    else {
        let windowName = "PrintPage";
        let wOption = "width=1000,height=600,menubar=yes,scrollbars=yes,location=no,left=100,top=100";
        let cloneTable = $("#"+tabname).clone();
        cloneTable.find(".buttons-excel").remove();
        cloneTable.find(".noprint").closest('td').remove();
        let head = $('#' + tabname+' thead tr');
        let printHeader = "";
        console.log($('#printheader').clone()[0])
        if ($('#printheader').clone()[0] != undefined) {
            printHeader = $('#printheader').clone()[0].innerHTML
        };


        cloneTable.find('input[type=text],select,textarea').each(function () {
            let elementType = $(this).prop('tagName');
            if (elementType == 'SELECT') {

                if ($(this).val() > 0)
                    let textVal = $(this).find("option:selected").text();
                else
                    textVal = '';
            } else
                let textVal = $(this).val();
            $(this).replaceWith('<label>' + textVal + '</label>');
        });
        cloneTable.find('a').each(function () {
            let anchorVal = $(this).html();
            $(this).replaceWith('<label>' + anchorVal + '</label>');
        });

        //let pageTitle = $("#title").text();
        let wWinPrint = window.open("", windowName, wOption);
        wWinPrint.document.write("<html><head><script type='text/javascript' src='/js/jquery-3.3.1.min.js'></script><link href='/css/custom.css' rel='stylesheet'><link href='/css/print.css' rel='stylesheet'><title>Enumeration Portal </title></head><body>");
        wWinPrint.document.write("<div id='header' class='mb-2 d-flex align-items-center justify-content-between'><h4 class='d-flex align-items-center'><p class='mb-0'><span >SURVEY PORTAL</span> <small class='d-block'>Odisha State Commission for Backward Classes</small> <small class='d-block'> " + printHeader + "</small></p> </h4><div class='pull-left text_logo'><h4 class='logo'><a href='javascript:void(0)' class='btn btn-success btn-sm pull-right' style='float:right;' title='Print' onclick='$(this).hide();window.print();$(this).show();'>Print</a></h4></div></div>")
        //wWinPrint.document.write("<div id='printHeader'>" + pageTitle + "</div>");
        //<img src='/images/print-logo.png' alt='Enumeration Portal ' width='70' class='mr-2'> 
        wWinPrint.document.write("<div id='printContent'>" + cloneTable.html() + "</div>");
        // wWinPrint.document.write("<div id='printFooter' class='text-center'>&copy; 2019 - GO SKILL</div>");
        wWinPrint.document.write("</body></html>");
        wWinPrint.document.close();
        wWinPrint.focus();
        return wWinPrint;
        $('#title').append(title);
        printHeader = title;
    }
    
}

function PrintPagetwo() {
    //alert();
    // alert(2);

    let windowName = "PrintPage";
    let wOption = "width=1000,height=600,menubar=yes,scrollbars=yes,location=no,left=100,top=100";
    let cloneTable = $("#viewtabletwo").clone();
    cloneTable.find(".buttons-excel").remove();
    cloneTable.find(".noprint").closest('td').remove();
    let head = $('#viewtabletwo thead tr');
    let printHeader = "";
    console.log($('#printHeadertwo').clone()[0])
    if ($('#printHeadertwo').clone()[0] != undefined) {
        printHeader = $('#printHeadertwo').clone()[0].innerHTML
    };


    cloneTable.find('input[type=text],select,textarea').each(function () {
        let elementType = $(this).prop('tagName');
        if (elementType == 'SELECT') {

            if ($(this).val() > 0)
                let textVal = $(this).find("option:selected").text();
            else
                textVal = '';
        } else
            let textVal = $(this).val();
        $(this).replaceWith('<label>' + textVal + '</label>');
    });
    cloneTable.find('a').each(function () {
        let anchorVal = $(this).html();
        $(this).replaceWith('<label>' + anchorVal + '</label>');
    });

    //let pageTitle = $("#title").text();
    let wWinPrint = window.open("", windowName, wOption);
    wWinPrint.document.write("<html><head><script type='text/javascript' src='/js/jquery-3.3.1.min.js'></script><link href='/css/custom.css' rel='stylesheet'><link href='/css/print.css' rel='stylesheet'><title>Enumeration Portal </title></head><body>");
    wWinPrint.document.write("<div id='header' class='mb-2 d-flex align-items-center justify-content-between'><h4 class='d-flex align-items-center'><p class='mb-0'><span >SURVEY PORTAL</span> <small class='d-block'>Odisha State Commission for Backward Classes</small> <small class='d-block'> Feedback Report</small></p> </h4><div class='pull-left text_logo'><h4 class='logo'><a href='javascript:void(0)' class='btn btn-success btn-sm pull-right' style='float:right;' title='Print' onclick='$(this).hide();window.print();$(this).show();'>Print</a></h4></div></div>")
    wWinPrint.document.write("<div id='printHeader'>" + printHeader + "</div>");
    //<img src='/images/print-logo.png' alt='Enumeration Portal ' width='70' class='mr-2'> 
    wWinPrint.document.write("<div id='printContent'>" + cloneTable.html() + "</div>");
    // wWinPrint.document.write("<div id='printFooter' class='text-center'>&copy; 2019 - GO SKILL</div>");
    wWinPrint.document.write("</body></html>");
    wWinPrint.document.close();
    wWinPrint.focus();
    return wWinPrint;
    $('#title').append(title);
    printHeader = title;
}

function PrintAppointmentLetter(thisHtml) {
    alert(1);
    let windowName = "PrintPage";
    let wOption = "width=1000,height=600,menubar=yes,scrollbars=yes,location=no,left=100,top=100";

    let pageTitle = $("#title").text();
    let wWinPrint = window.open("", windowName, wOption);
    wWinPrint.document.write("<html><head><script type='text/javascript' src='../js/jquery-3.3.1.min.js'></script><link href='../css/custom.css' rel='stylesheet'><link href='../css/print.css' rel='stylesheet'><title>Enumeration Portal</title></head><body>");
    wWinPrint.document.write("<div class='pull-left text_logo'><h4 class='logo'><a href='javascript:void(0)' class='btn btn-success btn-sm pull-right' style='float:right;' title='Print' onclick='$(this).hide();window.print();$(this).show();'>Print</a></h4></div>")
    wWinPrint.document.write("<div id='printHeader'>" + pageTitle + "</div>");
    wWinPrint.document.write("<div id='printContent'>" + thisHtml + "</div>");
    // wWinPrint.document.write("<div id='printFooter' class='text-center'>&copy; 2019 - GO SKILL</div>");
    wWinPrint.document.write("</body></html>");
    wWinPrint.document.close();
    wWinPrint.focus();
    return wWinPrint;
    $('#title').append(title);
    printHeader = title;
}

//// Slim scroll funtion
//(function($) {
//    $(".notifications ul,.fixed-height,.instruction-details").mCustomScrollbar({
//        theme: "inset-2-dark"
//    });

//})(jQuery);
//// Slim scroll funtion

//let windowWidth = $(window).width();
//if (windowWidth < 800) {
//    $(document).on("click", function(event) {
//        let $trigger = $(".nav-toggle-btn");
//        if ($trigger !== event.target && !$trigger.has(event.target).length) {
//            $('.page-container').addClass('display-full');
//            $('.sidemenu').removeClass('active');
//        }
//    });
//}
//if (windowWidth > 700) {
//    let ppdiv = $('.profile-info-div').height();
//    let uprofilediv = $('.user-profile').height();

//    if (ppdiv > uprofilediv) {

//        $('.user-profile').css({ 'height': ppdiv })
//    } else {
//        $('.profile-info-div').css({ 'height': uprofilediv })

//    }

//}



//$(window).scroll(function() {
//    let height = $(window).scrollTop();
//    if (height > 0) {

//        $('header').addClass('active');
//    } else {
//        $('header').removeClass('active');

//    }

//});



(function($) {
    $.fn.ResponsiveTables = function(options) {
        // This is the easiest way to have default options.
        let settings = $.extend({
            // These are the defaults.
            containerBreakPoint: 0 //allows a user to force the vertical mode at a certain pixel width of its container, in the case when a table may technically fit but you'd prefer the vertical mode
        }, options);

        rtStartingOuterWidth = $(window).width(); //used later to detect orientation change across all mobile browsers (other methods don't always work on Android)
        is_iOS = /(iPad|iPhone|iPod)/g.test(navigator.userAgent); //needed due to the fact that iOS scrolling causes false resizes
        rt_responsive_table_object = this;

        function isEmpty(el) {
            return !$.trim(el.html())
        }

        function rt_write_css(rt_class_identifier) {
            rt_css_code = '<style type="text/css">';
            $(rt_class_identifier).find('th').each(function(index, element) {
                rt_css_code += rt_class_identifier + '.vertical-table td:nth-of-type(' + (index + 1) + '):before { content: "' + $(this).text().trim() + '"; }';
            });
            rt_css_code += '</style>';
            $(rt_css_code).appendTo('head');
        }

        function determine_table_width(rt_table_object) { //outerWidth doesn't work properly in Safari if the table is overflowing its container
            rt_table_width = 0;
            if (rt_table_object.hasClass('vertical-table')) {
                rt_table_width = rt_table_object.outerWidth();
            } else {
                rt_table_object.find('th').each(function(index, element) {
                    rt_table_width += $(this).outerWidth();
                });
                rt_table_width = rt_table_width; //this seems to fix a rounding bug in firefox
            }
            return rt_table_width;
        }

        window.fix_responsive_tables = function() {
            if ($("table.responsive-table").length) {

                $("table.responsive-table").each(function(index) {
                    rt_containers_width = $(this).parent().width();
                    rt_current_width = determine_table_width($(this)) - 1; //this "-1" seems to fix an issue in firefox without harming any other browsers
                    rt_max_width = $(this).attr('data-rt-max-width');
                    rt_has_class_rt_vertical_table = $(this).hasClass('vertical-table');

                    if ($(this).attr("data-rtContainerBreakPoint")) {
                        rt_user_defined_container_breakpoint = $(this).attr("data-rtContainerBreakPoint");
                    } else {
                        rt_user_defined_container_breakpoint = settings.containerBreakPoint;
                    }

                    if (rt_containers_width < rt_current_width || rt_containers_width <= rt_user_defined_container_breakpoint) { //the parent element is less than the current width of the table or the parent element is less than or equal to a user supplied breakpoint
                        $(this).addClass('vertical-table'); //switch to vertical orientation (or at least keep it that orientation)

                        if (rt_max_width > rt_current_width && rt_max_width > rt_user_defined_container_breakpoint) { //the max width was set too high and needs to be adjusted to this lower number
                            $(this).attr('data-rt-max-width', rt_current_width);
                        } else if (rt_max_width > rt_current_width && rt_max_width <= rt_user_defined_container_breakpoint) { //same as above but in this case the breakpoint is larger or equal so it needs to be set as the max width
                            $(this).attr('data-rt-max-width', rt_user_defined_container_breakpoint);
                        }

                    } else if (rt_containers_width > rt_max_width && rt_containers_width > rt_user_defined_container_breakpoint) { //the parent element is bigger than the max width and user supplied breakpoint
                        $(this).removeClass('vertical-table'); //switch to horizontal orientation (or at least keep it that orientation)

                        if ((rt_max_width > rt_current_width && !rt_has_class_rt_vertical_table) && (rt_max_width > rt_user_defined_container_breakpoint && !rt_has_class_rt_vertical_table)) { //max width is greater than the table's current width and it's in horizontal mode currently...so the max width was set to low and needs to be adjusted to a higher number
                            $(this).attr('data-rt-max-width', rt_current_width);
                        } else if ((rt_max_width > rt_current_width && !rt_has_class_rt_vertical_table) && (rt_max_width <= rt_user_defined_container_breakpoint && !rt_has_class_rt_vertical_table)) { //same as above but in this case the user supplied breakpoint is larger or equal so it needs to be set as the max width
                            $(this).attr('data-rt-max-width', rt_user_defined_container_breakpoint);
                        }

                    } else { //equal

                    }
                });
            }
        }

        rt_responsive_table_object.each(function(index, element) {
            $(this).addClass('responsive-table-' + index).addClass('responsive-table');
            $(this).find('tbody > tr > td').wrapInner('<div class="responsive-container"></div>');
            if (index == rt_responsive_table_object.length - 1) {
                $(window).resize(function() {
                    if (!is_iOS || (is_iOS && (rtStartingOuterWidth !== $(window).width()))) {
                        rtStartingOuterWidth = $(window).width(); //MUST update the starting width so future orientation changes will be noticed
                        fix_responsive_tables();
                    }
                });
                rt_responsive_table_count = $('table.responsive-table').length;
                $('table.responsive-table').each(function(index2, element2) {
                    rt_write_css('table.responsive-table-' + index2);
                    $('table.responsive-table-' + index2).attr('data-rt-max-width', determine_table_width($(this)));
                    $(this).find("td,th").each(function(index3, element3) { //empty td tags made them disappear
                        if (isEmpty($(this))) {
                            $(this).html("&#160;");
                        }
                    });
                    if (rt_responsive_table_count - 1 == index2) {
                        fix_responsive_tables();
                    }
                });
            }
        });

        return this;
    };
}(jQuery));


function IsNumber(evt) {
    let charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


function IsDecimal(e, o) {
    let character = String.fromCharCode(e.keyCode)
    let newValue = o.value + character;
    if (isNaN(newValue) || hasDecimalPlace(newValue, 3)) {
        e.preventDefault();
        return false;
    }
}


function hasDecimalPlace(value, x) {
    let pointIndex = value.indexOf('.');
    return pointIndex >= 0 && pointIndex < value.length - x;
}

function IsCharacter(e) {
    if (!(/[a-z ]/i.test(String.fromCharCode(e.keyCode)))) {
        e.preventDefault();
        return false;
    }
    return true;
}
function isEmail(email) {
    let regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function isMobile(mobile) {
    let regex = /^\d*(?:\.\d{1,2})?$/;
    return regex.test(mobile);
}
let dateFormat = function () {
    let token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        let dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        let _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};

