/* 
Simple JQuery menu.
HTML structure to use:

Notes: 

1: each menu MUST have an ID set. It doesn't matter what this ID is as long as it's there.
2: each menu MUST have a class 'menu' set. If the menu doesn't have this, the JS won't make it dynamic

Optional extra classnames:

noaccordion : no accordion functionality
collapsible : menu works like an accordion but can be fully collapsed
expandfirst : first menu item expanded at page load

<ul id="menu1" class="menu [optional class] [optional class]">
<li><a href="#">Sub menu heading</a>
<ul>
<li><a href="http://site.com/">Link</a></li>
<li><a href="http://site.com/">Link</a></li>
<li><a href="http://site.com/">Link</a></li>
...
...
</ul>
<li><a href="#">Sub menu heading</a>
<ul>
<li><a href="http://site.com/">Link</a></li>
<li><a href="http://site.com/">Link</a></li>
<li><a href="http://site.com/">Link</a></li>
...
...
</ul>
...
...
</ul>

Copyright 2008 by Marco van Hylckama Vlieg

web: http://www.i-marco.nl/weblog/
email: marco@i-marco.nl

Free for non-commercial use
*/

function initMenus() {
	$('ul.menu ul').hide();
	
	var page_name = location.href.substring(location.href.lastIndexOf('\/')+1);
	var menu_to_expand = "";

	switch(page_name)
	{
		case "commercial":
			menu_to_expand = "solutionsMenu";
			break;
		case "residential":
			menu_to_expand = "solutionsMenu";
			break;		
		case "lonworks":
			menu_to_expand = "technologyMenu";
			break;
		case "knx":
			menu_to_expand = "technologyMenu";
			break;
		case "dali":
			menu_to_expand = "technologyMenu";
			break;	
		case "wireless":
			menu_to_expand = "technologyMenu";
			break;	
		case "other":
			menu_to_expand = "technologyMenu";
			break;
		case "consultancy":
			menu_to_expand = "servicesMenu";
			break;		
		case "projects":
			menu_to_expand = "servicesMenu";
			break;
		case "installation":
			menu_to_expand = "servicesMenu";
			break;
		case "maintenance":
			menu_to_expand = "servicesMenu";
			break;	
		case "integration":
			menu_to_expand = "servicesMenu";
			break;	
	}
		$('#' + menu_to_expand).show();

	$.each($('ul.menu'), function(){
		$('#' + this.id + '.expandfirst ul:first').show();
	});
	$('ul.menu li a').click(
		function() {
			var checkElement = $(this).next();
			var parent = this.parentNode.parentNode.id;

			if($('#' + parent).hasClass('noaccordion')) {
				$(this).next().slideToggle('normal');
				return false;
			}
			if((checkElement.is('ul')) && (checkElement.is(':visible'))) {
				if($('#' + parent).hasClass('collapsible')) {
					$('#' + parent + ' ul:visible').slideUp('normal');
				}
				return false;
			}
			if((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
				$('#' + parent + ' ul:visible').slideUp('normal');
				checkElement.slideDown('normal');
				return false;
			}
		}
	);
}
$(document).ready(function() {initMenus();});