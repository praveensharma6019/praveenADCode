$('#dep_flight_search').keyup(function(){
        if(!$(this).val()){
        $('#arrival_form button').attr('disabled','true');
        } else{
           $('#arrival_form button').removeAttr('disabled');
        }
    });


$('#rec_username').keyup(function(){
        if(!$(this).val()){
        $('#departure_from button').attr('disabled','true');
        } else{
           $('#departure_from button').removeAttr('disabled');
        }
    });
	     $('#arrival_form button').attr('disabled','true');
 $('#departure_from button').attr('disabled','true');