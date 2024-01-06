function sendMessageToServer(){
    BotClient.SendMessage(BotClient.UIComponent.input.val(),true);
    BotClient.UIComponent.input.val("");
}
$(document).ready(function(){
   
})
BotClient.initUtil=function(){
	
    BotClient.UIComponent={

        typingIndicator : $("#typingIndicator"),
        content : $('#list_messages'),
        input : $('#input'),
        status : $('#status'),
        suggestions : $('#suggestions'),
        suggestionsBar:$("#suggestionsBar"),
        messages:$("#messages"),
        title : $("#title"),
        sendButton:$("#submit"),


    }
    BotClient.UIComponent.input.attr('disabled', 'disabled');         
    BotClient.UIComponent.suggestionsBar.hide();
    BotClient.UIComponent.typingIndicator.hide();
  
    

    // $("#submit").click (function() {


        
    //     BotClient.SendMessage(BotClient.UIComponent.input.val(),true);
    //     BotClient.UIComponent.input.val("");

    // });
	

    

    BotClient.UI={

        
    }

    BotClient.UI.imback = function(button) {

        BotClient.SendMessage(button.textContent,true);
        BotClient.UIComponent.suggestionsBar.hide();
    }
    BotClient.UI.postBack = function(button,type) {

        BotClient.SendMessage(button.value,false);
        BotClient.UIComponent.suggestionsBar.hide();
    }


    BotClient.UI.RetryMessage=function(text){
        BotClient.SendMessage(msg,false);
        alert(text);
    }
	
    BotClient.UI.toggleModule=function (radio){


     
        var configuration={
            type:BotClient.MessageType.ModuleSelection,
            moduleName:radio.value,
            conversationId:BotClient.userContext.conversationId,
            currentSessionid:BotClient.userContext.currentSessionId
        }
        BotClient.SendToBot(configuration);
    }
   

    BotClient.UIComponent.preventHtml=function(message){

        var reg =/<(.|\n)*?>/g; 
        if (reg.test(message) == true) {

              return   String(message).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');;           
        
            }
            else
            return message;

    }
    
    BotClient.UIComponent.input.keypress(function(e) {


        if (e.keyCode != 13) return ;;
        e.preventDefault();
         var msg =  BotClient.UIComponent.input.val().replace("\n", "");
        
         if (msg!="")
         {
            BotClient.SendMessage(msg,true);
            BotClient.UIComponent.input.val("");
         }
        return false;
       
    });

    BotClient.UI.ShowHideSuggetions=function(suggestionsData){

        if(typeof suggestionsData==undefined||suggestionsData==""){
            BotClient.UIComponent.suggestions.html("");
            BotClient.UIComponent.messages.removeClass("suggestion-open");
           // BotClient.UIComponent.messages.removeClass(");
            BotClient.UIComponent.suggestionsBar.hide();
        }    
        else{
            BotClient.UIComponent.typingIndicator.hide();
            BotClient.UIComponent.messages.addClass("suggestion-open");
            //BotClient.UIComponent.messages.addClass("suggestions");
            BotClient.UIComponent.suggestionsBar.show();
            BotClient.UIComponent.suggestions.append(suggestionsData);
          
        }


    }

    BotClient.UI.Retry=function(a,author){

        if(a.name==""){
            return;
        }
        //find parent of parent and remove from the list
        $('#'+a.id).closest('li').remove()
        //if need to display message again then use false so message will not duplicate
        BotClient.SendMessage(a.name,true);
        a.style.display = 'none';
    }
    

      /**
     * @description this function will used to add the formatted message to the chat window
     * @param {author is the person who send the message}  
     * @param {* message is the actual text to be add}  
     * @param {* color is a temparory value}
     * @param {*dt is the date on which message sent/received} 
     * @param {*formuser os a flag to determine the message is sent from user or received from bot} 
     */
   
   BotClient.UI.addMessageToUI=function(author, message, color, dt, fromUser,msgid) {

    

        if (fromUser == 1) {

				//<img src="images/harveyspecter.png" alt="">' +
				//'<img src="images/mikeross.png" alt="">' +
            var finalMessage = '<li class="replies chat__item">' +
                
                '<div class="info-text arrow_box">' +
                '<span class="message-info align-right">' +
                '<small class="text-muted">' +
                '<span class="message-data-botname">' + author +
                '</span>' +
                '<span class="message-data-time">' +
                '<i class="fa fa-clock-o">' +
                '</i>' + getDate(dt) +
                '</span>' +
                '</small>' +
                '</span>' +
                '<span class="msj-rta macro">' + BotClient.UIComponent.preventHtml(message)+
              
                 '</span>' +
                 '<a id="'+msgid+'" onclick="BotClient.UI.Retry(this)" name="'+message+'" style="margin-top:7px;float:right;font-size: 9px;display:none;line-height: 7px;text-decoration: none;"  href="#" onclick="BotClient.UI.RetryMessage">Retry</a>'+
            '</div>' +
            '</li>'
            BotClient.UIComponent.content.append(finalMessage);
           
        } else {

			
			
            var botMessage = '<li class="sent chat__item">' +
                
                '<div class="info-text arrow_box2">' +

                '<span class="message-info align-right">' +
                '<small class="text-muted">' +
                '<span class="message-data-botname">' + author +
                '</span>' +
                '<span class="message-data-time">' +
                '<i class="fa fa-clock-o">' +
                '</i>' + getDate(dt) +
                '</span>' +
                '</small>' +
                '</span>' +
                '<span class="msj macro">' + message +
                '</span>' +
               
                '</div>' +
                '</li>';
		   
           BotClient.UIComponent.content.append(botMessage);

        }

        scrollToBottom();
        
		$(".ac-adaptivecard a").click(function(e){
            e.preventDefault();
            var currobj=this;
            window.open(this.href,'_blank');
        });
		$(".ac-action-submit").click(function(){
			var Message =$(this).attr('data-ac-submitdata');
			    BotClient.SendMessage(Message,false);
        });
        $('.ac-actionset').find("button").click(function(){ 
            var cardid= $(this).attr("data-ac-showcardid");

            $('.ac-actionset').find("button").each(function(index,btn){

                //var tempid=btn.attributes.find('data-ac-showcardid');
               // var tempid= $("#"+btn).attr("data-ac-showcardid");
                $("#"+btn.attributes['data-ac-showcardid'].value).hide();

            });
         $("#"+cardid).show();

        });

       
    }


    BotClient.UI.IsTyping=function(istyping){

        if(istyping){
            BotClient.UIComponent.typingIndicator.show(100);
        }
        else{
            BotClient.UIComponent.typingIndicator.hide(100);
        }

    }

    BotClient.UI.DisableUI=function(isDisable){

        if(isDisable){

            BotClient.UIComponent.input.attr('disabled', 'disabled').val('');
           
            BotClient.UIComponent.title.html(BotClient.ServerConfiguration.botName +' is offline');

        }
        else{

          BotClient.UIComponent.input.removeAttr('disabled');
          BotClient.UIComponent.title.html(BotClient.ServerConfiguration.botName + ' is available');
        }

    }

   

    function scrollToBottom(){

        //Known issue :the scroll bar does not totaly scroll to bottom
        var objDiv = document.getElementById("messages");
        objDiv.scrollTop = objDiv.scrollHeight;
    
    }
    
    /**
     * 
     * @param {dt is the date on which message is sent/received}  
     */
    function getDate(dt) {

        var hours = dt.getHours();
        var minutes = dt.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0'+minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
        
    }


}


