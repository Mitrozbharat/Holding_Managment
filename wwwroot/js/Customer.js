

function openEditModal(id, businessName, customerName, email, gstNo, contactNo, alternateNo, city,address, state) {
        // Set the values of the input fields in the modal
    $('#editCustomerId').val(id);
    $('#editBusinessName').val(businessName);
    $('#editCustomerName').val(customerName);
    $('#editEmail').val(email);
    $('#editGstn').val(gstNo);
    $('#editContactNumber').val(contactNo);
    $('#editAlternateNumber').val(alternateNo);
    $('#editcity').val(city);
    $('#editAddress').val(address);
    $('#editState').val(state);
    // Show the modal
    document.getElementById('editCustomerModal').style.display = 'block';

        }


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

    // Check Email
    if ($('#editEmail').val().trim() === '') {
        $('#editEmail').after('<span class="error-message text-danger">This field is required.</span>');
    isValid = false;
               }

    // Check Contact Number
    if ($('#editContactNumber').val().trim() === '') {
        $('#editContactNumber').after('<span class="error-message text-danger">This field is required.</span>');
    isValid = false;
               }

    // Check if GST Number (if filled) is exactly 15 characters long
    var gstNumber = $('#editGstn').val().trim();
    if (gstNumber !== '' && gstNumber.length !== 15) {
        $('#editGstn').after('<span class="error-message text-danger">GST Number must be exactly 15 characters.</span>');
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






