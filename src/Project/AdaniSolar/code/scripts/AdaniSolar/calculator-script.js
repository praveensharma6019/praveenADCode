var states_data = {
    'pb' : {
		'name' : 'Punjab',
        'avg_solar_irradiation' : {
			'value' : 1156.39,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 4.6,
			'unit' : 'kWh'
		} 
        
    },
	'hr' : {
		'name' : 'Haryana',
        'avg_solar_irradiation' : {
			'value' : 1156.39,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 4.6,
			'unit' : 'kWh'
		} 
        
    },
	'up' : {
		'name' : 'Uttar Pradesh',
        'avg_solar_irradiation' : {
			'value' : 1156.39,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 4.6,
			'unit' : 'kWh'
		} 
        
    },
	'mp' : {
		'name' : 'Madhya Pradesh',
        'avg_solar_irradiation' : {
			'value' : 1266.52,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 5,
			'unit' : 'kWh'
		} 
        
    },
	'gj' : {
		'name' : 'Gujarat',
        'avg_solar_irradiation' : {
			'value' : 1266.52,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 5,
			'unit' : 'kWh'
		} 
        
    },
	'rj' : {
		'name' : 'Rajasthan',
        'avg_solar_irradiation' : {
			'value' : 1266.52,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 5,
			'unit' : 'kWh'
		} 
        
    },
	'mh' : {
		'name' : 'Maharashtra',
        'avg_solar_irradiation' : {
			'value' : 1266.52,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 5,
			'unit' : 'kWh'
		} 
        
    },'ap' : {
		'name' : 'Andhra Pradesh',
        'avg_solar_irradiation' : {
			'value' : 1266.52,
			'unit' : 'W / sq.m'
		},
		'elec_generated_per_kwp_rftp_plant' : {
			'value' : 5,
			'unit' : 'kWh'
		} 
        
    }
};


