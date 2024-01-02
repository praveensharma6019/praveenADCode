function filterList(inputObj, listObj, hiddenTitle = false, titleSearch = false, noData = false, bold = false, removeSections = false) {
    var catArray = [];
    if (bold) {
        catList = document.querySelectorAll(".faqCategory span");
        for (let i of catList) {
            catArray.push(i.innerHTML.toUpperCase());
        }
    }

    var input, filter, ul, li, a, i, txtValue;
    input = document.querySelector(inputObj);
    if(input.value.replace(/\s/g, "") === "") {
        input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
        if (removeSections) {

            document.querySelectorAll('.sub-section-title').forEach(function (e) {

                e.classList.remove('d-none');

            });

        }
        } else {
            if (removeSections) {

                document.querySelectorAll('.sub-section-title').forEach(function (e) {

                    e.classList.add('d-none');

                });

            }
        input.closest('.inside-search').querySelector(".clearIcon").classList.remove('d-none');
    }
    
    filter = input.value.toUpperCase().trim();
    li = document.querySelectorAll(listObj);
    //console.log(li);
    // li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        a = li[i];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
            if(titleSearch){
                li[i].closest(".serviceList_detail").style.display = "";
            }
            
        } else {
            li[i].style.display = "none";
            if(titleSearch){
                li[i].closest(".serviceList_detail").style.display = "none";
            }
        }
        if(bold){
            li[i].querySelector('.desc').innerHTML = li[i].querySelector('.desc').textContent.trim();
            if(filter != ''){
                li[i].querySelector('.desc').innerHTML = li[i].querySelector('.desc').innerHTML.replace(new RegExp(filter, "gi"), (match) => `<strong>${match}</strong>`);
                if (document.querySelectorAll(listObj + ':not([style*="display: none"])').length) {
                    if (catArray.indexOf(filter) != -1) {
                        console.log('sdsd');
                        document.querySelector('.viewAll_faqList').classList.remove('d-none');
                    } else {
                        document.querySelector('.viewAll_faqList').classList.add('d-none');
                    } 
                    document.querySelector(listObj).closest('div').querySelector('.searchText').innerHTML = input.value;
                    linkvalue = document.querySelector(listObj).closest('div').querySelector('.searchText').closest('a');
                    linkvalue.href = linkvalue.getAttribute("data-href") + '/' + input.value;
                } else {
                    document.querySelector('.viewAll_faqList').classList.add('d-none'); 
                }
            } else {
                document.querySelector('.viewAll_faqList').classList.add('d-none'); 
            }
        }
    }
    
    if(hiddenTitle){
        sectionList = document.querySelectorAll('.filtersection');
        for (i = 0; i < sectionList.length; i++) {
            subsectionList = sectionList[i].querySelectorAll('.filtersubsection');
            /* filter on main Subtitle */
            for (si = 0; si < subsectionList.length; si++) {
                subsectionListELM = subsectionList[si].closest('.filtersubsection').querySelector('.filtersubsectionTitle')
                if(subsectionList[si].querySelectorAll('li:not([style*="display: none"])').length == 0){
                    if(typeof(subsectionListELM) != 'undefined' && subsectionListELM != null){
                        subsectionListELM.closest('.filtersubsection').classList.add('d-none');
                    }
                } else {
                    if(typeof(subsectionListELM) != 'undefined' && subsectionListELM != null){
                        subsectionListELM.closest('.filtersubsection').classList.remove('d-none');
                    }
                }
        
            }

            /* filter on main title */
            sectionListELM = sectionList[i].closest('.filtersection').querySelector('.filtersectionTitle')
            if(sectionList[i].querySelectorAll('li:not([style*="display: none"])').length == 0){
                if(typeof(sectionListELM) != 'undefined' && sectionListELM != null){
                    sectionListELM.closest('.filtersection').classList.add('d-none');
                }
            } else {
                if(typeof(sectionListELM) != 'undefined' && sectionListELM != null){
                    sectionListELM.closest('.filtersection').classList.remove('d-none');
                }
            }
        } 

        if(document.querySelectorAll(listObj+':not([style*="display: none"])').length){
           document.querySelector('.noFoundsection').classList.add('d-none'); 
        } else {
            document.querySelector('.noFoundsection').classList.remove('d-none'); 
        }

    }

    if(titleSearch || noData ){
        if(document.querySelectorAll(listObj+':not([style*="display: none"])').length){
            document.querySelector('.noFoundsection').classList.add('d-none'); 
        } else {
             document.querySelector('.noFoundsection').classList.remove('d-none'); 
        }
    }
    
    
 }

 function clearSearch(inputObj,listObj,hiddenTitle,titleSearch,noData=false,bold = false ){
    input = document.querySelector(inputObj);
    input.value = '';
    input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    filterList(inputObj, listObj,hiddenTitle,titleSearch,noData,bold);
    if(document.querySelector(inputObj).closest('.inside-search').querySelector("input")){
        document.querySelector(inputObj).closest('.inside-search').querySelector("input").focus();
    }
 }

