$(document).ready( function() {
	
$(function() {
	/**
	 * the menu
	 */
	var $menu = $('#ldd_menu');
	
	/**
	 * for each list element,
	 * we show the submenu when hovering and
	 * expand the span element (title) to 510px
	 */
	$menu.children('li').each(function(){
		var $this = $(this);
		/*var $span = $this.children('span');
		$span.data('width',$span.width());*/
		
		$this.bind('mouseenter',function(){
			/*$menu.find('.ldd_submenu').stop(true,true).hide();*/
			$this.find('.ldd_submenu').slideDown(10);
			$this.find('.hr_submenu').slideDown(10);
			/*$span.stop().animate({'width':'510px'},300,function(){
				$this.find('.ldd_submenu').slideDown(300);
			});*/
		});
		$this.bind('mouseleave',function(){
			$this.find('.ldd_submenu').slideUp(10);
			$this.find('.hr_submenu').slideUp(10);
			/*$this.find('.ldd_submenu').stop(true,true).hide();
			$span.stop().animate({'width':$span.data('width')+'px'},300);*/
		});
	});
});	


});

function menu(url)
{
	window.location = url;
}


function compare_date2_bigger_equal_date1(date_1,date_2)
{
	var array_date_1 = new Array();
	//split string and store it into array
	array_date_1 = date_1.split('-');

	var array_date_2 = new Array();
	//split string and store it into array
	array_date_2 = date_2.split('-');


if(array_date_2[2] == array_date_1[2])
{
   if(array_date_2[1] == array_date_1[1])
   {
      if(array_date_2[0] == array_date_1[0])
      {
       return true;
	  }
	  else if(array_date_2[0] > array_date_1[0])
	  {
       return true;
	  }
	  else if(array_date_2[0] < array_date_1[0])
	  {
       return false;
	  }
   }
   else if(array_date_2[1] > array_date_1[1])
   { 
      return true;
   }
   else(array_date_2[1] < array_date_1[1])
   {
      return false;
   }
   
}
else if(array_date_2[2] > array_date_1[2])
{
  return true;
}
else if(array_date_2[2] < array_date_1[2])
{
  return false;
}

}


function compare_date2_bigger_only_date1(date_1,date_2)
{
var array_date_1 = new Array();
//split string and store it into array
array_date_1 = date_1.split('-');

var array_date_2 = new Array();
//split string and store it into array
array_date_2 = date_2.split('-');


if(array_date_2[2] == array_date_1[2])
{
	return false;
}
else if(array_date_2[2] > array_date_1[2])
{
  return true;
}
else if(array_date_2[2] < array_date_1[2])
{
  return false;
}

}

function compare_date2_smaller_equal_date1(date_1,date_2)
{
var array_date_1 = new Array();
//split string and store it into array
array_date_1 = date_1.split('-');

var array_date_2 = new Array();
//split string and store it into array
array_date_2 = date_2.split('-');


if(array_date_2[2] == array_date_1[2])
{
   if(array_date_2[1] == array_date_1[1])
   {
      if(array_date_2[0] == array_date_1[0])
      {
       return true;
	  }
	  else if(array_date_2[0] > array_date_1[0])
	  {
       return false;
	  }
	  else if(array_date_2[0] < array_date_1[0])
	  {
       return true;
	  }
   }
   else if(array_date_2[1] > array_date_1[1])
   { 
      return false;
   }
   else(array_date_2[1] < array_date_1[1])
   {
      return true;
   }
   
}
else if(array_date_2[2] > array_date_1[2])
{
  return false;
}
else if(array_date_2[2] < array_date_1[2])
{
  return true;
}


}


function compare_date2_smaller_only_date1(date_1,date_2)
{
var array_date_1 = new Array();
//split string and store it into array
array_date_1 = date_1.split('-');

var array_date_2 = new Array();
//split string and store it into array
array_date_2 = date_2.split('-');


if(array_date_2[2] == array_date_1[2])
{
	return false;
}
else if(array_date_2[2] < array_date_1[2])
{
  return true;
}
else if(array_date_2[2] > array_date_1[2])
{
  return false;
}

}