$( document ).ready(function() {	
	//using drop down
    $('#states').change(function(){
		var state_id = $("#states :selected").val();
		
		if(state_id == "rj"){
			//$("#main_india_map").attr('src', 'images/India-map-en_rj.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').show();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
			
		}else if(state_id == "mp"){
			//$("#main_india_map").attr('src', 'images/India-map-en_mp.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').show();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
		}else if(state_id == "mh"){
			//$("#main_india_map").attr('src', 'images/India-map-en_mh.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').show();
			$('#main_india_map_ap').hide();
		}else if(state_id == "ap"){
			//$("#main_india_map").attr('src', 'images/India-map-en_ap.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').show();
		}else if(state_id == "gj"){
			//$("#main_india_map").attr('src', 'images/India-map-en_gj.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').show();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
		}
		
		//$('.states').hide();
		$("#state_info").html("<p> Average solar irradiation in <span><b>" +states_data[state_id]['name']+" </span></b>is <span><b>"+states_data[state_id]['avg_solar_irradiation']['value']+" W / sq.m</span></b><b/></p><p>1kWp solar rooftop plant will generate "+states_data[state_id]['elec_generated_per_kwp_rftp_plant']['value']+" kWh of electricity per day (considering 5.5 sunshine hours)</p>" );
		$('#state_info').show();
		$('#solarForm').slideDown( "slow", function() {
			// Animation complete.
		});
		$( "#solarInfo" ).slideUp( "slow", function() {
			// Animation complete.
		});
	});
	
	//using map
	
	$('.state_map').click(function(ev){
		var state_id = $(this).attr('id');
		$('#states').val(state_id);
		if(state_id == "rj"){
			//$("#main_india_map").attr('src', 'images/India-map-en_rj.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').show();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
			
		}else if(state_id == "mp"){
			//$("#main_india_map").attr('src', 'images/India-map-en_mp.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').show();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
		}else if(state_id == "mh"){
			//$("#main_india_map").attr('src', 'images/India-map-en_mh.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').show();
			$('#main_india_map_ap').hide();
		}else if(state_id == "ap"){
			//$("#main_india_map").attr('src', 'images/India-map-en_ap.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').hide();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').show();
		}else if(state_id == "gj"){
			//$("#main_india_map").attr('src', 'images/India-map-en_gj.png');
			$('#main_india_map').hide();
			$('#main_india_map_rj').hide();
			$('#main_india_map_gj').show();
			$('#main_india_map_mp').hide();
			$('#main_india_map_mh').hide();
			$('#main_india_map_ap').hide();
		}
		//alert($(this).attr('id'));
		
		$("#state_info").html("<p> Average solar irradiation in <span><b>" +states_data[state_id]['name']+" </span></b>is <span><b>"+states_data[state_id]['avg_solar_irradiation']['value']+" W / sq.m</span></b><b/></p><p>1kWp solar rooftop plant will generate "+states_data[state_id]['elec_generated_per_kwp_rftp_plant']['value']+" kWh of electricity per day (considering 5.5 sunshine hours)</p>" );
		$('#state_info').show();
		$('#solarForm').slideDown( "slow", function() {
			// Animation complete.
		});
		$( "#solarInfo" ).slideUp( "slow", function() {
			// Animation complete.
		});
		
		
		
		ev.preventDefault();
	});
	
	$('input[name=estimation_base]').click(function(){ 
		if($(this).val()=="roof_top"){
			$("input[name=roof_top_area]").prop('disabled', false);
			$("input[name=roof_top_area_percentage]").prop('disabled', false);
			$("input[name=install_capacity]").prop('disabled', true);
			$("input[name=your_budget]").prop('disabled', true);
			//clear other values
			$("input[name=install_capacity]").val("");
			$("input[name=your_budget]").val("");
			
		}else if($(this).val()=="capacity"){
			$("input[name=roof_top_area]").prop('disabled', true);
			$("input[name=install_capacity]").prop('disabled', false);
			$("input[name=your_budget]").prop('disabled', true);
			$("input[name=roof_top_area_percentage]").prop('disabled', true);
			
			//clear other values
			$("input[name=roof_top_area]").val("");
			$("input[name=your_budget]").val("");
			$("input[name=roof_top_area_percentage]").val("");
		}else{
			$("input[name=roof_top_area]").prop('disabled', true);
			$("input[name=install_capacity]").prop('disabled', true);
			$("input[name=your_budget]").prop('disabled', false);
			$("input[name=roof_top_area_percentage]").prop('disabled', true);
			
			//clear other values
			$("input[name=roof_top_area]").val("");
			$("input[name=install_capacity]").val("");
			$("input[name=roof_top_area_percentage]").val("");
		}	
	});
	
	//setting default electricity rate
	
	var residential_rate = 6.75;
	var industry_rate = 10.5;
	var commercial_rate = 8.75;
	var institute_rate = 6.75;
	
	//annual Escalation Rate
	var annual_escalation_rate = 4;
	
	
	$('#annualEscalationRate').val(annual_escalation_rate);
	
	$('#resi').val(residential_rate);
	$('#insti').val(institute_rate);
	$('#ind').val(industry_rate);
	$('#comm').val(commercial_rate);
	
	$('#customerType').change(function (){ 
		$('#electricityCost').val($( "#customerType option:selected" ).val());
		
	});
	
	//last section of the form
	
	
	
});

