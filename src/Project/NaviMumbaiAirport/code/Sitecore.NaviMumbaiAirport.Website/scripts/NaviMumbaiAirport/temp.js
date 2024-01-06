    $(document).ready(function(){
 $('a[href^="https://stage.adaniuat.com:443/svpia-ahmedabad-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("https://stage.adaniuat.com:443/svpia-ahmedabad-airport/", "https://stageahmedabad.adaniairports.com/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
		 $('a[href^="/svpia-ahmedabad-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/svpia-ahmedabad-airport/", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		
	 $('a[href="/svpia-ahmedabad-airport"]').each(function(){ 
            var oldUrl = $(this).attr("href"); // Get current url
            var newUrl = oldUrl.replace("/svpia-ahmedabad-airport", "/"); // Create new url
            $(this).attr("href", newUrl); // Set herf value
        });
		});