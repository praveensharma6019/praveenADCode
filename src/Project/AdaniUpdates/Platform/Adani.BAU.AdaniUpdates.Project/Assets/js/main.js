console.log('Microsite custom JS file');
let currentPage = 1;
let datefilterFlag = false;
$(document).ready(function () {
    /* Initial Search field | START */
    $('.search-box').click(function (e) {
        var $this = $(e.currentTarget);
        setTimeout(function () {
            $this.closest('.search-box-wrapper').find('.search-box-dd-field input').focus();
        }, 300);
    });
    /* Initial Search field | END */

    /* Search - cross icon click | START */
    $('.clear-input').click(function (e) {
        e.stopPropagation();
        $(this).fadeOut(200);
    });

    /* Search - keyup | START */
    $('.search-box-dd-field input, .search-box input').on("keyup", function () {
        if ($(this).val().length > 0) {
            $(this).parents('.search-box-dd-field').find('.clear-input').fadeIn(200);
        } else {
            $(this).parents('.search-box-dd-field').find('.clear-input').fadeOut(200);
        }
    });
    /* Search - keyup | END */

    /* Date range picker | START */
    if ($('input[name="datefilter"]').length > 0) {
        var $datefield = $('input[name="datefilter"]');
        let dtrange = (utils.qsToJSON().dt || '').split('-');
        let startDate = moment(dtrange[0], "DD/MM/YYYY");
        let endDate = moment(dtrange[1], "DD/MM/YYYY");

        $datefield.daterangepicker({
            opens: 'left',
            autoUpdateInput: false,
            maxDate: new Date(),
            startDate: startDate.isValid() ? startDate : new Date(),
            endDate: endDate.isValid() ? endDate : new Date() ,
            locale: {
                cancelLabel: 'Clear'
            }
        });

        $datefield.on('apply.daterangepicker', function (ev, picker) {
            var dtpValue = picker.startDate.format('DD/MM/YYYY') + '-' + picker.endDate.format('DD/MM/YYYY');
            $(this).val(dtpValue);
            applyFilter("dt");
        });

        $datefield.on('cancel.daterangepicker', function (ev, picker) {
            clearDateFilterField();
            resetFilter("dt");
        });

        $datefield.on('hide.daterangepicker', function (ev, picker) {
            $(this).parent().removeClass('show');
            $(this).parent().addClass('hide');
            datefilterFlag = false;
        });

        $datefield.on('click', function (ev, picker) {
            if (!datefilterFlag) {
                datefilterFlag = true;
                $(this).parent().removeClass('hide');
                $(this).parent().addClass('show');
                return;
            }

            if ($(this).parent().hasClass('show')) {
                $(this).parent().removeClass('show');
                $(this).parent().addClass('hide');
                $('.daterangepicker').hide();
            }
            else {
                $(this).parent().removeClass('hide');
                $(this).parent().addClass('show');
                $('.daterangepicker').show();
            }
        });

        $datefield.parent().addClass('hide');

    }
    /* Date range picker | END */


    bindSearch($('.search-box-wrapper.global'), 0, renderGlobalSearchDdl); // global search
    bindSearch($('.search-box-wrapper.pg'), location.pathname.toLowerCase().indexOf('/media-news') == 0 ? 1 : 2, renderPageSearchDdl); // page search

    //paging media and release
    $('button.load-more').on('click', loadMore);
    $("#pg_search_form").submit(function (e) {
        e.preventDefault();
        searchQuery($(this).find('input').val());

        let ddlEl = bootstrap.Dropdown.getInstance($('.search-box-wrapper.pg .search-box')[0]);
        ddlEl.hide();
    });

    //on page load, load data for 1st page
    search(utils.qsToJSON());
});

function loadMore(e) {
    e.preventDefault();
    let qs = utils.qsToJSON();
    ++currentPage;
    search(qs, true);
}

function clearDateFilterField() {
    var $datefield = $('input[name="datefilter"]');
    $datefield.val('');
    $datefield.data('daterangepicker').setStartDate(new Date());
    $datefield.data('daterangepicker').setEndDate(new Date());
}

function clearCategoryDll() {
    $('.filter-btn.category-btn').val('');
    $(".category-filter input:checkbox:checked").each(function () {
        $(this)[0].checked = false;
    });
}

function getSelectedCategory() {
    var value = [];
    $(".category-filter input:checkbox:checked").each(function () {
        value.push($(this).val());
    });

    return value;
}

