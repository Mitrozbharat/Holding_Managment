$(document).ready(function ()
{

    $("#updateVendor").click(function () {
        // Clear any previous error messages
        $(".error-message").remove();

        // Validation regex
        const textRegex = /^[a-zA-Z\s]+$/; // Text only (letters and spaces)
        const contactNumberRegex = /^\d{10}$/; // Exactly 10 digits
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Simple email validation


        // Retrieve values
        const vendorId = $('#editVendorId').val().trim();
        const editBusinessName = $('.editBusinessName').val().trim();
        const editVendorPersonName = $('.editVendorPersonName').val().trim();
        const editContactNo = $('.editContactNo').val().trim();
        const editemail = $('.editemail').val().trim();
        const editGstNo = $('.editGstNo').val().trim();
        const editalternateNumber = $('.editalternateNumber').val().trim();
        const editcity = $('.editcity').val().trim();
        const editaddress = $('.editaddress').val().trim();
        const editstate = $('.editstate').val();

        let isValid = true;

        // Validation logic
        if (!editBusinessName) {
            $('#editBusinessName').after('<span class="error-message text-danger">This field is required.</span>');
            isValid = false;
        } else if (!textRegex.test(editBusinessName)) {
            $('#editBusinessName').after('<span class="error-message text-danger">Business Name must contain only letters.</span>');
            isValid = false;
        }

        if (!editVendorPersonName) {
            $('#editVendorPersonName').after('<span class="error-message text-danger">This field is required.</span>');
            isValid = false;
        } else if (!textRegex.test(editVendorPersonName)) {
            $('#editVendorPersonName').after('<span class="error-message text-danger">Vendor Name must contain only letters.</span>');
            isValid = false;
        }

        if (!editemail) {
            $('#editemail').after('<span class="error-message text-danger">This field is required.</span>');
            isValid = false;
        } else if (!emailRegex.test(editemail)) {
            $('#editemail').after('<span class="error-message text-danger">Enter a valid email address.</span>');
            isValid = false;
        }

        if (!editContactNo) {
            $('#editContactNo').after('<span class="error-message text-danger">This field is required.</span>');
            isValid = false;
        } else if (!contactNumberRegex.test(editContactNo)) {
            $('#editContactNo').after('<span class="error-message text-danger">Contact Number must be exactly 10 digits.</span>');
            isValid = false;
        }

        if (editGstNo && editGstNo.length !== 15) {
            $('#editGstNo').after('<span class="error-message text-danger">GST Number must be exactly 15 characters.</span>');
            isValid = false;
        }

        // Proceed with AJAX if validation passes
        if (isValid) {
            const formData = {
                Id: vendorId,
                BusinessName: editBusinessName,
                VendorName: editVendorPersonName,
                City: editcity,
                Address: editaddress,
                Email: editemail,
                AlternateNumber: editalternateNumber,
                ContactNo: editContactNo,
                State: editstate,
                GstNo: editGstNo
            };

            console.table(" data: updated: ", formData);


            //$.ajax({
            //    url: '/Vendor/UpdateVendor',
            //    type: 'POST',
            //    data: JSON.stringify({
            //        id: 2, // Example data
            //        vendorName: "Vendor Name",
            //        businessName: "Business Name",
            //        email: "vendor@example.com",
            //        gstNo: "22AAAAA0000A1Z5",
            //        contactNo: "1234567890",
            //        alternateNumber: "0987654321",
            //        city: "City Name",
            //        address: "Address",
            //        state: "State"
            //    }),
            //    contentType: 'application/json',
            //    success: function (response) {
            //        if (response.success) {
            //            toastr.success("Vendor updated successfully.");
            //            $('#editVendorModal').modal('hide'); // Hide modal
            //            location.reload(); // Reload the page or update the vendor list as needed
            //        } else {
            //            toastr.error(response.message || "Failed to update vendor.");
            //        }
            //    },
            //    error: function (xhr, status, error) {
            //        console.error("Error details:", error); // Log the error for debugging
            //        console.error("Response text:", xhr.responseText); // Log response text for debugging
            //        toastr.error("An error occurred while updating the vendor.");
            //    }
            //});



            $.ajax({
                url: '/Vendor/UpdateVendor',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    if (response.success) {
                        toastr.success('Customer added successfully.');
                        $('#editVendorModal').modal('hide');
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

        } 


    });
       
 });



function openDeleteModalforvendor(id) {

    $('#deleteVendorId').val(id); // Set the customer ID in the hidden input
    $('#deleteConfirmButton').click(function () {

        var id = $('#deleteVendorId').val(); // Get customer ID from hidden input

        $.ajax({
            type: "DELETE",
            url: '/Vendor/Delete/' + id, // Update URL to include customer ID
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


