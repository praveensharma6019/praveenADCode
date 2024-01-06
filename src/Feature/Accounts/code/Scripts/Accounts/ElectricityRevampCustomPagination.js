//Directions to use
//create 4 Input type hidden element on the page under div with class="div-table" and Add id attr to it 
//and change value attr as per your requirement and call this JS on the view after creating below elements
//Examples given below copy these and change value attr
//<input type="hidden" id="paginationDataSelector" value="#TrackComplaintsUL>li" />
//<input type="hidden" id="paginationNumberElementSelector" value="#trackComplaintPaginationNumber" />
//<input type="hidden" id="paginationScrollToElementSelector" value=".cgrf-complaint" />
//<input type="hidden" id="paginationPageSize" value="10" />
//<script src="~/Scripts/Accounts/ElectricityRevampCustomPagination.js"></script>

//Call below JS on the view
//$(document).ready(function () {
//    DrawPaginationNumber(1, '#TrackComplaintPaginationDiv');
//})

//Add these attr to the repetive elements data-id="" style="display:block/none" and
// 1. Set style display:block for the firstno of items you want to display in my case I have shown first 10 items and hide items after 10 elements and to hide set style display:none for others
// 2. In the data-id attr provide the number through the loop like for first 10 it should be 1 and for next 10 it should 2



function DrawPaginationNumber(id, paginationTableDiv) {
    var paginationDataSelector = $(paginationTableDiv).find('#paginationDataSelector').val();
    var paginationNumberElementSelector = $(paginationTableDiv).find('#paginationNumberElementSelector').val();
    var paginationPageSize = $(paginationTableDiv).find('#paginationPageSize').val();
    if ($(paginationDataSelector).length > 0) {
        var totalLength = $(paginationDataSelector).length;
        if (totalLength > paginationPageSize) {
            var pageSize = parseInt(paginationPageSize);
            var noOfPage = totalLength / pageSize;
            noOfPage = Math.ceil(noOfPage);
            var paginationNumbering = ``;
            var clickedElementID = parseInt(id);
            if (clickedElementID < 5) {
                if (noOfPage > 5) {
                    var isDotsShown = false;
                    for (var i = 1; i <= noOfPage; i++) {
                        if (i == clickedElementID) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect current">${i}</a>`;
                        } else if (i == 1 || i == noOfPage || i < 5) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">${i}</a>`;
                        } else if (!isDotsShown) {
                            isDotsShown = true;
                            paginationNumbering += `<a href="javascript:void(0)" class="wave-effect">...</a>`;
                        }
                    }
                } else {
                    for (var i = 1; i <= noOfPage; i++) {
                        if (i == clickedElementID) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect current">${i}</a>`;
                        } else {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">${i}</a>`;
                        }
                    }
                }
            } else {
                paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">1</a>
                                         <a href="javascript:void(0)" class="wave-effect">...</a>`;


                if ((clickedElementID + 3) < (noOfPage - 1)) {

                    for (var i = clickedElementID; i <= (clickedElementID + 2); i++) {
                        if (i == clickedElementID) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect current">${i}</a>`;
                        } else {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">${i}</a>`;
                        }
                    }
                    paginationNumbering += `<a href="javascript:void(0)" class="wave-effect">...</a>
                                            <a href="javascript:void(0)" data-divid="${noOfPage}" onClick="ShowElements(this)" class="wave-effect">${noOfPage}</a>`;


                } else if (clickedElementID == noOfPage || (clickedElementID + 3) > noOfPage) {
                    for (var i = (noOfPage - 3); i <= noOfPage; i++) {
                        if (i == id) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect current">${i}</a>`;
                        } else {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">${i}</a>`;
                        }
                    }
                } else {
                    for (var i = clickedElementID; i <= noOfPage; i++) {
                        if (i == clickedElementID) {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect current">${i}</a>`;
                        } else {
                            paginationNumbering += `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowElements(this)" class="wave-effect">${i}</a>`;
                        }
                    }
                }
            }

            //if (noOfPage > 1) {
            var pagination = `<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowPreviousElements(this)"><i class="i-arrow-l"></i></a>${paginationNumbering}<a href="javascript:void(0)" data-divid="${paginationTableDiv}" onClick="ShowNextElements(this)"><i class="i-arrow-r"></i></a>`;
            $(paginationNumberElementSelector).html(pagination);
            //}
        }
    }
}

