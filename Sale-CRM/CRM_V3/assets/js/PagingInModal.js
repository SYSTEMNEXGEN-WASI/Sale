﻿

// Link for the Pagining
////https://codepen.io/yasser-mas/pen/pyWPJd


// Method for calling function
//getPagination('#CustomerTable');



function getPagination(table) {

    var lastPage = 1;

    $('#maxRows').on('change', function (evt) {
        //$('.paginationprev').html('');						// reset pagination 


        lastPage = 1;
        $('.pagination').find("li").slice(1, -1).remove();
        var trnum = 0;									// reset tr counter 
        var maxRows = parseInt($(this).val());			// get Max Rows from select option

        if (maxRows == 5000) {

            $('.pagination').hide();
        } else {

            $('.pagination').show();
        }
        debugger
        var totalRows = $(table + ' tbody tr').length + 1;		// numbers of rows 
        $(table + ' tr:gt(0)').each(function () {			// each TR in  table and not the header
            trnum++;									// Start Counter 
            if (trnum > maxRows) {						// if tr number gt maxRows

                $(this).hide();							// fade it out 
            } if (trnum <= maxRows) { $(this).show(); }// else fade in Important in case if it ..
        });											//  was fade out to fade it in 
        if (totalRows > maxRows) {						// if tr total rows gt max rows option
            var pagenum = Math.ceil(totalRows / maxRows);	// ceil total(rows/maxrows) to get ..  
            //	numbers of pages 
            for (var i = 1; i <= pagenum ;) {			// for each page append pagination li 
                $('.pagination #prev').before('<li data-page="' + i + '">\
								      <span>'+ i++ + '<span class="sr-only">(current)</span></span>\
								    </li>').show();
            }											// end for i 
        } 												// end if row count > max rows
        $('.pagination [data-page="1"]').addClass('active'); // add active class to the first li 
        $('.pagination li').on('click', function (evt) {		// on click each page
            evt.stopImmediatePropagation();
            evt.preventDefault();
            var pageNum = $(this).attr('data-page');	// get it's number

            var maxRows = parseInt($('#maxRows').val());			// get Max Rows from select option

            if (pageNum == "prev") {
                if (lastPage == 1) { return; }
                pageNum = --lastPage;
            }
            if (pageNum == "next") {
                if (lastPage == ($('.pagination li').length - 2)) { return; }
                pageNum = ++lastPage;
            }

            lastPage = pageNum;
            var trIndex = 0;							// reset tr counter
            $('.pagination li').removeClass('active');	// remove active class from all li 
            $('.pagination [data-page="' + lastPage + '"]').addClass('active');// add active class to the clicked 
            // $(this).addClass('active');					// add active class to the clicked 
            $(table + ' tr:gt(0)').each(function () {		// each tr in table not the header
                trIndex++;								// tr index counter 
                // if tr index gt maxRows*pageNum or lt maxRows*pageNum-maxRows fade if out
                if (trIndex > (maxRows * pageNum) || trIndex <= ((maxRows * pageNum) - maxRows)) {
                    $(this).hide();
                } else { $(this).show(); } 				//else fade in 
            }); 										// end of for each tr in table
        });										// end of on click pagination list

    }).val(5).change();

    // end of on select change 



    // END OF PAGINATION 
}


$(function () {
    // Just to append id number for each row  
    $('table tr:eq(0)').prepend('<th> ID </th>')

    var id = 0;

    $('table tr:gt(0)').each(function () {
        id++
        $(this).prepend('<td>' + id + '</td>');
    });
})