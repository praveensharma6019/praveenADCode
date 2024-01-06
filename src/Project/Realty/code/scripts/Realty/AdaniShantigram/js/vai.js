/**
 * 
 * @param val
 * @returns {Boolean}
 */
function isSet(val) {
    if ((val != undefined) && (val != null) && ($.trim(val) != '')) {
        return true;
    }
    return false;
};

/**
 * 
 * @param name - Name of the cookie
 * @param value - Value of the cookie
 * @param expires - In days
 * @param path
 * @param domain
 * @param secure
 */
function Set_Cookie(name, value, expires, path, domain, secure) {
    // set time, it's in milliseconds
    var today = new Date();
    today.setTime(today.getTime());
    /*
	 * if the expires variable is set, make the correct expires time, the
	 * current script below will set it for x number of days, to make it for
	 * hours, delete * 24, for minutes, delete * 60 * 24
	 */
    if (expires) {
        expires = expires * 1000 * 60 * 60 * 24;
    }
    var expires_date = new Date(today.getTime() + (expires));
    document.cookie = name + "=" + escape(value) + ((expires) ? ";expires=" + expires_date.toGMTString() : "") + ((path) ? ";path=" + path : "") + ((domain) ? ";domain=" + domain : "") + ((secure) ? ";secure" : "");
}

/**
 * 
 * @param check_name
 * @returns
 */
function Get_Cookie(check_name) {
    // first we'll split this cookie up into name/value pairs
    // note: document.cookie only returns name=value, not the other components
    var a_all_cookies = document.cookie.split(';');
    var a_temp_cookie = '';
    var cookie_name = '';
    var cookie_value = '';
    var b_cookie_found = false; // set boolean t/f default f
    for (var i = 0; i < a_all_cookies.length; i++) {
        // now we'll split apart each name=value pair
        a_temp_cookie = a_all_cookies[i].split('=');

        // and trim left/right whitespace while we're at it
        cookie_name = a_temp_cookie[0].replace(/^\s+|\s+$/g, '');

        // if the extracted name matches passed check_name
        if (cookie_name == check_name) {
            b_cookie_found = true;
            // we need to handle case where cookie has no value but exists (no =
            // sign, that is):
            if (a_temp_cookie.length > 1) {
                cookie_value = unescape(a_temp_cookie[1].replace(/^\s+|\s+$/g, ''));
            }
            // note that in cases where cookie is initialized but no value, null
            // is returned
            return cookie_value;
            break;
        }
        a_temp_cookie = null;
        cookie_name = '';
    }
    if (!b_cookie_found) {
        return null;
    }
}

/**
 * 
 * @param name
 * @param path
 * @param domain
 */
function Delete_Cookie(name, path, domain) {
    if (Get_Cookie(name))
        document.cookie = name + "=" + ((path) ? ";path=" + path : "") + ((domain) ? ";domain=" + domain : "") + ";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}

$(document).ready(function() {
    $.extend({      
        getQueryString: function (name) {           
            function parseParams() {
                var params = {},
                e,
                a = /\+/g,  // Regex for replacing addition symbol with a space
                r = /([^&=]+)=?([^&]*)/g,
                d = function (s) {
                    return decodeURIComponent(s.replace(a, " "));
                },
                q = window.location.search.substring(1);

                while (e = r.exec(q))
                    params[d(e[1])] = d(e[2]);

                return params;
            }

            if (!this.queryStringParams)
                this.queryStringParams = parseParams(); 

            return this.queryStringParams[name];
        }
    });

    var Campaign = $.getQueryString('Campaign');
    if(isSet(Campaign)) {
        Delete_Cookie('Lead_Campaign');
        Set_Cookie('Lead_Campaign', Campaign);
    }

    var AdGroup = $.getQueryString('AdGroup');
    if(isSet(AdGroup)) {
        Delete_Cookie('Lead_AdGroup');
        Set_Cookie('Lead_AdGroup', AdGroup);
    }

    var Keyword = $.getQueryString('Keyword');
    if(isSet(Keyword)) {
        Delete_Cookie('Lead_Keyword');
        Set_Cookie('Lead_Keyword', Keyword);
    }

    var MatchType = $.getQueryString('MatchType');
    if(isSet(MatchType)) {
        Delete_Cookie('Lead_MatchType');
        Set_Cookie('Lead_MatchType', MatchType);
    }

    var Distribution = $.getQueryString('Distribution');
    if(isSet(Distribution)) {
        Delete_Cookie('Lead_Distribution');
        Set_Cookie('Lead_Distribution', Distribution);
    }

    var AdID = $.getQueryString('AdID');
    if(isSet(AdID)) {
        Delete_Cookie('Lead_AdID');
        Set_Cookie('Lead_AdID', AdID);
    }

    var Placement = $.getQueryString('Placement');
    if(isSet(Placement)) {
        Delete_Cookie('Lead_Placement');
        Set_Cookie('Lead_Placement', Placement);
    }

    var Channel = $.getQueryString('Channel');
    if(isSet(Channel)) {
        Delete_Cookie('Lead_Channel');
        Set_Cookie('Lead_Channel', Channel);
    }

    var Device = $.getQueryString('Device');
    if(isSet(Device)) {
        Delete_Cookie('Lead_Device');
        Set_Cookie('Lead_Device', Device);
    }
    
    
    
});
/*
function saveLead(leadParams) {
    jQuery(function($) {
        var leadData = {
            "recordData.fields['name'].value" : leadParams.name,
            "recordData.fields['email'].value" : leadParams.email,
            "recordData.fields['mobile'].value" : leadParams.mobile,
            "recordData.fields['channel'].value" : Get_Cookie('vaLead_Channel'),
            "recordData.fields['keyword'].value" : Get_Cookie('vaLead_Keyword'),
            "recordData.fields['placement'].value" : Get_Cookie('vaLead_Placement'),
            "recordData.fields['device'].value" : Get_Cookie('vaLead_Device'),
            "recordData.fields['pageUrl'].value" : $(location).attr('href'),
            "recordData.fields['prospectCapturingPoint'].value" : leadParams.prospectCapturingPoint,
            "recordData.fields['campaign'].value" : Get_Cookie('vaLead_Campaign'),
            "recordData.fields['description'].value" : leadParams.description
        };

        // convert our object to a query string. 
        leadData = $.map(leadData, function(value, index) {
            return index + '=' + encodeURIComponent(value);
        });
        leadData = leadData.join("&");
        var finalSrc = 'http://54.251.248.213:8080/VitruvianAnalytics/webToLead.html?module=lead&action=create&webToLead=Yes&appID=423702511461754965&' + leadData;

        var iframe = document.createElement('iframe');
        iframe.style.width = '0px';
        iframe.style.height = '0px';
        iframe.name = 'va_iframe';
        iframe.id = 'va_iframe';
        document.body.appendChild(iframe);
        iframe.src = finalSrc;

    });
}
*/