$(document).ready(function () {
   
    $('#saveVendorButton').click(function () {
        var formData = $('#addVendorForm').serialize();
        console.log(formData); // Check serialized data in console

        $.ajax({
            type: "POST",
            url: '/Vendor/AddVendor', // Match the controller action name
            data: formData,
            success: function (response) {
                console.log("success" + response);
                if (response.success) {
                    toastr.success('Vendor added successfully.');
                    location.reload(); // Reload the page on success
                } else {
                    toastr.error('Error adding vendor.');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // Log any errors
                toastr.error('An error occurred while adding the vendor.');
            }
        });
    });


    // Update Vendor

    $('#updateVendorButton').click(function () {
        var vendorId = $('#editVendorId').val(); // Get vendor ID from hidden input
        var formData = $('#editVendorForm').serialize(); // Serialize form data

        console.log("Updating Vendor with ID: " + formData);

        $.ajax({
            type: "PUT",
            url: '/Vendor/EditVendor', // Update URL as needed
            data: formData,
            success: function (response) {
                if (response.success) {
                    toastr.success('Vendor updated successfully.');
                    document.getElementById("editVendorModal").style.display = 'none'
                    location.reload(); // Reload the page after successful update
                } else {
                    toastr.error('Error updating vendor.');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // Log any errors
                toastr.error('An error occurred while updating the vendor.');
            }
        });
    });
});


// <!-- using ajax call -->

//function openEditModal(vendorId) {

//    $.ajax({
//        type: "GET",
//        url: '/Vendor/EditVendor', // Adjust this to match your controller action
//        data: { id: vendorId },
//        success: function (response) {
//            // Populate form fields with the vendor data
//            $('#editVendorId').val(response.id);
//            $('#editBusinessName').val(response.businessName);
//            $('#editVendorPersonName').val(response.vendorName);
//            $('#editEmail').val(response.email);
//            $('#editGstNo').val(response.gstNo);
//            $('#editContactNo').val(response.contactNo);
//            $('#editContactNo2').val(response.contactNo2);
//            $('#editAddress').val(response.address);
//            $('#editState').val(response.state);

//            // Open the modal
//            $('#editVendorModal').modal('show');
//        },
//        error: function (xhr, status, error) {
//            console.error(xhr.responseText); // Log any errors
//            toastr.error('An error occurred while retrieving the vendor details.');
//        }
//    });
//}

function openEditmodal(id, businessName, vendorName, email, gstNo, contactNo, alternateNumber, address, state) {
    // Set the values of the input fields in the modal
    $('#editVendorId').val(id);
    $('#editBusinessName').val(businessName);
    $('#editVendorPersonName').val(vendorName);
    $('#editEmail').val(email);
    $('#editGstNo').val(gstNo);
    $('#editContactNo').val(contactNo);
    $('#editContactNo2').val(alternateNumber);
    $('#editAddress').val(address);
    $('#editState').val(state);

    // Show the modal using Bootstrap's modal method
    document.getElementById("editVendorModal").style.display = 'block'
}
function openDeleteModal(id) {
   // alert("ID: " + id);
    $('#deleteVendorId').val(id); // Set the vendor ID in the hidden input

    $('#deleteConfirmButton').click(function () {

        var vendorId = $('#deleteVendorId').val(); // Get vendor ID from hidden input

        $.ajax({
            type: "DELETE",
            url: '/Vendor/DeleteVendor/' + vendorId, // Update URL to include vendor ID
            success: function (response) {
                if (response.success) {
                    toastr.success('Delete successfully.');
                    location.reload(); 
                } else {
                    toastr.error('Error deleting vendor.');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // Log any errors
                toastr.error('An error occurred while deleting the vendor.');
            }
        });
    });
}
