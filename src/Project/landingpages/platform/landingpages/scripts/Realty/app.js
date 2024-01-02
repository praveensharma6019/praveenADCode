

    /**
     * @description this function will be used to start the chatclient 
     * and the needs to pass the required param else app wont start
     * this function must be called after the document ready
     * @param {psno PSNO is the user id for the current user} 
     * @param {*username this is name of current user}  
     * @param {*email id of current user *optionl} 
     */
function startChatApp (psno,username,email){


   // alert(psno + username+ email);

    BotClient.Init();
    if(psno==undefined ||psno==""){
        console.log("Could not start the bot as PSNO is not available..");
    }
    if(username==undefined ||username==""){
        console.log("Could not start the bot as PSNO is not available..");
    }
    
    //#region initialze the data below
    BotClient.userContext.psno=psno;
    BotClient.userContext.userName=username;
    BotClient.userContext.email=email;
    BotClient.userContext.isLoggedIn = true;
    BotClient.userContext.authData = null;
   
    BotClient.userContext.isActive = true;  
  
    //#endregion

}

$(function() {
    "use strict";
    var connection = false;
    

    BotClient.Init=function(){

        BotClient.initUtil();
      
        clientHook();
        createWebhookConnection();
        BotClient.Session.InitSession();
    }

 //   startChatApp("445534343","sunil soni","sunil.soni@advaiya.com");
 
    
    BotClient.SendToBot=function(message){

        connection.send(JSON.stringify(message));

    }
    

    
/**
 * @description this function is used to initialize the basic websoket object for socket connection
 */
   function clientHook() {
        // if browser doesn't support WebSocket, just show some notification and exit
        window.WebSocket = window.WebSocket || window.MozWebSocket;
        if (!window.WebSocket) {
            BotClient.UIComponent.content.html($('<p>', {
                text: errorMessages.SocketNotSupported
            }));
            BotClient.UI.DisableUI(true);
         
            return;
        }
    }
    // if user is running mozilla then use it's built-in WebSocket

    /**
     * @description this function is used to create a socket based connection and add to user context
     */

   function createWebhookConnection() {


        connection = new WebSocket(BotClient.ServerConfiguration.serverURL);

        //open the connection
        connection.onopen = function() {
            // first we want users to enter their names
           
        };
        // in case of errors
        connection.onerror = function(error) {
            // just in there were some problems with conenction...
           BotClient.UIComponent.content.html($('<p>', {
                text: BotClient.errorMessages.ServerNotAvailable
            }));
        };


        // most important part - incoming messages
        connection.onmessage = function(message) {
            // try to parse JSON message. Because we know that the server always returns
            // JSON this should work without any problem but we should make sure that
            // the massage is not chunked or otherwise damaged.
            try {
                var json = JSON.parse(message.data);
            } catch (e) {
                console.log('This doesn\'t look like a valid JSON: ', message.data);
                return;
            }
            BotClient.isSessionActive=true;
            // NOTE: if you're not sure about the JSON structure
            // check the server source code above

            if (json.type == BotClient.MessageType.Typing) {

                BotClient.UI.IsTyping(true);
                return;
            }
            BotClient.UI.IsTyping(false);

            if (json.type === BotClient.MessageType.BotConfig) { // first response from the server with user's color

                //user name must be asssigned at the user's end and session must be shared then
              
                BotClient.Session.SetupBotConfig(json);
                //need to remove the get current user as it is available in the rootobject namespace
              //  getCurrentUser();
            
               

                //share the logged in information with chat server;
                var configuration = {
                    type: BotClient.MessageType.ClientConfig,
                    userContext: BotClient.userContext,


                }
             
                BotClient.SendToBot(configuration);


               
                // from now user can start sending messages
            } else if (json.type === BotClient.MessageType.History) { 
             
                for (var i = 0; i < json.data.length; i++) {
                    BotClient.UI.addMessageToUI(json.data[i].author, json.data[i].text,
                        json.data[i].color, new Date());
                }
            } else if (json.type === BotClient.MessageType.Message) { 
             
                BotClient.UI.addMessageToUI(json.data.author, json.data.text,
                    json.data.color, new Date(),"botmsg"+Math.floor((Math.random() * 1000) + 1));

                    BotClient.UI.ShowHideSuggetions(json.data.suggetions);
                        
            
            }
            else if(json.type==BotClient.MessageType.Error){

                console.log(json.data.text);
               
                if(json.data.status==BotClient.StatusCodesForClient.ERROR_DIRECT_LINE_NOT_CONNECTED){
 
                    console.log(BotClient.errorMessages.DirectLineFailedToCreateSession+" "+json.text);
                    BotClient.UI.addMessageToUI("Ira",BotClient.errorMessages.DirectLineFailedToCreateSession,"red",new Date(),0,"1");
                    BotClient.UI.DisableUI(true);
                    BotClient.isSessionActive=false;
                }
                else 
                if(json.data.status==BotClient.StatusCodesForClient.ERROR_DIRECT_LINE_UNAUTHORIZED){
                    console.log(BotClient.errorMessages.DirectLineUnauthorized);
                    BotClient.UI.addMessageToUI("Ira",BotClient.errorMessages.DirectLineUnauthorized,"red",new Date(),0,"2");
                    BotClient.isSessionActive=false;
                }
                else
                if(json.data.status==BotClient.StatusCodesForClient.ERROR_DIRECT_LINE_SESSION_EXPIRE){
                    console.log(BotClient.errorMessages.DirectLineSessionExpire);
                    BotClient.UI.addMessageToUI("Ira",BotClient.errorMessages.DirectLineSessionExpire,"red",new Date(),0,"3");
                    BotClient.isSessionActive=false;
                }
                else
                if(json.data.status==BotClient.StatusCodesForClient.ERROR_MESSAGE_NOT_SENT_TO_DIRECT_LINE)
                {
                    $("#"+json.data.msgid).show();
                    console.log(BotClient.errorMessages.DirectLineMessageSendFailed);
             
                }

            }
            else if(json.type==BotClient.MessageType.Success){

                console.log(json.data.text);
                if(json.data.status==BotClient.StatusCodesForClient.SUCCESS_MESSAGE_SENT_TO_BOT){

                    $("#"+json.data.msgid).show();
                    $("#"+json.data.msgid).text("");
                    $("#"+json.data.msgid).attr('name',"");
                    $("#"+json.data.msgid).html("&#10003;");
                    $("#"+json.data.msgid).on("click",function(){});
                    
                }

            }
             else {
                console.log('Hmm..., I\'ve never seen JSON like this: ', json);
            }
        };


    }


    
  
    /**
     * Send mesage when user presses Enter key
     */
  


    

    /**
     * @description actionButtonSend alllow you to send the message to server
     * @param {* val is the message you want to send to server} 
     */
  BotClient.SendMessage=  function(val,addtoMessages) {

           
    
        if( BotClient.isSessionActive==false){

            //
            return;
        }

        if(val==""||val==undefined){

            return;
        }

        var msgid="msg"+Math.floor((Math.random() * 1000) + 1);
        var json = {
            type: "message",
            userContext: BotClient. userContext,
            message: String(val),
            msgid:msgid
        };
        // send the message as an ordinary text
      
        BotClient.SendToBot(json);
      val=  BotClient.UIComponent.preventHtml(val);
        if(addtoMessages){
            BotClient.UI.addMessageToUI(BotClient.userContext.userName,val, "red", new Date(), 1,msgid);
        }
    
        
     
    }





   

   
    setInterval(function() {
     
        if(BotClient!=undefined){
        var json = {
            type: "refresh",
            userContext: BotClient.userContext,
        };
        BotClient.SendToBot(json);
       }

    }, 900000);


    /**
     * This method is optional. If the server wasn't able to respond to the
     * in 3 seconds then show some error message to notify the user that
     * something is wrong.
     */
    setInterval(function() {

        if(connection.readyState==3){
            BotClient.UI.DisableUI(true);
        }
        else
        if (connection.readyState !== 1) {
            // status.text('Error');
            if(BotClient.isSessionActive==true){
                BotClient.UI.DisableUI(false);
            }
            else{
                BotClient.UI.DisableUI(true);
            }
           
        } else {

            if(BotClient.isSessionActive==false){
                BotClient.UI.DisableUI(true);
            }
            else{
                BotClient.UI.DisableUI(false);   
            }
            
        }
    }, 8000);

    




  
});
