var BotClient = BotClient || {};
BotClient.isSessionActive=false;

BotClient.ServerConfiguration={

    maxReconnectTry: 3,
    serverURL: 'wss://sscchatbotira.ltindia.com:8080',
    botName : "Ira"
}
BotClient.errorMessages={

    DirectLineFailedToCreateSession:"Could not able to create a session with Direct line:",
    DirectLineUnauthorized:"Could not establish connection with Direct Line as 401 Unauthorized error codes received please contact IRA administrator",
    DirectLineSessionExpire:"Your session is expired, you can create a new session by reloading the page.",
    DirectLineMessageSendFailed:"Unable to send message to direct line, please try to resend",
    ServerNotAvailable: "Sorry, but there\'s some problem with your connection or the server is down.",
    NoInternet: "you are offline.. please check your internet connection",
    ConnectionClosed: "IRA is offline, trying to reconnect.",
    MaxReconnectAttempted: "We have tried to connect with Ira few times, but not able to reconnect. Please try after some time...",
    SocketNotSupported: "Sorry, but your browser doesn\'t 'support WebSockets.",
    InvalidDataInMessage: "Could not understand the response"
}


BotClient.MessageType={

    Typing:"typing",
    Message:"message",
    BotConfig:"botconfig",
    ClientConfig:"clientconfig",
    History:"history",
    ModuleSelection:"moduleselection",
    Error:"error",
    Information:"information",
    Success:"success",


}
BotClient.StatusCodesForClient={
    ERROR_DIRECT_LINE_UNAUTHORIZED:401,
    ERROR_DIRECT_LINE_NOT_CONNECTED:500,
    ERROR_DIRECT_LINE_SESSION_EXPIRE:403,
    ERROR_MESSAGE_NOT_SENT_TO_DIRECT_LINE:502,
    SUCCESS_MESSAGE_SENT_TO_BOT:200,
    MESSAGE_RECEIVED:201,

}
BotClient.Session={};

BotClient.Session.InitSession=function(){

    BotClient.userContext = {


        userName: false,
        conversationId: false,
        userid: false,
        currentSessionId: false,
        isActive: false,
        connectionCreatedOn: false,
        canHaveMultipleSession: false,
        canDuplicateTheChatWindow: false,
        authData: false,
        psno:false,
        isLoggedIn: false,
        loadLastHistory: false,
        activeSocket: false,
        maxActiveConnections: 3,
        email:false
    
    
    
    }
    BotClient.Session.SetupBotConfig=function(json){

        BotClient.  userContext.conversationId = json.conversationId;
        BotClient.  userContext.currentSessionId = json.currentSessionId;

        BotClient.isSessionActive=true;
        BotClient. userContext.botName = json.botName;
        BotClient.ServerConfiguration.botName = json.botName;
        BotClient.userContext.connectionCreatedOn = json.connectionCreatedOn;
    }


}