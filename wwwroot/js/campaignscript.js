$(document).ready(function () {
    // Listen for changes on the Start Date and End Date

    var today = new Date().toISOString().split('T')[ 0 ]; // Get today's date in YYYY-MM-DD format
    $('#editfromdate').attr('min', today); // Set the min attribute to today's date, disabling past dates

    // Initialize variables to store valid start and end dates
    var previousStartDate = $('#editfromdate').val();
    var previousEndDate = $('#edittodate').val();

    // Handle start date change
    $('.fromdate').on('change', function () {
        var selectedDate = $('#editfromdate').val(); // Get the selected date

        // Check if the selected date is valid (today or later)
        if (selectedDate >= today) {
            toastr.success('Valid date selected.');
            previousStartDate = selectedDate; // Update the valid start date
        } else {
            toastr.error('Please select a date that is today or later.');
            $('#editfromdate').val(previousStartDate); // Revert to the previous valid start date
        }
    });

    // Handle end date change
    $('.todate').on('change', function () {
        var startDate = new Date($('#editfromdate').val()); // Get the selected start date
        var endDate = new Date($('#edittodate').val()); // Get the selected end date

        // Add 1 day to the start date for validation
        var minEndDate = new Date(startDate);
        minEndDate.setDate(minEndDate.getDate() + 1); // Set minimum end date to start date + 1 day

        // Check if the selected end date is at least 1 day after the start date
        if (endDate >= minEndDate) {
            toastr.success('Valid end date selected.');
            previousEndDate = $('#edittodate').val(); // Update the valid end date
        } else {
            toastr.error('End date must be at least one day after the start date.');
            $('#edittodate').val(endDate); // Revert to the previous valid end date
        }
    });

    function searchCustomers() {
        var searchQuery = document.getElementById('searchQuery').value;
        var pageSize = document.getElementById('rowQuantity').value;
        window.location.href = `/OngoingCampain/Index?searchQuery=${searchQuery}&pageNumber=1&pageSize=${pageSize}`;
    }

    function updateRowQuantity() {
        var pageSize = document.getElementById('rowQuantity').value;
        var searchQuery = document.getElementById('searchQuery').value;
        window.location.href = `/OngoingCampain/Index?searchQuery=${searchQuery}&pageNumber=1&pageSize=${pageSize}`;
    }

});

function openEditcampModal(id, fromDate, toDate, budget, fk_id) {
    // Check if valid dates and budget
    if (fromDate && toDate && budget) {
        $('#editId').val(id);
        $('#editfromdate').val(fromDate);
        $('#edittodate').val(toDate);
        $('#budget').val(budget);
        $('#fk_id').val(fk_id);

        $('#editCampaignModal').modal('show');
    } else {
        toastr.error('Invalid data provided for editing.');
    }
}

$('#saveEditcampain').on('click', function () {
    var id = $('#editId').val();
    var fromDate = $('#editfromdate').val();
    var toDate = $('#edittodate').val();
    var bookingAmt = $('#budget').val();
    var fk_id = $('#fk_id').val();

    $.ajax({
        url: '/OngoingCampain/UpdateCampaign',
        type: 'POST',
        data: { Id: id, FromDate: fromDate, ToDate: toDate, BookingAmt: bookingAmt, fk_id: fk_id },
        success: function (response) {
            toastr.success('Campaign updated successfully.');
            location.reload();
        },
        error: function (xhr, status, error) {
            toastr.error('Error updating campaign.');
        }
    });
});


function deleteModel(id, fk_id) {
    $('#editId').val(id);
    $('#fk_id').val(fk_id);
}

$('#DeleteBtn').on('click', function () {

    var id = $('#editId').val();
    var fk_id = $('#fk_id').val();

    console.log(id);
    console.log(fk_id);

    $.ajax({
        url: '/OngoingCampain/DeleteCampaign',
        type: 'DELETE',
        data: { id: id, fk_id },
        success: function (response) {
            toastr.success('Campaign deleted successfully.');
            location.reload();
        },
        error: function (xhr, status, error) {
            toastr.error('Error deleting campaign.');
        }
    });
});