//page filter category & period
function applyFilter(key) {
    let qs = utils.qsToJSON();
    delete qs['q'];
    delete qs['pg'];

    $('.search-box-wrapper.pg input').val('');
    if (key == 'cat') {
        let categories = getSelectedCategory();
        if (categories.length > 0) {
            qs["cat"] = categories.join(',');
            $('.filter-btn.category-btn').val(categories.join(', '));
        }
    }

    if (key == 'dt') {
        let period = $('input[name="datefilter"]').val();
        if ($.trim(period) != '') {
            qs["dt"] = period;
        }
    }

    if (location.search !== '') {
        currentPage = 1;
        search(qs);
    }
}

function resetFilter(key) {
    let qs = utils.qsToJSON();
    delete qs[key];

    if (key == 'cat') {
        clearCategoryDll();
    }

    if (key == 'q') {
        $('.search-box-wrapper.pg input').val('');
    }

    currentPage = 1;
    search(qs);
}

function renderPageSearchDdl(response, $dll) {
    response.forEach((item) => {
        $dll.append('<li onclick="searchQuery(\'' + item.title + '\')">' + item.title + '</li>');
    });
}

function searchQuery(query) {
    clearDateFilterField();
    clearCategoryDll();
    currentPage = 1;
    search({ q: query });
    $('.search-box-wrapper.pg input').val(query);
}

function renderGlobalSearchDdl(response, $dll) {
    response.forEach((item) => {
        $dll.append('<li><a target="_blank" href="' + item.linkurl + '">' + item.title + '</a> <span>' + item.summary + '</span></li>');
    });
}

function bindSearch($containerEl, type, renderCallback) {
    var searchTimeOut;
    var jqxhr;

    let $searchTextbox1El = $('.search-box input', $containerEl);
    let $searchTextboxEl = $('.search-box-dd input', $containerEl);
    let $searchBoxDdlEl = $('.search-box-dd > ul', $containerEl);
    let $clearSearchEl = $('.clear-input', $containerEl);

    $clearSearchEl.on('click', function () {
        //$searchTextboxEl.val('');
        clearSearchDdl();
        if (type != 0) resetFilter('q')
        else $containerEl.parent().find('input').val('');
    });

    $searchTextboxEl.on('keyup', function (e) {
        let query = $searchTextboxEl.val();
        $searchTextbox1El.val(query);

        if (query.length == 0 && type != 0) resetFilter('q');

        if (query.length < 3) {
            //reset ddl item if char count is less than 3
            clearSearchDdl();
            return;
        }

        if (searchTimeOut) {
            clearTimeout(searchTimeOut);
        }

        searchTimeOut = setTimeout(function () {
            //abort previous request if any
            if (jqxhr != null) {
                jqxhr.abort();
            }

            jqxhr = $.get("/api/adani-updates/search/summary?query=" + query + "&type=" + type)
                .done(function (response) {
                    clearSearchDdl();
                    if (response.length == 0) {
                        $searchBoxDdlEl.append('<li>No data found</li>');
                        return;
                    }
                    if (typeof (renderCallback) == 'function') {
                        renderCallback.call(null, response, $searchBoxDdlEl);
                    }
                })
                .fail(function () {
                    console.log("error in search api");
                });
        }, 300);
    });

    function clearSearchDdl() {
        $searchBoxDdlEl.empty();
    }
}

function isMobileView() {
    return $('.mobile:visible').length == 1;
}

function getPageSize() {
    if (location.pathname == "/news-highlights") {
        return 20;
    }

    return isMobileView() ? 8 : 12;
}

