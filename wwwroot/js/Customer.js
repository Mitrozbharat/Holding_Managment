

    $('#editCustomerForm').on('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission
    // Clear previous error messages
    $('.error-message').remove();

    // Validate required fields
    var isValid = true;

    // Check Business Name
    if ($('#editBusinessName').val().trim() === '') {
        $('#editBusinessName').after('<span class="error-message text-danger">This field is required.</span>');
    isValid = false;
               }

    // Check Contact Person Name
    if ($('#editCustomerName').val().trim() === '') {
        $('#editCustomerName').after('<span class="error-message text-danger">This field is required.</span>');
    isValid = false;
               }

    //// Check Email
    //if ($('#editEmail').val().trim() === '') {
    //    $('#editEmail').after('<span class="error-message text-danger">This field is required.</span>');
    //isValid = false;
    //           }

    // Check Contact Number
    if ($('#editContactNumber').val().trim() === '') {
        $('#editContactNumber').after('<span class="error-message text-danger">This field is required.</span>');
    isValid = false;
               }

  
    // If the form is valid, proceed with the AJAX submission
    if (isValid) {

                   // Get the form data
     var formData = {
     Id: $('#editCustomerId').val(),
    BusinessName: $('#editBusinessName').val(),
    CustomerName: $('#editCustomerName').val(),
    Email: $('#editEmail').val(),
    GstNo: $('#editGstn').val(),
    ContactNo: $('#editContactNumber').val(),
    AlternateNumber: $('#editAlternateNumber').val(),
    City: $('.editcity').val(),
    Address: $('.editAddress').val(),
    State: $('.editState').val()
                   };

    console.table(formData);

    $.ajax({
        url: '/Customer/UpdateCustomer',
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(formData),
    success: function (response) {
                           if (response.success) {
        toastr.success('Customer added successfully.');
    location.reload(); // Reload the page on success
                           } else {
        toastr.error('Error adding Customer.');
                           }
                       },
    error: function (error) {
        // Handle error response
        alert('An error occurred while updating customer details.');
                       }
                   });

               } else {
        toastr.error('Please fill all the required fields.');
               }


    });

    function openDeleteModal(id) {

        $('#customerId').val(id); // Set the customer ID in the hidden input
    $('#deleteConfirmButton').click(function () {

               var id = $('#customerId').val(); // Get customer ID from hidden input

    $.ajax({
        type: "DELETE",
    url: '/Customer/Delete/' + id, // Update URL to include customer ID
    success: function (response) {
                       if (response.success) {
        toastr.success('Delete successfully.');
    location.reload();
                       } else {
        toastr.error('Error deleting .');
                       }
                   },
    error: function (xhr, status, error) {
        console.error(xhr.responseText); // Log any errors
    toastr.error('An error occurred while deleting the vendor.');
                   }
               });
           });
        }





        // add inventory




$('#saveInventoryButton').click(function () {
    // Get the form and other input values
    var form = $('#addCustomerForm')[ 0 ];
    var imageFile = $('#imageInput')[ 0 ].files[ 0 ];
    var vendorName = $('#vendorName').val();
    var styp = $('#styp').val();
    var vendorId = $('#vendorids').val();
    var city = $('#city').val();
    var area = $('#Area').val();
    var arealocation = $('#location').val();
    var width = $('#width').val();
    var height = $('#height').val();
    var rate = $('#rate').val();
    var vendorAmt = $('#vendoramt').val();

    function highlightEmptyField(fieldId) {
        $(fieldId).css('border', '1px solid red');
        $(fieldId).focus();
    }

    // Clear previous error styles
    $('input, select').css('border', '1px solid #ccc');

    // Validation function to check if a value is numeric and not empty
    function isValidNumber(value) {
        return value !== null && value !== '' && !isNaN(value);
    }

    if (city =='') {
        toastr.error('This field is required.');
        highlightEmptyField('#city');
        return;
    }
    if (area == '') {
        toastr.error('This field is required.');
        highlightEmptyField('#Area');
        return;
    }
    if (location == '') {
        toastr.error('This field is required.');
        highlightEmptyField('#location');
        return;
    }

    if (!isValidNumber(width)) {
        toastr.error('Please enter a valid number for Width.');
        highlightEmptyField('#width');
        return;
    }


    // Validate the numeric fields
   
    if (!isValidNumber(height)) {
        toastr.error('Please enter a valid number for Height.');
        highlightEmptyField('#height');
        return;
    }
    if (!isValidNumber(rate)) {
        toastr.error('Please enter a valid number for Rate.');
        highlightEmptyField('#rate');
        return;
    }
    
    if (!isValidNumber(vendorAmt)) {
        toastr.error('Please enter a valid number for Vendor Amount.');
        return;
    }

    // Check other required fields
    if (!imageFile) {
        toastr.error('Please select an image file.');
        return;
    }
    if (!vendorName) {
        toastr.error('Please select a vendor name.');
        highlightEmptyField('#vendorName');
        return;
    }
    if (!vendorName) {
        toastr.error('Please select a vendor name.');
        return;
    }
    // Check for required fields
    if (!imageFile) {
        toastr.error('Please select an image file.');
        highlightEmptyField('#imageInput');
        return;
    }
   

    // Convert image to Base64
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result.replace("data:", "").replace(/^.+,/, '');

        // Send data via AJAX
        $.ajax({
            type: "POST",
            url: '/Dashboard/AddNewInventory',
            data: {
                city: city,
                area: area,
                location: arealocation,
                width: width,
                height: height,
                rate: rate,
                vendoramt: vendorAmt,
                vendorid: vendorId,
                stype: styp,
                Image: base64String
            },
            success: function (response) {
                toastr.success('Inventory added successfully.');
                $('#addCustomerModal').modal('hide'); // Use Bootstrap's modal hide function
                location.reload(); // Optionally reload the page
            },
            error: function (xhr, status, error) {
                toastr.error('An error occurred while adding the inventory.');
            }
        });
    };
    reader.readAsDataURL(imageFile); // Start reading the image file
});






