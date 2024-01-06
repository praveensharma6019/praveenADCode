$(document).ready(function() {
    $('#example').DataTable( {
        "order": [[0, "desc"]],
        "columnDefs": [{ type: 'date', 'targets': 0 }]
    } );
} );