function search(qs, append) {
    let $gridContainerEl = $('.search-result-grid');
    if ($gridContainerEl.length == 0) return;

    let searchStr = "?pg=" + currentPage;
    if (!jQuery.isEmptyObject(qs)) {
        history.pushState({}, '', location.pathname + utils.jsonToQs(qs));
        searchStr = utils.jsonToQs(qs) + "&pg=" + currentPage;
    }
    else {
        history.pushState({}, '', location.pathname);
    }

    if (currentPage > 1) {
        $('button.load-more').addClass('d-none');
        $('div.load-more-progress').removeClass('d-none');
    }

    $.get("/api/adani-updates/search" + location.pathname + searchStr + '&mb=' + isMobileView())
        .done(searchSuccess)
        .fail(function () {
            console.log("error in search api");
            $('button.load-more').addClass('d-none');
            $('div.load-more-progress').addClass('d-none');
        });

    function searchSuccess(response) {
        $('div.load-more-progress').addClass('d-none');

        if (!append) {
            $gridContainerEl.empty();
        }

        let pageSize = getPageSize();

        if (response.length < pageSize || !response.NextPage)
            $('button.load-more').addClass('d-none');
        else
            $('button.load-more').removeClass('d-none');

        if (location.pathname == "/news-highlights") {
            renderNewsHighlights($gridContainerEl, response.Result);
        }
        else {
            renderMediaItems($gridContainerEl, response.Result);
        }
    }

    function renderMediaItems($container, response) {
        response.forEach((item, i) => {
            var card = `<div class="col-lg-4 col-md-6 mb-4 card-item">
                            <a href="${item.LinkUrl}" target="${item.LinkTarget || '_blank'}">
                                  <button type="button" class="btn btn-default btn-sm share-play ${item.IsVideo ? '' : 'd-none'}"> <img src="/-/media/Project/Adani Updates/images/images/player" width="80px" /></button>
                                <div class="single-item d-flex justify-content-between flex-column">
                                    <div>
                                        <figure>
                                            <img src="${item.ImageSrc}" alt="${item.ImageAlt}">
                                        </figure>
                                        <div class="content">
                                            <span class="subtext blackTx_666">${getDate(item.Date)}</span>
                                            <h5 class="subtext blackTx_222">${item.Title}</h5>
                                            <p>${item.Summary}</p>
                                        </div>
                                    </div>
                                    <div class="share-div" data-bs-toggle="modal" data-bs-target="#shareArticleModal">
                                        <img src="/-/media/Project/Adani-Updates/images/images/share" alt="" class="share-icon">
                                    </div>
                                    <span class="btm-text subtext blackTx_666 black-link">Know more</span>
                                </div>
                            </a>
                        </div>`;

            $container.append(card);
        });
    }

    function getFileUrl(link) {
        if (link.startsWith('https://'))
            return link;

        return `${location.origin}${link}`;
    }

    function renderNewsHighlights($container, response) {
        response.forEach((item, i) => {
            var date = parseDate(item.Date);
            var grpDate = moment(parseDate(item.Date)).format('MMMM YYYY');

            //render first section
            if (!append && i == 0 && location.search == '') {
                $container.append(`<div class="infoCntr mt-5">
                                        <p class="info-head">LATEST NEWSLETTER</p>
                                        <ul class="flex-block">
                                            <li class="flex-item">
                                                <div class="d-flex infoCntrData">
                                                    <div>
                                                        <div class="flex-date">${getDate(item.Date)}</div>
                                                        <p class="flex-para">${item.Title}</p>
                                                    </div>
                                                    <div class="listEvents">
                                                        <a href="${getFileUrl(item.LinkUrl)}" target="${item.LinkTarget || '_blank'}" rel="noopener noreferrer">
                                                            <img src="/-/media/Project/Adani-Updates/images/images/download.ashx" /></a>
                                                        <span class="share-div-news" data-bs-toggle="modal" data-bs-target="#shareArticleModal">
                                                            <img src="/-/media/Project/Adani-Updates/images/images/share.ashx" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>`);
                return;
            }

            //render section
            var sectionId = `section_${date.getMonth()}_${date.getFullYear()}`;
            var $sectionEl = $('#' + sectionId);

            if ($sectionEl.length == 0) {
                var $sectionEl = $(`<div id="${sectionId}" class="infoCntr mt-5">
                                        <p class="info-head">${grpDate}</p>
                                        <ul class="flex-block">
                                        </ul>
                                    </div>`);
                $container.append($sectionEl);
            }

            $sectionEl.find('ul').append(`
                            <li class="flex-item">
                                <div class="d-flex infoCntrData">
                                    <div>
                                        <div class="flex-date">${getDate(item.Date)}</div>
                                        <p class="flex-para">${item.Title}</p>
                                    </div>
                                    <div class="listEvents">
                                        <a href="${getFileUrl(item.LinkUrl)}" target="${item.LinkTarget || '_blank'}" rel="noopener noreferrer"><img src="assets/images/download.svg" /></a>
                                        <span class="share-div-news" data-bs-toggle="modal" data-bs-target="#shareArticleModal">
                                             <img src="/-/media/Project/Adani-Updates/images/images/share.ashx" />
                                        </span>
                                    </div>
                                </div>
                            </li>`);
        });
    }

    function getDate(value) {
        let dateInTicks = parseFloat(value.match(/\d+/g)[0]);
        return moment(dateInTicks).format('D MMM YYYY');
    }

    function parseDate(value) {
        return new Date(parseFloat(value.match(/\d+/g)[0]));
    }
}

