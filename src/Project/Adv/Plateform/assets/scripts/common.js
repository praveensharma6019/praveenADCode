function filterList(inputObj, listObj,hiddenTitle = false,titleSearch = false, noData = false) {
    var input, filter, ul, li, a, i, txtValue;
    input = document.querySelector(inputObj);
    if(input.value.replace(/\s/g, "") === "") {
        input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    } else {
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

 function clearSearch(inputObj,listObj,hiddenTitle,titleSearch,noData=false){
    input = document.querySelector(inputObj);
    input.value = '';
    input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    filterList(inputObj, listObj,hiddenTitle,titleSearch,noData)
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

function serviceFaqListToggle(obj,className,flag) {
    if(flag) {
        obj.closest('.serviceFaqList').classList.add(className);
    } else {
        if(serviceFaqListhover){
            obj.closest('.serviceFaqList').classList.remove(className);
        }
    }
}



function accordionFilterList(inputObj, listObj, nofound){
    var input, filter, ul, li, a, i, txtValue;
    input = document.querySelector(inputObj);
    console.log(input.value);
    if(input.value.replace(/\s/g, "") === "") {
        input.closest('.inside-search').querySelector(".clearIcon").classList.add('d-none');
    } else {
        input.closest('.inside-search').querySelector(".clearIcon").classList.remove('d-none');
    }

    filter = input.value.toUpperCase().trim();
    accordionlist = document.querySelectorAll(listObj);

    for (i = 0; i < accordionlist.length; i++) {
        a = accordionlist[i];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            accordionlist[i].classList.remove('d-none');
        } else {
            accordionlist[i].classList.add("d-none");
        }
    }
    if(document.querySelectorAll(listObj+'.d-none').length == document.querySelectorAll(listObj).length){
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




 