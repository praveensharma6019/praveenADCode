// location
var countryObj = [{
        locations: [{lat: 26,lng: 78}],
        locations1: [{lat: 20.110455212912825, lng: 77.98653298093666}],
        depthScale: 0.4,
        scale:1.3,
        labelName:"India",
        placeName:"India",
        placeContent:"Lorem ipsum is a placeholder text commonly used to demonstrate the visual form of a document or a typeface without relying on meaningful content.",
        mplaceContent:"Lorem ipsum is a placeholder text commonly used to demonstrate...",
        placeLink:"https://www.edgeconnex.com/india/",
        bgImg:""

    },
    {
        locations: [{lat: 54,lng: -105}],
        locations1: [{lat: 43.07364794552939, lng: -98.65255616235754}],
        depthScale: 0.4,
        scale:2,
        labelName:"North America",
        placeName:"North America",
        placeContent:"With more than 30 data centers in over 25 markets, ranging from Edge to Hyperscale solutions, our global partner continues to deliver new capacity across North America to meet growing customer demand.",
        mplaceContent:"With more than 30 data centers in over 25 markets, ranging from Edge to Hyperscale solutions, our global partner continues to deliver new capacity across North America to meet growing customer demand.",
        placeLink:"https://www.edgeconnex.com/north-america/",
        bgImg:"../../images/AdaniConneX/globe/northamerica.png"
    },

    {
        locations: [{lat: -23,lng: -57}],
        locations1: [{lat: -26.986786029147993, lng: -54.726191336991064}],
        depthScale: 0.4,
        scale:2,
        labelName:"South America",
        placeName:"South America",
        placeContent:"Bringing cloud services to fast-growing cities and business communities in South America along with a global stage for innovation, products, and culture.",
        mplaceContent:"Bringing cloud services to fast-growing cities and business communities in South America along with a global stage for innovation, products, and culture.",
        placeLink:"https://www.edgeconnex.com/south-america/",
        bgImg:"../../images/AdaniConneX/globe/southamerica.png"

    },

    {
        locations: [{lat: 54,lng: 15}],
        locations1: [{lat: 44.97007597597241, lng: 11.707373033341355}],
        depthScale: 0.4,
        scale:1.3,
        labelName:"EMEA",
        placeName:"EMEA",
        placeContent:"Data centers that help bring cloud, commerce, content, and connectivity to EMEA’s businesses and citizens who rely on critical internet services to accelerate the global digital economy.",
        mplaceContent:"Data centers that help bring cloud, commerce, content, and connectivity to EMEA’s businesses and citizens who rely on critical internet services to accelerate the global digital economy.",
        placeLink:"https://www.edgeconnex.com/emea/",
        bgImg:"../../images/AdaniConneX/globe/emea.png"

    }]

var nascale = 0.35;
var escale = 0.45;
var sascale = 0.5;

    var placesToplaces = [

        //india lc
        {locations: [{lat: 27,lng: 119}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: 0.3,
        labelName:"noida",
        contentInfo:0,
        rotationX: -90,
        rotationY: 180,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]
    },

    {
        locations: [{lat:41.3874, lng:2.1686-0.75}],
        connection: [19, 73, 41.3874, 2.1686],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Barcelona, Spain",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]
    },

    {
        locations: [{lat:48.1351, lng:11.5820}],
        connection:[],// [19,73,-35,-72],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Munich, Germany",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]

    },

    {
        locations: [{lat:52.2297, lng:21.0122}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Warsaw, Poland",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]

    },

    {
        locations: [{lat:53.3498, lng:-6.2603}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Dublin, Ireland",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]

    },

    {
        locations: [{lat:52.3676, lng:4.9041}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Amsterdam, Netherlands",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]

    },

    {
        locations: [{lat:50.8476, lng:4.3572}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Brussels ",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]

    },

    {
        locations: [{lat:32.0853, lng:34.7818}],
        connection: [],
        color: "#ffffff",
        depthScale: 0.4,
        className: "label", scale: escale,
        labelName:"Tel Aviv",
        contentInfo:3,
        rotationX: 0,
        rotationY: 0,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]
    },

 {
        locations: [{lat:25.7617, lng:-80.1918}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: nascale,
        labelName:"Miami FL.",
        contentInfo:1,
            rotationX: 0,
            rotationY: -90,
            rotationZ: 0,
            lookAt:[{lat:0, lng:0}]
    }, {
        locations: [{lat:32.7157, lng:-117.1611}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: nascale,
        labelName:"San Diego CA.",
        contentInfo:1,
            rotationX: 0,
            rotationY: -90,
            rotationZ: 0,
            lookAt:[{lat:0, lng:0}]
    },{
        locations: [{lat:47.6062, lng:-122.3321}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: nascale,
        labelName:"Seattle WA.",
        radius: 0.8,
        contentInfo:1,
        rotationX: 0,
            rotationY: -90,
            rotationZ: 0,
            lookAt:[{lat:0, lng:0}]
    },
{
    locations: [{lat:30.3322, lng:-81.6557}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Jacksonville FL.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:30.4383, lng:-84.2807}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Tallahassee FL.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:29.9511, lng:-90.0715}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "New Orleans LA.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:29.7604, lng:-95.3698}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Houston TX.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:33.7490, lng:-84.3880}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Atlanta GA.",
    contentInfo:1,
     rotationX: 0,
     rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:36.1627, lng:-86.7816}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Nashville TN.",
    contentInfo:1,
     rotationX: 0,
     rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:35.1495, lng:-90.0490}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Memphis TN.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:37.5407, lng:-77.4360}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Richmond VA.",
    contentInfo:1,
    height:0.5,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:36.8508, lng:-76.2859}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Norfolk VA.",
    height:1.5,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:40.4406, lng:-79.9959}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Pittsburgh PA.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
 lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:42.3314, lng:-83.0458}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Detroit MI.",
    radius: 0.7,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:43.6532, lng:-79.3832}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Toronto Canada",
    radius: 0.6,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:42.3601, lng:-71.0589}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Boston MA.",
    contentInfo:1,
        rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:41.8781, lng:-87.6298}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Chicago IL.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:43.0731, lng:-89.4012}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Madison WI.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:44.9778, lng:-93.2650}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Minneapolis MN.",
    contentInfo:1,
        rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:39.7392, lng:-104.9903}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Denver CO.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:40.7608, lng:-111.8910}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Salt Lake City UT.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:36.1699, lng:-115.1398}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Las Vegas NV.",
    radius: 0.8,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:38.5816, lng:-121.4944}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Sacramento CA.",
    radius: 1,
    height:1.25,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
 lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:33.4484, lng:-112.0740}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Phoenix AZ.",
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:37.3541, lng:-121.9552}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Santa Clara CA.",
    radius: 1.3,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },
{
    locations: [{lat:45.5152, lng:-122.6784}],
    connection: [],
    color: "#eeeeee",
    depthScale: 0.4,
    className: "label", scale: nascale,
    labelName: "Portland OR.",
    radius: 0.8,
    contentInfo:1,
    rotationX: 0,
    rotationY: -90,
    rotationZ: 0,
    lookAt:[{lat:0, lng:0}]
  },



        {
        locations: [{lat:-34.6037, lng:-58.3816}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: sascale,
        labelName:"Buenos Aires",
        contentInfo:2,
        rotationX: 0,
        rotationY: -90,
        rotationZ: 0,
        lookAt:[{lat:0, lng:0}]
    },
    {
        locations: [{lat:-33.4489, lng:-70.6693}],
        connection: [],
        color: "#eeeeee",
        depthScale: 0.4,
        className: "label", scale: sascale,
        labelName:"Santiago",
        contentInfo:2,
        rotationX: 0,
        rotationY: -90,
            rotationZ: 0,
        lookAt:[{lat:0, lng:0}]
    },



    ]


