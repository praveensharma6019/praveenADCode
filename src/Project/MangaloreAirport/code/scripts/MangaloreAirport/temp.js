    $(document).ready(function(){
 $('a[href^="https://stage.adaniuat.com:443/mangaluru-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("https://stage.adaniuat.com:443/mangaluru-airport/", "https://stagemangaluru.adaniairports.com/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
		 $('a[href^="/mangaluru-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/mangaluru-airport/", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
	 $('a[href="/mangaluru-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/mangaluru-airport", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		});