function ShowElements(el) {
    var paginationTableDiv = $(el).attr('data-divid');
    var paginationDataSelector = $(paginationTableDiv).find('#paginationDataSelector').val();
    var paginationScrollToElementSelector = $(paginationTableDiv).find('#paginationScrollToElementSelector').val();

    var id = $(el).html();
    $(paginationDataSelector).each(function () {
        if ($(this).attr('data-id') == id) {
            $(this).show();
        } else {
            $(this).hide();
        }
    })
    $(el).addClass('current').siblings().removeClass('current');
    $(paginationScrollToElementSelector)[0].scrollIntoView(true);
    DrawPaginationNumber(id, paginationTableDiv);
}

function ShowPreviousElements(el) {
    var paginationTableDiv = $(el).attr('data-divid');
    var paginationDataSelector = $(paginationTableDiv).find('#paginationDataSelector').val();
    var paginationNumberElementSelector = $(paginationTableDiv).find('#paginationNumberElementSelector').val();
    var paginationScrollToElementSelector = $(paginationTableDiv).find('#paginationScrollToElementSelector').val();

    var currentActiveId = $(paginationNumberElementSelector + ' .current').html();
    //$(paginationNumberElementSelector).each(function () {
    //    if ($(this).hasClass('current')) {
    //        currentActiveId = $(this).html();
    //        //break;
    //    }
    //})
    var previousID = parseInt(currentActiveId) - 1;
    if (currentActiveId > 1) {
        $(paginationDataSelector).each(function () {
            if ($(this).attr('data-id') == previousID) {
                $(this).show();
            } else {
                $(this).hide();
            }
        })

        $(paginationNumberElementSelector + '>a').each(function () {
            if ($(this).html() == previousID) {
                $(this).addClass('current').siblings().removeClass('current');
                //break;
            }
        })

        $(paginationScrollToElementSelector)[0].scrollIntoView(true);
        DrawPaginationNumber(previousID, paginationTableDiv);
    }
}

function ShowNextElements(el) {
    var paginationTableDiv = $(el).attr('data-divid');
    var paginationDataSelector = $(paginationTableDiv).find('#paginationDataSelector').val();
    var paginationNumberElementSelector = $(paginationTableDiv).find('#paginationNumberElementSelector').val();
    var paginationScrollToElementSelector = $(paginationTableDiv).find('#paginationScrollToElementSelector').val();
    var paginationPageSize = $(paginationTableDiv).find('#paginationPageSize').val();

    var currentActiveId = $(paginationNumberElementSelector + ' .current').html();
    //$(paginationNumberElementSelector).each(function () {
    //    if ($(this).hasClass('current')) {
    //        currentActiveId = $(this).html();
    //        //break;
    //    }
    //})

    var totalLength = $(paginationDataSelector).length;
    if (totalLength > 10) {
        var pageSize = parseInt(paginationPageSize);
        var noOfPage = totalLength / pageSize;
        noOfPage = Math.ceil(noOfPage);
        var nextID = parseInt(currentActiveId) + 1;
        if (currentActiveId < noOfPage) {
            $(paginationDataSelector).each(function () {
                if ($(this).attr('data-id') == nextID) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            })

            $(paginationNumberElementSelector + '>a').each(function () {
                if ($(this).html() == nextID) {
                    $(this).addClass('current').siblings().removeClass('current');
                    //break;
                }
            })
            $(paginationScrollToElementSelector)[0].scrollIntoView(true);
            DrawPaginationNumber(nextID, paginationTableDiv);
        }
    }
}