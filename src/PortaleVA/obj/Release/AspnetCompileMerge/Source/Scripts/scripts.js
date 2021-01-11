/*** Hack per impedire a ie7 di usare la versione responsive ***/

//if (bowser.msie && bowser.version <= 7) {
//    var resCSS = document.getElementById("responsiveCSS");
//    resCSS.style.disabled = true;
//}

var MSIE = navigator.userAgent.match(/MSIE|Trident/i) != null;


function SetVieportWidth(event) {
    try {
        //var iPhone = navigator.userAgent.match(/iPhone|iPad|iPod/i) != null;
        var ora = window.orientation || (screen.orientation && screen.orientation.angle) || (window.innerWidth > window.innerHeight ? 90 : 0);
        var cw = Math.max(470, Math.abs(ora) == 90 ? Math.max(screen.height, screen.width) : Math.min(screen.height, screen.width));
        //var cw = Math.max(470, iPhone && (ori == 90 || ori == -90) ? screen.height : screen.width);
        //$('#debug').append(
        //    'e:' + event && event.type +
        //    ' , ora:' + ora +
        //    ' , sw:' + screen.width +
        //    ' , sh:' + screen.height +
        //    ' , cw:' + cw +
        //    '<br>');
        document.querySelector("meta[name=viewport]").setAttribute('content', 'width=' + cw + ' ,maximum-scale=1');
    } catch (err) {
        //$('#debug').append('e:' + (event && event.type) + 'err: ' + err.message + '<br>');
    }
}
//SetVieportWidth();
//$(window).bind('ready resize orientationchange', SetVieportWidth);

function FixTdHeight() {
    $("TABLE.table TR").each(function (i, e) {
        var tr = $(e);
        var heights = tr.children("TD:not(:first)").map(function () { return $(this).height(); }).get();
        var maxHeight = Math.max.apply(null, heights) - 1;
        var td = tr.children("TD:first");
        td.length && $(td.children("DIV")[0] || td.wrapInner("<div></div>").children("DIV")[0]).css("min-height", (maxHeight) + "px");
    })
}
MSIE && $(window).bind('ready resize orientationchange', FixTdHeight);

$(document).ready(function () {

    $('#cssmenu #menu-button').on('click', function () {
        var menu = $(this).next('ul');
        if (menu.hasClass('open')) {
            menu.removeClass('open');
        }
        else {
            menu.addClass('open');
        }
    });

    if ($("#navigation").length > 0)
        $("#navigation").treeview({
            persist: "location",
            animated: "fast",
            collapsed: true,
            persist: "cookie",
            cookieId: $('#navigation').attr("class").split(' ')[0]
        });

    $('.form-control.it').attr("placeholder", "Cerca nel sito...");
    $('.form-control.en').attr("placeholder", "Search this site...");

    $('.form-control.vipera.it').attr("placeholder", "ID_VIP");
    $('.form-control.vipera.en').attr("placeholder", "ID_VIP");


    /*** Tooltip ***/
    if ($(".someClass, .toolTipDialog").length > 0) {
        $(".someClass, .toolTipDialog").tipTip({ fadeIn: 300, maxWidth: "450px" });
    }

    $("a.external").attr('target', '_blank');

    $("A[data-toggle='modal']").click(function () {
        $($(this).data('target')).dialog('open');
    }).each(function (i, e) {
        $($(e).data('target')).dialog({ autoOpen: false, modal: true, show: "fade", hide: "fade" })
    });

});

/**** modale *****/

$(".modal").appendTo("body");

/**** accordion procedure *****/
$(document).ready(function () {
    var parentRow = null;
    var tr = null;
    $(".procedura").click(function () {
        parentRow = $(this).parent();
        tr = parentRow.nextUntil(".trProcedura");
        tr.toggle("slow");
        parentRow.toggleClass("trClasseAttiva");
    });
});

/**** link verso cart.ancitel.it *****/
$(document).ready(function () {
    var linksToCartAncitel = $('a[href*="cart.ancitel.it"]');
    var linksToSinvaAncitel = $('a[href*="sinva.minambiente.it"]');
    var currentLanguage = $('html')[0].lang;

    linksToCartAncitel.each(function (index) {
        $(this).attr("href", $(this).attr("href") + "&l=" + currentLanguage);
    });

    linksToSinvaAncitel.each(function (index) {
        $(this).attr("href", $(this).attr("href") + "&l=" + currentLanguage);
    });
});

/**** autopost casella a discesa statistiche procedure *****/
$(document).ready(function () {
    $("#Anno").change(function () {
        $('#frmStatistiche').submit();
    });

});