inputFieldJSElem = document.querySelectorAll('.inputFieldJS');

if(inputFieldJSElem.length) {
    for (let i of inputFieldJSElem) {
        i.addEventListener('keyup', 
            e => { 
                if(e.target.value.trim() != ''){
                    e.target.closest('.inputFieldJS').querySelector('.clearIcon').classList.remove('d-none');
                } else {
                    e.target.closest('.inputFieldJS').querySelector('.clearIcon').classList.add('d-none');
                }
            }
        );
    }
}

inputElem_clear = document.querySelectorAll('.inputFieldJS .clearIcon');
if(inputElem_clear.length) {
    for (let i of inputElem_clear) {
        i.addEventListener('click', 
            e => { 
                e.target.closest('.inputFieldJS').querySelector('input').value = '';
                e.target.closest('.inputFieldJS').querySelector('.clearIcon').classList.add('d-none');
            }
        );
    }
}

serviceFaqListHover = false;

if(jQuery(".faq-heading .serviceFaqList .inside-search")){
    jQuery(".faq-heading .serviceFaqList .inside-search, .faq-heading .serviceFaqList .search-wrapper").mouseenter(function() {
        serviceFaqListHover = true;
    }).mouseleave(function() {
        serviceFaqListHover = false;
    });
}


jQuery('body').click(function() {    
    if(serviceFaqListHover) {
        jQuery('#serviceInput1').closest('.serviceFaqList').addClass('show');
    } else {
        jQuery('#serviceInput1').closest('.serviceFaqList').removeClass('show');
    }
});

function accordionFilterList(inputObj, listObj, nofound, limit = 99999) {
    var input, filter, ul, li, a, i, txtValue;
    input = document.querySelector(inputObj);
    if (input.value.replace(/\s/g, "") === "") {
        input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    } else {
        input.closest('.inside-search').querySelector(".clearIcon").classList.remove('d-none');
    }
    filter = input.value.toUpperCase().trim();
    accordionlist = document.querySelectorAll(listObj);
    countlist = 0;
    for (i = 0; i < accordionlist.length; i++) {
        a = accordionlist[i];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1 && countlist < limit) {
            countlist++;
            accordionlist[i].classList.remove('d-none');
        } else {
            accordionlist[i].classList.add("d-none");
        }
    }
    if (document.querySelectorAll(listObj + '.d-none').length == document.querySelectorAll(listObj).length) {
        document.querySelector(nofound).classList.remove('d-none')
    } else {
        document.querySelector(nofound).classList.add('d-none')
    }
}

function clearAccordionFilterList(inputObj, listObj, nofound){
    input = document.querySelector(inputObj);
    input.value = '';
    input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    accordionFilterList(inputObj, listObj, nofound)
 }
if ($('#serviceList li').length < 11) {
    $('.searchSection').hide();
}

function getRows(selector) {
    var height = jQuery(selector).height();
    var line_height = jQuery(selector).css('line-height');
    line_height = parseFloat(line_height)
    var rows = height / line_height;
    return Math.round(rows);
}
jQuery(document).ready(function () {
    jQuery('.text-wrapper').each(function (e) {
        if (!jQuery(this).hasClass('cacographic')) {
            if (getRows(jQuery(this)) > 2) {
                jQuery(this).addClass('cacographic');
            } else {
                jQuery(this).siblings('.sectionReadMore').remove();
            }
        }
    });
})
function sectionReadMoreToggle(obj) {
    if (jQuery(obj).siblings('.text-wrapper').hasClass('cacographic')) {
        jQuery(obj).siblings('.text-wrapper').removeClass("cacographic");
        jQuery(obj).siblings('.sectionReadMore').html('Read Less');
        jQuery(obj).html('Read Less');
    } else {
        jQuery(obj).siblings('.text-wrapper').addClass("cacographic");
        jQuery(obj).html('Read More');
    }
}

jQuery(document).ready(function () {

    var $this = $('.items');
    if ($this.find('li').length > 2) {
        $('.items').append('<div><a href="javascript:;" class="showMore black_link"></a></div>');
    }

    // If more than 2 Education items, hide the remaining
    $('.items li').slice(0, 5).addClass('shown');
    $('.items li').not('.shown').hide();
    $('.items .showMore').on('click', function () {
        $('.items li').not('.shown').toggle(400);
        $(this).toggleClass('showLess');
    });

});