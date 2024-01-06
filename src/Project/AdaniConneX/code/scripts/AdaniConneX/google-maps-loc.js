// function initMap() {
  // const map = new google.maps.Map(document.getElementById("map"), {
    // zoom: 3,
    // center: { lat: -28.024, lng: 140.887 },
  // });
  // const infoWindow = new google.maps.InfoWindow({
    // content: "",
    // disableAutoPan: true,
  // });
  // // Create an array of alphabetical characters used to label the markers.
  // const labels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  // // Add some markers to the map.
  // const markers = locations.map((position, i) => {
    // const label = labels[i % labels.length];
    // const marker = new google.maps.Marker({
      // position,
      // label,
    // });

    // // markers can only be keyboard focusable when they have click listeners
    // // open info window when marker is clicked
    // marker.addListener("click", () => {
      // infoWindow.setContent(label);
      // infoWindow.open(map, marker);
    // });
    // return marker;
  // });

  // // Add a marker clusterer to manage the markers.
  // //new MarkerClusterer({ markers, map });
  // var testing = new markerClusterer.MarkerClusterer({ map, markers });
// }

// const locations = [
  // { lat: -31.56391, lng: 147.154312 },
  // { lat: -33.718234, lng: 150.363181 },
  // { lat: -33.727111, lng: 150.371124 },
  // { lat: -33.848588, lng: 151.209834 },
  // { lat: -33.851702, lng: 151.216968 },
  // { lat: -34.671264, lng: 150.863657 },
  // { lat: -35.304724, lng: 148.662905 },
  // { lat: -36.817685, lng: 175.699196 },
  // { lat: -36.828611, lng: 175.790222 },
  // { lat: -37.75, lng: 145.116667 },
  // { lat: -37.759859, lng: 145.128708 },
  // { lat: -37.765015, lng: 145.133858 },
  // { lat: -37.770104, lng: 145.143299 },
  // { lat: -37.7737, lng: 145.145187 },
  // { lat: -37.774785, lng: 145.137978 },
  // { lat: -37.819616, lng: 144.968119 },
  // { lat: -38.330766, lng: 144.695692 },
  // { lat: -39.927193, lng: 175.053218 },
  // { lat: -41.330162, lng: 174.865694 },
  // { lat: -42.734358, lng: 147.439506 },
  // { lat: -42.734358, lng: 147.501315 },
  // { lat: -42.735258, lng: 147.438 },
  // { lat: -43.999792, lng: 170.463352 },
// ];

// // import { MarkerClusterer } from "@googlemaps/markerclusterer";

//Initialize and add the map
var newMapType;
function initMap() {
  // The location of Uluru
  const uluru = { lat: 12.821503, lng: 80.225235 };
  // The map, centered at Uluru
  const map = new google.maps.Map(document.getElementById("map"), {
    zoom: 10,
    center: uluru,
	//mapTypeControl: false,
	//mapTypeId: google.maps.MapTypeId.ROADMAP,
	streetViewControl: false
  });

  // The marker, positioned at Uluru
  const marker = new google.maps.Marker({
    position: uluru,
    map: map,	
	icon: "https://stage.adaniconnex.com/images/ACX Chennai pointer.svg"
  });
	google.maps.event.addListener(marker, 'click', (function(marker) {
        return function() {
          infowindow.setContent('AdaniConnex Data Center');
          infowindow.open(map, marker);
        }
      })(marker)); 
	google.maps.event.addListener(map, "maptypeid_changed", function() {
		 var selectedMapType = map.getMapTypeId();
		 newMapType=selectedMapType;
	});	  
}

function initMapLocations(locate) {
  var map = new google.maps.Map(document.getElementById('map'), {
      zoom: 9,
      center: new google.maps.LatLng(12.821503, 80.225235),
      //mapTypeControl: false,
	  mapTypeId: newMapType,
	  streetViewControl: false
    });
	
    var infowindow = new google.maps.InfoWindow();
	const uluru = { lat: 12.821503, lng: 80.225235 };
    var marker, i;
    var markers=[];
    for (i = 0; i < locate.length; i++) {  
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(locate[i][1], locate[i][2]),
        map: map
      });
      
      google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
          infowindow.setContent('<p>' + locate[i][0]+ '<br />' + locate[i][3] + '</p>');
          infowindow.open(map, marker);
        }		
      })(marker, i));
	  infowindow.setContent('<p>' + locate[i][0]+ '<br />' + locate[i][3] + '</p>');	  
	  markers.push(marker);
	  infowindow.open(map,marker);
    }
	google.maps.event.addListener(map, "maptypeid_changed", function() {
		 var selectedMapType = map.getMapTypeId();
		 newMapType=selectedMapType;
	});
	const mark = new google.maps.Marker({
     position: uluru,
     map: map,
	 icon: "https://stage.adaniconnex.com/images/ACX Chennai pointer.svg"
    });
	google.maps.event.addListener(mark, 'click', (function(mark) {
        return function() {
          infowindow.setContent('AdaniConnex Data Center');
          infowindow.open(map, mark);
        }
      })(mark));
	
	var test= new markerClusterer.MarkerClusterer({ map, markers });
}

var Airport = [
      ['Chennai International Airport', 12.9948333,80.1685891,'27.5 km']
    ];
var CableLandingStation = [
      ['Tata Communications Limited', 13.131375, 80.166197,'45.3 km', 1],
      ['Airtel', 13.028417, 80.234917,'27 km', 2]
    ];

var InternetExchange = [
      ['Bharti Airtel Santhome', 13.033766, 80.277089,'26.6 km', 1],
      ['ST Telemedia', 13.068417, 80.279167,'31.1 km', 2]
    ];
var ITParks = [
      ['DLF Cybercity', 13.022306, 80.174583,'31 km', 1],
      ['International Tech Park', 12.985803, 80.246061,'20.1 km', 2],
      ['SIPCOT IT Park', 12.826078, 80.219344,'1.2 km', 3],
      ['Ambattur IT Park', 13.095111, 80.170000,'41.6 km', 4]
    ];
var SEZs = [
      ['ELCOT SEZ', 12.905444, 80.218417,'10.9 km', 1],
      ['MEPZ - Special Economic Zone', 12.936267, 80.126793,'24.6 km', 2],
      ['Chennai One IT SEZ', 12.947935, 80.231992,'16.1 km', 3]
    ];
	
	
//import { MarkerClusterer } from "@googlemaps/markerclusterer";