let utils = {
    qsToJSON: function () {
        try {
            var qsString = location.search.substring(1);
            return JSON.parse('{"' + qsString.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (n, t) {
                return n === "" ? t : decodeURIComponent(t)
            })
        } catch (t) {
            return {}
        }
    },
    jsonToQs: function (json) {
        return "?" + Object.keys(json).map(t => encodeURIComponent(t) + "=" + encodeURIComponent(json[t])).join("&")
    },
    setQs: function (key, value) {
        var qsJson = this.qsToJSON(), qs;
        qsJson[key] = value;
        qs = this.jsonToQs(qsJson);
        location.href = location.pathname + qs;
    }
};

//----------------Share - icon----------------
$(document).ready(function () {
    $(".search-result-grid").on("click", ".share-div", function (e) {
        e.preventDefault();

        var $cardItemEl = $(this).parents('.card-item');
        var selectedUrl = $cardItemEl.find('a').attr('href');
        var imageUrl = $cardItemEl.find('figure img').attr('src');
        var heading = $cardItemEl.find('.content h5').text();
        var summary = $cardItemEl.find('.content p').text();

        $("#share-div-url").val(selectedUrl);

        var $modelEl = $('#shareArticleModal');
        $modelEl.find('.thumb-img img').attr('src', imageUrl);
        $modelEl.find('.thumb-text h2').text(heading);
        $modelEl.find('.thumb-text p').text(summary);

        $('.social-share').off().on('click', function (e) {
            $(this).attr('href', $(this).data('url') + selectedUrl);
            $(".linkCopied").addClass("d-none");
        });
    });

    $(".share-div-center").on("click", function (e) {
        e.preventDefault();

        var selectedUrl = $(this).prev().attr("href");
        $("#share-div-url").val(selectedUrl);
        $('.thumb-div').addClass('d-none');

        $('.social-share').off().on('click', function (e) {
            $(this).attr('href', $(this).data('url') + selectedUrl);
            $(".linkCopied").addClass("d-none");
        });
    });

    $(".search-result-grid").on("click", ".share-div-news", function (e) {
        e.preventDefault();

        var $cardItemEl = $(this).parents('.flex-item');

        var heading = $cardItemEl.find('.flex-date').text();
        var summary = $cardItemEl.find('.flex-para').text();

        var selectedUrl = $(this).prev().attr("href");

        $("#share-div-url").val(selectedUrl);

        var $modelEl = $('#shareArticleModal');
        $modelEl.find('h2.modal-title').text("Share this Newsletter");
        $modelEl.find('.thumb-img img').attr('src', '/-/media/Project/Adani Updates/images/images/news-share');
        $modelEl.find('.thumb-text h2').text(heading);
        $modelEl.find('.thumb-text p').text(summary);

        $('.social-share').off().on('click', function (e) {
            $(this).attr('href', $(this).data('url') + selectedUrl);
            $(".linkCopied").addClass("d-none");
        });
    });

    $(".clipboard").on("click", function () {
        var currentShareDivUrl = $("#share-div-url").val();

        navigator.clipboard.writeText(currentShareDivUrl);
        $(".linkCopied").removeClass("d-none");
    });

    const myModalEl = document.getElementById("shareArticleModal");
    if (myModalEl) {
        myModalEl.addEventListener("show.bs.modal", (event) => {
            $(".linkCopied").addClass("d-none");
        });

        myModalEl.addEventListener("hide.bs.modal", (event) => {
            $("#share-div-url").val("#");
        });
    }

    function resetModal() {
        $(".linkCopied").addClass("d-none");
    }

    $("#shareArticleModal .btn-primary").click(resetModal);

    $('.custom-tab ul li').on('click', function (e) {
        e.preventDefault();
        let tabEl = this;
        let tabContainerEl = tabEl.parentElement;
        let tabOffsetLeft = tabEl.offsetLeft;
        let newLeftOffset = tabOffsetLeft - 80;
        tabContainerEl.scrollLeft = newLeftOffset;
    });
});