//script for section 3
$( document ).ready(function() {
	
	// last section constants
	var solar_plant_cost = 80000;
	var subsidy = 30;
	var debt = 70;
	var equity = 30;
	var down_pymt = 24000;
	var loan_amount = 56000;
	var loan_interest = 12;
	var loan_period = 5;
	var discount_rate = 10.81;
	
	$("input[name=solar_plant_cost]").val(solar_plant_cost);
	$("input[name=subsidy]").val(subsidy);
	$("input[name=debt]").val(debt);
	$("input[name=equity]").val(equity);
	$("input[name=dwn_pymt]").val(down_pymt);
	$("input[name=loan_amt]").val(loan_amount);
	$("input[name=interest_rate]").val(loan_interest);
	$("input[name=period]").val(loan_period);
	$("input[name=discount_rate]").val(discount_rate);
	
	
	
	//on changeing debt
	$('input[name=debt]').change(function(){
		
		if($('input[name=debt]').val() < 100) 
			$('input[name=equity]').val(100 - $('input[name=debt]').val());
		else{
			$('input[name=debt]').val("100");
			$('input[name=equity]').val(0);
		}
		
		var down_payment = $('input[name=solar_plant_cost]').val() * $('input[name=equity]').val()/100;
		$('input[name=dwn_pymt]').val(down_payment);
		
		var loan_amount = $('input[name=solar_plant_cost]').val() * $('input[name=debt]').val()/100;
		$('input[name=loan_amt]').val(loan_amount);
		
		
	});
	
	//on changning equity 
	$('input[name=equity]').change(function(){
		if($('input[name=equity]').val() < 100) 
			$('input[name=debt]').val(100 - $('input[name=equity]').val());
		else{
			$('input[name=equity]').val("100");
			$('input[name=debt]').val(0);
		}
		
		var down_payment = $('input[name=solar_plant_cost]').val() * $('input[name=equity]').val()/100;
		$('input[name=dwn_pymt]').val(down_payment);
		
		var loan_amount = $('input[name=solar_plant_cost]').val() * $('input[name=debt]').val()/100;
		$('input[name=loan_amt]').val(loan_amount);
		
	});

	//on changing dwn pymt
	$('input[name=dwn_pymt]').change(function(){
		if($('input[name=dwn_pymt]').val() < $('input[name=solar_plant_cost]').val()) 
			$('input[name=loan_amt]').val($('input[name=solar_plant_cost]').val() - $('input[name=dwn_pymt]').val());
		else{
			$('input[name=dwn_pymt]').val($('input[name=solar_plant_cost]').val());
			$('input[name=loan_amt]').val(0);
		}
		
		$('input[name=equity]').val($('input[name=dwn_pymt]').val()*100/$('input[name=solar_plant_cost]').val());
		$('input[name=debt]').val($('input[name=loan_amt]').val()*100/$('input[name=solar_plant_cost]').val());
	});
	
	// on changing loan amount
	$('input[name=loan_amt]').change(function(){
		if($('input[name=loan_amt]').val() < $('input[name=solar_plant_cost]').val()) 
			$('input[name=dwn_pymt]').val($('input[name=solar_plant_cost]').val() - $('input[name=loan_amt]').val());
		else{
			$('input[name=loan_amt]').val($('input[name=solar_plant_cost]').val());
			$('input[name=dwn_pymt]').val(0);
		}
		
		$('input[name=equity]').val($('input[name=dwn_pymt]').val()*100/$('input[name=solar_plant_cost]').val());
		$('input[name=debt]').val($('input[name=loan_amt]').val()*100/$('input[name=solar_plant_cost]').val());
	});

});