//connections markers
    var countryInternal = [{
        locations: [{lat: 28,lng: 77.6}],
        locationst: [{lat: 27.7855,lng: 77.641}],
        connection: [19, 73, 27.7855, 77.641],
        infoPosition: {lat:-3.8,lng:-1.5,mlat:-5.6,mlng:-1.5},
        animatePosition:{lat:0,lng:-14},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: 'noida',
        labelName:"Noida",
        placeName:"Noida",
        placeContent:"Together with Gurgaon, Noida is one among the many emerging IT hubs in the Delhi NCR region. Our facility is strategically...",
        mplaceContent:"Together with Gurgaon, Noida is one among the many emerging IT hubs...",
        placeLink:"https://stage.adaniconnex.com/data-centers#Noida",
        placeImg:"../../images/AdaniConneX/globe/noida-pin.png",
        placePosition:"left",
        bgImg:"../../images/AdaniConneX/globe/Noida-white.png"

    },


    {
        locations: [{lat: 19.6,lng: 72.75}],
        locationst: [{lat: 19.6,lng: 72.75}],
        connection: [17, 74, 19.6, 72.75],
        infoPosition: {lat:4,lng:28,mlat:4,mlng:32},
        animatePosition:{lat:0,lng:7},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: '',
        labelName:"Mumbai",
        placeName:"Mumbai",
        placeContent:"The country's economic and financial hub is also the largest data center market with the presence of all major hyperscalers. Our soon to be launched...",
        mplaceContent:"The country's economic and financial hub is also the largest data center...",
        placeLink:"https://stage.adaniconnex.com/data-centers#Mumbai",
        placeImg:"../../images/AdaniConneX/globe/mumbai-pin.png",
        placePosition:"left",
        bgImg:"../../images/AdaniConneX/globe/mumbai-white.png"

    },

    {
        locations: [{lat: 17.7,lng: 74.3}],
        locationst: [{lat: 17.7,lng: 74.3}],
        connection: [13, 80, 17.7, 74.3],
        infoPosition: {lat:4,lng:28,mlat:4,mlng:30},
        animatePosition:{lat:0,lng:7},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: '',
        labelName:"Pune",
        placeName:"Pune",
        placeContent:"Preferred DC hub due to proximity to Mumbai as disaster recovery location. Our soon to be launched...",
        mplaceContent:"Preferred DC hub due to proximity to Mumbai as disaster recovery...",
        placeLink:"https://stage.adaniconnex.com/data-centers#Pune",
        placeImg:"../../images/AdaniConneX/globe/pune-pin.png",
        placePosition:"left",
        bgImg:"../../images/AdaniConneX/globe/PUNE-white.png"
    },

     {
        locations: [{lat: 13,lng: 80}],
        locationst: [{lat: 13,lng: 74.75}],
        connection: [17, 78, 13, 80],
        infoPosition: {lat:1.5,lng:0,mlat:1.5,mlng:0},
        animatePosition:{lat:0,lng:-10},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: '',
        labelName:"Chennai",
        placeName:"Chennai",
        placeContent:"Strategically Located in SIPCOT IT park, AdaniConneX Chennai 1 Data Center is a first-of-its-kind multi-tenant purpose-built facility...",
        mplaceContent:"Strategically Located in SIPCOT IT park, AdaniConneX Chennai 1 Data Center...",
        placeLink:"https://stage.adaniconnex.com/data-centers/chennai",
        placeImg:"../../images/AdaniConneX/globe/chennai-pin.png",
        placePosition:"right",
        bgImg:"../../images/AdaniConneX/globe/chennai-white.png"
    },

    {
        locations: [{lat: 16.9,lng: 77.9}],
        locationst: [{lat: 16.9,lng: 77.9}],
        connection:[17, 82, 16.9, 77.9],
        infoPosition: {lat:4,lng:28,mlat:4,mlng:31},
        animatePosition:{lat:0,lng:7},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: 'hyderabad',
        labelName:"Hyderabad",
        placeName:"Hyderabad",
        placeContent:"Our Hyderabad facility will offer world class colocation services, connectivity and end to end portfolio of managed services in...",
        mplaceContent:"Our Hyderabad facility will offer world class colocation services...",
        placeLink:"https://stage.adaniconnex.com/data-centers#Hyderabad",
        placeImg:"../../images/AdaniConneX/globe/hyderabad-pin.png",
        placePosition:"left",
        bgImg:"../../images/AdaniConneX/globe/hyderabad-white.png"

    },

    {
        locations: [{lat: 17.11,lng: 82.1}],
        locationst: [{lat: 17.11,lng: 82.1}],
        connection: [28, 77, 17.11, 82.1],
        infoPosition: {lat:1.5,lng:0,mlat:1.8,mlng:-1},
        animatePosition:{lat:0,lng:-11},
        color: [0x00FF00,"red"],
        content: '',
        depthScale: 0.4,
        className: '',
        labelName:"Vizag",
        placeName:"Vizag",
        placeContent:"The strategically located Hyperscale Data Center campus in Vizag will offer direct submarine connectivity to major hubs in...",
        mplaceContent:"The strategically located Hyperscale Data Center campus...",
        placeLink:"https://stage.adaniconnex.com/data-centers#Vizag",
        placeImg:"../../images/AdaniConneX/globe/vizag-pin.png",
        placePosition:"right",
        bgImg:"../../images/AdaniConneX/globe/mumbai_location_bg.png"

    },
        ];


var interconnectp = [
    {
        locations: [{lat:19, lng:73}],
        connection: [19, 73, 41, 2],
        color: "red",
        depthScale: 0.4,
        className: 'label',
        nos:2,
        duration: 0,
        sprites:[],
        ofst : 0,
    },

    ]

