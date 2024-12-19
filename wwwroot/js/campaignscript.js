$(document).ready(function () {
    // Listen for changes on the Start Date and End Date

    var today = new Date().toISOString().split('T')[ 0 ]; // Get today's date in YYYY-MM-DD format
    $('#editfromdate').attr('min', today); // Set the min attribute to today's date, disabling past dates

    // Initialize variables to store valid start and end dates
    var previousStartDate = $('#editfromdate').val();
    var previousEndDate = $('#edittodate').val();

    // Handle start date change
    $('.editfromdate').on('change', function () {


        var id = $('#editId').val();
        var fromDate = $('#editfromdate').val();
        var toDate = $('#edittodate').val();
        var bookingAmt = $('#budget').val();
        var fk_id = $('#fk_id').val();

        // Check if the selected date is valid (today or later)
        if (fromDate >= today) {

            $.ajax({
                url: '/OngoingCampain/checkValidatedate',
                type: 'POST',
                data: { Id: id, FromDate: fromDate, ToDate: toDate, BookingAmt: bookingAmt, fk_id: fk_id },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);

                    }
                    else {
                        toastr.error(response.message);
                    }

                },
                error: function (xhr, status, error) {
                    toastr.error('Error  campaign.');
                }
            });

            previousStartDate = fromDate; // Update the valid start date

        } else {
            toastr.error('Please select a date that is today or later.');
            $('#editfromdate').val(previousStartDate); // Revert to the previous valid start date
        }


    });



    // Handle end date change
    $('.edittodate').on('change', function () {
        var startDate = new Date($('#editfromdate').val()); // Get the selected start date
        var endDate = new Date($('#edittodate').val()); // Get the selected end date
        var today = new Date(); // Get today's date
        today.setHours(0, 0, 0, 0); // Normalize time to midnight for comparison

        // Add 1 day to the start date for validation
        var minEndDate = new Date(startDate);
        minEndDate.setDate(minEndDate.getDate() + 1); // Minimum end date is start date + 1 day

        // Validate the end date against the start date
        if (isNaN(startDate) || isNaN(endDate)) {
            toastr.error('Invalid dates. Please select valid Start and End Dates.');
            return;
        }

        if (endDate < minEndDate) {
            toastr.error('End date must be at least one day after the start date.');
            $('#edittodate').val(previousEndDate); // Revert to the previous valid end date
            return;
        }

        // Validate the end date against today's date
        if (endDate < today) {
            toastr.error('End date cannot be in the past.');
            $('#edittodate').val(previousEndDate); // Revert to the previous valid end date
            return;
        }

        // Update previous valid end date
        previousEndDate = $('#edittodate').val();

        // Extract data to send in the AJAX request
        var id = $('#campaignId').val(); // Ensure the `id` is properly set in your HTML
        var fromDate = $('#editfromdate').val();
        var toDate = $('#edittodate').val();
        var bookingAmt = $('#bookingAmount').val(); // Example field; replace with actual field
        var fk_id = $('#inventoryId').val(); // Example field; replace with actual field

        // Make AJAX call to validate the dates on the server
        $.ajax({
            url: '/OngoingCampain/checkTodateValidatedate',
            type: 'POST',
            data: {
                Id: id,
                FromDate: fromDate,
                ToDate: toDate,
                BookingAmt: bookingAmt,
                fk_id: fk_id
            },
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                    $('#edittodate').val(previousEndDate); // Revert to the previous valid end date
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Error validating campaign dates.');
            }
        });
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
    if (!bookingAmt || isNaN(bookingAmt) || parseFloat(bookingAmt) <= 0) {
        toastr.error('Booking amount must be a positive number.');
        return; // Stop further processing if validation fails
    }


    $.ajax({
        url: '/OngoingCampain/UpdateCampaign',
        type: 'POST',
        data: { Id: id, FromDate: fromDate, ToDate: toDate, BookingAmt: bookingAmt, fk_id: fk_id },
        success: function (response) {
            location.reload();
        },
        error: function (xhr, status, error) {
            toastr.error('Error updating campaign.');
        }
    });
});