//submitting the form
$( document ).ready(function() {
	
	$('#form_submit').click(function(event){
		//validations 
			if($('#roof_top_area').val() =="" && $('#install_capacity').val()=="" && $('#budget').val()==""){
				var dialog = new Messi('Please enter one of the Total roof area, Capacity you want to install or Budget.');
				return;
			}
			
			if($('#roof_top_area').val() !=""){
				if(!$.isNumeric($('#roof_top_area').val())) {
					var dialog = new Messi('Roof top area should be numeric be numeric only');
					return;
				}
			}
			
			if($('#install_capacity').val() !=""){
				if(!$.isNumeric($('#install_capacity').val())) {
					var dialog = new Messi('Budget should be numeric be numeric only');
					return;
				}
			}
			
			if($('#budget').val() !=""){
				if(!$.isNumeric($('#budget').val())) {
					var dialog = new Messi('Budget should be numeric be numeric only');
					return;
				}
			}
			
			if($('#roof_top_area').val() !="" && $('#roof_top_area_percentage').val() =="" ){
				var dialog = new Messi('Please enter one of the roof area percentage');
				return;
			}
			
			if($('#roof_top_area_percentage').val() !=""){
				if(!$.isNumeric($('#roof_top_area_percentage').val())) {
					var dialog = new Messi('Roof top area percentage should be numeric be numeric only');
					return;
				}
			}
			
			
			if($('#customerType').val()=="" || $('#electricityCost').val()==""){
				var dialog = new Messi('Please select customer type or give electricity cost.');
				return;
			}else if(!$.isNumeric($('#electricityCost').val())) {
				var dialog = new Messi('Electricity cost should be numeric');
				return;
			};
			
			if($('#annualEscalationRate').val()=="" ){
				var dialog = new Messi('Please enter annual escalation rate .');
				return;
			}else if(!$.isNumeric($('#annualEscalationRate').val())) {
				var dialog = new Messi('Annual escalation rate should be numeric');
				return;
			};
			
			if($('#solar_plant_cost').val()=="" ){
				var dialog = new Messi('Please enter solar plant cost .');
				return;
			}else if(!$.isNumeric($('#solar_plant_cost').val())) {
				var dialog = new Messi('solar plant cost should be numeric');
				return;
			};
			
			if($('#subsidy').val()=="" ){
				var dialog = new Messi('Please enter subsidy .');
				return;
			}else if(!$.isNumeric($('#subsidy').val())) {
				var dialog = new Messi('subsidy should be numeric');
				return;
			};
			
			if($('#subsidy').val()=="" ){
				var dialog = new Messi('Please enter subsidy .');
				return;
			}else if(!$.isNumeric($('#subsidy').val())) {
				var dialog = new Messi('subsidy should be numeric');
				return;
			};
			
			if($('#debt').val()=="" || $('#equity').val()==""){
				var dialog = new Messi('Please enter debt and equity ratio .');
				return;
			}else if(!$.isNumeric($('#debt').val()) && !$.isNumeric($('#debt').val())) {
				var dialog = new Messi('Equity debt ratio should be numeric');
				return;
			};
			
			if($('#dwn_pymt').val()=="" ){
				var dialog = new Messi('Please enter Down payment .');
				return;
			}else if(!$.isNumeric($('#dwn_pymt').val())) {
				var dialog = new Messi('Down Payment should be numeric');
				return;
			};		
			
			if($('#loan_amt').val()=="" ){
				var dialog = new Messi('Please enter loan amount.');
				return;
			}else if(!$.isNumeric($('#loan_amt').val())) {
				var dialog = new Messi('Loan amount should be numeric');
				return;
			};	
			
			if($('#loan_amt').val()=="" ){
				var dialog = new Messi('Please enter loan amount.');
				return;
			}else if(!$.isNumeric($('#loan_amt').val())) {
				var dialog = new Messi('Loan amount should be numeric');
				return;
			};	
			
			if($('#interest_rate').val()=="" ){
				var dialog = new Messi('Please enter interest rate.');
				return;
			}else if(!$.isNumeric($('#interest_rate').val())) {
				var dialog = new Messi('interest rate should be numeric');
				return;
			};
			
			if($('#period').val()=="" ){
				var dialog = new Messi('Please enter loan period.');
				return;
			}else if(!$.isNumeric($('#period').val())) {
				var dialog = new Messi('Loan period should be numeric');
				return;
			}; 
			
			if($('#discount_rate').val()=="" ){
				var dialog = new Messi('Please enter Discount rate.');
				return;
			}else if(!$.isNumeric($('#discount_rate').val())) {
				var dialog = new Messi('Discount rate should be numeric');
				return;
			};
		//validations end here
		
		
		var per_kw_plant_cost = 80000;
		var constant_for_sqm = 10 ;
		var constant_for_sqft = 10.784 ;
		var solar_radiance = 5.5;
		var min_efficiency_expected = 0.85;
		var subsidy = 30;
		var life_time = 25;
		var co2_mitigation_constant = 0.82;
		var subsidy_cap = 73000;
		var subsidy_amount_above_cap = 22000;
		//var plant_size = $('#plant_size').text();
		
		
		//clicking on estimation_base
		
		
		
		
		
		//calculating total capacity
		var capacity=0;
		if($('input[name=estimation_base]:checked').val()=='roof_top'){ //user has given roof top area
			
			//clear other values if filled
			
			
			if($('input[name=roof_top_area_unit]:checked').val()=='sqm'){
				capacity = ($('#roof_top_area').val()*$('#roof_top_area_percentage').val()/100)/constant_for_sqm;
				//alert(capacity);
			}else{
				capacity = ($('#roof_top_area').val()*$('#roof_top_area_percentage').val()/100)/constant_for_sqft;
			}
		}
		else if($('input[name=estimation_base]:checked').val()=='capacity'){ // user has given capacity
			capacity = $('#install_capacity').val();
		}
		else{ // user had given budget
			capacity = $('#budget').val()/per_kw_plant_cost*capacity;
		}
		
		$('#cost_without_subsidy').text(Math.round(per_kw_plant_cost*capacity));
		
		//if amount is more than 73000 subsity is 22000
		var actual_subsidy=0;
		if(per_kw_plant_cost*capacity > subsidy_cap){
			actual_subsidy = subsidy_amount_above_cap;
		}else{
			actual_subsidy = (per_kw_plant_cost*capacity)*subsidy/100;
		}
		
		$('#cost_with_subsidy').text(Math.round((per_kw_plant_cost*capacity) - actual_subsidy));
		
		$('#plant_size').text(Math.round(capacity)); // plant size is capacity
		
		var annual_generate_electricity = Math.round(capacity*solar_radiance*min_efficiency_expected); // supposing it is yearly
		
		//alert('total elect' + annual_generate_electricity);
		
		$('#annual_generated_elec').text(annual_generate_electricity);
		$('#life_time_generated_elec').text(annual_generate_electricity*life_time);
		
		//without increasing tarrif
		var x = 1;
		var total_saved_electricity_per_year = Math.round(annual_generate_electricity * $('#electricityCost').val() * x);
		//alert(total_saved_electricity_per_year);
		$('#saving_without_increasing_tarrif_annualy').text(total_saved_electricity_per_year.toString());
		$('#saving_without_increasing_tarrif_monthly').text(Math.round(total_saved_electricity_per_year/12));
		$('#saving_without_increasing_tarrif_life_time').text(Math.round(total_saved_electricity_per_year*life_time)); 
		
		//with increasing tarrif
		var x = (1+(0.6 * $('#annualEscalationRate').val()));
		var total_saved_electricity_per_year = Math.round(annual_generate_electricity * $('#electricityCost').val() * x); // because only on year is there.
		$('#saving_with_increasing_tarrif_annually').text(total_saved_electricity_per_year);
		
		var total_saved_electricity_life_time = Math.round(annual_generate_electricity * $('#electricityCost').val() * Math.pow(x, life_time));
		$('#saving_with_increasing_tarrif_life_time').text(total_saved_electricity_life_time);
		
		
		
		// for EMI
		$('#loan_amount').text($('#loan_amt').val());
		$('#loan_period').text($('#period').val());
		$('#loan_interest_rate').text($('#interest_rate').val());
		
		var emi=0;
		var P = $('#loan_amt').val();
		var r = $('#interest_rate').val()/(12*100);
		var N = $('#period').val()*12;
		emi = Math.round(P*r*Math.pow(1+r, N)/((Math.pow(1+r, N))-1));
		
		$('#emi').text(emi);
		
		
		//co2 mitigation
		var co2_mitigated = Math.round((annual_generate_electricity*life_time*co2_mitigation_constant)/1000);
		$('#co2_mitigated').text(co2_mitigated);
		//tree plantation
		$('#tree_plant').text(Math.round(co2_mitigated*1.6));
		
		//pay back
		$('#self_without_subsidy_present').text(Math.round($('#cost_without_subsidy').text()/$('#emi').text()));
		$('#self_with_subsidy_present').text(Math.round($('#cost_with_subsidy').text()/$('#emi').text()));
		
		
		
		event.preventDefault();
		$( "#solarForm" ).slideUp( "slow", function() {
			// Animation complete.
		});
		$( "#solarInfo" ).slideDown( "slow", function() {
			// Animation complete.
		});
	});
});


//responsivness

$( document ).ready(function() {
	if ($(window).width() < 786) {
	  $('#mapArea').hide();
	  $('.contact_us').show();
	}
	else {
	   //alert('More than 960');
	}
});
