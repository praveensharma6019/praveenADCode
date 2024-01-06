    $(document).ready(function(){
 $('a[href^="https://stage.adaniuat.com:443/ccsia-lucknow-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("https://stage.adaniuat.com:443/ccsia-lucknow-airport/", "https://stagelucknow.adaniairports.com/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
		 $('a[href^="/ccsia-lucknow-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/ccsia-lucknow-airport/", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
	 $('a[href="/ccsia-lucknow-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/ccsia-lucknow-airport", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		});