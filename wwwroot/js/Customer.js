
function openEditModal(id, businessName, customerName, email, gstNo, contactNo, alternateNo, address, state) {
    // Set the values of the input fields in the modal
    $('#editCustomerId').val(id);
    $('#editBusinessName').val(businessName);
    $('#editCustomerName').val(customerName);
    $('#editEmail').val(email);
    $('#editGstn').val(gstNo);
    $('#editContactNumber').val(contactNo);
    $('#editAlternateNumber').val(alternateNo);
    $('#editAddress').val(address);
    $('#editState').val(state);
    // Show the modal
    document.getElementById('editCustomerModal').style.display = 'block';

}

$(document).ready(function ()
{
    //$('#saveInventoryButton').click(function () {

    //    var formData = $('#addCustomerForm').serialize();
    //    console.log("Serialized Form Data: ", formData); // For debugging



    //    $.ajax({
    //        type: "POST",
    //        url: '/Dashboard/AddNewInventory', // Update the URL to match your controller action
    //        data: formData,
    //        success: function (response) {

    //            console.log("obj : " + response);

    //            toastr.success('Inventory added successfully.');
    //            document.getElementById('addCustomerModal').style.display = 'none';
    //            location.reload(); // Optionally reload the page

    //        },
    //        error: function (xhr, status, error) {
    //            toastr.error('An error occurred while adding the customer.');
    //        }
    //    });
    //});

    $('#saveInventoryButton').click(function ()
    {
        debugger
        var form = $('#addCustomerForm')[0]; // Get the form element
        var formData = new FormData(form); // Create FormData object from form

        var imageFile = $('#imageInput')[0].files[0]; // Get the selected image file
        var vendorname = $('#vendorName').val();
        var vendornameid = $('#vendorids').val();

        if (imageFile && vendorname != "") {
            var reader = new FileReader();

            reader.onloadend = function () {
                var base64String = reader.result.replace("data:", "").replace(/^.+,/, ''); // Convert image to Base64 string

                // Send the form data along with the Base64 string via AJAX
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/AddNewInventory', // Update the URL to match your controller action
                    data: {
                        city: $('#city').val(),
                        area: $('#Area').val(),
                        location: $('#location').val(),
                        width: $('#width').val(),
                        height: $('#height').val(),
                        rate: $('#rate').val(),
                        vendoramt: $('#vendoramt').val(),
                        vendorid: $('#vendorids').val(),
                        Image: base64String // Send the Base64 encoded image string
                    },
                    success: function (response)
                    {
                        console.log("Response: " + response);
                        toastr.success('Customer added successfully.');
                        document.getElementById('addCustomerModal').style.display = 'none';
                        location.reload(); // Optionally reload the page
                    },
                    error: function (xhr, status, error)
                    {
                        toastr.error('An error occurred while adding the customer.');
                    }
                });
            };
            reader.readAsDataURL(imageFile); // Convert the image to Base64 string
        } else {

            if (imageFile) {
                toastr.error('Please select vendor name.');
            }
            else {
                toastr.error('Please select an image file.');
            }
           
        }
    });


    $('#saveCustomerButton').click(function () {

        var formData = $('#addCustomerForm').serialize();
        console.log("Serialized Form Data: ", formData); // For debugging

        $.ajax({
            type: "POST",
            url: '/Customer/AddNewCustomer', // Update the URL to match your controller action
            data: formData,
            success: function (response) {

                console.log("obj : " + response);

                toastr.success('Customer added successfully.');
                document.getElementById('addCustomerModal').style.display = 'none';
                location.reload(); // Optionally reload the page

            },
            error: function (xhr, status, error) {
                toastr.error('An error occurred while adding the customer.');
            }
        });
    });

    $('#editCustomerForm').on('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission


        // Get the form data
        var formData = {
            Id: $('#editCustomerId').val(),
            BusinessName: $('#editBusinessName').val(),
            CustomerName: $('#editCustomerName').val(),
            Email: $('#editEmail').val(),
            GstNo: $('#editGstn').val(),
            ContactNo: $('#editContactNumber').val(),
            AlternateNumber: $('#editAlternateNumber').val(),
            Address: $('#editAddress').val(),
            State: $('#editState').val()
        };


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
    });
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