var discmesh = "o disc\nv 0.0000 -0.00056700 -0.15013\nv 0.0000 0.042601 -0.15013\nv 0.030509 -0.00056700 -0.14712\nv 0.030509 0.042601 -0.14712\nv 0.059845 -0.00056700 -0.13822\nv 0.059845 0.042601 -0.13822\nv 0.086881 -0.00056700 -0.12377\nv 0.086881 0.042601 -0.12377\nv 0.11058 -0.00056700 -0.10432\nv 0.11058 0.042601 -0.10432\nv 0.13003 -0.00056700 -0.080625\nv 0.13003 0.042601 -0.080625\nv 0.14448 -0.00056700 -0.053588\nv 0.14448 0.042601 -0.053588\nv 0.15338 -0.00056700 -0.024252\nv 0.15338 0.042601 -0.024252\nv 0.15638 -0.00056700 0.0062560\nv 0.15638 0.042601 0.0062560\nv 0.15338 -0.00056700 0.036765\nv 0.15338 0.042601 0.036765\nv 0.14448 -0.00056700 0.066101\nv 0.14448 0.042601 0.066101\nv 0.13003 -0.00056700 0.093137\nv 0.13003 0.042601 0.093137\nv 0.11058 -0.00056700 0.11683\nv 0.11058 0.042601 0.11683\nv 0.086881 -0.00056700 0.13628\nv 0.086881 0.042601 0.13628\nv 0.059845 -0.00056700 0.15073\nv 0.059845 0.042601 0.15073\nv 0.030509 -0.00056700 0.15963\nv 0.030509 0.042601 0.15963\nv -0.0000 -0.00056700 0.16264\nv -0.0000 0.042601 0.16264\nv -0.030509 -0.00056700 0.15963\nv -0.030509 0.042601 0.15963\nv -0.059845 -0.00056700 0.15073\nv -0.059845 0.042601 0.15073\nv -0.086881 -0.00056700 0.13628\nv -0.086881 0.042601 0.13628\nv -0.11058 -0.00056700 0.11683\nv -0.11058 0.042601 0.11683\nv -0.13003 -0.00056700 0.093137\nv -0.13003 0.042601 0.093137\nv -0.14448 -0.00056700 0.066101\nv -0.14448 0.042601 0.066101\nv -0.15338 -0.00056700 0.036765\nv -0.15338 0.042601 0.036765\nv -0.15638 -0.00056700 0.0062560\nv -0.15638 0.042601 0.0062560\nv -0.15338 -0.00056700 -0.024252\nv -0.15338 0.042601 -0.024252\nv -0.14448 -0.00056700 -0.053588\nv -0.14448 0.042601 -0.053588\nv -0.13003 -0.00056700 -0.080625\nv -0.13003 0.042601 -0.080625\nv -0.11058 -0.00056700 -0.10432\nv -0.11058 0.042601 -0.10432\nv -0.086881 -0.00056700 -0.12377\nv -0.086881 0.042601 -0.12377\nv -0.059845 -0.00056700 -0.13822\nv -0.059845 0.042601 -0.13822\nv -0.030509 -0.00056700 -0.14712\nv -0.030509 0.042601 -0.14712\nvt 1.0000 0.50000\nvt 1.0000 1.0000\nvt 0.96875 1.0000\nvt 0.96875 0.50000\nvt 0.93750 1.0000\nvt 0.93750 0.50000\nvt 0.90625 1.0000\nvt 0.90625 0.50000\nvt 0.87500 1.0000\nvt 0.87500 0.50000\nvt 0.84375 1.0000\nvt 0.84375 0.50000\nvt 0.81250 1.0000\nvt 0.81250 0.50000\nvt 0.78125 1.0000\nvt 0.78125 0.50000\nvt 0.75000 1.0000\nvt 0.75000 0.50000\nvt 0.71875 1.0000\nvt 0.71875 0.50000\nvt 0.68750 1.0000\nvt 0.68750 0.50000\nvt 0.65625 1.0000\nvt 0.65625 0.50000\nvt 0.62500 1.0000\nvt 0.62500 0.50000\nvt 0.59375 1.0000\nvt 0.59375 0.50000\nvt 0.56250 1.0000\nvt 0.56250 0.50000\nvt 0.53125 1.0000\nvt 0.53125 0.50000\nvt 0.50000 1.0000\nvt 0.50000 0.50000\nvt 0.46875 1.0000\nvt 0.46875 0.50000\nvt 0.43750 1.0000\nvt 0.43750 0.50000\nvt 0.40625 1.0000\nvt 0.40625 0.50000\nvt 0.37500 1.0000\nvt 0.37500 0.50000\nvt 0.34375 1.0000\nvt 0.34375 0.50000\nvt 0.31250 1.0000\nvt 0.31250 0.50000\nvt 0.28125 1.0000\nvt 0.28125 0.50000\nvt 0.25000 1.0000\nvt 0.25000 0.50000\nvt 0.21875 1.0000\nvt 0.21875 0.50000\nvt 0.18750 1.0000\nvt 0.18750 0.50000\nvt 0.15625 1.0000\nvt 0.15625 0.50000\nvt 0.12500 1.0000\nvt 0.12500 0.50000\nvt 0.093750 1.0000\nvt 0.093750 0.50000\nvt 0.062500 1.0000\nvt 0.062500 0.50000\nvt 0.29682 0.48539\nvt 0.25000 0.49000\nvt 0.20318 0.48539\nvt 0.15816 0.47173\nvt 0.11666 0.44955\nvt 0.080294 0.41971\nvt 0.050447 0.38334\nvt 0.028269 0.34184\nvt 0.014612 0.29682\nvt 0.010000 0.25000\nvt 0.014612 0.20318\nvt 0.028269 0.15816\nvt 0.050447 0.11666\nvt 0.080294 0.080294\nvt 0.11666 0.050447\nvt 0.15816 0.028269\nvt 0.20318 0.014612\nvt 0.25000 0.010000\nvt 0.29682 0.014612\nvt 0.34184 0.028269\nvt 0.38334 0.050447\nvt 0.41971 0.080294\nvt 0.44955 0.11666\nvt 0.47173 0.15816\nvt 0.48539 0.20318\nvt 0.49000 0.25000\nvt 0.48539 0.29682\nvt 0.47173 0.34184\nvt 0.44955 0.38334\nvt 0.41971 0.41971\nvt 0.38334 0.44955\nvt 0.34184 0.47173\nvt 0.031250 1.0000\nvt 0.031250 0.50000\nvt 0.0000 1.0000\nvt 0.0000 0.50000\nvt 0.75000 0.49000\nvt 0.79682 0.48539\nvt 0.84184 0.47173\nvt 0.88334 0.44955\nvt 0.91971 0.41971\nvt 0.94955 0.38334\nvt 0.97173 0.34184\nvt 0.98539 0.29682\nvt 0.99000 0.25000\nvt 0.98539 0.20318\nvt 0.97173 0.15816\nvt 0.94955 0.11666\nvt 0.91971 0.080294\nvt 0.88334 0.050447\nvt 0.84184 0.028269\nvt 0.79682 0.014612\nvt 0.75000 0.010000\nvt 0.70318 0.014612\nvt 0.65816 0.028269\nvt 0.61666 0.050447\nvt 0.58029 0.080294\nvt 0.55045 0.11666\nvt 0.52827 0.15816\nvt 0.51461 0.20318\nvt 0.51000 0.25000\nvt 0.51461 0.29682\nvt 0.52827 0.34184\nvt 0.55045 0.38334\nvt 0.58029 0.41971\nvt 0.61666 0.44955\nvt 0.65816 0.47173\nvt 0.70318 0.48539\nvn 0.098000 0.0000 -0.99520\nvn 0.29030 0.0000 -0.95690\nvn 0.47140 0.0000 -0.88190\nvn 0.63440 0.0000 -0.77300\nvn 0.77300 0.0000 -0.63440\nvn 0.88190 0.0000 -0.47140\nvn 0.95690 0.0000 -0.29030\nvn 0.99520 0.0000 -0.098000\nvn 0.99520 0.0000 0.098000\nvn 0.95690 0.0000 0.29030\nvn 0.88190 0.0000 0.47140\nvn 0.77300 0.0000 0.63440\nvn 0.63440 0.0000 0.77300\nvn 0.47140 0.0000 0.88190\nvn 0.29030 0.0000 0.95690\nvn 0.098000 0.0000 0.99520\nvn -0.098000 0.0000 0.99520\nvn -0.29030 0.0000 0.95690\nvn -0.47140 0.0000 0.88190\nvn -0.63440 0.0000 0.77300\nvn -0.77300 0.0000 0.63440\nvn -0.88190 0.0000 0.47140\nvn -0.95690 0.0000 0.29030\nvn -0.99520 0.0000 0.098000\nvn -0.99520 0.0000 -0.098000\nvn -0.95690 0.0000 -0.29030\nvn -0.88190 0.0000 -0.47140\nvn -0.77300 0.0000 -0.63440\nvn -0.63440 0.0000 -0.77300\nvn -0.47140 0.0000 -0.88190\nvn 0.0000 1.0000 -0.0000\nvn -0.29030 0.0000 -0.95690\nvn -0.098000 0.0000 -0.99520\nvn 0.0000 -1.0000 -0.0000\ns off\nf 1/1/1 2/2/1 4/3/1 3/4/1\nf 3/4/2 4/3/2 6/5/2 5/6/2\nf 5/6/3 6/5/3 8/7/3 7/8/3\nf 7/8/4 8/7/4 10/9/4 9/10/4\nf 9/10/5 10/9/5 12/11/5 11/12/5\nf 11/12/6 12/11/6 14/13/6 13/14/6\nf 13/14/7 14/13/7 16/15/7 15/16/7\nf 15/16/8 16/15/8 18/17/8 17/18/8\nf 17/18/9 18/17/9 20/19/9 19/20/9\nf 19/20/10 20/19/10 22/21/10 21/22/10\nf 21/22/11 22/21/11 24/23/11 23/24/11\nf 23/24/12 24/23/12 26/25/12 25/26/12\nf 25/26/13 26/25/13 28/27/13 27/28/13\nf 27/28/14 28/27/14 30/29/14 29/30/14\nf 29/30/15 30/29/15 32/31/15 31/32/15\nf 31/32/16 32/31/16 34/33/16 33/34/16\nf 33/34/17 34/33/17 36/35/17 35/36/17\nf 35/36/18 36/35/18 38/37/18 37/38/18\nf 37/38/19 38/37/19 40/39/19 39/40/19\nf 39/40/20 40/39/20 42/41/20 41/42/20\nf 41/42/21 42/41/21 44/43/21 43/44/21\nf 43/44/22 44/43/22 46/45/22 45/46/22\nf 45/46/23 46/45/23 48/47/23 47/48/23\nf 47/48/24 48/47/24 50/49/24 49/50/24\nf 49/50/25 50/49/25 52/51/25 51/52/25\nf 51/52/26 52/51/26 54/53/26 53/54/26\nf 53/54/27 54/53/27 56/55/27 55/56/27\nf 55/56/28 56/55/28 58/57/28 57/58/28\nf 57/58/29 58/57/29 60/59/29 59/60/29\nf 59/60/30 60/59/30 62/61/30 61/62/30\nf 4/63/31 2/64/31 64/65/31 62/66/31 60/67/31 58/68/31 56/69/31 54/70/31 52/71/31 50/72/31 48/73/31 46/74/31 44/75/31 42/76/31 40/77/31 38/78/31 36/79/31 34/80/31 32/81/31 30/82/31 28/83/31 26/84/31 24/85/31 22/86/31 20/87/31 18/88/31 16/89/31 14/90/31 12/91/31 10/92/31 8/93/31 6/94/31\nf 61/62/32 62/61/32 64/95/32 63/96/32\nf 63/96/33 64/95/33 2/97/33 1/98/33\nf 1/99/34 3/100/34 5/101/34 7/102/34 9/103/34 11/104/34 13/105/34 15/106/34 17/107/34 19/108/34 21/109/34 23/110/34 25/111/34 27/112/34 29/113/34 31/114/34 33/115/34 35/116/34 37/117/34 39/118/34 41/119/34 43/120/34 45/121/34 47/122/34 49/123/34 51/124/34 53/125/34 55/126/34 57/127/34 59/128/34 61/129/34 63/130/34";




var myearth;
var sunAngle = 90;
var activeHotspot;
var extHotspot = [];
var locationInfo = [];
var extLabel = [];
var locationmarker = [];
var placeName,placeContent,placeLink,contentInfo,placeImg,forImg,forClose,forPosition,bgImg;
var connectionhostpot = [];
var connectionpulse = [];
var connectionInfo = [];

function contentInfofn(placeName,placeContent,placeLink,placeImg,bgImg){
contentInfo = '<div class="location_popup" style="width: 300px;position: absolute; top: 50%; left: 40%; z-index: 9999; background: #fff;border-radius: 15px;-webkit-border-radius: 15px;-moz-border-radius: 15px;box-shadow: 0px 4px 10px #00000029;-moz-box-shadow: 0px 4px 10px #00000029;-webkit-box-shadow: 0px 4px 10px #00000029;padding: 15px;"><a href="javascript:void(0);" class="closeInfobox" data-attr="closeInfo" style="position: absolute;top: 9px;right: 9px;color: #4d4d4d;"><i class="i-close i-20"></i></a><p class="full h4 fm-asb tx-000" style="text-transform: capitalize;">'+placeName+'</p><p class="full fs-15" style="color: #4d4d4d;">'+placeContent+'</p><a href="'+placeLink+'" class="know-more-link" target="_blank">Know More</a></div>';
}

window.addEventListener('earthjsload', function() {
    Earth.addMesh( discmesh );
    myearth = new Earth('myearth', {
        location: {lat: 20.110455212912825, lng: 77.98653298093666},
        sunLocation: {
            lat: 170,
            lng: -20
        },
        zoom: 0.8,
        zoomMin: 0.8,
        zoomMax: 1.7,
        light: 'sun',
        shadows: true,
        lightAmbience: 0.8,
        shininess: 0.8,
        autoRotateSpeed: 0.5,
        autoRotateDelay: 0,
        autoRotateStart: 2000,
        mapImage:'../../images/AdaniConneX/globe/maplines-initload.jpg'
    });

    myearth.addEventListener("ready", function() {
        myearth.animate('lightAmbience', 1, {
            duration: 2500
        });

        connectionmarker2 = [];
        for ( var i in countryInternal ) {
            var placeName = countryInternal[i].placeName;
            var placeContent = countryInternal[i].placeContent;
            if(window.screen.width < 1024){
                placeContent = countryInternal[i].mplaceContent
            }
            var placeLink = countryInternal[i].placeLink;
            var placeImg = countryInternal[i].placeImg;
            var bgImg = countryInternal[i].bgImg;
            if(countryInternal[i].placePosition == "right"){
                forImg = "placeImg"
                forClose = "placeClose";
            }
            else{
                forImg = ""
                forClose = "";
            }
        contentInfofn(placeName,placeContent,placeLink,placeImg,bgImg);

            connectionpulse[i] = this.addImage({
                location: countryInternal[i]['locations'][0],
                loopvalue: i,
                placePosition: countryInternal[i].placePosition,
                scale: 0.2,
                image: '../../images/AdaniConneX/globe/disk_thicker.png',
                color: '#8BDEFF',
                visible: true,
                opacity: 1,
                transparent: true
            });
            connectionhostpot[i] = this.addMarker({
                location: countryInternal[i]['locations'][0],
                loopvalue: i,
                placePosition: countryInternal[i].placePosition,
                animatePosition:countryInternal[i].animatePosition,
                scale: 0.4,
                hotspot: true,
                className: "locationmarker",
                mesh: 'disc',
                color: 'white',
                rotationY: -90,
                visible: true,
                opacity: 1,
                transparent: true
            });
                var conlat = countryInternal[i].infoPosition['lat'];
                var conlng = countryInternal[i].infoPosition['lng'];

            if(window.screen.width < 1200){
                conlat = countryInternal[i].infoPosition['mlat'];
                conlng = countryInternal[i].infoPosition['mlng'];
            }


            var conscale=1;

            if(window.screen.width < 1024){conscale=0.5;}
            if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
            if (window.screen.width < 1024) {
                conscale=0.8;
            }
        }


            connectionInfo[i] = this.addOverlay({
                location: {lat: countryInternal[i].locations[0].lat-conlat, lng:countryInternal[i].locations[0].lng-conlng},
                offset: 0,
               // scale: 0.010,
                hostpot: true,
                content: contentInfo,
               // zoomScale:0.005,
               // depthScale: countryInternal[i].depthScale,
                visible: false,
                className: "internalConnection",
                containerScale:conscale
            });
            connectionhostpot[i].tip = this.addOverlay( {
                location : countryInternal[i].locationst[0],
                content : countryInternal[i].labelName,
                offset: 0,
                depthScale : 0.25,
                hostpot:true,
                elementScale : 0.5,
                visible : false,
                className : 'tip-overlay tip-down placestoplacesLabel '+countryInternal[i].className
            });

            function am(i) {

                connectionpulse[i].opacity = 0.5;
                connectionpulse[i].animate('opacity', 1, {duration: 200, complete: function (){
                    connectionpulse[i].animate('opacity', 0, {duration: 800});
                    }});
                connectionpulse[i].animate('scale', connectionpulse[i].scale*2, {duration:1000, complete: function (){
                    connectionpulse[i].scale = connectionpulse[i].scale/2;
                    connectionpulse[i].opacity = 0;
                    }} )

                connectionhostpot[i].animate('scale', connectionhostpot[i].scale*1.2, {duration: 800,
                complete: function() {
                    connectionhostpot[i].animate('scale', connectionhostpot[i].scale, {duration: 200, easing: 'linear',
                    complete: function (){
                        connectionhostpot[i].animate('scale', connectionhostpot[i].scale/1.2, {duration: 800, easing: 'linear',
                        complete: function (){
                            connectionhostpot[i].animate('scale', connectionhostpot[i].scale, {duration: 200, easing: 'linear',
                            complete: function (){
                                am(i);
                    }}) }}) }}) }}); }
            am(i);
            connectionhostpot[i].addEventListener('click',showConnectionInfo);
            connectionhostpot[i].addEventListener("mouseover",function(){
                this.tip.visible = true;
            });
            connectionhostpot[i].addEventListener("mouseout",function(){
                this.tip.visible = false;
            });
        }

        function showConnectionInfo(location,loopvalue){
            if(loopvalue == undefined){
                loopvalue = this.loopvalue;
                location = this.location;
            }
            var city_boxes = document.getElementsByClassName("city_box");
            var con_li = document.getElementsByClassName("country_li");
            for(var con=1;con< con_li.length; con++){
                con_li[con].classList.remove("active");
            }
            for(var sls = 0;sls < city_boxes.length;sls++){city_boxes[sls].classList.remove("active");
                if(city_boxes[sls].getAttribute("data-index") == loopvalue){
                    city_boxes[sls].classList.add("active");
                }
            }
            reverseMarker(locationInfo,locationmarker);
            reverseMarker(connectionInfo,connectionhostpot,'connection');
            if(loopvalue == 0){locationmarker[0].tip.visible = false;}
            var drLocDur = 2000;
            if (Math.abs(myearth.zoom-1.7)>0.1) {
                myearth.animate('zoom', 1.8, {duration: 2000 });
            }
            var animatelat=0;var animatelng=0

            if(window.screen.width < 1200){
            animatelat=connectionhostpot[loopvalue].animatePosition['lat'];animatelng=connectionhostpot[loopvalue].animatePosition['lng']
            }

            if(loopvalue == 0 || loopvalue == 2 || loopvalue == 5){
                     document.getElementsByClassName("city_box_list")[0].scroll({
                        left: 500,
                        behavior: "smooth"
                      });

                    }
                    else{
                    document.getElementsByClassName("city_box_list")[0].scroll({
                        left: - 500,
                        behavior: "smooth"
                      });
                    }

            myearth.animate('location', {lat:location.lat-animatelat,lng:location.lng-animatelng} ,{duration: drLocDur});
                this.color = '#8BDEFF';
                connectionhostpot[loopvalue].color = '#8BDEFF';
                connectionInfo[loopvalue]['visible'] = true;
        }

        for(var i = 0;i<countryObj.length;i++){
            var placeName = countryObj[i].placeName;
            var placeContent = countryObj[i].placeContent;
            if(window.screen.width < 1024){
                placeContent = countryObj[i].mplaceContent
            }

            var placeLink = countryObj[i].placeLink;
            var bgImg = countryObj[i].bgImg;
            contentInfofn(placeName,placeContent,placeLink,bgImg,bgImg);

            locationmarker[i] = this.addImage({
                location: countryObj[i].locations[0],
                loopvalue:i,
                scale: countryObj[i].scale,
                hotspot: true,
                hotspotRadius:1.2,
                visible:true,
                opacity:0,
            });

            locationmarker[i].tip = this.addOverlay( {
                location : countryObj[i].locations[0],
                content : countryObj[i].labelName,
                depthScale : 0.25,
                elementScale : 0.5,
                visible : true,
                className : 'tip-overlay tip-down '+countryObj[i].labelName,
            } );

            var conscale=1;
            if(window.screen.width < 1024){conscale=0.5;}
            if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
            if (window.screen.width < 1024) {
                conscale=0.8;
            }
        }

            locationInfo[i] = this.addOverlay({
                location : countryObj[i].locations[0],
                offset: 0,
                scale: 0.010,
                containerScale: conscale,
                content:contentInfo,
                depthScale: countryObj[i].depthScale,
                className : 'my-overlay-above',
                visible: false,
            });

            locationmarker[i].addEventListener( "click", showTip );
            locationmarker[0].removeEventListener( "click", showTip );
        }

        function fix_points(pi) {
            var po = [];
            for (var i = 0; i < pi.length; i++) {
                if (pi[i].hasOwnProperty("location")) {
                    po.push(pi[i].location);
                } else {
                    po.push(pi[i]);
                }

            }
            return po;
        }

        function rectToSprite(e, p, sx, sz, ry, c) {
            return e.addImage({
                image: '../../images/AdaniConneX/globe/oblong_trans.png',
                scaleX: sx,
                scaleZ: sz,
                offset: 0,
                opacity: 1,
                location: p[0],
                rotationY: ry,
                // color: c,
            });
        }

        var loop_rect = function (e, r, t, i, j, curr_rect) {
            if (j == 0) {
                i = i + 1;
                curr_rect = rectToSprite(e, r[i], 0.3, 0.04, 0, '#138EE5');
            }
            var tm = t;
            var nj = j + 1;
            var ato = r[i][nj];
            curr_rect.lookAt = r[i][nj];
            curr_rect.animate('location', ato, {
                "duration": tm, "easing": "linear", "lerpLatLng": true, "complete": function () {
                    if (nj == r[i].length) {
                        curr_rect.remove();
                        nj = 0;
                        if (i == r.length - 1) {
                            i = -1;
                        }
                        loop_rect(e, r, tm, i, nj, null);
                    } else {
                        loop_rect(e, r, tm, i, nj, curr_rect);
                    }
                }
            })
        }

var ntom = fix_points([
    {"location":{"lat":27.96423557561152,"lng":77.35512503870709+0.4},"scale":0.07},
    {"location":{"lat":27.081761726335344,"lng":76.25108788987987+0.2},"scale":0.07},
    {"location":{"lat":25.795965864699383,"lng":75.03683788072617+0.2},"scale":0.07},
    {"location":{"lat":24.615316345060755,"lng":74.17437990146722+0.2},"scale":0.07},
    {"location":{"lat":23.25623874394789,"lng":73.2050430652871+0.4},"scale":0.07},
    {"lat":21.712918873744268,"lng":72.45529358079799+0.4},
    {"lat":20.675946502413282,"lng":72.34170221682834+0.5},
    // {"lat":20.138319900114972,"lng":72.4110783397678},
    {"lat":19.520143465198803,"lng":72.68418165757078+0.4},

]);

var mtop = fix_points([
    {"location": {"lat": 19.526733343257522, "lng": 72.83442812097309+0.4}, "scale": 0.07},
    {"location": {"lat": 19.123106531636132, "lng": 72.84865032570698+0.4}, "scale": 0.07},
    {"location": {"lat": 18.813956847722203, "lng": 72.96141579026634+0.4}, "scale": 0.07},
    {"location": {"lat": 18.59727148142705, "lng": 73.16218950648391+0.4}, "scale": 0.07},
    {"location": {"lat": 18.418944493254283, "lng": 73.30535173923177+0.4}, "scale": 0.07},
    {"lat": 18.285641264073266, "lng": 73.44779665760815+0.4},
    {"lat": 18.198059506536232, "lng": 73.63619472011258+0.4}
]);

var mtoc = fix_points([
    {"location":{"lat":18.194301387687435,"lng":73.18781726363797+0.2},"scale":0.07},
    {"location":{"lat":17.036076835926178,"lng":73.66505621095503+0.2},"scale":0.07},
    {"location":{"lat":15.919297520292734,"lng":74.44786115285768+0.4},"scale":0.07},
    {"location":{"lat":14.986966812159705,"lng":75.31481790255839+0.4},"scale":0.07},
    {"lat":14.105065648622697,"lng":76.56603223142977+0.4},
    {"lat":13.5409438190851,"lng":78.04853481558641+0.4},
    {"lat":13.040577126827564,"lng":79.6109726785278+0.4},
]);

var ntop = fix_points([
    {"location": {"lat": 27.87602560058266, "lng": 77.45723701506422+0.4}, "scale": 0.07},
    {"location": {"lat": 26.942261164705684, "lng": 76.70693701619355+0.4}, "scale": 0.07},
    {"location": {"lat": 25.835006864581608, "lng": 75.93249570614506+0.4}, "scale": 0.07},
    {"location": {"lat": 24.652844598005885, "lng": 75.2361386445052+0.4}, "scale": 0.07},
    {"location": {"lat": 23.564070023871576, "lng": 74.6498907485741+0.4}, "scale": 0.07},
    {"location": {"lat": 22.344568086390225, "lng": 74.16032902652238+0.4}, "scale": 0.07},
    {"location": {"lat": 21.085267998221617, "lng": 73.84519402415579+0.4}, "scale": 0.07},
    {"location": {"lat": 19.50979542269488, "lng": 73.65353647407147+0.4}, "scale": 0.07},
    {"lat": 18.201979446940886, "lng": 73.85994659552581+0.4},
])

var ptoc = fix_points([
    {"location": {"lat": 18.10917501629179, "lng": 73.73118075864855+0.4}, "scale": 0.07},
    {"location": {"lat": 16.717588910804725, "lng": 74.41090644724102+0.4}, "scale": 0.07},
    {"location": {"lat": 15.733916724326562, "lng": 75.31880434637829+0.4}, "scale": 0.07},
    {"location": {"lat": 14.795527588358823, "lng": 76.5011000856037+0.4}, "scale": 0.07},
    {"location": {"lat": 14.075555931344162, "lng": 77.75845256452027+0.4}, "scale": 0.07},
    {"location": {"lat": 13.330433561804615, "lng": 79.24842840450258+0.2}, "scale": 0.07},
    {"lat": 13.174839800987886, "lng": 79.80649675080501+0.4},
])

var ntoh = fix_points([
    {"location": {"lat": 27.91400313668069, "lng": 77.44430057993338}, "scale": 0.07},
    {"location": {"lat": 26.373317418612785, "lng": 77.83143706939865}, "scale": 0.07},
    {"location": {"lat": 25.1005803822324, "lng": 78.08342532479676}, "scale": 0.07},
    {"location": {"lat": 23.566213591726534, "lng": 78.22657408964542}, "scale": 0.07},
    {"location": {"lat": 22.3103553334482, "lng": 78.28476891551568}, "scale": 0.07},
    {"location": {"lat": 20.8830685409432, "lng": 78.27950988640185}, "scale": 0.07},
    {"location": {"lat": 19.73010881253242, "lng": 78.25334819138331}, "scale": 0.07},
    {"lat": 18.435801383339737, "lng": 78.12894606489338},
    {"lat": 16.956588816367915, "lng": 77.9033633675533},
])

var htoc = fix_points([
    {"location": {"lat": 17.040704121340955, "lng": 77.79884130657248}, "scale": 0.07},
    {"location": {"lat": 15.37149791260061, "lng": 78.03202105263085-0.1}, "scale": 0.07},
    {"location": {"lat": 13.858933651470835, "lng": 78.83154016133348}, "scale": 0.07},
    {"lat": 13.171696458487197, "lng": 79.75931411475082},
])

var ntov = fix_points([
    {"location": {"lat": 27.799500401439406, "lng": 77.48392930154405}, "scale": 0.07},
    {"location": {"lat": 26.814490910175014, "lng": 78.54069045994049}, "scale": 0.07},
    {"location": {"lat": 25.83836477807435, "lng": 79.33814284739569}, "scale": 0.07},
    {"location": {"lat": 24.737736789118923, "lng": 80.11919720565031}, "scale": 0.07},
    {"location": {"lat": 23.6879100062964, "lng": 80.70296697543125}, "scale": 0.07},
    {"location": {"lat": 22.462198901431915, "lng": 81.23427763639016}, "scale": 0.07},
    {"location": {"lat": 21.112675673634822, "lng": 81.72129491592388}, "scale": 0.07},
    {"location": {"lat": 19.675109993685158, "lng": 82.03454364277265}, "scale": 0.07},
    {"location": {"lat": 18.282606668768405, "lng": 82.12084085510288}, "scale": 0.07},
    {"lat": 17.05963630387156, "lng": 82.18952046298298},
])

var vtoc = fix_points([
    {"location": {"lat": 16.89723759983311, "lng": 82.03079170017833}, "scale": 0.07},
    {"location": {"lat": 16.333800420064456, "lng": 80.53648190104046+0.1}, "scale": 0.07},
    {"lat": 15.2735137337968, "lng": 79.84518707284064+0.1},
    {"lat": 13.777463134772361, "lng": 79.69702074213725},
    {"lat": 13.227419143910202, "lng": 79.7449096694645},
])

var mtov = fix_points([
    {"location": {"lat": 19.474445415270587, "lng": 72.7354219317726}, "scale": 0.07},
    {"location": {"lat": 19.419918852566074-0.1, "lng": 74.51893032182151}, "scale": 0.07},
    {"location": {"lat": 19.22079204901727-0.1, "lng": 76.42308663490343}, "scale": 0.07},
    {"location": {"lat": 18.84171525300929-0.1, "lng": 78.13588497165789}, "scale": 0.07},
    {"location": {"lat": 18.383496958055854-0.1, "lng": 79.59115724652216}, "scale": 0.07},
    {"location": {"lat": 17.71709550683418-0.1, "lng": 81.1414429461617}, "scale": 0.07},
    {"lat": 16.968886687135466, "lng": 82.12648780825552},
])

var vtoh = fix_points([
    {"location": {"lat": 17.007548257727988-0.15, "lng": 81.88644508668855}, "scale": 0.07},
    {"location": {"lat": 16.9810087930498-0.17, "lng": 79.9549415118547}, "scale": 0.07},
])

var htop = fix_points([
    {"location": {"lat": 17.040704121340955-0.16, "lng": 77.79884130657224}, "scale": 0.07},
    {"location": {"lat": 17.268508971140887-0.16, "lng": 75.5722525326281}, "scale": 0.07},
])

var ptom = fix_points([
    {"location": {"lat": 18.00427156783055-0.125, "lng": 73.83038742696392}, "scale": 0.07},
    {"location": {"lat": 19.039869956223708+0.125, "lng": 72.65128897470737}, "scale": 0.07},
])

var rects = [ntom.concat(mtoc), ntoh.concat(htoc), ntop.concat(ptoc), ntov.concat(vtoc), mtov, (vtoh.concat(htop)).concat(ptom)]

var rects2 = [mtov, (vtoh.concat(htop)).concat(ptom), ntom.concat(mtoc), ntoh.concat(htoc), ntop.concat(ptoc), ntov.concat(vtoc), ]

loop_rect(this, rects, 90, -1, 0, null);
loop_rect(this, rects2, 90, -1, 0, null);


function animatePulse(p, s, to, d, o) {

                if (o === 0) {
                    to = 500;
                }
                else if (to === 989) {
                    to = 750;
                }
                else {
                    to = 0;
                }
                p.animate('opacity', o, {duration: d});
                p.animate('scale', s, {duration:d,
                complete: function(){
                    if (s >= 0.4) {
                        s = 0.18;
                        d = 0;
                        o = 0.75;
                    }
                    else {
                        s += 0.04;
                        d = 200;
                        o -= 0.25;
                    }

                    setTimeout(function (){
                        animatePulse(p, s, to, d, o);
                    }, to)

                }})
            }

function showTip(){
    loopvalue = this.loopvalue
    reverseMarker(locationInfo,locationmarker);
    reverseMarker(connectionInfo,connectionhostpot,'connection');
    var city_boxes = document.getElementsByClassName("city_box");
            for(var con=0;con< city_boxes.length; con++){
                city_boxes[con].classList.remove("active");
            }
    var extraspace = 10;
    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
        extraspace = 16;
    }

        myearth.animate('location', {lat:this.location['lat'],lng:this.location['lng']+extraspace} ,{duration: 2000});
        myearth.animate('zoom', 1.8, {duration: 1000 });
        locationInfo[loopvalue]['visible'] = true;
}

var closeinfobox = document.getElementsByClassName("closeInfobox");
for(var cl = 0; cl < closeinfobox.length; cl++) { // on close button info box hide
    var reverseMarkerele = closeinfobox[cl];
    reverseMarkerele.onclick = function() {
         reverseMarker(locationInfo,locationmarker);
        reverseMarker(connectionInfo,connectionhostpot,'connection');
    };
}

// mobile city boxes
var city_boxes = document.getElementsByClassName("city_box");
for(var cl = 0; cl < city_boxes.length; cl++) { // on close button info box hide
    var city_boxesele = city_boxes[cl];
    city_boxesele.onclick = function() {
        var location = connectionhostpot[this.getAttribute("data-index")].location
        var loopvalue = connectionhostpot[this.getAttribute("data-index")].loopvalue
        showConnectionInfo(location,loopvalue);

    };
}



 var placesToplacesLine = {
        color: "white",
        width: 0.2,
        dashed: true,
        scale: 2,
        dashSize: 0.030,
        offsetFlow: 0.1,
        dashRatio: 0.5,
        className: "line",
        alwaysBehind:true,
        clip:0,
};


var placesToplacesdashlines = [];
var countDistance = [];
var extPulse = [];
for ( var i in placesToplaces) {
    placesToplacesLine.locations = [{lat:placesToplaces[i]['connection'][0], lng: placesToplaces[i]['connection'][1] }, { lat: placesToplaces[i]['connection'][2], lng: placesToplaces[i]['connection'][3] }];
    placeStart = {lat:placesToplaces[i]['connection'][2],lng:placesToplaces[i]['connection'][3]};
    placeEnd = {lat:placesToplaces[i]['connection'][0],lng:placesToplaces[i]['connection'][1]};
    placesToplaces[i]['distance'] = Earth.getDistance(placeStart, placeEnd)/1000;
    var placedistance = placesToplaces[i]['distance'];

    if(placesToplaces[i].connection.length > 0){
        countDistance.push(Number(placedistance));
    }

    if (i > 0) {
        var pulsevis = true;
    }
    else {
        var pulsevis = false;
    }

    extPulse[i] = this.addImage({
            location: placesToplaces[i]['locations'][0],
            loopvalue: i,
            scale: 0.24,
            image: '../../images/AdaniConneX/globe/disk_thicker.png',
            color: 'white',
            visible: pulsevis,
            opacity: 1,
            transparent: true
        });

    extHotspot[i] = this.addMarker({
        location: placesToplaces[i].locations[0],
        loopvalue:i,
        contentIndex:placesToplaces[i].contentInfo,
        scale: placesToplaces[i].scale,
        hotspot: true,
        visible:true,
        hotspotRadius:placesToplaces[i].radius || 0.5,
        hotspotHeight:placesToplaces[i].height || 1.5,
        mesh: 'disc',
        rotationX: placesToplaces[i].rotationX,
        rotationY: placesToplaces[i].rotationY,
        rotationZ: placesToplaces[i].rotationZ,
        color: placesToplaces[i].color,
        lookAt: false
    });

    extHotspot[i].tip = this.addOverlay( {
        location: placesToplaces[i].locations[0],
        content : placesToplaces[i].labelName,
        offset: 0,
        depthScale : 0.25,
        hostpot:true,
        elementScale : 0.5,
        visible : false,
        className : 'tip-overlay tip-down placestoplacesLabel '+placesToplaces[i].className,
    } );

    animatePulse(extPulse[i], 0.24, 989, 200, 0.75);


    extHotspot[i].addEventListener("mouseover",function(){
        this.tip.visible = true;
    });
    extHotspot[i].addEventListener("mouseout",function(){
        this.tip.visible = false;
    });

     extHotspot[i].addEventListener("click",function(){
        if(this.contentIndex != 0){
            reverseMarker(locationInfo,locationmarker);
            reverseMarker(connectionInfo,connectionhostpot,'connection');
            var city_boxes = document.getElementsByClassName("city_box");
            for(var con=0;con< city_boxes.length; con++){
                city_boxes[con].classList.remove("active");
            }
            if(this.contentIndex == 3){
                var con_li = document.getElementsByClassName("country_li");
                for(var con=0;con< con_li.length; con++){
                    con_li[con].classList.remove("active");
                    con_li[3].classList.add("active");
                }
            }
            var extraspace = 10;
            if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
                extraspace = 16;
            }
             myearth.animate('zoom', 1.8, {duration: 1000 });
            myearth.animate('location', {lat: locationmarker[this.contentIndex].location.lat, lng: locationmarker[this.contentIndex].location.lng+extraspace} ,{duration: 2000});
            locationInfo[this.contentIndex]['visible'] = true;
        }
    });
    extHotspot[0].visible = false;
    extHotspot[0].removeEventListener("mouseout");
    extHotspot[0].removeEventListener("click");
}


var placestoplacesLabeloverlay = document.getElementsByClassName("placestoplacesLabel");
for(var y = 0; y < placestoplacesLabeloverlay.length; y++) {
    var placeelem = placestoplacesLabeloverlay[y].parentElement;
    placeelem.classList.remove("positions");
}
if (document.getElementById('myearth').getBoundingClientRect().top > window.innerHeight/2) {
        window.onscroll = function () {
            var zoomOnce = false;
            var scrollDiff = scrollZoom();
            if (scrollDiff < 2 && !zoomOnce) {
                zoomOnce = true;
                window.oldCountryIndex = 1;
                myearth.zoomable = false;
                myearth.animate( 'zoom', 1.5, { duration : 2000, complete : function() {
			        setTimeout(function() {myearth.zoomable = true;},1000)
		        } } );
            }
}
}
else {
    setTimeout(function() {
    initialZoom();
},1000);
}

function scrollZoom() {
        var sp = window.scrollY | document.body.scrollTop;
        var hp = document.getElementById('myearth').getBoundingClientRect().top;
        return sp - hp;
}

function initialZoom(){
    var zoomOnce = false;
    myearth.zoomable = false;
    myearth.animate( 'zoom', 1.5, { duration : 2000, complete : function() {
    setTimeout(function() {myearth.zoomable = true;},1000)
    }
});
    zoomOnce = true;
    window.oldCountryIndex = 1;
}

var country_li = document.getElementsByClassName("country_li");
for(var con = 0; con < country_li.length; con++) {
    var country_li_element = country_li[con];
    country_li_element.onclick = function() {
        for(var ss=0;ss<country_li.length;ss++){
            country_li[ss].classList.remove("active");
        }
        this.classList.add("active");
    };
}


updateMapTexture();

var m1 = {lat: 17,lng: 78};
var m2 = {lat:35.1495, lng:-90.0490};
var m3 = {lat:-34.6037, lng:-58.3816};
var m4 = {lat:50, lng:4};
var earthobj = [0,9,34,6]//[m1,m2,m3,m4]


myearth.addEventListener( 'dragend', function( event ) {

    var con_li = document.getElementsByClassName("country_li");

    for(var ea=0;ea<earthobj.length;ea++){
        if(!extHotspot[earthobj[ea]].occluded){ //7

                    for(var con=0;con< con_li.length; con++){
                        con_li[con].classList.remove("active");
                        con_li[ea].classList.add("active");
                    }
             break;
        }

    }

    });
});
});


function updateMapTexture() {
        setTimeout(function(){
        var oReq = new XMLHttpRequest();
        oReq.open("GET", "../../images/AdaniConneX/globe/mesh_map_light.jpg", true);
        oReq.responseType = "blob";
        oReq.onload = function(oEvent) {
            var blob = oReq.response;
            var urlCreator = window.URL || window.webkitURL;
            var imageUrl = urlCreator.createObjectURL(blob);
            myearth.mapImage = imageUrl;
            myearth.redrawMap();
        };
        oReq.send();
        },3000)
}

    function locationMarker(ci, zval) {
        try {
            var city_boxes = document.getElementsByClassName("city_box");
            for(var sls = 0;sls < city_boxes.length;sls++){city_boxes[sls].classList.remove("active");}

            window.activeCountryIndex = ci;
            var intZoom = zval;
            if (window.activeCountryIndex != window.oldCountryIndex) {
                intZoom = 0.8;
                window.oldCountryIndex = window.activeCountryIndex;
            }
            var targetLoc = countryObj[ci-1].locations1[0];
            myearth.animate('zoom', intZoom, {relativeDuration: 300, complete: function() {
                    myearth.goTo( targetLoc, { zoom:zval, relativeDuration: 100});
                }});
            reverseMarker(locationInfo,locationmarker);
            reverseMarker(connectionInfo,connectionhostpot,'connection');

        } catch {}
    }

    function reverseMarker(locationInfo,marker,from){
           for(var j=0;j<locationInfo.length;j++){
                locationInfo[j].visible = false;
           }
           if(from == "connection"){
               for(var k=0;k<marker.length;k++){
                    marker[k].color = 'white';
               }
           }
           locationmarker[0].tip.visible = true;
        }