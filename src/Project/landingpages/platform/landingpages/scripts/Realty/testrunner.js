$(function() {


// //   url for dev :/test/testData/test_users.json
// //   url for test:/chatbot/testData/test_users.json
   
//enable code below to test the chatbot with random test users
//     $.ajax({
//         type: "GET",
//         url: "/chatbotui/testData/test_users.json",
//         dataType: "json",
//         success: function(data) {


         

//             var index= Math.floor((Math.random() * data.length) + 1);
//             startChatApp(data[index].psno,data[index].name,data[index].email);

//         }
//      });
    

  
 startChatApp(12345,"testname","test@test.com");   




});