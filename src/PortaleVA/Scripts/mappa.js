// JavaScript Document    
	var regioneSelezionata = "";
    var mappa = null;
    var mappaSelezionata = null;


    $(document).ready(function () {
		var urlImg = $(".Map img").attr("src");
		//$(".Map img").attr("src", urlImg.split(".")[0] + ".gif");
		$(".Map img").attr("src", urlImg.replace("cartina-regioni.png","dot.gif"));
		
		var top = $('.Map').offset().top - parseFloat($('.Map').css('marginTop').replace(/auto/, 0));
  		
		$(window).scroll(function (event) {
    		// what the y position of the scroll is
   			var y = $(this).scrollTop();
  
    		// whether that's below the form
    		if (y >= top) {
      			// if so, ad the fixed class
      			$('.Map').addClass('fixed');
    		} else {
      			// otherwise remove it
      			$('.Map').removeClass('fixed');
    		}
  		});
		
        /* INIT VARIABILI */
        regioneSelezionata = getParameterByName('searchRegion');
        mappa = $("#region_map");
        mappaSelezionata = $("#region_map_selected");

        if (regioneSelezionata != '') {
            mappaSelezionata.addClass(getClassFromRegion(regioneSelezionata));
            mappaSelezionata.addClass('sprite_region');
        }
		$("#Map").children().each(function(index, currentElem){
			//$(currentElem).attr("onmouseover","selectRegion("+(index+1)+");");
			//$(currentElem).attr("onmouseout","unselectRegion("+(index+1)+");");
			
			$(currentElem).mouseover(function() {
       			mappa.addClass('sprite_region_' + (index+1));
        		mappa.addClass('sprite_region');
        		if (regioneSelezionata != '') {
            		//alert(regioneSelezionata);
            		mappaSelezionata.removeClass();
            		mappaSelezionata.addClass(getClassFromRegion(regioneSelezionata));
            		mappaSelezionata.addClass('sprite_region');
        		}
        		return true;
    		});
			
			$(currentElem).mouseout(function () {
        		mappa.removeClass();
        		if (regioneSelezionata != '') {
            		mappaSelezionata.addClass(getClassFromRegion(regioneSelezionata));
        		}
        		return true;
    		});
			
			nome = $(currentElem).attr("alt");
			$(currentElem).attr("title", nome);
			
			nomeNormalizzato = nome.toLowerCase();
			nomeNormalizzato = nomeNormalizzato.split(".").join("-");
			nomeNormalizzato = nomeNormalizzato.split(" ").join("-");
			nomeNormalizzato = nomeNormalizzato.split("'").join("-");
			$(currentElem).attr("id", "area_"+nomeNormalizzato);
			
		});
		
		// id="area_albagiara" title="Albagiara" 
		var mapLink = ".menuRegioni a";
		
		
		$(mapLink).each(function(index){
			$(this).attr("id",$(this).text());
		});
		
		
		$(mapLink).mouseover(function() {
			
			n = $(this).attr("id");
			
			nN = n.toLowerCase();
			nN = nN.split(" ").join("-");
			nN = nN.split(".").join("-");
			nN = nN.split("'").join("-");
			//alert(nN);
  			$("#area_"+nN).mouseover();
		});
		
		$(mapLink).mouseout(function() {
			
			n = $(this).attr("id");
			
			nN = n.toLowerCase();
			nN = nN.split(".").join("-");
			nN = nN.split(" ").join("-");
			nN = nN.split("'").join("-");
  			$("#area_"+nN).mouseout();
		});
		
    });

    function selectRegion(region) {
        mappa.addClass('sprite_region_' + region);
        mappa.addClass('sprite_region');
        if (regioneSelezionata != '') {
            //alert(regioneSelezionata);
            mappaSelezionata.removeClass();
            mappaSelezionata.addClass(getClassFromRegion(regioneSelezionata));
            mappaSelezionata.addClass('sprite_region');
        }
        return true;
    }

    function unselectRegion(region) {
        mappa.removeClass();
        if (regioneSelezionata != '') {
            mappaSelezionata.addClass(getClassFromRegion(regioneSelezionata));
        }
        return true;
    }

    function getClassFromRegion(region)
    {
        var base = "sprite_region_";
        switch(region)
        {
            case "Albagiara":
                base = base + "1Selected";
                break;
            case "piemonte":
                base = base + "2Selected";
                break;
            case "liguria":
                base = base + "3Selected";
                break;
            case "lombardia":
                base = base + "4Selected";
                break;
            case "trentino":
                base = base + "5Selected";
                break;
            case "veneto":
                base = base + "6Selected";
                break;
            case "friuli":
                base = base + "7Selected";
                break;
            case "emilia":
                base = base + "8Selected";
                break;
            case "toscana":
                base = base + "9Selected";
                break;
            case "umbria":
                base = base + "10Selected";
                break;
            case "lazio":
                base = base + "11Selected";
                break;
            case "marche":
                base = base + "12Selected";
                break;
            case "abruzzo":
                base = base + "13Selected";
                break;
            case "molise":
                base = base + "14Selected";
                break;
            case "campania":
                base = base + "15Selected";
                break;
            case "puglia":
                base = base + "16Selected";
                break;
            case "basilicata":
                base = base + "17Selected";
                break;
            case "calabria":
                base = base + "18Selected";
                break;
            case "sardegna":
                base = base + "19Selected";
                break;
            case "sicilia":
                base = base + "20Selected";
                break;
            default:
                base = "";
                break;
        }
        return base;
    }

    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }